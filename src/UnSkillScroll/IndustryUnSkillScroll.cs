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
    [LocDisplayName("Parchemin d'oubli : Industrie")]
    [LocDescription("Lire ce parchemin vous fera oublier la spécialité Industrie sous certaines conditions")]
    public partial class IndustryUnSkillScroll : UnSkillScroll<IndustrySkill> { }

    [RequiresSkill(typeof(IndustrySkill), 7)]
    [Ecopedia("Professions", "Engineer", subPageName: "Industry UnSkill Scroll Item")]
    public partial class IndustryUnSkillScrollRecipe : RecipeFamily
    {
        public IndustryUnSkillScrollRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Industry",  //noloc
                displayName: Localizer.DoStr("Industry UnSkill Scroll"),

                ingredients: new List<IngredientElement>
                {
                    new (typeof(PaperItem), 1, true),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<IndustryUnSkillScroll>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.LaborInCalories = CreateLaborInCaloriesValue(1000);
            this.CraftMinutes = CreateCraftTimeValue(15);


            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Industry UnSkill Scroll"), recipeType: typeof(IndustryUnSkillScrollRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ResearchTableObject), recipe: this);
        }
        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }
}