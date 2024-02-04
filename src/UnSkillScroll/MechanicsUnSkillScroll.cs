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
    [LocDisplayName("Parchemin d'oubli : Mécanique")]
    [LocDescription("Lire ce parchemin vous fera oublier la spécialité Mécanique sous certaines conditions")]
    public partial class MechanicsUnSkillScroll : UnSkillScroll<MechanicsSkill> { }

    [RequiresSkill(typeof(MechanicsSkill), 7)]
    [Ecopedia("Professions", "Engineer", subPageName: "Mechanics UnSkill Scroll Item")]
    public partial class MechanicsUnSkillScrollRecipe : RecipeFamily
    {
        public MechanicsUnSkillScrollRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Mechanics",  //noloc
                displayName: Localizer.DoStr("Mechanics UnSkill Scroll"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(PaperItem), 1, true),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<MechanicsUnSkillScroll>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.LaborInCalories = CreateLaborInCaloriesValue(1000);
            this.CraftMinutes = CreateCraftTimeValue(15);


            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Mechanics UnSkill Scroll"), recipeType: typeof(MechanicsUnSkillScrollRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ResearchTableObject), recipe: this);
        }
        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }
}