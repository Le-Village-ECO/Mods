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
    [LocDisplayName("Parchemin d'oubli : Minage")]
    [LocDescription("Lire ce parchemin vous fera oublier la spécialité Minage sous certaines conditions : /unskill conditions")]
    public partial class MiningUnSkillScroll : UnSkillScroll<MiningSkill> { }

    [RequiresSkill(typeof(MiningSkill), 7)]
    [Ecopedia("Professions", "Carpenter", subPageName: "Mining UnSkill Scroll Item")]
    public partial class MiningUnSkillScrollRecipe : RecipeFamily
    {
        public MiningUnSkillScrollRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Mining",  //noloc
                displayName: Localizer.DoStr("Mining UnSkill Scroll"),

                ingredients: new List<IngredientElement>
                {
                    new (typeof(PaperItem), 1, true),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<MiningUnSkillScroll>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.LaborInCalories = CreateLaborInCaloriesValue(1000);
            this.CraftMinutes = CreateCraftTimeValue(15);


            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Mining UnSkill Scroll"), recipeType: typeof(MiningUnSkillScrollRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ResearchTableObject), recipe: this);
        }
        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }
}