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
    [LocDisplayName("Parchemin d'oubli : Boucherie")]
    [LocDescription("Lire ce parchemin vous fera oublier la spécialité Boucherie sous certaines conditions")]
    public partial class ButcheryUnSkillScroll : UnSkillScroll<ButcherySkill> { }

    [RequiresSkill(typeof(ButcherySkill), 7)]
    [Ecopedia("Professions", "Hunter", subPageName: "Butchery UnSkill Scroll Item")]
    public partial class ButcheryUnSkillScrollRecipe : RecipeFamily
    {
        public ButcheryUnSkillScrollRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Butchery",  //noloc
                displayName: Localizer.DoStr("Butchery UnSkill Scroll"),

                ingredients: new List<IngredientElement>
                {
                    new (typeof(PaperItem), 1, true),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<ButcheryUnSkillScroll>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.LaborInCalories = CreateLaborInCaloriesValue(1000);
            this.CraftMinutes = CreateCraftTimeValue(15);


            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Butchery UnSkill Scroll"), recipeType: typeof(ButcheryUnSkillScrollRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ResearchTableObject), recipe: this);
        }
        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }
}