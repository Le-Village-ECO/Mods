//Mod le Village le kit de réparation remplace le système de réparation vanille du jeu pour le rendre plus polyvalent
//Kit de réparation pour tracteur - Mécanicien - Pour les outils du tracteur

using Eco.Core.Items;
using Eco.Gameplay.Components;
using Eco.Gameplay.Items;
using Eco.Gameplay.Items.Recipes;
using Eco.Gameplay.Skills;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using System.Collections.Generic;
using System.ComponentModel;

namespace Eco.Mods.TechTree
{
    [RequiresSkill(typeof(ElectronicsSkill), 1)]
    public partial class PlasticRepairKitRecipe : RecipeFamily
    {
        public PlasticRepairKitRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Kit de réparation pour appareil en plastique",  //noloc
                displayName: Localizer.DoStr("Kit de réparation pour appareil en plastique"),

                ingredients: new List<IngredientElement>
                {
                    new(typeof(PlasticItem), 4, typeof(ElectronicsSkill), typeof(ElectronicsLavishResourcesTalent)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<PlasticRepairKitItem>(1),
                });
            this.Recipes = new List<Recipe> { recipe };

            this.ExperienceOnCraft = 1;
            this.LaborInCalories = CreateLaborInCaloriesValue(240, typeof(ElectronicsSkill));
            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(PlasticRepairKitRecipe), start: 2, skillType: typeof(ElectronicsSkill), typeof(ElectronicsFocusedSpeedTalent), typeof(ElectronicsParallelSpeedTalent));

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Kit de réparation pour appareil en plastique"), recipeType: typeof(PlasticRepairKitRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ElectricMachinistTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Kit de réparation pour appareil en plastique")]
    [Weight(1000)]  //Défini le poids.
    [Category("Tool")]
    [Tag("RepairKit")]
    [Ecopedia("Items", "Tools", createAsSubPage: true)]  //Page ECOpedia
    [LocDescription("Un kit de réparation pour appareil en plastique. Car le plastique c'est fantastique !")]  //Description détaillée.
    public partial class PlasticRepairKitItem : Item
    {

    }
}