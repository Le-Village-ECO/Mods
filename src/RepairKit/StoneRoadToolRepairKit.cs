//Mod le Village le kit de réparation remplace le système de réparation vanille du jeu pour le rendre plus polyvalent
//Kit de réparation routier en pierre - Ingénieur basique - Pour l'outil de route en pierre

namespace Eco.Mods.TechTree
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Eco.Gameplay.Blocks;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.DynamicValues;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Skills;
    using Eco.Gameplay.Systems;
    using Eco.Gameplay.Systems.TextLinks;
    using Eco.Shared.Localization;
    using Eco.Shared.Serialization;
    using Eco.Shared.Utils;
    using Eco.Core.Items;
    using Eco.World;
    using Eco.World.Blocks;
    using Eco.Gameplay.Pipes;
    using Eco.Core.Controller;
    using Eco.Gameplay.Items.Recipes;

    [RequiresSkill(typeof(BasicEngineeringSkill), 1)]
    public partial class StoneRoadRepairKitRecipe : RecipeFamily
    {
        public StoneRoadRepairKitRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Kit de réparation routier en pierre",  //noloc
                displayName: Localizer.DoStr("Kit de réparation routier en pierre"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("Rock", 5),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<StoneRoadRepairKitItem>(1),
                });
            this.Recipes = new List<Recipe> { recipe };

            this.ExperienceOnCraft = 1;
            this.LaborInCalories = CreateLaborInCaloriesValue(10, typeof(BasicEngineeringSkill));
            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(StoneRoadRepairKitRecipe), start: 2, skillType: typeof(BasicEngineeringSkill), typeof(BasicEngineeringFocusedSpeedTalent), typeof(BasicEngineeringParallelSpeedTalent));

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Kit de réparation routier en pierre"), recipeType: typeof(StoneRoadRepairKitRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ToolBenchObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Kit de réparation routier en pierre")]
    [Weight(1000)]  //Défini le poids.
    [Category("Tool")]  //??? Category Tool ou RepairKit ???
    [Tag("RepairKit")]
    [Ecopedia("Items", "Tools", createAsSubPage: true)]  //??? Tools ou RepairKit ???
    [LocDescription("Un kit de réparation en pierre pour l'outil de route. Tous les chemins mènent à Rome !")]  //Description détaillée.
    public partial class StoneRoadRepairKitItem : Item
    {

    }
}