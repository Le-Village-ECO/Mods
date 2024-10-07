// Le Village

using Eco.Core.Items;
using Eco.Gameplay.Components;
using Eco.Gameplay.Items.Recipes;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using System.Collections.Generic;

namespace Village.Eco.Mods.UnSkillScroll
{
    [Serialized]
    [LocDisplayName("Parchemin d'oubli : Cuisine feu de camp")]
    [LocDescription("Lire ce parchemin vous fera oublier la spécialité Cuisine feu de camp sous certaines conditions : /unskill conditions")]
    public partial class CampfireCookingUnSkillScroll : UnSkillScroll<CampfireCookingSkill> { }

    [RequiresSkill(typeof(CampfireCookingSkill), 7)]
    [Ecopedia("Professions", "Chef", subPageName: "Campfire Cooking UnSkill Scroll Item")]
    public partial class CampfireCookingUnSkillScrollRecipe : RecipeFamily
    {
        public CampfireCookingUnSkillScrollRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Campfire Cooking",  //noloc
                displayName: Localizer.DoStr("Campfire Cooking UnSkill Scroll"),

                ingredients: new List<IngredientElement>
                {
                    new (typeof(PaperItem), 1, true),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<CampfireCookingUnSkillScroll>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.LaborInCalories = CreateLaborInCaloriesValue(1000);
            this.CraftMinutes = CreateCraftTimeValue(15);


            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Campfire Cooking UnSkill Scroll"), recipeType: typeof(CampfireCookingUnSkillScrollRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ResearchTableObject), recipe: this);
        }
        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }
}