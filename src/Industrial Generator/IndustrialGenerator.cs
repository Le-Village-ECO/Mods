// Le Village

namespace Eco.Mods.TechTree
{
    using System;
    using System.Collections.Generic;
    using Eco.Core.Items;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Components.Auth;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Skills;
//    using Eco.Gameplay.Pipes.LiquidComponents;
    using Eco.Gameplay.Systems.Tooltip;
    using Eco.Shared.Math;
    using Eco.Shared.Localization;
    using Eco.Shared.Serialization;
    using Eco.Shared.Utils;
    using Eco.Shared.Items;
    using Eco.Gameplay.Housing.PropertyValues;
    using Eco.Gameplay.Items.Recipes;

    [Serialized]
    [RequireComponent(typeof(AirPollutionComponent))]
//    [RequireComponent(typeof(ChimneyComponent))]
//  [RequireComponent(typeof(LiquidProducerComponent))]
    [RequireComponent(typeof(AttachmentComponent))]
    [RequireComponent(typeof(OnOffComponent))]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(FuelSupplyComponent))]
    [RequireComponent(typeof(FuelConsumptionComponent))]
    [RequireComponent(typeof(PowerGridComponent))]
    [RequireComponent(typeof(PowerGeneratorComponent))]
//  [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(SolidAttachedSurfaceRequirementComponent))]
//  [RequireComponent(typeof(LiquidConverterComponent))]
    [PowerGenerator(typeof(ElectricPower))]
    [Ecopedia("Crafted Objects", "Power Generation", subPageName: "Industrial Generator Item")]
    public partial class IndustrialGeneratorObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(IndustrialGeneratorItem);
        public override LocString DisplayName => Localizer.DoStr("Industrial Generator");
        public override TableTextureMode TableTexture => TableTextureMode.Metal;
        private static string[] fuelTagList = new[] { "Liquid Fuel" }; //noloc

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<FuelSupplyComponent>().Initialize(2, fuelTagList);
            this.GetComponent<FuelConsumptionComponent>().Initialize(75);
            this.GetComponent<PowerGridComponent>().Initialize(30, new ElectricPower());
            this.GetComponent<PowerGeneratorComponent>().Initialize(16000);
//            this.GetComponent<HousingComponent>().HomeValue = IndustrialGeneratorItem.homeValue;
//           this.GetComponent<LiquidProducerComponent>().Setup(typeof(SmogItem), 1.2f, this.GetOccupancyType(BlockOccupancyType.ChimneyOut)); // NECESSAIRE
//            this.GetComponent<AirPollutionComponent>().Initialize(this.GetComponent<LiquidProducerComponent>());
            this.GetComponent<AirPollutionComponent>().Initialize(4.3f);
//            this.GetComponent<LiquidConverterComponent>().Setup(typeof(WaterItem), typeof(SewageItem), this.GetOccupancyType(BlockOccupancyType.WaterInputPort), this.GetOccupancyType(BlockOccupancyType.SewageOutputPort), 2, 0.9f);
            this.ModsPostInitialize();
        }

        static IndustrialGeneratorObject() => WorldObject.AddOccupancy<IndustrialGeneratorObject>(new List<BlockOccupancy>()
    {
      new BlockOccupancy(new Vector3i(-2, 0, -1)),
      new BlockOccupancy(new Vector3i(-2, 0, 0)),
      new BlockOccupancy(new Vector3i(-2, 0, 1)),
      new BlockOccupancy(new Vector3i(-1, 0, -1)),
      new BlockOccupancy(new Vector3i(-1, 0, 0)),
      new BlockOccupancy(new Vector3i(-1, 0, 1)),
      new BlockOccupancy(new Vector3i(0, 0, -1)),
      new BlockOccupancy(new Vector3i(0, 0, 0)),
      new BlockOccupancy(new Vector3i(0, 0, 1)),
      new BlockOccupancy(new Vector3i(1, 0, -1)),
      new BlockOccupancy(new Vector3i(1, 0, 0)),
      new BlockOccupancy(new Vector3i(1, 0, 1)),
      new BlockOccupancy(new Vector3i(-2, 1, -1)),
      new BlockOccupancy(new Vector3i(-2, 1, 0)),
      new BlockOccupancy(new Vector3i(-2, 1, 1)),
      new BlockOccupancy(new Vector3i(-1, 1, -1)),
      new BlockOccupancy(new Vector3i(-1, 1, 0)),
      new BlockOccupancy(new Vector3i(-1, 1, 1)),
      new BlockOccupancy(new Vector3i(0, 1, -1)),
      new BlockOccupancy(new Vector3i(0, 1, 0)),
      new BlockOccupancy(new Vector3i(0, 1, 1)),
      new BlockOccupancy(new Vector3i(1, 1, -1)),
      new BlockOccupancy(new Vector3i(1, 1, 0)),
      new BlockOccupancy(new Vector3i(1, 1, 1))
    });


        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Industrial Generator")]
    [Ecopedia("Crafted Objects", "Power Generation", createAsSubPage: true)]
