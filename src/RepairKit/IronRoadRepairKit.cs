//Mod le Village le kit de réparation remplace le système de réparation vanille du jeu pour le rendre plus polyvalent
//Kit de réparation routier en fer - Ingénieur basique - Pour l'outil de route en fer

namespace Eco.Mods.TechTree
{
    using Eco.Core.Items;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Items.Recipes;
    using Eco.Gameplay.Skills;
    using Eco.Shared.Localization;
    using Eco.Shared.Serialization;
    using System.Collections.Generic;
    using System.ComponentModel;

    [RequiresSkill(typeof(BasicEngineeringSkill), 1)]
    public partial class IronRoadRepairKitRecipe : RecipeFamily
    {
        public IronRoadRepairKitRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Kit de réparation routier en fer",  //noloc
                displayName: Localizer.DoStr("Kit de réparation routier en fer"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronBarItem), 4),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<IronRoadRepairKitItem>(1),
                });
            this.Recipes = new List<Recipe> { recipe };

            this.ExperienceOnCraft = 1;
            this.LaborInCalories = CreateLaborInCaloriesValue(10, typeof(BasicEngineeringSkill));
            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(IronRoadRepairKitRecipe), start: 2, skillType: typeof(BasicEngineeringSkill), typeof(BasicEngineeringFocusedSpeedTalent), typeof(BasicEngineeringParallelSpeedTalent));

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Kit de réparation routier en fer"), recipeType: typeof(IronRoadRepairKitRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(WainwrightTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Kit de réparation routier en fer")]
    [Weight(1000)]  //Défini le poids.
    [Category("Tool")]
    [Tag("RepairKit")]
    [Ecopedia("Items", "Tools", createAsSubPage: true)]  //Page ECOpedia
    [LocDescription("Un kit de réparation en fer pour l'outil de route. Moi, j'aime poser des routes !")]  //Description détaillée.
    public partial class IronRoadRepairKitItem : Item
    {

    }
}