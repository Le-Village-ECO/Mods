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
    [LocDisplayName("Parchemin d'oubli : Maçonnerie")]
    [LocDescription("Lire ce parchemin vous fera oublier la spécialité Maçonnerie sous certaines conditions : /unskill conditions")]
    public partial class MasonryUnSkillScroll : UnSkillScroll<MasonrySkill> { }

    [RequiresSkill(typeof(MasonrySkill), 7)]
    [Ecopedia("Professions", "Mason", subPageName: "Masonry UnSkill Scroll Item")]
    public partial class MasonryUnSkillScrollRecipe : RecipeFamily
    {
        public MasonryUnSkillScrollRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Masonry",  //noloc
                displayName: Localizer.DoStr("Masonry UnSkill Scroll"),

                ingredients: new List<IngredientElement>
                {
                    new (typeof(PaperItem), 1, true),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<MasonryUnSkillScroll>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.LaborInCalories = CreateLaborInCaloriesValue(1000);
            this.CraftMinutes = CreateCraftTimeValue(15);


            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Masonry UnSkill Scroll"), recipeType: typeof(MasonryUnSkillScrollRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ResearchTableObject), recipe: this);
        }
        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }
}