//    [LiquidProducer(typeof(SmogItem), 1.2f)]
    [AirPollution(4.3f)]  //Le Village
    public partial class IndustrialGeneratorItem : WorldObjectItem<IndustrialGeneratorObject>
    {
        public override LocString DisplayDescription => Localizer.DoStr("Consumes most fuels to produce energy.");


        public override DirectionAxisFlags RequiresSurfaceOnSides { get;} = 0
                    | DirectionAxisFlags.Down
                ;
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            Category                 = HousingConfig.GetRoomCategory("Industrial"),
            TypeForRoomLimit         = Localizer.DoStr(""),
        };

        [Tooltip(7)] private LocString PowerConsumptionTooltip => Localizer.Do($"Consumes: {Text.Info(75)}w of {new ElectricPower().Name} power from fuel");
        [Tooltip(8)] private LocString PowerProductionTooltip  => Localizer.Do($"Produces: {Text.Info(16000)}w of {new ElectricPower().Name} power");
    }

    /// <summary>
    /// <para>Server side recipe definition for "IndustrialGenerator".</para>
    /// <para>More information about RecipeFamily objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.RecipeFamily.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>
    [RequiresSkill(typeof(IndustrySkill), 4)]
    [Ecopedia("Crafted Objects", "Power Generation", subPageName: "Industrial Generator Item")]
    public partial class IndustrialGeneratorRecipe : RecipeFamily
    {
        public IndustrialGeneratorRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "IndustrialGenerator",  //noloc
                displayName: Localizer.DoStr("Industrial Generator"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(SteelPlateItem), 48, typeof(IndustrySkill), typeof(IndustryLavishResourcesTalent)),
                    new IngredientElement(typeof(AdvancedCircuitItem), 32, typeof(IndustrySkill), typeof(IndustryLavishResourcesTalent)),
                    new IngredientElement(typeof(SteelPipeItem), 32, typeof(IndustrySkill), typeof(IndustryLavishResourcesTalent)),
                    new IngredientElement(typeof(RivetItem), 64, typeof(IndustrySkill), typeof(IndustryLavishResourcesTalent)),
                    new IngredientElement(typeof(AdvancedCombustionEngineItem), 1, true),
                    new IngredientElement(typeof(RadiatorItem), 24, true),
                    new IngredientElement(typeof(SteelAxleItem), 1, true),
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<IndustrialGeneratorItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 60; // Defines how much experience is gained when crafted.
            
            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(2400, typeof(IndustrySkill));

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(IndustrialGeneratorRecipe), start: 40, skillType: typeof(IndustrySkill), typeof(IndustryFocusedSpeedTalent), typeof(IndustryParallelSpeedTalent));

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Industrial Generator"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Industrial Generator"), recipeType: typeof(IndustrialGeneratorRecipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(RoboticAssemblyLineObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }
}
