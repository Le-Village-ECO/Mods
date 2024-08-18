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
    [LocDisplayName("Parchemin d'oubli : Fonte avancée")]
    [LocDescription("Lire ce parchemin vous fera oublier la spécialité Fonte avancée sous certaines conditions : /unskill conditions")]
    public partial class AdvancedSmeltingUnSkillScroll : UnSkillScroll<AdvancedSmeltingSkill> { }

    [RequiresSkill(typeof(AdvancedSmeltingSkill), 7)]
    [Ecopedia("Professions", "Smith", subPageName: "Advanced Smelting UnSkill Scroll Item")]
    public partial class AdvancedSmeltingUnSkillScrollRecipe : RecipeFamily
    {
        public AdvancedSmeltingUnSkillScrollRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Advanced Smelting",  //noloc
                displayName: Localizer.DoStr("Advanced Smelting UnSkill Scroll"),

                ingredients: new List<IngredientElement>
                {
                    new (typeof(PaperItem), 1, true),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<AdvancedSmeltingUnSkillScroll>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.LaborInCalories = CreateLaborInCaloriesValue(1000);
            this.CraftMinutes = CreateCraftTimeValue(15);


            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Butchery UnSkill Scroll"), recipeType: typeof(AdvancedSmeltingUnSkillScrollRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ResearchTableObject), recipe: this);
        }
        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }
}