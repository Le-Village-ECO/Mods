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
    [LocDisplayName("Parchemin d'oubli : Maçonnerie avancée")]
    [LocDescription("Lire ce parchemin vous fera oublier la spécialité Maçonnerie avancée sous certaines conditions")]
    public partial class AdvancedMasonryUnSkillScroll : UnSkillScroll<AdvancedMasonrySkill> { }

    [RequiresSkill(typeof(AdvancedMasonrySkill), 7)]
    [Ecopedia("Professions", "Mason", subPageName: "Advanced Masonry UnSkill Scroll Item")]
    public partial class AdvancedMasonryUnSkillScrollRecipe : RecipeFamily
    {
        public AdvancedMasonryUnSkillScrollRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Advanced Masonry",  //noloc
                displayName: Localizer.DoStr("Advanced Masonry UnSkill Scroll"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(PaperItem), 1, true),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<AdvancedMasonryUnSkillScroll>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.LaborInCalories = CreateLaborInCaloriesValue(1000);
            this.CraftMinutes = CreateCraftTimeValue(15);


            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Advanced Masonry UnSkill Scroll"), recipeType: typeof(AdvancedMasonryUnSkillScrollRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ResearchTableObject), recipe: this);
        }
        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }
}