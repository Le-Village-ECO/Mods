// Le village - Ore detector - Sur le principe du chaud-froid
// TODO:
// - En faire un outil générique qui est appelé par des outils spécifiques en fonction du minerai : il reste de passer le paramètre du type de minerai
// - Améliorer le contrôle sur le talent : Tooltip sur l'outil ? Affichage message bloquant ? Utiliser un attribut de l'item ?
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
using Village.Eco.Mods.Core;
using System.Diagnostics;

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
                                          //public override ItemCategory ItemCategory => ItemCategory.Drill;
                                          //public abstract Block Block { get; }  //Get the Ore from item

        public abstract HashSet<Type> OreTypes { get; }

        // Calories burn
        private static SkillModifiedValue caloriesBurn = CreateCalorieValue(20, typeof(MiningSkill), typeof(OreDetectorItem));
        public override IDynamicValue CaloriesBurn => caloriesBurn;

        // Item tier level
        static IDynamicValue tier = new ConstantValue(0);
        public override IDynamicValue Tier { get { return tier; } }

        // Repair - TODO
        //public override float DurabilityRate { get { return 0; } }
        /*
        private static IDynamicValue skilledRepairCost = new ConstantValue(1);
        public override IDynamicValue SkilledRepairCost { get { return skilledRepairCost; } }
        public override int FullRepairAmount { get { return 1; } }
		*/

        [Interaction(InteractionTrigger.InteractKey, overrideDescription: "Analyze area", animationDriven: false, interactionDistance: 3, authRequired: AccessType.ConsumerAccess)]
        public void Analyze(Player player, InteractionTriggerInfo triggerInfo, InteractionTarget target)
        {
            // Controle du talent du joueur
            if (player.User.Talentset.HasTalent<MiningGoldRusherTalent>() is false)
            {
                player.MsgLocStr($"Talent {TextLoc.BoldLocStr("Chercheur d'or : Minage")} requis pour utiliser L'objet", NotificationStyle.Error);
                return;
            }

            if (target.IsBlock && Durability > 0f)
            {
                var perfCounter = Stopwatch.StartNew();
                Vector3i? targetPos = target.BlockPosition.Value + (Vector3i)target.HitNormal;
                WorldRange range = WorldRange.SurroundingSpace(targetPos.Value, SCAN_RANGE);
                Dictionary<Type, int> ores = new();
                foreach (Vector3i pos in range.XYZIterInc())
                {
                    Block block = World.GetBlock(pos);
                    if (block is null or EmptyBlock) continue;

                    Type creatingItemType = block is IRepresentsItem representsItem
                        ? representsItem.RepresentedItemType : BlockItem.CreatingItem(block.GetType())?.GetType();
                    if (creatingItemType is not null && OreTypes.Contains(creatingItemType))
                    {
                        int distanceToBlock = (int)Math.Round(Vector3.Distance(targetPos.Value, pos));
                        if (distanceToBlock > SCAN_RANGE) continue;

                        if (ores.TryGetValue(creatingItemType, out int dst) is false) ores.Add(creatingItemType, distanceToBlock);
                        else if (distanceToBlock < dst) ores[creatingItemType] = distanceToBlock;
                    }
                }
                perfCounter.Stop();
                Logger.SendLog(Criticity.Info, "MiningSpeciality", $"Ore scanning took {perfCounter.ElapsedMilliseconds}ms");

                //Calories consumption
                //this.BurnCaloriesNow(player);

                DisplayMessage(player, ores);

                return;
            }

            if (Durability <= 0f)
            {
                player.ErrorLoc($"L'outil est cassé. Il faut le réparer.");
                return;
            }
        }

        public abstract void DisplayMessage(Player player, Dictionary<Type, int> ores);
    }
}