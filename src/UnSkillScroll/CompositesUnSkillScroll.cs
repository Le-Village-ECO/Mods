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
    [LocDisplayName("Parchemin d'oubli : Composites")]
    [LocDescription("Lire ce parchemin vous fera oublier la spécialité Composites sous certaines conditions")]
    public partial class CompositesUnSkillScroll : UnSkillScroll<CompositesSkill> { }

    [RequiresSkill(typeof(CompositesSkill), 7)]
    [Ecopedia("Professions", "Carpenter", subPageName: "Composites UnSkill Scroll Item")]
    public partial class CompositesUnSkillScrollRecipe : RecipeFamily
    {
        public CompositesUnSkillScrollRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Composites",  //noloc
                displayName: Localizer.DoStr("Composites UnSkill Scroll"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(PaperItem), 1, true),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<CompositesUnSkillScroll>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.LaborInCalories = CreateLaborInCaloriesValue(1000);
            this.CraftMinutes = CreateCraftTimeValue(15);


            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Composites UnSkill Scroll"), recipeType: typeof(CompositesUnSkillScrollRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ResearchTableObject), recipe: this);
        }
        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }
}