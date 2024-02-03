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
    [LocDisplayName("Parchemin d'oubli : Chasse")]
    [LocDescription("Lire ce parchemin vous fera oublier la spécialité Chasse sous certaines conditions")]
    public partial class HuntingUnSkillScroll : UnSkillScroll<HuntingSkill> { }

    [RequiresSkill(typeof(HuntingSkill), 7)]
    [Ecopedia("Professions", "Hunter", subPageName: "Hunting UnSkill Scroll Item")]
    public partial class HuntingUnSkillScrollRecipe : RecipeFamily
    {
        public HuntingUnSkillScrollRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Hunting",  //noloc
                displayName: Localizer.DoStr("Hunting UnSkill Scroll"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(PaperItem), 1, true),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<HuntingUnSkillScroll>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.LaborInCalories = CreateLaborInCaloriesValue(1000);
            this.CraftMinutes = CreateCraftTimeValue(15);


            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Hunting UnSkill Scroll"), recipeType: typeof(HuntingUnSkillScrollRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ResearchTableObject), recipe: this);
        }
        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }
}