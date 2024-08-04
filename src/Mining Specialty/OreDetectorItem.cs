// Le village - Ore detector - Sur le principe du chaud-froid
// TODO:
// - En faire un outil générique qui est appelé par des outils spécifiques en fonction du minerai : il reste de passer le paramètre du type de minerai
// - Ajouter un contrôle optionnel en fonction d'un talent qui l'active
// - Finaliser la gestion du coût calorique (override possible par l'outil final)
// - finaliser la gestion de la durabilité (override possible par l'outil final)

using Eco.Core.Items;
using Eco.Gameplay.DynamicValues;
using Eco.Gameplay.Interactions.Interactors;
using Eco.Gameplay.Items;
using Eco.Gameplay.Players;
using Eco.Mods.TechTree;
using Eco.Shared.Items;
using Eco.Shared.Localization;
using Eco.Shared.Math;
using Eco.Shared.Serialization;
using Eco.Shared.SharedTypes;
using Eco.World;
using Eco.World.Blocks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using Eco.Shared.Services;

namespace Village.Eco.Mods.MiningSpecialty
{
    [Serialized]
	[LocDisplayName("Ore detector")]
	[LocDescription("Detector tool which can spot ore proximity")]
    [Weight(0)]
    [Ecopedia("Items", "Tools")]
    [Category("Hidden")]
	public abstract partial class OreDetectorItem : ToolItem, IInteractor
	{
		public const int SCAN_RANGE = 30; //Max range for detection
		public override ItemCategory ItemCategory => ItemCategory.Drill;
        public override float DurabilityRate { get { return 0; } }

		// Calories burn
        private static SkillModifiedValue caloriesBurn = CreateCalorieValue(20, typeof(MiningSkill), typeof(OreDetectorItem));
        public override IDynamicValue CaloriesBurn => caloriesBurn;

        // Item tier level
        static IDynamicValue tier = new ConstantValue(0);
        public override IDynamicValue Tier { get { return tier; } }

        // Repair - TODO
        /*
        private static IDynamicValue skilledRepairCost = new ConstantValue(1);
        public override IDynamicValue SkilledRepairCost { get { return skilledRepairCost; } }
        public override int FullRepairAmount { get { return 1; } }
		*/

        static OreDetectorItem() { }

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
					Block block = World.GetBlock(pos);
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
                        <= 1 => "Bouillant", //"In front of you"
                        <= 2 => "Très chaud", //"So close"
                        <= 4 => "Chaud", //"Getting close"
                        <= 7 => "Tiède", //"Lukewarm"
                        <= 10 => "Froid", //"Cold"
                        <= 15 => "Très froid", //"Colder"
                        <= SCAN_RANGE => "Glacial", //"Very cold"
                        > SCAN_RANGE => "Hors de portée", //"Out of range"
                    };
					text.AppendLineLoc($"{Get(oreAtDst.Key)?.MarkedUpName} : {proximityString} (distance {dst})");
                }
                // Display info to player
                //player.LargeInfoBox(Localizer.Do($"{DisplayName} {target.BlockPosition}"), text.ToLocString());
                player.MsgLocStr(text.ToLocString(), NotificationStyle.InfoBox);
                
				//Calories consumption
				//this.BurnCaloriesNow(player);

                return;
			}

            if (this.Durability <= 0f) 
			{ 
				player.ErrorLoc($"L'outil est cassé. Il faut le réparer.");

                //Calories consumption
                //this.BurnCaloriesNow(player);

				return;
            }
        }
    }
}
