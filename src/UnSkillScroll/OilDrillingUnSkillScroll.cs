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
    [LocDisplayName("Parchemin d'oubli : Extraction de pétrole")]
    [LocDescription("Lire ce parchemin vous fera oublier la spécialité Extraction de pétrole sous certaines conditions : /unskill conditions")]
    public partial class OilDrillingUnSkillScroll : UnSkillScroll<OilDrillingSkill> { }

    [RequiresSkill(typeof(OilDrillingSkill), 7)]
    [Ecopedia("Professions", "Engineer", subPageName: "OilDrilling UnSkill Scroll Item")]
    public partial class OilDrillingUnSkillScrollRecipe : RecipeFamily
    {
        public OilDrillingUnSkillScrollRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "OilDrilling",  //noloc
                displayName: Localizer.DoStr("OilDrilling UnSkill Scroll"),

                ingredients: new List<IngredientElement>
                {
                    new (typeof(PaperItem), 1, true),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<OilDrillingUnSkillScroll>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.LaborInCalories = CreateLaborInCaloriesValue(1000);
            this.CraftMinutes = CreateCraftTimeValue(15);


            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("OilDrilling UnSkill Scroll"), recipeType: typeof(OilDrillingUnSkillScrollRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ResearchTableObject), recipe: this);
        }
        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }
}