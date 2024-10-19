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
    [LocDisplayName("Parchemin d'oubli : Récolte")]
    [LocDescription("Lire ce parchemin vous fera oublier la spécialité Récolte sous certaines conditions : /unskill conditions")]
    public partial class GatheringUnSkillScroll : UnSkillScroll<GatheringSkill> { }

    [RequiresSkill(typeof(GatheringSkill), 7)]
    [Ecopedia("Professions", "Hunter", subPageName: "Gathering UnSkill Scroll Item")]
    public partial class GatheringUnSkillScrollRecipe : RecipeFamily
    {
        public GatheringUnSkillScrollRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Gathering",  //noloc
                displayName: Localizer.DoStr("Gathering UnSkill Scroll"),

                ingredients: new List<IngredientElement>
                {
                    new (typeof(PaperItem), 1, true),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<GatheringUnSkillScroll>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.LaborInCalories = CreateLaborInCaloriesValue(1000);
            this.CraftMinutes = CreateCraftTimeValue(15);


            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Gathering UnSkill Scroll"), recipeType: typeof(GatheringUnSkillScrollRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ResearchTableObject), recipe: this);
        }
        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }
}