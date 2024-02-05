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
    [LocDisplayName("Parchemin d'oubli : Boulangerie")]
    [LocDescription("Lire ce parchemin vous fera oublier la spécialité Boulangerie sous certaines conditions")]
    public partial class BakingUnSkillScroll : UnSkillScroll<BakingSkill> { }

    [RequiresSkill(typeof(BakingSkill), 7)]
    [Ecopedia("Professions", "Chef", subPageName: "Baking UnSkill Scroll Item")]
    public partial class BakingUnSkillScrollRecipe : RecipeFamily
    {
        public BakingUnSkillScrollRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Baking",  //noloc
                displayName: Localizer.DoStr("Baking UnSkill Scroll"),

                ingredients: new List<IngredientElement>
                {
                    new (typeof(PaperItem), 1, true),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<BakingUnSkillScroll>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.LaborInCalories = CreateLaborInCaloriesValue(1000);
            this.CraftMinutes = CreateCraftTimeValue(15);


            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Baking UnSkill Scroll"), recipeType: typeof(BakingUnSkillScrollRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ResearchTableObject), recipe: this);
        }
        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }
}