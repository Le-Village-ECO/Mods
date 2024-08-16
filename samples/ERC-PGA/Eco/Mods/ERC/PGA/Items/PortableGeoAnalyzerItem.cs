using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Eco.Core.Controller;
using Eco.Core.Items;
using Eco.Core.Systems;
using Eco.Gameplay.Components;
using Eco.Gameplay.DynamicValues;
using Eco.Gameplay.Interactions.Interactors;
using Eco.Gameplay.Items;
using Eco.Gameplay.Items.Recipes;
using Eco.Gameplay.Players;
using Eco.Mods.TechTree;
using Eco.Shared.Items;
using Eco.Shared.Localization;
using Eco.Shared.Math;
using Eco.Shared.Serialization;
using Eco.Shared.SharedTypes;
using Eco.Shared.Utils;
using Eco.Shared.View;
using Eco.Shared.Voxel;
using Eco.Simulation.WorldLayers;
using Eco.Simulation.WorldLayers.Layers;
using Eco.World;
using Eco.World.Blocks;

namespace Eco.Mods.ERC.PGA.Items
{
	// Token: 0x0200000A RID: 10
	[NullableContext(1)]
	[Nullable(0)]
	[Serialized]
	[LocDisplayName("Portable Geological Analyzer")]
	[LocDescription("Analyzer tools for detecting valuable resources under the ground.")]
	[Category("Tools")]
	[Tag("Tool")]
	[Weight(1000)]
	internal class PortableGeoAnalyzerItem : ToolItem, IInteractor, IHasInteractions, IController, IViewController, IHasUniversalID
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000F RID: 15 RVA: 0x00002375 File Offset: 0x00000575
		public override ItemCategory ItemCategory
		{
			get
			{
				return 13;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000010 RID: 16 RVA: 0x00002379 File Offset: 0x00000579
		public override LocString Label
		{
			get
			{
				return Localizer.DoStr("Charge");
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000011 RID: 17 RVA: 0x00002385 File Offset: 0x00000585
		public override float DurabilityRate
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000012 RID: 18 RVA: 0x0000238C File Offset: 0x0000058C
		public override Item RepairItem
		{
			get
			{
				return Item.Get<BasicBatteryItem>();
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000013 RID: 19 RVA: 0x00002393 File Offset: 0x00000593
		public override int FullRepairAmount
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000014 RID: 20 RVA: 0x00002396 File Offset: 0x00000596
		public override IDynamicValue DurabilityBurn
		{
			get
			{
				return PortableGeoAnalyzerItem.durabilityBurn;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000015 RID: 21 RVA: 0x0000239D File Offset: 0x0000059D
		public override IDynamicValue SkilledRepairCost
		{
			get
			{
				return PortableGeoAnalyzerItem.skilledRepairCost;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000016 RID: 22 RVA: 0x000023A4 File Offset: 0x000005A4
		public override IDynamicValue CaloriesBurn
		{
			get
			{
				return PortableGeoAnalyzerItem.caloriesBurn;
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000023AC File Offset: 0x000005AC
		[Interaction(1, "Analyze area", 0, null, 15f, 0f, 0, 0, false, 0, null, 1, false, 0, new string[]
		{

		})]
		public void Analyze(Player player, InteractionTriggerInfo triggerInfo, InteractionTarget target)
		{
			if (target.IsBlock && base.Durability > 0f)
			{
				target.BlockPosition.Value + (Vector3i)target.HitNormal;
				LocStringBuilder locStringBuilder = new LocStringBuilder();
				locStringBuilder.AppendLineNTStr(Text.Header(Localizer.DoStr("Oil Amount"))).AppendLine(1);
				locStringBuilder.AppendLineLoc(FormattableStringFactory.Create("In radius {0} found: {1}", new object[]
				{
					Text.Num(PumpJackObject.Radius),
					Text.Num(PortableGeoAnalyzerItem.GetOilAmount(target.BlockPosition.Value.XZ))
				}));
				LocStringBuilder locStringBuilder2 = locStringBuilder;
				string format = "1x {0} crafting time: {1}";
				object[] array = new object[2];
				int num = 0;
				Item item = Item.Get(typeof(PetroleumItem));
				array[num] = ((item != null) ? new LocString?(item.MarkedUpName) : null);
				array[1] = Text.Num(PortableGeoAnalyzerItem.GetCraftMinutes(player.User, target.BlockPosition.Value));
				locStringBuilder2.AppendLineLoc(FormattableStringFactory.Create(format, array));
				locStringBuilder.AppendLine(1);
				WorldRange worldRange;
				worldRange..ctor(target.BlockPosition.Value.XZ - (Vector2.one * PumpJackObject.Radius).RoundUp, target.BlockPosition.Value.XZ + (Vector2.one * PumpJackObject.Radius).RoundUp);
				worldRange.Fix(World.VoxelSize);
				locStringBuilder.AppendLineNTStr(Text.Header(Localizer.DoStr("Ore Amount"))).AppendLine(1);
				locStringBuilder.AppendLineLoc(FormattableStringFactory.Create("Analysed area {0} - {1} [{2} m2]", new object[]
				{
					worldRange.min.XZ,
					worldRange.max.XZ,
					worldRange.VolumeInc
				}));
				HashSet<Type> itemsByTag = ItemUtils.GetItemsByTag(new string[]
				{
					"Ore"
				});
				Dictionary<Type, Dictionary<int, int>> dictionary = new Dictionary<Type, Dictionary<int, int>>();
				int num2 = 0;
				for (int i = target.BlockPosition.Value.Y; i > 0; i--)
				{
					foreach (Vector2i vector2i in worldRange.XZIterInc())
					{
						Vector3i vector3i;
						vector3i..ctor(vector2i.X, i, vector2i.Y);
						Block block = World.GetBlock(vector3i);
						if (block != null && !(block is EmptyBlock))
						{
							num2++;
							IRepresentsItem representsItem = block as IRepresentsItem;
							Type type;
							if (representsItem == null)
							{
								BlockItem blockItem = BlockItem.CreatingItem(block.GetType());
								type = ((blockItem != null) ? blockItem.GetType() : null);
							}
							else
							{
								type = representsItem.RepresentedItemType;
							}
							Type type2 = type;
							if (type2 != null && itemsByTag.Contains(type2))
							{
								block.GetType();
								Dictionary<int, int> dictionary2;
								if (dictionary.TryGetValue(type2, out dictionary2))
								{
									DictionaryExtensions.AddOrUpdate<int, int>(dictionary2, vector3i.Y, 1, (int oldVal, int newVal) => oldVal + newVal);
								}
								else
								{
									dictionary.Add(type2, new Dictionary<int, int>
									{
										{
											vector3i.Y,
											1
										}
									});
								}
							}
						}
					}
				}
				int num3 = dictionary.Sum((KeyValuePair<Type, Dictionary<int, int>> t) => t.Value.Sum((KeyValuePair<int, int> o) => o.Value));
				if (dictionary.Count > 0)
				{
					double num4 = Math.Round((double)num3 / (double)num2 * 100.0, 2);
					locStringBuilder.AppendLineLoc(FormattableStringFactory.Create("Found {0} ore blocks ({1}% of blocks in area):", new object[]
					{
						Text.Num((float)num3),
						Text.Num(num4)
					}));
					using (Dictionary<Type, Dictionary<int, int>>.Enumerator enumerator2 = dictionary.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							KeyValuePair<Type, Dictionary<int, int>> keyValuePair = enumerator2.Current;
							int num5 = 0;
							foreach (KeyValuePair<int, int> keyValuePair2 in keyValuePair.Value)
							{
								num5 += keyValuePair2.Value;
							}
							double num6 = Math.Round((double)num5 / (double)num3 * 100.0, 2);
							LocStringBuilder locStringBuilder3 = locStringBuilder;
							string format2 = "- {0} : {1} ({2}%)";
							object[] array2 = new object[3];
							int num7 = 0;
							Item item2 = Item.Get(keyValuePair.Key);
							array2[num7] = ((item2 != null) ? new LocString?(item2.MarkedUpName) : null);
							array2[1] = Text.Num((float)num5);
							array2[2] = Text.Num(num6);
							locStringBuilder3.AppendLineLoc(FormattableStringFactory.Create(format2, array2));
						}
						goto IL_4D2;
					}
				}
				locStringBuilder.AppendLineLoc(FormattableStringFactory.Create("There no any ore in area!", Array.Empty<object>()));
				IL_4D2:
				player.LargeInfoBox(Localizer.Do(FormattableStringFactory.Create("{0} {1}", new object[]
				{
					this.DisplayName,
					target.BlockPosition
				})), locStringBuilder.ToLocString(), null);
				this.UseDurability(1f, player);
				base.BurnCaloriesNow(player, 1f);
				return;
			}
			base.BurnCaloriesNow(player, 1f);
			if (base.Durability <= 0f)
			{
				player.ErrorLoc(FormattableStringFactory.Create("Tool not working anymore, looks like {0} discharged, you need to repair it.", new object[]
				{
					Item.Get<BasicBatteryItem>().MarkedUpName
				}));
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000297C File Offset: 0x00000B7C
		private static float GetCraftMinutes(User user, Vector3i pos)
		{
			RecipeFamily recipeFamily = CraftingComponent.RecipesOnWorldObject(typeof(PumpJackObject)).FirstOrDefault((RecipeFamily rf) => rf.RecipeItems.Any((Type ri) => ri == typeof(PetroleumItem)));
			if (recipeFamily == null)
			{
				return 0f;
			}
			if (!(recipeFamily.CraftMinutes is MultiDynamicValue))
			{
				return 0f;
			}
			return recipeFamily.CraftMinutes.GetCurrentValue(new ModuleContext(user, pos, null));
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000029F4 File Offset: 0x00000BF4
		private static float GetOilAmount(Vector2i pos)
		{
			WorldLayer layer = Singleton<WorldLayerManager>.Obj.GetLayer("Oilfield");
			float value = 0f;
			float total = 0f;
			if (PumpJackObject.Radius > 0f)
			{
				layer.ForRadius(layer.WorldPosToLayerPos(pos), PumpJackObject.Radius, delegate(Vector2i x, float val)
				{
					value += val;
					float total = total;
					total += 1f;
					return total;
				});
			}
			else
			{
				value = layer[layer.WorldPosToLayerPos(pos)];
				total = 1f;
			}
			return value / total;
		}

		// Token: 0x04000004 RID: 4
		private static IDynamicValue durabilityBurn = new ConstantValue(10f);

		// Token: 0x04000005 RID: 5
		private static IDynamicValue skilledRepairCost = new ConstantValue(4f);

		// Token: 0x04000006 RID: 6
		private static IDynamicValue caloriesBurn = new ConstantValue(25f);
	}
}
