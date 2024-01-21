//Mod le Village le kit de réparation remplace le système de réparation vanille du jeu pour le rendre plus polyvalent
//Kit de réparation en pierre - Toutes spécialités - Pour les outils en pierre

namespace Eco.Mods.TechTree
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Skills;
    using Eco.Shared.Localization;
    using Eco.Shared.Serialization;
    using Eco.Core.Items;
    using Eco.Gameplay.Items.Recipes;

    public partial class StoneRepairKitRecipe : RecipeFamily
    {
        public StoneRepairKitRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Kit de réparation en pierre",  //noloc
                displayName: Localizer.DoStr("Kit de réparation en pierre"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("Wood", 5),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<StoneRepairKitItem>(1),
                });
            this.Recipes = new List<Recipe> { recipe };

            this.ExperienceOnCraft = 1;
            this.LaborInCalories = CreateLaborInCaloriesValue(10);
            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Kit de réparation en pierre"), recipeType: typeof(StoneRepairKitRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ToolBenchObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Kit de réparation en pierre")]
    [Weight(1000)]  //Défini le poids.
    [Category("Tool")]  //??? Category Tool ou RepairKit ???
    [Tag("RepairKit")]
    [Ecopedia("Items", "Tools", createAsSubPage: true)]  //??? Tools ou RepairKit ???
    [LocDescription("Un kit de réparation pour les outils de l'âge de pierre. Pierre ? Présent ! Pierre ? Présent !")]  //Description détaillée.
    public partial class StoneRepairKitItem : Item
    {

    }
}