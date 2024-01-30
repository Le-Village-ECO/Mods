//Mod le Village le kit de réparation remplace le système de réparation vanille du jeu pour le rendre plus polyvalent
//Kit de réparation en acier - Forgeron - Pour les outils en acier

using Eco.Core.Items;
using Eco.Gameplay.Components;
using Eco.Gameplay.Items;
using Eco.Gameplay.Items.Recipes;
using Eco.Gameplay.Skills;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using System.Collections.Generic;
using System.ComponentModel;
using Eco.Mods.TechTree;

namespace Village.Eco.Mods.RepairKit
{
    [RequiresSkill(typeof(AdvancedSmeltingSkill), 1)]
    public partial class SteelRepairKitRecipe : RecipeFamily
    {
        public SteelRepairKitRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Kit de réparation en acier",  //noloc
                displayName: Localizer.DoStr("Kit de réparation en acier"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(SteelBarItem), 4, typeof(AdvancedSmeltingSkill), typeof(AdvancedSmeltingLavishResourcesTalent)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<SteelRepairKitItem>(1),
                });
            this.Recipes = new List<Recipe> { recipe };

            this.ExperienceOnCraft = 1;
            this.LaborInCalories = CreateLaborInCaloriesValue(240, typeof(AdvancedSmeltingSkill));
            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(SteelRepairKitRecipe), start: 2, skillType: typeof(AdvancedSmeltingSkill), typeof(AdvancedSmeltingFocusedSpeedTalent), typeof(SmeltingParallelSpeedTalent));

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Kit de réparation en acier"), recipeType: typeof(SteelRepairKitRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(AnvilObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Kit de réparation en acier")]
    [Weight(5000)]  //Défini le poids.
    [Category("Tool")]
    [Tag("RepairKit")]
    [Ecopedia("Items", "Tools", createAsSubPage: true)]  //Page ECOpedia
    [LocDescription("Un kit de réparation pour tous les outils en acier. L'acier c'est tellement plus léger !")]  //Description détaillée.
    public partial class SteelRepairKitItem : Item
    {

    }
}