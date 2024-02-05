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
    [LocDisplayName("Parchemin d'oubli : Boulangerie avancée")]
    [LocDescription("Lire ce parchemin vous fera oublier la spécialité Boulangerie avancée sous certaines conditions")]
    public partial class AdvancedBakingUnSkillScroll : UnSkillScroll<AdvancedBakingSkill> { }

    [RequiresSkill(typeof(AdvancedBakingSkill), 7)]
    [Ecopedia("Professions", "Chef", subPageName: "Advanced Baking UnSkill Scroll Item")]
    public partial class AdvancedBakingUnSkillScrollRecipe : RecipeFamily
    {
        public AdvancedBakingUnSkillScrollRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Advanced Baking",  //noloc
                displayName: Localizer.DoStr("Advanced Baking UnSkill Scroll"),

                ingredients: new List<IngredientElement>
                {
                    new(typeof(PaperItem), 1, true),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<AdvancedBakingUnSkillScroll>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.LaborInCalories = CreateLaborInCaloriesValue(1000);
            this.CraftMinutes = CreateCraftTimeValue(15);


            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Advanced Baking UnSkill Scroll"), recipeType: typeof(AdvancedBakingUnSkillScrollRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ResearchTableObject), recipe: this);
        }
        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }
}