using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Shared.Utils;
using Eco.Simulation.WorldLayers.Layers;
using Eco.Simulation.WorldLayers;
using Eco.WorldGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eco.Gameplay.Interactions.Interactors;
using Eco.Gameplay.Items;
using Eco.Gameplay.DynamicValues;
using Eco.Shared.SharedTypes;
using Eco.Core.Items;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using System.ComponentModel;
using Eco.Gameplay.Plants;
using Eco.Gameplay.Players;
using Eco.Mods.Organisms;
using Eco.Shared.Math;
using Eco.Simulation;
using Eco.Shared.Voxel;
using Eco.Mods.TechTree;
using Eco.Gameplay.Components;
using Eco.Gameplay.Items.Recipes;
using System.Runtime.CompilerServices;

namespace Village.Eco.Mods.OilField
{

    // Definition de l'outil
    [Serialized]
    [LocDisplayName("Oil Prospector")]
    [LocDescription("Analyse le sol pour savoir combien de pétrole peut y être extrait")]
    [Category("Tools")]
    [Tag("Tool")]
    [Ecopedia("Items", "Tools", createAsSubPage: true)]
    [Weight(1000)]
    public class OilProspectorItem : ToolItem, IInteractor
    {
        public override float DurabilityRate { get { return 0; } }

        private static IDynamicValue skilledRepairCost = new ConstantValue(4);
        public override IDynamicValue SkilledRepairCost { get { return skilledRepairCost; } }

        [Interaction(InteractionTrigger.LeftClick,"Prospecter le sol")]
        public bool Prospect(Player player, InteractionTriggerInfo triggerInfo, InteractionTarget target)
        {
            if (target.IsBlock && base.Durability > 0f)
            {
                var samplePosition = Vector3i.Zero;
                samplePosition = target.BlockPosition.Value + Vector3i.Up;
                samplePosition = World.GetWrappedWorldPosition(samplePosition); //Convert position into wrapped world coords.

                samplePosition = World.GetWrappedWorldPosition(target.BlockPosition.Value);

                var title = new LocStringBuilder();
                var text = new LocStringBuilder();
                var button = new LocStringBuilder();

                string format = "1x {0} peut être extrait en {1}";
                object[] array = new object[2];
                Item item = Item.Get(typeof(PetroleumItem));
                array[0] = item.MarkedUpName;
                array[1] = Text.Num(OilProspectorItem.GetCraftMinutes(player.User, target.BlockPosition.Value));

                title.Append($"{this.DisplayName} {target.BlockPosition}");
                text.AppendLine(TextLoc.HeaderLocStr("Pétrole en sous-sol")).AppendLine(1);
                text.Append($"Dans un rayon de {PumpJackObject.Radius} cases, ");
                text.Append($"il y a une quantite de : {Text.Num(OilProspectorItem.GetOilAmount(target.BlockPosition.Value.XZ))}.").AppendLine(1);
                text.AppendLine();
                text.AppendLine(TextLoc.HeaderLocStr("Production potentielle")).AppendLine(1);
                text.Append(FormattableStringFactory.Create(format, array));

                button.Append($"Intéressant !");

                //player.OpenInfoPanel(title.ToString(), text.ToString(), "soilsampler");
                player.LargeInfoBox(title.ToLocString(), text.ToLocString(), button.ToLocString());

                this.BurnCaloriesNow(player);
                return true;
            }
            return false;
        }

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

        private static float GetOilAmount(Vector2i pos)
        {
            WorldLayer layer = Singleton<WorldLayerManager>.Obj.GetLayer("Oilfield");
            float value = 0f;
            float total = 0f;
            if (PumpJackObject.Radius > 0f)
            {
                layer.ForRadius(layer.WorldPosToLayerPos(pos), PumpJackObject.Radius, delegate (Vector2i x, float val)
                {
                    value += val;
                    //float total = total;
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
    }
}
