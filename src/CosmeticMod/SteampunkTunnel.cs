// Le Village - Tunnel SteamPunk de 3 de large, 4 de long et 3 de haut

using Eco.Core.Items;
using Eco.Gameplay.Components;
using Eco.Gameplay.Components.Auth;
using Eco.Gameplay.Items;
using Eco.Gameplay.Items.Recipes;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Occupancy;
using Eco.Gameplay.Skills;
using Eco.Gameplay.Systems.NewTooltip;
using Eco.Shared.Items;
using Eco.Shared.Localization;
using Eco.Shared.Math;
using Eco.Shared.Serialization;
using Eco.Shared.Utils;
using System;
using System.Collections.Generic;
namespace Eco.Mods.TechTree
{
    [Serialized]
    [RequireComponent(typeof(OnOffComponent))]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(PowerGridComponent))]
    [RequireComponent(typeof(PowerConsumptionComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [Tag("Decoration")]
    [Ecopedia("Decoration", "SteamPunk", subPageName: "Tunnel SteamPunk Item")]
    public partial class TunnelSteamPunkObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(TunnelSteamPunkItem);
        public override LocString DisplayName => Localizer.DoStr("Tunnel SteamPunk");
        public override TableTextureMode TableTexture => TableTextureMode.Metal;
        static TunnelSteamPunkObject()
        {
            var BlockOccupancyList = new List<BlockOccupancy>
            {
                new BlockOccupancy(new Vector3i(0, 0, 0)),
                new BlockOccupancy(new Vector3i(0, 0, 1)),
                new BlockOccupancy(new Vector3i(0, 0, 2)),
                new BlockOccupancy(new Vector3i(0, 0, 3)),
                new BlockOccupancy(new Vector3i(0, 1, 0)),
                new BlockOccupancy(new Vector3i(0, 1, 1)),
                new BlockOccupancy(new Vector3i(0, 1, 2)),
                new BlockOccupancy(new Vector3i(0, 1, 3)),
                new BlockOccupancy(new Vector3i(0, 2, 0)),
                new BlockOccupancy(new Vector3i(0, 2, 1)),
                new BlockOccupancy(new Vector3i(0, 2, 2)),
                new BlockOccupancy(new Vector3i(0, 2, 3)),
                new BlockOccupancy(new Vector3i(1, 0, 0)),
                new BlockOccupancy(new Vector3i(1, 0, 1)),
                new BlockOccupancy(new Vector3i(1, 0, 2)),
                new BlockOccupancy(new Vector3i(1, 0, 3)),
                new BlockOccupancy(new Vector3i(1, 1, 0)),
                new BlockOccupancy(new Vector3i(1, 1, 1)),
                new BlockOccupancy(new Vector3i(1, 1, 2)),
                new BlockOccupancy(new Vector3i(1, 1, 3)),
                new BlockOccupancy(new Vector3i(1, 2, 0)),
                new BlockOccupancy(new Vector3i(1, 2, 1)),
                new BlockOccupancy(new Vector3i(1, 2, 2)),
                new BlockOccupancy(new Vector3i(1, 2, 3)),
                new BlockOccupancy(new Vector3i(2, 0, 0)),
                new BlockOccupancy(new Vector3i(2, 0, 1)),
                new BlockOccupancy(new Vector3i(2, 0, 2)),
                new BlockOccupancy(new Vector3i(2, 0, 3)),
                new BlockOccupancy(new Vector3i(2, 1, 0)),
                new BlockOccupancy(new Vector3i(2, 1, 1)),
                new BlockOccupancy(new Vector3i(2, 1, 2)),
                new BlockOccupancy(new Vector3i(2, 1, 3)),
                new BlockOccupancy(new Vector3i(2, 2, 0)),
                new BlockOccupancy(new Vector3i(2, 2, 1)),
                new BlockOccupancy(new Vector3i(2, 2, 2)),
                new BlockOccupancy(new Vector3i(2, 2, 3)),
            };

            AddOccupancy<TunnelSteamPunkObject>(BlockOccupancyList);
        }

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<PowerConsumptionComponent>().Initialize(50);
            this.GetComponent<PowerGridComponent>().Initialize(10, new ElectricPower());
            this.ModsPostInitialize();
        }
        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Tunnel SteamPunk")]
    [LocDescription("Un tunnel qui semble abandonné dans le style SteamPunk. Ce tunnel mesure 3 de large, 4 de long et 3 de haut")]
    [Ecopedia("Decoration", "SteamPunk", createAsSubPage: true)]
    [Tag("Decoration")]
    [Weight(7000)] 
    public partial class TunnelSteamPunkItem : WorldObjectItem<TunnelSteamPunkObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        [NewTooltip(CacheAs.SubType, 7)] public static LocString PowerConsumptionTooltip() => Localizer.Do($"Consumes: {Text.Info(100)}w of {new ElectricPower().Name} power.");
    }


    [RequiresSkill(typeof(MechanicsSkill), 4)]
    [Ecopedia("Decoration", "SteamPunk", subPageName: "Tunnel SteamPunk Item")]
    public partial class TunnelSteamPunkRecipe : RecipeFamily
    {
        public TunnelSteamPunkRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "TunnelSteamPunk",  //noloc
                displayName: Localizer.DoStr("Tunnel SteamPunk"),
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronBarItem), 12, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)),
                    new IngredientElement(typeof(CopperWiringItem), 2, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)),
                    new IngredientElement(typeof(IronPlateItem), 4, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)),
                },
                items: new List<CraftingElement>
                {
                    new CraftingElement<TunnelSteamPunkItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 4;

            this.LaborInCalories = CreateLaborInCaloriesValue(480, typeof(MechanicsSkill));

            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(TunnelSteamPunkRecipe), start: 5, skillType: typeof(MechanicsSkill), typeof(MechanicsFocusedSpeedTalent), typeof(MechanicsParallelSpeedTalent));

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Tunnel SteamPunk"), recipeType: typeof(TunnelSteamPunkRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(MachinistTableObject), recipe: this);
        }
        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }
}
