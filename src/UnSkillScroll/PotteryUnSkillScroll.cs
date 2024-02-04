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
    [LocDisplayName("Parchemin d'oubli : Potterie")]
    [LocDescription("Lire ce parchemin vous fera oublier la spécialité Potterie sous certaines conditions")]
    public partial class PotteryUnSkillScroll : UnSkillScroll<PotterySkill> { }

    [RequiresSkill(typeof(PotterySkill), 7)]
    [Ecopedia("Professions", "Mason", subPageName: "Pottery UnSkill Scroll Item")]
    public partial class PotteryUnSkillScrollRecipe : RecipeFamily
    {
        public PotteryUnSkillScrollRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Pottery",  //noloc
                displayName: Localizer.DoStr("Pottery UnSkill Scroll"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(PaperItem), 1, true),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<PotteryUnSkillScroll>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.LaborInCalories = CreateLaborInCaloriesValue(1000);
            this.CraftMinutes = CreateCraftTimeValue(15);


            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Pottery UnSkill Scroll"), recipeType: typeof(PotteryUnSkillScrollRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ResearchTableObject), recipe: this);
        }
        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }
}