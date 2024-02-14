// Le Village
// Boisson énergisante de 15min avec recette de fabrication
//TODO : Revoir la recette de fabrication
//TODO : Incorporer un contrôle pour ne permettre cette boisson que 1 fois par jour.

using Eco.Core.Items;
using Eco.Gameplay.Components;
using Eco.Gameplay.Items;
using Eco.Gameplay.Items.Recipes;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using System.Collections.Generic;
using Eco.Mods.TechTree;
using System.ComponentModel;

namespace Village.Eco.Mods.ExhaustionMod
{
    [Serialized]
    [LocDisplayName("Coffee Boost")]
    [LocDescription("Un petit café pour 15 min. d'énergie : Meilleur rapport qualité/prix !")]  //Description détaillée.
    //[Weight(10000)]  //Défini le poids.
    [Category("Food")]
    [Tag("Boost")]
    [Ecopedia("Food", "Boost", createAsSubPage: true)]  //Page ECOpedia
    public partial class LVBCoffeeItem : ExhaustionBoost
    {
        public override float BoostTime => 0.25f; //15min de boost
        public override bool CheckDate => true; //Vérifie consommation 1 fois par jour
    }

    //[Ecopedia("Food", "Coffee Boost", subPageName: "Coffee Boost Item")]
    [Ecopedia("Food", "Boost", createAsSubPage: true)]
    public partial class CoffeeBoostRecipe : RecipeFamily
    {
        public CoffeeBoostRecipe() 
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "CoffeeBoost",  //noloc
                displayName: Localizer.DoStr("Coffee Boost"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new(typeof(PaperItem), 1, true),
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<LVBCoffeItem>(1)
                });
            this.Recipes = new List<Recipe> { recipe };

            // Defines how much experience is gained when crafted.
            this.ExperienceOnCraft = 0.5f;

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(15);

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Coffee Boost"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Coffee Boost"), recipeType: typeof(CoffeeBoostRecipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(CampfireObject), recipe: this);
        }
        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }
}
