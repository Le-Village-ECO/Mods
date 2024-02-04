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
    [LocDisplayName("Parchemin d'oubli : Agriculture")]
    [LocDescription("Lire ce parchemin vous fera oublier la spécialité Agriculture sous certaines conditions")]
    public partial class FarmingUnSkillScroll : UnSkillScroll<FarmingSkill> { }

    [RequiresSkill(typeof(FarmingSkill), 7)]
    [Ecopedia("Professions", "Farmer", subPageName: "Farming UnSkill Scroll Item")]
    public partial class FarmingUnSkillScrollRecipe : RecipeFamily
    {
        public FarmingUnSkillScrollRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Farming",  //noloc
                displayName: Localizer.DoStr("Farming UnSkill Scroll"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(PaperItem), 1, true),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<FarmingUnSkillScroll>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.LaborInCalories = CreateLaborInCaloriesValue(1000);
            this.CraftMinutes = CreateCraftTimeValue(15);


            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Farming UnSkill Scroll"), recipeType: typeof(FarmingUnSkillScrollRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ResearchTableObject), recipe: this);
        }
        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }
}