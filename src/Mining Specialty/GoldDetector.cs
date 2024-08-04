using Eco.Core.Items;
using Eco.Gameplay.Components;
using Eco.Gameplay.DynamicValues;
using Eco.Gameplay.Interactions.Interactors;
using Eco.Gameplay.Items;
using Eco.Gameplay.Items.Recipes;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace Village.Eco.Mods.MiningSpecialty
{
    [RequiresModule(typeof(AnvilObject))]
    [RequiresSkill(typeof(BlacksmithSkill), 1)]
    [Ecopedia("Items", "Tools", subPageName: "Gold Detector Item")]
    public partial class GoldDetectorRecipe : RecipeFamily
    {
        public GoldDetectorRecipe() 
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "GoldDetector",  //noloc
                displayName: Localizer.DoStr("Gold Detector"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronBarItem), 4, typeof(BlacksmithSkill)),
                    new IngredientElement(typeof(LeatherHideItem), 2, typeof(BlacksmithSkill)),
                    new IngredientElement("WoodBoard", 4, typeof(BlacksmithSkill)), //noloc
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<GoldDetectorItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            
            this.ExperienceOnCraft = 0.5f;
            this.LaborInCalories = CreateLaborInCaloriesValue(250, typeof(BlacksmithSkill));
            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(GoldDetectorItem), start: 0.5f, skillType: typeof(BlacksmithSkill));

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Gold Detector Item"), recipeType: typeof(IronRockDrillRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(GrindstoneObject), recipe: this);
        }
        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Gold Detector")]
    [LocDescription("Indique la proximité de filon d'or (chaud-froid)")]
    [Category("Tools"), Tag("Tool"), Weight(1000)]
    [Tier(1)]
    [Ecopedia("Items", "Tools", createAsSubPage: true)]
    public partial class GoldDetectorItem : OreDetectorItem, IInteractor
    {
        //Calories
        private static IDynamicValue caloriesBurn = new MultiDynamicValue(
            MultiDynamicOps.Multiply, 
            new TalentModifiedValue(typeof(GoldDetectorItem), typeof(MiningToolEfficiencyTalent)), 
            CreateCalorieValue(15, typeof(MiningSkill), typeof(GoldDetectorItem)));
        public override IDynamicValue CaloriesBurn => caloriesBurn;
        //Tier
        private static IDynamicValue tier = new ConstantValue(1);


    }
}
