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
    [RequireComponent(typeof(PowerGridComponent))]
    [RequireComponent(typeof(PowerConsumptionComponent))]
    [RequireComponent(typeof(PluginModulesComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireRoomContainment]
    [RequireComponent(typeof(PartsComponent))]
    [RequireRoomVolume(24)]
    [RequireRoomMaterialTier(2.8f, typeof(ButcheryLavishReqTalent), typeof(ButcheryFrugalReqTalent))]
    [Tag("Usable")]
    [Ecopedia("Work Stations", "Craft Tables", subPageName: "Table de boucherie Industrielle Item")]
    public partial class IndustrialButcheryTableObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(IndustrialButcheryTableItem);
        public override LocString DisplayName => Localizer.DoStr("Table de boucherie Industrielle");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<PartsComponent>().Config(() => this.GetComponent<CraftingComponent>().DecayDescription, new PartInfo[]
            {
            new() { TypeName = nameof(IronSawBladeItem), Quantity = 1},
            });
            this.GetComponent<PowerConsumptionComponent>().Initialize(100);
            this.GetComponent<PowerGridComponent>().Initialize(10, new ElectricPower());
            this.GetComponent<MinimapComponent>().SetCategory(Localizer.DoStr("Crafting"));
            this.GetComponent<HousingComponent>().HomeValue = IndustrialButcheryTableItem.homeValue;
            this.ModsPostInitialize();
        }


        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Table de boucherie Industrielle")]
    [LocDescription("Le nec plus ultra de la boucherie")]
    [IconGroup("World Object Minimap")]
    [Ecopedia("Housing Objects", "Kitchen", createAsSubPage: true)]
    [Tag("Usable")]
    [Weight(2000)] // Défini le poids de la Table de boucherie Industrielle.
    [AllowPluginModules(Tags = new[] { "AdvancedUpgrade" }, ItemTypes = new[] { typeof(AdvancedButcheryUpgradeItem) })] //noloc
    public partial class IndustrialButcheryTableItem : WorldObjectItem<IndustrialButcheryTableObject>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName = typeof(IndustrialButcheryTableObject).UILink(),
            Category = HousingConfig.GetRoomCategory("Industrial"),
            TypeForRoomLimit = Localizer.DoStr(""),

        };
        [NewTooltip(CacheAs.SubType, 7)] public static LocString PowerConsumptionTooltip() => Localizer.Do($"Consumes: {Text.Info(100)}w of {new ElectricPower().Name} power.");

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }

    /// <summary>
    /// <para>Server side recipe definition for "IndustrialButcheryTable".</para>
    /// <para>More information about RecipeFamily objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.RecipeFamily.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>
    [RequiresSkill(typeof(MechanicsSkill), 2)]
    [Ecopedia("Housing Objects", "Kitchen", subPageName: "Table de boucherie Industrielle Item")]
    public partial class IndustrialButcheryTableRecipe : RecipeFamily
    {
        public IndustrialButcheryTableRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Table de boucherie Industrielle ",  //noloc
                displayName: Localizer.DoStr("Table de boucherie Industrielle"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(LightBulbItem),1, typeof(MechanicsLavishResourcesTalent)), //noloc
                    new IngredientElement(typeof(CopperPlateItem), 4, typeof(MechanicsLavishResourcesTalent)),
                    new IngredientElement(typeof(RivetItem), 8, typeof(MechanicsLavishResourcesTalent)),//noloc
                    
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<IndustrialButcheryTableItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 3; // Defines how much experience is gained when crafted.

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(500, typeof(MechanicsSkill));

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(IndustrialButcheryTableRecipe), start: 10, skillType: typeof(MechanicsSkill), typeof(MechanicsFocusedSpeedTalent));

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Cut Table"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Table de boucherie Industrielle"), recipeType: typeof(IndustrialButcheryTableRecipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(ElectricMachinistTableObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }
}
