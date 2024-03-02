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
    [LocDisplayName("Parchemin d'oubli : Couture")]
    [LocDescription("Lire ce parchemin vous fera oublier la spécialité Couture sous certaines conditions")]
    public partial class TailoringUnSkillScroll : UnSkillScroll<TailoringSkill> { }

    [RequiresSkill(typeof(TailoringSkill), 7)]
    [Ecopedia("Professions", "Tailleur", subPageName: "Tailoring UnSkill Scroll Item")]
    public partial class TailoringUnSkillScrollRecipe : RecipeFamily
    {
        public TailoringUnSkillScrollRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Tailoring",  //noloc
                displayName: Localizer.DoStr("Tailoring UnSkill Scroll"),

                ingredients: new List<IngredientElement>
                {
                    new (typeof(PaperItem), 1, true),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<TailoringUnSkillScroll>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.LaborInCalories = CreateLaborInCaloriesValue(1000);
            this.CraftMinutes = CreateCraftTimeValue(15);


            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Tailoring UnSkill Scroll"), recipeType: typeof(TailoringUnSkillScrollRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ResearchTableObject), recipe: this);
        }
        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }
}