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
    [LocDisplayName("Parchemin d'oubli : Production de papier")]
    [LocDescription("Lire ce parchemin vous fera oublier la spécialité Production de papier sous certaines conditions")]
    public partial class PaperMillingUnSkillScroll : UnSkillScroll<PaperMillingSkill> { }

    [RequiresSkill(typeof(PaperMillingSkill), 7)]
    [Ecopedia("Professions", "Carpenter", subPageName: "Paper Milling UnSkill Scroll Item")]
    public partial class PaperMillingUnSkillScrollRecipe : RecipeFamily
    {
        public PaperMillingUnSkillScrollRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Paper Milling",  //noloc
                displayName: Localizer.DoStr("Paper Milling UnSkill Scroll"),

                ingredients: new List<IngredientElement>
                {
                    new (typeof(PaperItem), 1, true),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<PaperMillingUnSkillScroll>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.LaborInCalories = CreateLaborInCaloriesValue(1000);
            this.CraftMinutes = CreateCraftTimeValue(15);


            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Paper Milling UnSkill Scroll"), recipeType: typeof(PaperMillingUnSkillScrollRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ResearchTableObject), recipe: this);
        }
        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }
}