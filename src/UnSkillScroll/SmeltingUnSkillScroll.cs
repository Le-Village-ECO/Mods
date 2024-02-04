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
    [LocDisplayName("Parchemin d'oubli : Fonte")]
    [LocDescription("Lire ce parchemin vous fera oublier la spécialité Fonte sous certaines conditions")]
    public partial class SmeltingUnSkillScroll : UnSkillScroll<SmeltingSkill> { }

    [RequiresSkill(typeof(SmeltingSkill), 7)]
    [Ecopedia("Professions", "Smith", subPageName: "Smelting UnSkill Scroll Item")]
    public partial class SmeltingUnSkillScrollRecipe : RecipeFamily
    {
        public SmeltingUnSkillScrollRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Smelting",  //noloc
                displayName: Localizer.DoStr("Smelting UnSkill Scroll"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(PaperItem), 1, true),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<SmeltingUnSkillScroll>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.LaborInCalories = CreateLaborInCaloriesValue(1000);
            this.CraftMinutes = CreateCraftTimeValue(15);


            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Smelting UnSkill Scroll"), recipeType: typeof(SmeltingUnSkillScrollRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ResearchTableObject), recipe: this);
        }
        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }
}