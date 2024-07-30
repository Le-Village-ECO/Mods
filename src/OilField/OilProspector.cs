//Le Village - Détecteur de pétrole
//TODO - Revoir et comprendre GetOilAmount

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
using Eco.Gameplay.Skills;
using Village.Eco.Mods.Core;
using Eco.Gameplay.Systems.Messaging.Notifications;

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
    [RequiresTalent(typeof(LoggingToolEfficiencyTalent))]
    public class OilProspectorItem : ToolItem, IInteractor
    {
        public override float DurabilityRate { get { return 0; } }

        private static IDynamicValue skilledRepairCost = new ConstantValue(4);
        public override IDynamicValue SkilledRepairCost { get { return skilledRepairCost; } }

        [Interaction(InteractionTrigger.LeftClick,"Prospecter le sol")]
        public bool Prospect(Player player, InteractionTriggerInfo triggerInfo, InteractionTarget target)
        {
            // Le village - Controle du Tier de l'objet
            var requiredTier = this.Tier.GetCurrentValue(player?.User);
            var requiredTalent = player?.User.Talentset.HasTalent(typeof(LoggingToolEfficiencyTalent));
            if (requiredTier > 1 && requiredTalent is false)
            {
                NotificationManager.ServerMessageToAllLoc($"Objet Tier = {requiredTier} / Talent = {requiredTalent}");
                return false;
            }


            if (target.IsBlock && base.Durability > 0f)
            {
                var title = new LocStringBuilder();
                var text = new LocStringBuilder();
                var button = new LocStringBuilder();

                //Affichage du titre de la popup
                title.Append($"{this.DisplayName} {target.BlockPosition}");

                //Affichage du contenu de la popup - quantité de pétrole
                string text1 = "Dans un rayon de {0} cases, il y a une quantite de {1}";
                object[] array1 = new object[2];
                array1[0] = PumpJackObject.Radius;
                array1[1] = Text.Num(OilProspectorItem.GetOilAmount(target.BlockPosition.Value.XZ));
                text.AppendLine(TextLoc.HeaderLocStr("Pétrole en sous-sol")).AppendLine(1);
                text.Append(FormattableStringFactory.Create(text1, array1)).AppendLine(2);

                //Affichage du contenu de la popup - vitesse de production
                string text2 = "1x {0} peut être extrait en {1} minutes";
                object[] array2 = new object[2];
                Item item = Item.Get(typeof(PetroleumItem));
                array2[0] = item.MarkedUpName;
                array2[1] = Text.Num(OilProspectorItem.GetCraftMinutes(player.User, target.BlockPosition.Value));
                text.AppendLine(TextLoc.HeaderLocStr("Production potentielle")).AppendLine(1);
                text.Append(FormattableStringFactory.Create(text2, array2)).AppendLine(2);

                //Affichage du bouton de la popup
                button.Append($"Intéressant !");

                //Affichage de la popup
                player.LargeInfoBox(title.ToLocString(), text.ToLocString(), button.ToLocString());

                //Potentiellement ajouter un coût calorique
                this.BurnCaloriesNow(player);

                return true;
            }
            return false;
        }

        private static float GetCraftMinutes(User user, Vector3i pos)
        {
            RecipeFamily recipeFamily = CraftingComponent.RecipesOnWorldObject(typeof(PumpJackObject)).FirstOrDefault((RecipeFamily rf) => rf.RecipeItems.Any((Type ri) => ri == typeof(PetroleumItem)));
            if ((recipeFamily == null) || (!(recipeFamily.CraftMinutes is MultiDynamicValue))) return 0f;
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
                    //float total = total;  //???
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

    [RequiresSkill(typeof(GatheringSkill), 7)]
    public partial class OilProspectorRecipe : RecipeFamily 
    {
        public OilProspectorRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "OilProspector",
                displayName: Localizer.DoStr("Oil Prospector"),
                ingredients: new List<IngredientElement>
                {
                    new(typeof(IronBarItem), 2, true),
                    new("WoodBoard", 2, true)
                },
                new List<CraftingElement>
                {
                    new CraftingElement<OilProspectorItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 10f;
            this.LaborInCalories = CreateLaborInCaloriesValue(50f, typeof(GatheringSkill));
            this.CraftMinutes = CreateCraftTimeValue(3f);
            this.Initialize(displayText: Localizer.DoStr("Oil Prospector"), recipeType: typeof(OilProspectorRecipe));
            CraftingComponent.AddRecipe(tableType: typeof(ToolBenchObject), recipe: this);
        }
    }
}
