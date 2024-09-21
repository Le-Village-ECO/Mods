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
    [LocDisplayName("Parchemin d'oubli : Menuiserie")]
    [LocDescription("Lire ce parchemin vous fera oublier la spécialité Menuiserie sous certaines conditions : /unskill conditions")]
    public partial class CarpentryUnSkillScroll : UnSkillScroll<CarpentrySkill> { }

    [RequiresSkill(typeof(CarpentrySkill), 7)]
    [Ecopedia("Professions", "Carpenter", subPageName: "Carpentry UnSkill Scroll Item")]
    public partial class CarpentryUnSkillScrollRecipe : RecipeFamily
    {
        public CarpentryUnSkillScrollRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Carpentry",  //noloc
                displayName: Localizer.DoStr("Carpentry UnSkill Scroll"),

                ingredients: new List<IngredientElement>
                {
                    new (typeof(PaperItem), 1, true),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<CarpentryUnSkillScroll>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.LaborInCalories = CreateLaborInCaloriesValue(1000);
            this.CraftMinutes = CreateCraftTimeValue(15);


            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Carpentry UnSkill Scroll"), recipeType: typeof(CarpentryUnSkillScrollRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ResearchTableObject), recipe: this);
        }
        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }
}