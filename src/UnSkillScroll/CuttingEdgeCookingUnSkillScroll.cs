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
    [LocDisplayName("Parchemin d'oubli : Cuisine gastronomique")]
    [LocDescription("Lire ce parchemin vous fera oublier la spécialité Cuisine gastronomique sous certaines conditions")]
    public partial class CuttingEdgeCookingUnSkillScroll : UnSkillScroll<CuttingEdgeCookingSkill> { }

    [RequiresSkill(typeof(CuttingEdgeCookingSkill), 7)]
    [Ecopedia("Professions", "Chef", subPageName: "Cutting Edge Cooking UnSkill Scroll Item")]
    public partial class CuttingEdgeCookingUnSkillScrollRecipe : RecipeFamily
    {
        public CuttingEdgeCookingUnSkillScrollRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Cutting Edge Cooking",  //noloc
                displayName: Localizer.DoStr("Cutting Edge Cooking UnSkill Scroll"),

                ingredients: new List<IngredientElement>
                {
                    new (typeof(PaperItem), 1, true),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<CuttingEdgeCookingUnSkillScroll>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.LaborInCalories = CreateLaborInCaloriesValue(1000);
            this.CraftMinutes = CreateCraftTimeValue(15);


            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Cutting Edge Cooking UnSkill Scroll"), recipeType: typeof(CuttingEdgeCookingUnSkillScrollRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ResearchTableObject), recipe: this);
        }
        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }
}