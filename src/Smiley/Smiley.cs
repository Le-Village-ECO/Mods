// MOD Smiley par Plex 
// Mise à jour le 30/08/2024

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
    using static Eco.Gameplay.Housing.PropertyValues.HomeFurnishingValue;
    using Eco.Gameplay.Items.Recipes;



    // Smiley01

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Grinche-Molle")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley01Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley01Item);
        public override LocString DisplayName => Localizer.DoStr("Grinche-Molle");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Grinche-Molle")]
    [LocDescription("Ce sourire exagéré cache peut-être une légère envie de grincer des dents.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(150)] 
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley01Item : WorldObjectItem<Smiley01Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }


    [Ecopedia("Housing Objects", "Smiley", subPageName: "Grinche-Molle")]
    public partial class Smiley01Recipe : RecipeFamily
    {
        public Smiley01Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Grinche-Molle",  
                displayName: Localizer.DoStr("Grinche-Molle"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)), 
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley01Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2; 

            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Grinche-Molle"), recipeType: typeof(Smiley01Recipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }



  



    // SmileyTable

    [Serialized]
    [RequireComponent(typeof(OnOffComponent))]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(MinimapComponent))]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(CraftingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [Tag("Usable")]
    [Ecopedia("Work Stations", "Craft Tables", subPageName: "SmileyTable Item")]
    public partial class SmileyTableObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(SmileyTableItem);
        public override LocString DisplayName => Localizer.DoStr("SmileyTable");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<MinimapComponent>().SetCategory(Localizer.DoStr("Crafting"));
            this.ModsPostInitialize();
        }

        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("SmileyTable")]
    [LocDescription("A bench for the basics and making even more benches.")]
    [IconGroup("World Object Minimap")]
    [Ecopedia("Work Stations", "Craft Tables", createAsSubPage: true)]
    [Weight(1000)] // Defines how heavy Workbench is.
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class SmileyTableItem : WorldObjectItem<SmileyTableObject>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }

    /// <summary>
    /// <para>Server side recipe definition for "Workbench".</para>
    /// <para>More information about RecipeFamily objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.RecipeFamily.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>
    [Ecopedia("Work Stations", "Craft Tables", subPageName: "SmileyTable Item")]
    public partial class SmileyTableRecipe : RecipeFamily
    {
        public SmileyTableRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "SmileyTable",  //noloc
                displayName: Localizer.DoStr("SmileyTable"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("HewnLog", 15,typeof(Skill)), //noloc
                    new IngredientElement(typeof(IronBarItem), 2, typeof(Skill)), //noloc
                    new IngredientElement(typeof (NailItem), 20, typeof(Skill)), //noloc
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<SmileyTableItem>()
                });
            this.Recipes = new List<Recipe> { recipe };

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Workbench"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("SmileyTable"), recipeType: typeof(SmileyTableRecipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(WorkbenchObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }

}

