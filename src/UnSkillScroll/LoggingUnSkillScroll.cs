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
    [LocDisplayName("Parchemin d'oubli : Abattage")]
    [LocDescription("Lire ce parchemin vous fera oublier la spécialité Abattage sous certaines conditions")]
    public partial class LoggingUnSkillScroll : UnSkillScroll<LoggingSkill> { }

    [RequiresSkill(typeof(LoggingSkill), 7)]
    [Ecopedia("Professions", "Carpenter", subPageName: "Logging UnSkill Scroll Item")]
    public partial class LoggingUnSkillScrollRecipe : RecipeFamily
    {
        public LoggingUnSkillScrollRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Logging",  //noloc
                displayName: Localizer.DoStr("Logging UnSkill Scroll"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(PaperItem), 1, true),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<LoggingUnSkillScroll>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.LaborInCalories = CreateLaborInCaloriesValue(1000);
            this.CraftMinutes = CreateCraftTimeValue(15);


            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Logging UnSkill Scroll"), recipeType: typeof(LoggingUnSkillScrollRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ResearchTableObject), recipe: this);
        }
        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }
}