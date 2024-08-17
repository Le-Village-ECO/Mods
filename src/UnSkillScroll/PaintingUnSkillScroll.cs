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
    [LocDisplayName("Parchemin d'oubli : Peinture")]
    [LocDescription("Lire ce parchemin vous fera oublier la spécialité Peinture sous certaines conditions")]
    public partial class PaintingUnSkillScroll : UnSkillScroll<PaintingSkill> { }

    [RequiresSkill(typeof(PaintingSkill), 7)]
    [Ecopedia("Professions", "Scientist", subPageName: "Painting UnSkill Scroll Item")]
    public partial class PaintingUnSkillScrollRecipe : RecipeFamily
    {
        public PaintingUnSkillScrollRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Painting",  //noloc
                displayName: Localizer.DoStr("Painting UnSkill Scroll"),

                ingredients: new List<IngredientElement>
                {
                    new (typeof(PaperItem), 1, true),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<PaintingUnSkillScroll>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.LaborInCalories = CreateLaborInCaloriesValue(1000);
            this.CraftMinutes = CreateCraftTimeValue(15);


            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Painting UnSkill Scroll"), recipeType: typeof(PaintingUnSkillScrollRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ResearchTableObject), recipe: this);
        }
        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }
}