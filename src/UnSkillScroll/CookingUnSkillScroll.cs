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
    [LocDisplayName("Parchemin d'oubli : Cuisine")]
    [LocDescription("Lire ce parchemin vous fera oublier la spécialité Cuisine sous certaines conditions")]
    public partial class CookingUnSkillScroll : UnSkillScroll<CookingSkill> { }

    [RequiresSkill(typeof(CookingSkill), 7)]
    [Ecopedia("Professions", "Chef", subPageName: "Cooking UnSkill Scroll Item")]
    public partial class CookingUnSkillScrollRecipe : RecipeFamily
    {
        public CookingUnSkillScrollRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Cooking",  //noloc
                displayName: Localizer.DoStr("Cooking UnSkill Scroll"),

                ingredients: new List<IngredientElement>
                {
                    new (typeof(PaperItem), 1, true),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<CookingUnSkillScroll>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.LaborInCalories = CreateLaborInCaloriesValue(1000);
            this.CraftMinutes = CreateCraftTimeValue(15);


            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Cooking UnSkill Scroll"), recipeType: typeof(CookingUnSkillScrollRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ResearchTableObject), recipe: this);
        }
        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }
}