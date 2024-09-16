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

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(FakePlantComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))] 
    [RequireRoomVolume(4)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Decoration", subPageName: "No name")]
    public partial class Jardiniere_01Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Jardiniere_01Item);
        public override LocString DisplayName => Localizer.DoStr("No name");
        public override TableTextureMode TableTexture => TableTextureMode.Brick;



        static Jardiniere_01Object()

        {
            var BlockOccupancyList = new List<BlockOccupancy>
            {


            new BlockOccupancy(new Vector3i(0, 0, 0)),
            new BlockOccupancy(new Vector3i(1, 0, 0)),
            };

            AddOccupancy<Jardiniere_01Object>(BlockOccupancyList);
        }


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<HousingComponent>().HomeValue = Jardiniere_01Item.homeValue;
            this.GetComponent<FakePlantComponent>().Initialize();
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("No name")]
    [LocDescription("No description")]
    [Ecopedia("Housing Objects", "Decoration", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(1000)] 
    [Tag(nameof(SurfaceTags.CanBeOnTableTop))]
    public partial class Jardiniere_01Item : WorldObjectItem<Jardiniere_01Object>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName = typeof(Jardiniere_01Object).UILink(),
            Category = HousingConfig.GetRoomCategory("Decoration"),
            BaseValue = 1.5f,
            TypeForRoomLimit = Localizer.DoStr("Decoration"),
            DiminishingReturnMultiplier = 0.4f

        };

    }


    [RequiresSkill(typeof(PotterySkill), 3)]
    [Ecopedia("Housing Objects", "Decoration", subPageName: "No name")]
    public partial class Jardiniere_01Recipe : RecipeFamily
    {
        public Jardiniere_01Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "No name",  
                displayName: Localizer.DoStr("No name"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(ClayItem), 4, typeof(PotterySkill), typeof(PotteryLavishResourcesTalent)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Jardiniere_01Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 1; 

            this.LaborInCalories = CreateLaborInCaloriesValue(45, typeof(PotterySkill));

            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(Jardiniere_01Recipe), start: 2, skillType: typeof(PotterySkill), typeof(PotteryFocusedSpeedTalent), typeof(PotteryParallelSpeedTalent));

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("No name"), recipeType: typeof(Jardiniere_01Recipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(PotteryTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }
}