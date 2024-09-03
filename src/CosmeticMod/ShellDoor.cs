namespace Eco.Mods.TechTree
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Eco.Core.Items;
    using Eco.Gameplay.Blocks;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Components.Auth;
    using Eco.Gameplay.DynamicValues;
    using Eco.Gameplay.Economy;
    using Eco.Gameplay.Housing;
    using Eco.Gameplay.Interactions;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Modules;
    using Eco.Gameplay.Minimap;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Occupancy;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Property;
    using Eco.Gameplay.Skills;
    using Eco.Gameplay.Systems;
    using Eco.Gameplay.Systems.TextLinks;
    using Eco.Gameplay.Pipes.LiquidComponents;
    using Eco.Gameplay.Pipes.Gases;
    using Eco.Shared;
    using Eco.Shared.Math;
    using Eco.Shared.Localization;
    using Eco.Shared.Serialization;
    using Eco.Shared.Utils;
    using Eco.Shared.View;
    using Eco.Shared.Items;
    using Eco.Shared.Networking;
    using Eco.Gameplay.Pipes;
    using Eco.World.Blocks;
    using Eco.Gameplay.Housing.PropertyValues;
    using Eco.Gameplay.Civics.Objects;
    using Eco.Gameplay.Settlements;
    using Eco.Gameplay.Systems.NewTooltip;
    using Eco.Core.Controller;
    using Eco.Core.Utils;
    using Eco.Gameplay.Components.Storage;
    using Eco.Gameplay.Items.Recipes;

    [Serialized]
    [RequireComponent(typeof(OnOffComponent))]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(MinimapComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(LargeDoorUtils))]
    [Tag("Usable")]
    [Tag("LargeDoor")]
    [Ecopedia("Housing Objects", "Doors", subPageName: "Porte de Grange Large")]
    public partial class SheldDoorObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(SheldDoorItem);
        public override LocString DisplayName => Localizer.DoStr("Large Corrugated Steel Door");
        public override TableTextureMode TableTexture => TableTextureMode.Metal;
        public override bool HasTier => true;
        public override int Tier => 4;



        protected override void Initialize()
        {

            this.ModsPreInitialize();
            this.GetComponent<MinimapComponent>().SetCategory(Localizer.DoStr("Doors"));
            this.ModsPostInitialize();
            //protected override void PostInitialize() => LargeDoorUtils.InitializeDoor(this);
    }

        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Porte de Grange Large")]
    [LocDescription("Une porte de Grange Large.")]
    [IconGroup("World Object Minimap")]
    [Tier(4)]
    [Ecopedia("Housing Objects", "Doors", createAsSubPage: true)]
    [Weight(2000)] // Defines how heavy SheldDoor is.
    public partial class SheldDoorItem : WorldObjectItem<SheldDoorObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));

    }

    /// <summary>
    /// <para>Server side recipe definition for "SheldDoor".</para>
    /// <para>More information about RecipeFamily objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.RecipeFamily.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>
    [RequiresSkill(typeof(AdvancedSmeltingSkill), 5)]
    [Ecopedia("Housing Objects", "Doors", subPageName: "Porte de Grange Large item")]
    public partial class SheldDoorRecipe : RecipeFamily
    {
        public SheldDoorRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "SheldDoor",  //noloc
                displayName: Localizer.DoStr("Porte de Grange Large"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(CorrugatedSteelItem), 20, true),
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<SheldDoorItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2.5f; // Defines how much experience is gained when crafted.

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(480, typeof(AdvancedSmeltingSkill));

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(SheldDoorRecipe), start: 10, skillType: typeof(AdvancedSmeltingSkill), typeof(AdvancedSmeltingFocusedSpeedTalent), typeof(AdvancedSmeltingParallelSpeedTalent));

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Large Corrugated Steel Door"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Porte de Grange Large"), recipeType: typeof(SheldDoorRecipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(RollingMillObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }
}
