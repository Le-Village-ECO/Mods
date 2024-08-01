using Eco.Core.Items;
using Eco.Gameplay.DynamicValues;
using Eco.Gameplay.Interactions.Interactors;
using Eco.Gameplay.Items;
using Eco.Gameplay.Players;
using Eco.Shared.Items;
using Eco.Shared.Localization;
using Eco.Shared.Math;
using Eco.Shared.Serialization;
using Eco.Shared.SharedTypes;
using Eco.Shared.Utils;
using Eco.World.Blocks;
using System.ComponentModel;
using System.Numerics;
using System.Collections.Generic;
using System;

namespace Eco.Mods.ERC.PGA.Items
//namespace Village.Eco.Mods.MiningSpecialty
{
	[Serialized]
	[LocDisplayName("Ore detector")]
	[LocDescription("Detector tool which can spot ore proximity")]
	[Category("Tools"), Tag("Tool"), Weight(1000)]
	public class OreDetectorItem : ToolItem, IInteractor
	{
		public const int SCAN_RANGE = 30;
		public override ItemCategory ItemCategory => ItemCategory.Drill;
        public override float DurabilityRate { get { return 0; } }
        public override IDynamicValue CaloriesBurn => caloriesBurn;
		private static IDynamicValue caloriesBurn = new ConstantValue(25f);

		[Interaction(InteractionTrigger.InteractKey, overrideDescription: "Analyze area", animationDriven: false, interactionDistance: 3, authRequired: AccessType.ConsumerAccess)]
		public void Analyze(Player player, InteractionTriggerInfo triggerInfo, InteractionTarget target)
		{
            if (target.IsBlock && this.Durability > 0f)
            {
				Vector3i? targetPos = target.BlockPosition.Value + (Vector3i)target.HitNormal;
				WorldRange range = WorldRange.SurroundingSpace(targetPos.Value, SCAN_RANGE);
				HashSet<Type> oreTypes = ItemUtils.GetItemsByTag("Ore");
				Dictionary<Type, int> ores = new Dictionary<Type, int>();
				foreach (Vector3i pos in range.XYZIterInc())
				{
					Block block = World.World.GetBlock(pos);
					if (block is null or EmptyBlock) continue;
					Type creatingItemType = block is IRepresentsItem representsItem ? representsItem.RepresentedItemType : BlockItem.CreatingItem(block.GetType())?.GetType();
					if (creatingItemType is not null && oreTypes.Contains(creatingItemType))
					{
						int distanceToBlock = (int) Math.Round(Vector3.Distance(targetPos.Value, pos));
						if (distanceToBlock > SCAN_RANGE) continue;
						if (ores.TryGetValue(creatingItemType, out int dst))
						{
							if(distanceToBlock < dst)
							{
								ores[creatingItemType] = distanceToBlock;
							}
						}
						else
						{
							ores.Add(creatingItemType, distanceToBlock);
						}
					}
				}

				//Now we have list of ore type at it's closest position and can display to player how we want
				LocStringBuilder text = new LocStringBuilder();

				foreach(KeyValuePair<Type, int> oreAtDst in ores)
				{
					int dst = oreAtDst.Value;
                    string proximityString = dst switch
                    {
                        <= 1 => "In front of you",
                        <= 2 => "So close",
                        <= 4 => "Getting close",
                        <= 7 => "Lukewarm",
                        <= 10 => "Cold",
                        <= 15 => "Colder",
                        <= SCAN_RANGE => "Very cold",
                        > SCAN_RANGE => "Out of range",
                    };
					text.AppendLineLoc($"{Get(oreAtDst.Key)?.MarkedUpName} : {proximityString} (distance {dst})");
                }
				// Display info to player
				player.LargeInfoBox(Localizer.Do($"{DisplayName} {target.BlockPosition}"), text.ToLocString());
				return;
			}
		}
    }
}
