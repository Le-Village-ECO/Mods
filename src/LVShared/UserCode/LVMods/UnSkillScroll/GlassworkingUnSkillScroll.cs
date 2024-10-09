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
    [LocDisplayName("Parchemin d'oubli : Travail du verre")]
    [LocDescription("Lire ce parchemin vous fera oublier la spécialité Travail du verre sous certaines conditions : /unskill conditions")]
    public partial class GlassworkingUnSkillScroll : UnSkillScroll<GlassworkingSkill> { }

    [RequiresSkill(typeof(GlassworkingSkill), 7)]
    [Ecopedia("Professions", "Glassworking", subPageName: "Glassworking UnSkill Scroll Item")]
    public partial class GlassworkingUnSkillScrollRecipe : RecipeFamily
    {
        public GlassworkingUnSkillScrollRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Glassworking",  //noloc
                displayName: Localizer.DoStr("Glassworking UnSkill Scroll"),

                ingredients: new List<IngredientElement>
                {
                    new (typeof(PaperItem), 1, true),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<GlassworkingUnSkillScroll>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.LaborInCalories = CreateLaborInCaloriesValue(1000);
            this.CraftMinutes = CreateCraftTimeValue(15);


            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Glassworking UnSkill Scroll"), recipeType: typeof(GlassworkingUnSkillScrollRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ResearchTableObject), recipe: this);
        }
        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }
}