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
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Palette")]
    public partial class PaletteObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(PaletteItem);
        public override LocString DisplayName => Localizer.DoStr("Palette");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        public override bool PlacesBlocks => false; 

        static PaletteObject()

        {
            var BlockOccupancyList = new List<BlockOccupancy>
            {

                  
            new BlockOccupancy(new Vector3i(0, 0, 0)),
            new BlockOccupancy(new Vector3i(0, 0, 1)),
            new BlockOccupancy(new Vector3i(0, 1, 0)),
            new BlockOccupancy(new Vector3i(0, 1, 1)),
            new BlockOccupancy(new Vector3i(0, 2, 0)),
            new BlockOccupancy(new Vector3i(0, 2, 1)),
            new BlockOccupancy(new Vector3i(0, 3, 0)),
            new BlockOccupancy(new Vector3i(0, 3, 1)),
            new BlockOccupancy(new Vector3i(0, 4, 0)),
            new BlockOccupancy(new Vector3i(0, 4, 1)),
            new BlockOccupancy(new Vector3i(1, 0, 0)),
            new BlockOccupancy(new Vector3i(1, 0, 1)),
            new BlockOccupancy(new Vector3i(1, 1, 0)),
            new BlockOccupancy(new Vector3i(1, 1, 1)),
            new BlockOccupancy(new Vector3i(1, 2, 0)),
            new BlockOccupancy(new Vector3i(1, 2, 1)),
            new BlockOccupancy(new Vector3i(1, 3, 0)),
            new BlockOccupancy(new Vector3i(1, 3, 1)),
            new BlockOccupancy(new Vector3i(1, 4, 0)),
            new BlockOccupancy(new Vector3i(1, 4, 1)),

            };

            AddOccupancy<PaletteObject>(BlockOccupancyList);

        


        }
			



        protected override void Initialize()
        {
            this.ModsPreInitialize();
            var storage = this.GetComponent<PublicStorageComponent>();
            this.GetComponent<StockpileComponent>().Initialize(new Vector3i(2, 4, 2));
            this.GetComponent<PublicStorageComponent>().Initialize(5, 5000000);
            storage.Storage.AddInvRestriction(new StackLimitRestriction(30));
            storage.Inventory.AddInvRestriction(new TagRestriction(new string[]
            {
                "Constructable",
            }));
            this.ModsPostInitialize();
        }


        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Palette")]
    [LocDescription("Parfaite pour empiler tout ce qui construit, sauf vos rêves ! Compacte, carrée, et prête à accueillir vos briques, planches et béton... sans se plaindre !")]
    [Ecopedia("Crafted Objects", "Storage", createAsSubPage: true)]
    [Weight(1000)]
    [MaxStackSize(10)]
    public partial class PaletteItem : WorldObjectItem<PaletteObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));

    }


    [RequiresSkill(typeof(CarpentrySkill), 1)]
    public partial class PaletteRecipe : RecipeFamily
    {
        public PaletteRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Palette", 
                displayName: Localizer.DoStr("Palette"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 15, typeof(CarpentrySkill), typeof(CarpentryLavishResourcesTalent)),
                },


                items: new List<CraftingElement>
                {
                    new CraftingElement<PaletteItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2;

            this.LaborInCalories = CreateLaborInCaloriesValue(250, typeof(CarpentrySkill));

            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(PaletteRecipe), start: 0.32f, skillType: typeof(CarpentrySkill), typeof(CarpentryFocusedSpeedTalent), typeof(CarpentryParallelSpeedTalent));

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Palette"), recipeType: typeof(PaletteRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(CarpentryTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }
}
