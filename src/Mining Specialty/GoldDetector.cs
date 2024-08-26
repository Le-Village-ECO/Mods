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
using System.Linq;

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
                    new(typeof(IronBarItem), 4, typeof(BlacksmithSkill)),
                    new(typeof(LeatherHideItem), 2, typeof(BlacksmithSkill)),
                    new("WoodBoard", 4, typeof(BlacksmithSkill)), //noloc
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<GoldDetectorItem>()
                });

            Recipes = new List<Recipe> { recipe };
            ExperienceOnCraft = 0.5f;
            LaborInCalories = CreateLaborInCaloriesValue(250, typeof(BlacksmithSkill));
            CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(GoldDetectorItem), start: 0.5f, skillType: typeof(BlacksmithSkill));

            ModsPreInitialize();
            Initialize(displayText: Localizer.DoStr("Gold Detector Item"), recipeType: typeof(IronRockDrillRecipe));
            ModsPostInitialize();

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
        private HashSet<Type> oreTypes = new() { typeof(GoldBarStacked1Block), typeof(GoldBarStacked2Block), typeof(GoldBarStacked3Block), typeof(GoldBarStacked4Block) };
        public override HashSet<Type> OreTypes => oreTypes;

        //Calories
        private static IDynamicValue caloriesBurn = new MultiDynamicValue(
            MultiDynamicOps.Multiply,
            new TalentModifiedValue(typeof(GoldDetectorItem), typeof(MiningToolEfficiencyTalent)),
            CreateCalorieValue(15, typeof(MiningSkill), typeof(GoldDetectorItem)));
        public override IDynamicValue CaloriesBurn => caloriesBurn;

        //Tier
        public override IDynamicValue Tier => new ConstantValue(1);
    }
}