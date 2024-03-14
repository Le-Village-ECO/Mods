//Mod le Village le kit de réparation remplace le système de réparation vanille du jeu pour le rendre plus polyvalent
//Kit de réparation en bois - Toutes spécialités - Pour les outils en bois

using Eco.Core.Items;
using Eco.Gameplay.Components;
using Eco.Gameplay.Items;
using Eco.Gameplay.Items.Recipes;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using System.Collections.Generic;
using System.ComponentModel;

namespace Eco.Mods.TechTree
{
    public partial class WoodRepairKitRecipe : RecipeFamily
    {
        public WoodRepairKitRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Kit de réparation en bois",  //noloc
                displayName: Localizer.DoStr("Kit de réparation en bois"),

                ingredients: new List<IngredientElement>
                {
                    new("Wood", 5),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<WoodRepairKitItem>(1),
                });
            this.Recipes = new List<Recipe> { recipe };

            this.ExperienceOnCraft = 1;
            this.LaborInCalories = CreateLaborInCaloriesValue(10);
            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Kit de réparation en bois"), recipeType: typeof(WoodRepairKitRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ToolBenchObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Kit de réparation en bois")]
    [Weight(1000)] //Défini le poids.
    [Category("Tool")]
    [Tag("RepairKit")]
    [Ecopedia("Items", "Tools", createAsSubPage: true)]  //Page ECOpedia
    [LocDescription("Un kit de réparation pour tous les outils en bois. Fais à partir de séquoia géant, et toc !")] //Description détaillée.
    public partial class WoodRepairKitItem : Item
    {

    }
}