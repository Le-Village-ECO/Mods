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
    [LocDisplayName("Parchemin d'oubli : Mouture")]
    [LocDescription("Lire ce parchemin vous fera oublier la spécialité Mouture sous certaines conditions")]
    public partial class MillingUnSkillScroll : UnSkillScroll<MillingSkill> { }

    [RequiresSkill(typeof(MillingSkill), 7)]
    [Ecopedia("Professions", "Farmer", subPageName: "Milling UnSkill Scroll Item")]
    public partial class MillingUnSkillScrollRecipe : RecipeFamily
    {
        public MillingUnSkillScrollRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Milling",  //noloc
                displayName: Localizer.DoStr("Milling UnSkill Scroll"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(PaperItem), 1, true),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<MillingUnSkillScroll>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.LaborInCalories = CreateLaborInCaloriesValue(1000);
            this.CraftMinutes = CreateCraftTimeValue(15);


            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Milling UnSkill Scroll"), recipeType: typeof(MillingUnSkillScrollRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ResearchTableObject), recipe: this);
        }
        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }
}