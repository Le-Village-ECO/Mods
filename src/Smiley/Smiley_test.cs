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
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Joyo")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley01Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley01Item);
        public override LocString DisplayName => Localizer.DoStr("Smiley Joyo");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Smiley Joyo")]
    [LocDescription("Smiley radieux au sourire éclatant, symbole de bonheur pur.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(500)] // Defines how heavy HewnTable is.
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley01Item : WorldObjectItem<Smiley01Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }

    /// <summary>
    /// <para>Server side recipe definition for "HewnTable".</para>
    /// <para>More information about RecipeFamily objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.RecipeFamily.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>

    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Joyo")]
    public partial class Smiley01Recipe : RecipeFamily
    {
        public Smiley01Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Smiley Joyo",  //noloc
                displayName: Localizer.DoStr("Smiley Joyo"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)), //noloc
                    new IngredientElement(typeof (NailItem), 3, typeof(Skill)), //noloc
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley01Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2; // Defines how much experience is gained when crafted.

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Hewn Table"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Smiley Joyo"), recipeType: typeof(Smiley01Recipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }



    // Smiley02

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley02Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley02Item);
        public override LocString DisplayName => Localizer.DoStr("Smiley02");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Smiley Sourire Espiègle")]
    [LocDescription("Un smiley au sourire espiègle, avec des yeux allongés en forme de demi-lune, exprimant malice et amusement.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(500)] // Defines how heavy HewnTable is.
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley02Item : WorldObjectItem<Smiley02Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }

    /// <summary>
    /// <para>Server side recipe definition for "HewnTable".</para>
    /// <para>More information about RecipeFamily objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.RecipeFamily.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>

    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    public partial class Smiley02Recipe : RecipeFamily
    {
        public Smiley02Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Smiley02",  //noloc
                displayName: Localizer.DoStr("Smiley02"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)), //noloc
                    new IngredientElement(typeof (NailItem), 3, typeof(Skill)), //noloc
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley02Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2; // Defines how much experience is gained when crafted.

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Hewn Table"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Smiley02"), recipeType: typeof(Smiley02Recipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }


    // Smiley03

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley03Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley03Item);
        public override LocString DisplayName => Localizer.DoStr("Smiley03");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Smiley Sourire Espiègle")]
    [LocDescription("Un smiley au sourire espiègle, avec des yeux allongés en forme de demi-lune, exprimant malice et amusement.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(500)] // Defines how heavy HewnTable is.
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley03Item : WorldObjectItem<Smiley03Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }

    /// <summary>
    /// <para>Server side recipe definition for "HewnTable".</para>
    /// <para>More information about RecipeFamily objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.RecipeFamily.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>

    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    public partial class Smiley03Recipe : RecipeFamily
    {
        public Smiley03Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Smiley03",  //noloc
                displayName: Localizer.DoStr("Smiley03"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)), //noloc
                    new IngredientElement(typeof (NailItem), 3, typeof(Skill)), //noloc
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley03Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2; // Defines how much experience is gained when crafted.

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Hewn Table"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Smiley03"), recipeType: typeof(Smiley03Recipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }


    // Smiley04

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley04Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley04Item);
        public override LocString DisplayName => Localizer.DoStr("Smiley04");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Smiley Sourire Espiègle")]
    [LocDescription("Un smiley au sourire espiègle, avec des yeux allongés en forme de demi-lune, exprimant malice et amusement.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(500)] // Defines how heavy HewnTable is.
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley04Item : WorldObjectItem<Smiley04Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }

    /// <summary>
    /// <para>Server side recipe definition for "HewnTable".</para>
    /// <para>More information about RecipeFamily objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.RecipeFamily.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>

    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    public partial class Smiley04Recipe : RecipeFamily
    {
        public Smiley04Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Smiley04",  //noloc
                displayName: Localizer.DoStr("Smiley04"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)), //noloc
                    new IngredientElement(typeof (NailItem), 3, typeof(Skill)), //noloc
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley04Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2; // Defines how much experience is gained when crafted.

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Hewn Table"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Smiley04"), recipeType: typeof(Smiley04Recipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }



    // Smiley05

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley05Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley05Item);
        public override LocString DisplayName => Localizer.DoStr("Smiley05");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Smiley Sourire Espiègle")]
    [LocDescription("Un smiley au sourire espiègle, avec des yeux allongés en forme de demi-lune, exprimant malice et amusement.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(500)] // Defines how heavy HewnTable is.
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley05Item : WorldObjectItem<Smiley05Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }

    /// <summary>
    /// <para>Server side recipe definition for "HewnTable".</para>
    /// <para>More information about RecipeFamily objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.RecipeFamily.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>

    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    public partial class Smiley05Recipe : RecipeFamily
    {
        public Smiley05Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Smiley05",  //noloc
                displayName: Localizer.DoStr("Smiley05"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)), //noloc
                    new IngredientElement(typeof (NailItem), 3, typeof(Skill)), //noloc
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley05Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2; // Defines how much experience is gained when crafted.

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Hewn Table"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Smiley05"), recipeType: typeof(Smiley05Recipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }



    // Smiley06

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley06Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley06Item);
        public override LocString DisplayName => Localizer.DoStr("Smiley06");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Smiley Sourire Espiègle")]
    [LocDescription("Un smiley au sourire espiègle, avec des yeux allongés en forme de demi-lune, exprimant malice et amusement.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(500)] // Defines how heavy HewnTable is.
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley06Item : WorldObjectItem<Smiley06Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }

    /// <summary>
    /// <para>Server side recipe definition for "HewnTable".</para>
    /// <para>More information about RecipeFamily objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.RecipeFamily.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>

    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    public partial class Smiley06Recipe : RecipeFamily
    {
        public Smiley06Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Smiley06",  //noloc
                displayName: Localizer.DoStr("Smiley06"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)), //noloc
                    new IngredientElement(typeof (NailItem), 3, typeof(Skill)), //noloc
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley06Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2; // Defines how much experience is gained when crafted.

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Hewn Table"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Smiley06"), recipeType: typeof(Smiley06Recipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }


    // Smiley07

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley07Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley07Item);
        public override LocString DisplayName => Localizer.DoStr("Smiley07");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Smiley Sourire Espiègle")]
    [LocDescription("Un smiley au sourire espiègle, avec des yeux allongés en forme de demi-lune, exprimant malice et amusement.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(500)] // Defines how heavy HewnTable is.
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley07Item : WorldObjectItem<Smiley07Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }

    /// <summary>
    /// <para>Server side recipe definition for "HewnTable".</para>
    /// <para>More information about RecipeFamily objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.RecipeFamily.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>

    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    public partial class Smiley07Recipe : RecipeFamily
    {
        public Smiley07Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Smiley07",  //noloc
                displayName: Localizer.DoStr("Smiley07"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)), //noloc
                    new IngredientElement(typeof (NailItem), 3, typeof(Skill)), //noloc
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley07Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2; // Defines how much experience is gained when crafted.

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Hewn Table"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Smiley07"), recipeType: typeof(Smiley07Recipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }



    // Smiley08

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley08Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley08Item);
        public override LocString DisplayName => Localizer.DoStr("Smiley08");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Smiley Sourire Espiègle")]
    [LocDescription("Un smiley au sourire espiègle, avec des yeux allongés en forme de demi-lune, exprimant malice et amusement.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(500)] // Defines how heavy HewnTable is.
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley08Item : WorldObjectItem<Smiley08Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }

    /// <summary>
    /// <para>Server side recipe definition for "HewnTable".</para>
    /// <para>More information about RecipeFamily objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.RecipeFamily.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>

    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    public partial class Smiley08Recipe : RecipeFamily
    {
        public Smiley08Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Smiley08",  //noloc
                displayName: Localizer.DoStr("Smiley08"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)), //noloc
                    new IngredientElement(typeof (NailItem), 3, typeof(Skill)), //noloc
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley08Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2; // Defines how much experience is gained when crafted.

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Hewn Table"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Smiley08"), recipeType: typeof(Smiley08Recipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }



    // Smiley09

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley09Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley09Item);
        public override LocString DisplayName => Localizer.DoStr("Smiley09");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Smiley Sourire Espiègle")]
    [LocDescription("Un smiley au sourire espiègle, avec des yeux allongés en forme de demi-lune, exprimant malice et amusement.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(500)] // Defines how heavy HewnTable is.
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley09Item : WorldObjectItem<Smiley09Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }

    /// <summary>
    /// <para>Server side recipe definition for "HewnTable".</para>
    /// <para>More information about RecipeFamily objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.RecipeFamily.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>

    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    public partial class Smiley09Recipe : RecipeFamily
    {
        public Smiley09Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Smiley09",  //noloc
                displayName: Localizer.DoStr("Smiley09"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)), //noloc
                    new IngredientElement(typeof (NailItem), 3, typeof(Skill)), //noloc
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley09Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2; // Defines how much experience is gained when crafted.

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Hewn Table"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Smiley09"), recipeType: typeof(Smiley09Recipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }



    // Smiley10

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley10Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley10Item);
        public override LocString DisplayName => Localizer.DoStr("Smiley10");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Smiley Sourire Espiègle")]
    [LocDescription("Un smiley au sourire espiègle, avec des yeux allongés en forme de demi-lune, exprimant malice et amusement.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(500)] // Defines how heavy HewnTable is.
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley10Item : WorldObjectItem<Smiley10Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }

    /// <summary>
    /// <para>Server side recipe definition for "HewnTable".</para>
    /// <para>More information about RecipeFamily objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.RecipeFamily.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>

    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    public partial class Smiley10Recipe : RecipeFamily
    {
        public Smiley10Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Smiley10",  //noloc
                displayName: Localizer.DoStr("Smiley10"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)), //noloc
                    new IngredientElement(typeof (NailItem), 3, typeof(Skill)), //noloc
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley10Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2; // Defines how much experience is gained when crafted.

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Hewn Table"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Smiley10"), recipeType: typeof(Smiley10Recipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }



    // Smiley11

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley11Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley11Item);
        public override LocString DisplayName => Localizer.DoStr("Smiley11");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Smiley Sourire Espiègle")]
    [LocDescription("Un smiley au sourire espiègle, avec des yeux allongés en forme de demi-lune, exprimant malice et amusement.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(500)] // Defines how heavy HewnTable is.
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley11Item : WorldObjectItem<Smiley11Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }

    /// <summary>
    /// <para>Server side recipe definition for "HewnTable".</para>
    /// <para>More information about RecipeFamily objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.RecipeFamily.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>

    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    public partial class Smiley11Recipe : RecipeFamily
    {
        public Smiley11Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Smiley11",  //noloc
                displayName: Localizer.DoStr("Smiley11"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)), //noloc
                    new IngredientElement(typeof (NailItem), 3, typeof(Skill)), //noloc
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley11Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2; // Defines how much experience is gained when crafted.

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Hewn Table"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Smiley11"), recipeType: typeof(Smiley11Recipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }



    // Smiley12

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley12Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley12Item);
        public override LocString DisplayName => Localizer.DoStr("Smiley12");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Smiley Sourire Espiègle")]
    [LocDescription("Un smiley au sourire espiègle, avec des yeux allongés en forme de demi-lune, exprimant malice et amusement.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(500)] // Defines how heavy HewnTable is.
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley12Item : WorldObjectItem<Smiley12Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }

    /// <summary>
    /// <para>Server side recipe definition for "HewnTable".</para>
    /// <para>More information about RecipeFamily objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.RecipeFamily.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>

    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    public partial class Smiley12Recipe : RecipeFamily
    {
        public Smiley12Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Smiley12",  //noloc
                displayName: Localizer.DoStr("Smiley12"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)), //noloc
                    new IngredientElement(typeof (NailItem), 3, typeof(Skill)), //noloc
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley12Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2; // Defines how much experience is gained when crafted.

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Hewn Table"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Smiley12"), recipeType: typeof(Smiley12Recipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }




    // Smiley13

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley13Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley13Item);
        public override LocString DisplayName => Localizer.DoStr("Smiley13");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Smiley Sourire Espiègle")]
    [LocDescription("Un smiley au sourire espiègle, avec des yeux allongés en forme de demi-lune, exprimant malice et amusement.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(500)] // Defines how heavy HewnTable is.
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley13Item : WorldObjectItem<Smiley13Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }

    /// <summary>
    /// <para>Server side recipe definition for "HewnTable".</para>
    /// <para>More information about RecipeFamily objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.RecipeFamily.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>

    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    public partial class Smiley13Recipe : RecipeFamily
    {
        public Smiley13Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Smiley13",  //noloc
                displayName: Localizer.DoStr("Smiley13"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)), //noloc
                    new IngredientElement(typeof (NailItem), 3, typeof(Skill)), //noloc
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley13Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2; // Defines how much experience is gained when crafted.

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Hewn Table"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Smiley13"), recipeType: typeof(Smiley13Recipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }



    // Smiley14

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley14Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley14Item);
        public override LocString DisplayName => Localizer.DoStr("Smiley14");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Smiley Sourire Espiègle")]
    [LocDescription("Un smiley au sourire espiègle, avec des yeux allongés en forme de demi-lune, exprimant malice et amusement.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(500)] // Defines how heavy HewnTable is.
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley14Item : WorldObjectItem<Smiley14Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }

    /// <summary>
    /// <para>Server side recipe definition for "HewnTable".</para>
    /// <para>More information about RecipeFamily objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.RecipeFamily.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>

    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    public partial class Smiley14Recipe : RecipeFamily
    {
        public Smiley14Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Smiley14",  //noloc
                displayName: Localizer.DoStr("Smiley14"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)), //noloc
                    new IngredientElement(typeof (NailItem), 3, typeof(Skill)), //noloc
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley14Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2; // Defines how much experience is gained when crafted.

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Hewn Table"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Smiley14"), recipeType: typeof(Smiley14Recipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }



    // Smiley15

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley15Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley15Item);
        public override LocString DisplayName => Localizer.DoStr("Smiley15");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Smiley Sourire Espiègle")]
    [LocDescription("Un smiley au sourire espiègle, avec des yeux allongés en forme de demi-lune, exprimant malice et amusement.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(500)] // Defines how heavy HewnTable is.
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley15Item : WorldObjectItem<Smiley15Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }

    /// <summary>
    /// <para>Server side recipe definition for "HewnTable".</para>
    /// <para>More information about RecipeFamily objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.RecipeFamily.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>

    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    public partial class Smiley15Recipe : RecipeFamily
    {
        public Smiley15Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Smiley15",  //noloc
                displayName: Localizer.DoStr("Smiley15"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)), //noloc
                    new IngredientElement(typeof (NailItem), 3, typeof(Skill)), //noloc
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley15Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2; // Defines how much experience is gained when crafted.

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Hewn Table"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Smiley15"), recipeType: typeof(Smiley15Recipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }



    // Smiley16

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley16Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley16Item);
        public override LocString DisplayName => Localizer.DoStr("Smiley16");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Smiley Sourire Espiègle")]
    [LocDescription("Un smiley au sourire espiègle, avec des yeux allongés en forme de demi-lune, exprimant malice et amusement.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(500)] // Defines how heavy HewnTable is.
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley16Item : WorldObjectItem<Smiley16Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }

    /// <summary>
    /// <para>Server side recipe definition for "HewnTable".</para>
    /// <para>More information about RecipeFamily objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.RecipeFamily.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>

    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    public partial class Smiley16Recipe : RecipeFamily
    {
        public Smiley16Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Smiley16",  //noloc
                displayName: Localizer.DoStr("Smiley16"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)), //noloc
                    new IngredientElement(typeof (NailItem), 3, typeof(Skill)), //noloc
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley16Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2; // Defines how much experience is gained when crafted.

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Hewn Table"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Smiley16"), recipeType: typeof(Smiley16Recipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }



    // Smiley17

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley17Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley17Item);
        public override LocString DisplayName => Localizer.DoStr("Smiley17");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Smiley Sourire Espiègle")]
    [LocDescription("Un smiley au sourire espiègle, avec des yeux allongés en forme de demi-lune, exprimant malice et amusement.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(500)] // Defines how heavy HewnTable is.
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley17Item : WorldObjectItem<Smiley17Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }

    /// <summary>
    /// <para>Server side recipe definition for "HewnTable".</para>
    /// <para>More information about RecipeFamily objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.RecipeFamily.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>

    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    public partial class Smiley17Recipe : RecipeFamily
    {
        public Smiley17Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Smiley17",  //noloc
                displayName: Localizer.DoStr("Smiley17"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)), //noloc
                    new IngredientElement(typeof (NailItem), 3, typeof(Skill)), //noloc
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley17Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2; // Defines how much experience is gained when crafted.

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Hewn Table"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Smiley17"), recipeType: typeof(Smiley17Recipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }



    // Smiley18

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley18Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley18Item);
        public override LocString DisplayName => Localizer.DoStr("Smiley18");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Smiley Sourire Espiègle")]
    [LocDescription("Un smiley au sourire espiègle, avec des yeux allongés en forme de demi-lune, exprimant malice et amusement.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(500)] // Defines how heavy HewnTable is.
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley18Item : WorldObjectItem<Smiley18Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }

    /// <summary>
    /// <para>Server side recipe definition for "HewnTable".</para>
    /// <para>More information about RecipeFamily objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.RecipeFamily.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>

    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    public partial class Smiley18Recipe : RecipeFamily
    {
        public Smiley18Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Smiley18",  //noloc
                displayName: Localizer.DoStr("Smiley18"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)), //noloc
                    new IngredientElement(typeof (NailItem), 3, typeof(Skill)), //noloc
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley18Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2; // Defines how much experience is gained when crafted.

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Hewn Table"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Smiley18"), recipeType: typeof(Smiley18Recipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }




    // Smiley19

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley19Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley19Item);
        public override LocString DisplayName => Localizer.DoStr("Smiley19");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Smiley Sourire Espiègle")]
    [LocDescription("Un smiley au sourire espiègle, avec des yeux allongés en forme de demi-lune, exprimant malice et amusement.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(500)] // Defines how heavy HewnTable is.
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley19Item : WorldObjectItem<Smiley19Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }

    /// <summary>
    /// <para>Server side recipe definition for "HewnTable".</para>
    /// <para>More information about RecipeFamily objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.RecipeFamily.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>

    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    public partial class Smiley19Recipe : RecipeFamily
    {
        public Smiley19Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Smiley19",  //noloc
                displayName: Localizer.DoStr("Smiley19"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)), //noloc
                    new IngredientElement(typeof (NailItem), 3, typeof(Skill)), //noloc
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley19Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2; // Defines how much experience is gained when crafted.

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Hewn Table"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Smiley19"), recipeType: typeof(Smiley19Recipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }



    // Smiley20

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley20Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley20Item);
        public override LocString DisplayName => Localizer.DoStr("Smiley20");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Smiley Sourire Espiègle")]
    [LocDescription("Un smiley au sourire espiègle, avec des yeux allongés en forme de demi-lune, exprimant malice et amusement.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(500)] // Defines how heavy HewnTable is.
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley20Item : WorldObjectItem<Smiley20Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }

    /// <summary>
    /// <para>Server side recipe definition for "HewnTable".</para>
    /// <para>More information about RecipeFamily objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.RecipeFamily.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>

    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    public partial class Smiley20Recipe : RecipeFamily
    {
        public Smiley20Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Smiley20",  //noloc
                displayName: Localizer.DoStr("Smiley20"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)), //noloc
                    new IngredientElement(typeof (NailItem), 3, typeof(Skill)), //noloc
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley20Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2; // Defines how much experience is gained when crafted.

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Hewn Table"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Smiley20"), recipeType: typeof(Smiley20Recipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }




    // Smiley21

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley21Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley21Item);
        public override LocString DisplayName => Localizer.DoStr("Smiley21");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Smiley Sourire Espiègle")]
    [LocDescription("Un smiley au sourire espiègle, avec des yeux allongés en forme de demi-lune, exprimant malice et amusement.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(500)] // Defines how heavy HewnTable is.
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley21Item : WorldObjectItem<Smiley21Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }

    /// <summary>
    /// <para>Server side recipe definition for "HewnTable".</para>
    /// <para>More information about RecipeFamily objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.RecipeFamily.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>

    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    public partial class Smiley21Recipe : RecipeFamily
    {
        public Smiley21Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Smiley21",  //noloc
                displayName: Localizer.DoStr("Smiley21"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)), //noloc
                    new IngredientElement(typeof (NailItem), 3, typeof(Skill)), //noloc
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley21Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2; // Defines how much experience is gained when crafted.

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Hewn Table"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Smiley21"), recipeType: typeof(Smiley21Recipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }




    // Smiley22

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley22Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley22Item);
        public override LocString DisplayName => Localizer.DoStr("Smiley22");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Smiley Sourire Espiègle")]
    [LocDescription("Un smiley au sourire espiègle, avec des yeux allongés en forme de demi-lune, exprimant malice et amusement.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(500)] // Defines how heavy HewnTable is.
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley22Item : WorldObjectItem<Smiley22Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }

    /// <summary>
    /// <para>Server side recipe definition for "HewnTable".</para>
    /// <para>More information about RecipeFamily objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.RecipeFamily.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>

    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    public partial class Smiley22Recipe : RecipeFamily
    {
        public Smiley22Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Smiley22",  //noloc
                displayName: Localizer.DoStr("Smiley22"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)), //noloc
                    new IngredientElement(typeof (NailItem), 3, typeof(Skill)), //noloc
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley22Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2; // Defines how much experience is gained when crafted.

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Hewn Table"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Smiley22"), recipeType: typeof(Smiley22Recipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }




    // Smiley23

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley23Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley23Item);
        public override LocString DisplayName => Localizer.DoStr("Smiley23");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Smiley Sourire Espiègle")]
    [LocDescription("Un smiley au sourire espiègle, avec des yeux allongés en forme de demi-lune, exprimant malice et amusement.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(500)] // Defines how heavy HewnTable is.
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley23Item : WorldObjectItem<Smiley23Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }

    /// <summary>
    /// <para>Server side recipe definition for "HewnTable".</para>
    /// <para>More information about RecipeFamily objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.RecipeFamily.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>

    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    public partial class Smiley23Recipe : RecipeFamily
    {
        public Smiley23Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Smiley23",  //noloc
                displayName: Localizer.DoStr("Smiley23"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)), //noloc
                    new IngredientElement(typeof (NailItem), 3, typeof(Skill)), //noloc
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley23Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2; // Defines how much experience is gained when crafted.

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Hewn Table"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Smiley23"), recipeType: typeof(Smiley23Recipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }




    // Smiley24

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley24Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley24Item);
        public override LocString DisplayName => Localizer.DoStr("Smiley24");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Smiley Sourire Espiègle")]
    [LocDescription("Un smiley au sourire espiègle, avec des yeux allongés en forme de demi-lune, exprimant malice et amusement.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(500)] // Defines how heavy HewnTable is.
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley24Item : WorldObjectItem<Smiley24Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }

    /// <summary>
    /// <para>Server side recipe definition for "HewnTable".</para>
    /// <para>More information about RecipeFamily objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.RecipeFamily.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>

    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smiley Sourire Espiègle")]
    public partial class Smiley24Recipe : RecipeFamily
    {
        public Smiley24Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Smiley24",  //noloc
                displayName: Localizer.DoStr("Smiley24"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)), //noloc
                    new IngredientElement(typeof (NailItem), 3, typeof(Skill)), //noloc
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley24Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2; // Defines how much experience is gained when crafted.

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Hewn Table"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Smiley24"), recipeType: typeof(Smiley24Recipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
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

