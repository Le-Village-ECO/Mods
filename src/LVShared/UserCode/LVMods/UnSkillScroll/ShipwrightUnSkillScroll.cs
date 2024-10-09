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
    [LocDisplayName("Parchemin d'oubli : Charpentier naval")]
    [LocDescription("Lire ce parchemin vous fera oublier la spécialité Charpentier naval sous certaines conditions : /unskill conditions")]
    public partial class ShipwrightUnSkillScroll : UnSkillScroll<ShipwrightSkill> { }

    [RequiresSkill(typeof(ShipwrightSkill), 7)]
    [Ecopedia("Professions", "Carpenter", subPageName: "Shipwright UnSkill Scroll Item")]
    public partial class ShipwrightUnSkillScrollRecipe : RecipeFamily
    {
        public ShipwrightUnSkillScrollRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Shipwright",  //noloc
                displayName: Localizer.DoStr("Shipwright UnSkill Scroll"),

                ingredients: new List<IngredientElement>
                {
                    new (typeof(PaperItem), 1, true),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<ShipwrightUnSkillScroll>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.LaborInCalories = CreateLaborInCaloriesValue(1000);
            this.CraftMinutes = CreateCraftTimeValue(15);


            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Shipwright UnSkill Scroll"), recipeType: typeof(ShipwrightUnSkillScrollRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ResearchTableObject), recipe: this);
        }
        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }
}