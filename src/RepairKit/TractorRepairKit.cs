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
using Eco.Mods.TechTree;

namespace Village.Eco.Mods.RepairKit
{
    [RequiresSkill(typeof(MechanicsSkill), 1)]
    public partial class TractorRepairKitRecipe : RecipeFamily
    {
        public TractorRepairKitRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Kit de réparation pour tracteur",  //noloc
                displayName: Localizer.DoStr("Kit de réparation pour tracteur"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronPlateItem), 4, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<TractorRepairKitItem>(1),
                });
            this.Recipes = new List<Recipe> { recipe };

            this.ExperienceOnCraft = 1;
            this.LaborInCalories = CreateLaborInCaloriesValue(120, typeof(MechanicsSkill));
            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(TractorRepairKitRecipe), start: 2, skillType: typeof(MechanicsSkill), typeof(MechanicsFocusedSpeedTalent), typeof(MechanicsParallelSpeedTalent));

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Kit de réparation pour tracteur"), recipeType: typeof(TractorRepairKitRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(MachinistTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Kit de réparation pour tracteur")]
    [Weight(10000)]  //Défini le poids.
    [Category("Tool")]
    [Tag("RepairKit")]
    [Ecopedia("Items", "Tools", createAsSubPage: true)]  //Page ECOpedia
    [LocDescription("Un kit de réparation pour tous les outils du tracteur. Farm Simulator 2050 !")]  //Description détaillée.
    public partial class TractorRepairKitItem : Item
    {

    }
}