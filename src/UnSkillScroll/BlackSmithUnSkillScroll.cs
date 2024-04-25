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
    [LocDisplayName("Parchemin d'oubli : Forgeron")]
    [LocDescription("Lire ce parchemin vous fera oublier la spécialité Forgeron sous certaines conditions")]
    public partial class BlacksmithUnSkillScroll : UnSkillScroll<BlacksmithSkill> { }

    [RequiresSkill(typeof(BlacksmithSkill), 7)]
    [Ecopedia("Professions", "Smith", subPageName: "Blacksmith UnSkill Scroll Item")]
    public partial class BlacksmithUnSkillScrollRecipe : RecipeFamily
    {
        public BlacksmithUnSkillScrollRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Blacksmith",  //noloc
                displayName: Localizer.DoStr("Blacksmith UnSkill Scroll"),

                ingredients: new List<IngredientElement>
                {
                    new (typeof(PaperItem), 1, true),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<BlacksmithUnSkillScroll>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.LaborInCalories = CreateLaborInCaloriesValue(1000);
            this.CraftMinutes = CreateCraftTimeValue(15);


            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Blacksmith UnSkill Scroll"), recipeType: typeof(BlacksmithUnSkillScrollRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ResearchTableObject), recipe: this);
        }
        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }
}