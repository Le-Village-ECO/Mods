namespace Eco.Mods.TechTree
{
    using System.Collections.Generic;
    using Eco.Core.Items;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Items.Recipes;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Skills;
    using Eco.Shared.Localization;
    using Eco.Shared.Serialization;
    using Eco.Shared.Utils;



    [Serialized] // Tells the save/load system this object needs to be serialized. 
    [LocDisplayName("Biere")] // Defines the localized name of the item.
    [Weight(500)] // Defines how heavy the BakedCorn is.
    [Tag("BakedFood")]
    [Ecopedia("Food", "Alcool", createAsSubPage: true)]
    [LocDescription("Bouteille d'alcool concentré, je sens que j'ai une chance sur cinq de finir dans les vapes")] //The tooltip description for the food item.
    public partial class BiereItem : FoodItem
    {

        public override LocString DisplayNamePlural => Localizer.DoStr("Biere");

        public override float Calories => 1200;
        public override Nutrients Nutrition => new Nutrients() { Carbs = 5, Fat = 5, Protein = 25, Vitamins = 4 };

        protected override float BaseShelfLife => (float)TimeUtil.HoursToSeconds(72);


    }


    [RequiresSkill(typeof(MillingSkill), 1)]
    [Ecopedia("Food", "Milling", subPageName: "Biere Item")]
    public partial class BiereRecipe : RecipeFamily
    {
        public BiereRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Biere",  //noloc
                displayName: Localizer.DoStr("Biere"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(WheatItem), 15, typeof(BakingSkill), typeof(BakingLavishResourcesTalent)),
                    new IngredientElement(typeof(YeastItem), 3)
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<BiereItem>(1)
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 1; // Defines how much experience is gained when crafted.

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(25, typeof(MillingSkill));

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(BiereRecipe), start: 2, skillType: typeof(MillingSkill), typeof(MillingFocusedSpeedTalent), typeof(MillingParallelSpeedTalent));

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Baked Corn"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Biere"), recipeType: typeof(BiereRecipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(MillObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }
}