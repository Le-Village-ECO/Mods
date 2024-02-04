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
    [LocDisplayName("Parchemin d'oubli : Ingénierie basique")]
    [LocDescription("Lire ce parchemin vous fera oublier la spécialité Ingénierie basique sous certaines conditions")]
    public partial class BasicEngineeringUnSkillScroll : UnSkillScroll<BasicEngineeringSkill> { }

    [RequiresSkill(typeof(BasicEngineeringSkill), 7)]
    [Ecopedia("Professions", "Engineer", subPageName: "Basic Engineering UnSkill Scroll Item")]
    public partial class BasicEngineeringUnSkillScrollRecipe : RecipeFamily
    {
        public BasicEngineeringUnSkillScrollRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Basic Engineering",  //noloc
                displayName: Localizer.DoStr("Basic Engineering UnSkill Scroll"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(PaperItem), 1, true),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<BasicEngineeringUnSkillScroll>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.LaborInCalories = CreateLaborInCaloriesValue(1000);
            this.CraftMinutes = CreateCraftTimeValue(15);


            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Basic Engineering UnSkill Scroll"), recipeType: typeof(BasicEngineeringUnSkillScrollRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ResearchTableObject), recipe: this);
        }
        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }
}