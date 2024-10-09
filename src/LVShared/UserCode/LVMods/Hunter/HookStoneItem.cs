
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
    using Eco.Gameplay.Settlements;
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


    [Ecopedia("Items", "Products", subPageName: "Hook Stone Item")]
    public partial class HookStoneRecipe : RecipeFamily
    {
        public HookStoneRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "HookStone",  //noloc
                displayName: Localizer.DoStr("Hook Stone"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("Rock", 1, true), //noloc
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<HookStoneItem>()
                });
            this.Recipes = new List<Recipe> { recipe };

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(20);

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(0.2f);

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Hook Stone"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Hook Stone"), recipeType: typeof(HookStoneRecipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(ToolBenchObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized] // Tells the save/load system this object needs to be serialized. 
    [LocDisplayName("Hook Stone")] // Defines the localized name of the item.
    [Weight(100)] // Defines how heavy HookStone is.
    [Ecopedia("Items", "Products", createAsSubPage: true)]
    [LocDescription("A simple flattened piece of stone used to repair damaged tools. It will wear down your tools quickly.")] //The tooltip description for the item.
    public partial class HookStoneItem : PartItem, IRepairMaterial
    {
        public override IDynamicValue SkilledRepairCost => skilledRepairCost;
        private static IDynamicValue skilledRepairCost = new ConstantValue(1);

        public float ReducesMaxDurabilityByPercent => 0.06f;

    }
}