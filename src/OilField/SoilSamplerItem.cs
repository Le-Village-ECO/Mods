// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.
namespace Eco.Mods.TechTree
{
    using System.ComponentModel;
    using Eco.Core.Items;
    using Eco.Gameplay.Interactions;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Plants;
    using Eco.Shared.Math;
    using Eco.Simulation;
    using Eco.Shared.Serialization;
    using Eco.Simulation.WorldLayers;
    using System.Text;
    using Eco.Gameplay.DynamicValues;
    using Eco.Shared.Localization;
    using Eco.Gameplay.Interactions.Interactors;
    using Eco.Gameplay.Players;
    using Eco.Shared.SharedTypes;
    using Eco.Shared.Networking;
    using System.Collections.Generic;
    using Eco.Mods.Organisms;
    using Eco.Shared.Voxel;

    [Serialized]
    [LocDisplayName("Soil Sampler")]
    [LocDescription("Beaker and measuring tools for detecting the factors influencing plants in the environment.")]
    [Category("Tools")]
    [Tag("Tool")]
    [Ecopedia("Items", "Tools", createAsSubPage: true)]
    [Weight(1000)]
    public class SoilSamplerItem : ToolItem, IInteractor
    {
        public override float DurabilityRate { get { return 0; } }

        private static IDynamicValue skilledRepairCost = new ConstantValue(4);
        public override IDynamicValue SkilledRepairCost { get { return skilledRepairCost; } }

        [Interaction(InteractionTrigger.LeftClick, tags: new string[] { BlockTags.Samplable, BlockTags.Clearable, BlockTags.Choppable }, DisallowedEnvVars = new[] { "felled" })]
        public bool SampleSoil(Player player, InteractionTriggerInfo triggerInfo, InteractionTarget target)
        {
            if (target.IsBlock || target.NetObj is TreeEntity)
            {
                var samplePosition = Vector3i.Zero;
                if (target.Block() is PlantBlock)          samplePosition = target.BlockPosition.Value; //Allow select plants directly.
                else if (target.NetObj is TreeEntity tree) samplePosition = tree.Position.XYZi();       //Allow select trees directly.
                else                                       samplePosition = target.BlockPosition.Value + Vector3i.Up;

                samplePosition = World.GetWrappedWorldPosition(samplePosition); //Convert position into wrapped world coords.

                var plant = EcoSim.PlantSim.GetPlant(samplePosition);
                var title = new LocStringBuilder();
                var text  = new LocStringBuilder();
                if (plant != null)
                {
                    title.Append($"{plant.Species.DisplayName} {samplePosition}");
                    text.Append(plant.GetEcosystemInfo() + "\n" + text);
                }
                else
                {
                    samplePosition = World.GetWrappedWorldPosition(target.BlockPosition.Value); //If there is no plant then just use the block position.
                    title.Append($"{target.Block().GetType().Name} {samplePosition}");
                }

                text.AppendLine(WorldLayerManager.Obj.DescribePos(samplePosition.XZ));
                player.OpenInfoPanel(title.ToString(), text.ToString(), "soilsampler");

                this.BurnCaloriesNow(player);
                return true;
            }
            return false;
        }
    }
}
