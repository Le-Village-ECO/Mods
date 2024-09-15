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
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(ModularStockpileComponent))]
    [RequireComponent(typeof(PublicStorageComponent))]
    [Tag("Usable")]
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Support à Outils Mural")]
    public partial class StockageOutilsObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(StockageOutilsItem);
        public override LocString DisplayName => Localizer.DoStr("Support à Outils Mural");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            var storage = this.GetComponent<PublicStorageComponent>();
            this.GetComponent<StockpileComponent>().Initialize(new Vector3i(6, 3, 2));
            this.GetComponent<PublicStorageComponent>().Initialize(12, 5000000);
            storage.Storage.AddInvRestriction(new StackLimitRestriction(40));
            storage.Inventory.AddInvRestriction(new TagRestriction(new string[]
            {
                "Tool",
            }));
            this.ModsPostInitialize();
        }


        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Support à Outils Mural")]
    [LocDescription("Gagnez de la place en rangeant vos outils avec style. Le désordre, c'est fini !")]
    [Ecopedia("Crafted Objects", "Storage", createAsSubPage: true)]
    [Weight(1000)]
    [MaxStackSize(10)]
    public partial class StockageOutilsItem : WorldObjectItem<StockageOutilsObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }


    [RequiresSkill(typeof(CarpentrySkill), 1)]
    public partial class StockageOutilsRecipe : RecipeFamily
    {
        public StockageOutilsRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Support à Outils Mural",
                displayName: Localizer.DoStr("Support à Outils Mural"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("HewnLog", 10, typeof(CarpentrySkill), typeof(CarpentryLavishResourcesTalent)),
                    new IngredientElement(typeof(IronBarItem), 10, typeof(BlacksmithSkill)),
                },


                items: new List<CraftingElement>
                {
                    new CraftingElement<StockageOutilsItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2;

            this.LaborInCalories = CreateLaborInCaloriesValue(250, typeof(CarpentrySkill));

            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(StockageOutilsRecipe), start: 0.32f, skillType: typeof(CarpentrySkill), typeof(CarpentryFocusedSpeedTalent), typeof(CarpentryParallelSpeedTalent));

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Support à Outils Mural"), recipeType: typeof(StockageOutilsRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(CarpentryTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }

}