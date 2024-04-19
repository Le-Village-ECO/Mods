// Le Village - Ajout d'une amélioration pour les tables de boucheries T2 et T3

namespace Eco.Mods.TechTree
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Eco.Gameplay.Blocks;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.DynamicValues;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Modules;
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

    [RequiresModule(typeof(HunterJarObject))]
    [RequiresSkill(typeof(ButcherySkill), 7)]
    [Ecopedia("Upgrade Modules", "Specialty Upgrades", subPageName: "Advanced Butchery Upgrade Item")]
    public partial class AdvancedButcheryUpgradeRecipe : RecipeFamily
    {
        public AdvancedButcheryUpgradeRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "AdvancedButcheryUpgrade",  //noloc
                displayName: Localizer.DoStr("Advanced Butchery Upgrade"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(AdvancedUpgradeLvl4Item), 1, true),
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<AdvancedButcheryUpgradeItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 4; // Defines how much experience is gained when crafted.

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(6000, typeof(ButcherySkill));

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(ButcheryUpgradeRecipe), start: 10, skillType: typeof(ButcherySkill), typeof(ButcheryFocusedSpeedTalent), typeof(ButcheryParallelSpeedTalent));

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Advanced Butchery Upgrade"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Advanced Butchery Upgrade"), recipeType: typeof(AdvancedButcheryUpgradeRecipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(MechanicalChopperObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Advanced Butchery Upgrade")]
    [LocDescription("Modern Upgrade that greatly increases efficiency when crafting Advanced Butchery recipes.")]
    [Weight(1)]
    [Ecopedia("Upgrade Modules", "Specialty Upgrades", createAsSubPage: true)]                                                                      //_If_EcopediaPage_
    [Tag("Upgrade")]
    public partial class AdvancedButcheryUpgradeItem :
        EfficiencyModule
    {

        public AdvancedButcheryUpgradeItem() : base(
            ModuleTypes.ResourceEfficiency | ModuleTypes.SpeedEfficiency,
            0.5f + 0.05f,
            typeof(ButcherySkill),
            0.5f
        )
        { }
    }
}
