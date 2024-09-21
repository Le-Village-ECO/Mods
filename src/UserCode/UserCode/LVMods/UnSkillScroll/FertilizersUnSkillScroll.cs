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
    [LocDisplayName("Parchemin d'oubli : Engrais")]
    [LocDescription("Lire ce parchemin vous fera oublier la spécialité Engrais sous certaines conditions : /unskill conditions")]
    public partial class FertilizersUnSkillScroll : UnSkillScroll<FertilizersSkill> { }

    [RequiresSkill(typeof(FertilizersSkill), 7)]
    [Ecopedia("Professions", "Farmer", subPageName: "Fertilizers UnSkill Scroll Item")]
    public partial class FertilizersUnSkillScrollRecipe : RecipeFamily
    {
        public FertilizersUnSkillScrollRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Fertilizers",  //noloc
                displayName: Localizer.DoStr("Fertilizers UnSkill Scroll"),

                ingredients: new List<IngredientElement>
                {
                    new (typeof(PaperItem), 1, true),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<FertilizersUnSkillScroll>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.LaborInCalories = CreateLaborInCaloriesValue(1000);
            this.CraftMinutes = CreateCraftTimeValue(15);


            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Fertilizers UnSkill Scroll"), recipeType: typeof(FertilizersUnSkillScrollRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ResearchTableObject), recipe: this);
        }
        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }
}