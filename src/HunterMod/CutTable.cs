// Le Village - Nouvelle table de découpage pour le boucher

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
    using System.Runtime.Serialization;
    using static Eco.Gameplay.Components.PartsComponent;

    [Serialized]
    [RequireComponent(typeof(OnOffComponent))]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(MinimapComponent))]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(CraftingComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(PluginModulesComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PartsComponent))]
    [RequireRoomContainment]
    [RequireRoomVolume(18)]
    [RequireRoomMaterialTier(0.8f, typeof(ButcheryLavishReqTalent), typeof(ButcheryFrugalReqTalent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Kitchen", subPageName: "Table de découpe Item")]
    public partial class CutTableObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(CutTableItem);
        public override LocString DisplayName => Localizer.DoStr("Table de découpe");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<PartsComponent>().Config(() => this.GetComponent<CraftingComponent>().DecayDescription, new PartInfo[]
            {
            new() { TypeName = nameof(HookStoneItem), Quantity = 1},
            });

            this.GetComponent<MinimapComponent>().SetCategory(Localizer.DoStr("Cooking"));
            this.GetComponent<HousingComponent>().HomeValue = CutTableItem.homeValue;
            this.ModsPostInitialize();
        }
      

        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Table de découpe")]
    [LocDescription("En ce concentrant sur la peau, la viande est de moins bonne qualité")]
    [IconGroup("World Object Minimap")]
    [Ecopedia("Housing Objects", "Kitchen", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(2000)] // Défini le poids de la table de découpe.
    [AllowPluginModules(Tags = new[] { "BasicUpgrade" }, ItemTypes = new[] { typeof(ButcheryUpgradeItem)})] //noloc
    public partial class CutTableItem : WorldObjectItem<CutTableObject>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName = typeof(CutTableObject).UILink(),
            Category = HousingConfig.GetRoomCategory("Kitchen"),
            BaseValue = 2,
            TypeForRoomLimit = Localizer.DoStr("Food Preparation"),
            DiminishingReturnMultiplier = 0.5f
        };

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }

    /// <summary>
    /// <para>Server side recipe definition for "CutTable".</para>
    /// <para>More information about RecipeFamily objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.RecipeFamily.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>
    [RequiresSkill(typeof(LoggingSkill), 1)]
    [Ecopedia("Housing Objects", "Kitchen", subPageName: "Table de découpe Item")]
    public partial class CutTableRecipe : RecipeFamily
    {
        public CutTableRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Table de découpe ",  //noloc
                displayName: Localizer.DoStr("Table de découpe"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("HewnLog", 10, typeof(LoggingSkill)), //noloc
                    new IngredientElement("WoodBoard", 20, typeof(LoggingSkill)), //noloc
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<CutTableItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 3; // Defines how much experience is gained when crafted.

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(300, typeof(LoggingSkill));

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(CutTableRecipe), start: 2, skillType: typeof(LoggingSkill));

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Cut Table"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Table de découpe"), recipeType: typeof(CutTableRecipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(CarpentryTableObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }
}
