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
    [LocDisplayName("Parchemin d'oubli : Electroniques")]
    [LocDescription("Lire ce parchemin vous fera oublier la spécialité Electroniques sous certaines conditions : /unskill conditions")]
    public partial class ElectronicsUnSkillScroll : UnSkillScroll<ElectronicsSkill> { }

    [RequiresSkill(typeof(ElectronicsSkill), 7)]
    [Ecopedia("Professions", "Engineer", subPageName: "Electronics UnSkill Scroll Item")]
    public partial class ElectronicsUnSkillScrollRecipe : RecipeFamily
    {
        public ElectronicsUnSkillScrollRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Electronics",  //noloc
                displayName: Localizer.DoStr("Electronics UnSkill Scroll"),

                ingredients: new List<IngredientElement>
                {
                    new (typeof(PaperItem), 1, true),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<ElectronicsUnSkillScroll>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.LaborInCalories = CreateLaborInCaloriesValue(1000);
            this.CraftMinutes = CreateCraftTimeValue(15);


            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Electronics UnSkill Scroll"), recipeType: typeof(ElectronicsUnSkillScrollRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ResearchTableObject), recipe: this);
        }
        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }
}