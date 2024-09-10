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
    [RequireComponent(typeof(CustomTextComponent))]
    [RequireComponent(typeof(PublicStorageComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [Tag("Usable")]
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Container Vert 10m")]
    public partial class Shipping_02Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Shipping_02Item);
        public override LocString DisplayName => Localizer.DoStr("Container Vert 10m");
        public override TableTextureMode TableTexture => TableTextureMode.Metal;


        static Shipping_02Object()

        {
            var BlockOccupancyList = new List<BlockOccupancy>
            {
            new BlockOccupancy(new Vector3i(0, 0, 0)),
            new BlockOccupancy(new Vector3i(0, 0, 1)),
            new BlockOccupancy(new Vector3i(0, 0, 2)),
            new BlockOccupancy(new Vector3i(0, 0, 3)),
            new BlockOccupancy(new Vector3i(0, 0, 4)),
            new BlockOccupancy(new Vector3i(0, 0, 5)),
            new BlockOccupancy(new Vector3i(0, 0, 6)),
            new BlockOccupancy(new Vector3i(0, 0, 7)),
            new BlockOccupancy(new Vector3i(0, 0, 8)),
            new BlockOccupancy(new Vector3i(0, 0, 9)),
            new BlockOccupancy(new Vector3i(0, 1, 0)),
            new BlockOccupancy(new Vector3i(0, 1, 1)),
            new BlockOccupancy(new Vector3i(0, 1, 2)),
            new BlockOccupancy(new Vector3i(0, 1, 3)),
            new BlockOccupancy(new Vector3i(0, 1, 4)),
            new BlockOccupancy(new Vector3i(0, 1, 5)),
            new BlockOccupancy(new Vector3i(0, 1, 6)),
            new BlockOccupancy(new Vector3i(0, 1, 7)),
            new BlockOccupancy(new Vector3i(0, 1, 8)),
            new BlockOccupancy(new Vector3i(0, 1, 9)),
            new BlockOccupancy(new Vector3i(1, 0, 0)),
            new BlockOccupancy(new Vector3i(1, 0, 1)),
            new BlockOccupancy(new Vector3i(1, 0, 2)),
            new BlockOccupancy(new Vector3i(1, 0, 3)),
            new BlockOccupancy(new Vector3i(1, 0, 4)),
            new BlockOccupancy(new Vector3i(1, 0, 5)),
            new BlockOccupancy(new Vector3i(1, 0, 6)),
            new BlockOccupancy(new Vector3i(1, 0, 7)),
            new BlockOccupancy(new Vector3i(1, 0, 8)),
            new BlockOccupancy(new Vector3i(1, 0, 9)),
            new BlockOccupancy(new Vector3i(1, 1, 0)),
            new BlockOccupancy(new Vector3i(1, 1, 1)),
            new BlockOccupancy(new Vector3i(1, 1, 2)),
            new BlockOccupancy(new Vector3i(1, 1, 3)),
            new BlockOccupancy(new Vector3i(1, 1, 4)),
            new BlockOccupancy(new Vector3i(1, 1, 5)),
            new BlockOccupancy(new Vector3i(1, 1, 6)),
            new BlockOccupancy(new Vector3i(1, 1, 7)),
            new BlockOccupancy(new Vector3i(1, 1, 8)),
            new BlockOccupancy(new Vector3i(1, 1, 9)),
            };

            AddOccupancy<Shipping_02Object>(BlockOccupancyList);




        }


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            var storage = this.GetComponent<PublicStorageComponent>();
            this.GetComponent<PublicStorageComponent>().Initialize(64, 10000000);
            storage.Storage.AddInvRestriction(new StackLimitRestriction(14));
            this.GetComponent<LinkComponent>().Initialize(12);
            this.GetComponent<CustomTextComponent>().Initialize(700);
            this.ModsPostInitialize();
        }
        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Container Vert 10m")]
    [LocDescription("Assez grand pour y cacher une forêt... ou juste tout ce que vous ne savez plus où mettre. En vert, pour le côté \"écolo\", évidemment !")]
    [Ecopedia("Crafted Objects", "Storage", createAsSubPage: true)]
    [Weight(20000)]
    [MaxStackSize(10)]
    public partial class Shipping_02Item : WorldObjectItem<Shipping_02Object>
    {

    }

    [Ecopedia("Crafted Objects", "Storage", subPageName: "Container Vert 10m")]
    [RequiresSkill(typeof(MechanicsSkill), 1)]
    public partial class Shipping_02Recipe : RecipeFamily
    {
        public Shipping_02Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Container Vert 10m",  //noloc
                displayName: Localizer.DoStr("Container Vert 10m"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronPlateItem), 100, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)),
                    new IngredientElement(typeof(ScrewsItem), 50, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)),
                    new IngredientElement(typeof(IronBarItem), 30, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Shipping_02Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 50;

            this.LaborInCalories = CreateLaborInCaloriesValue(300, typeof(MechanicsSkill));

            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(Shipping_02Recipe), start: 15, skillType: typeof(MechanicsSkill), typeof(MechanicsFocusedSpeedTalent), typeof(MechanicsParallelSpeedTalent));

            this.ModsPreInitialize(); 
            this.Initialize(displayText: Localizer.DoStr("Container Vert 10m"), recipeType: typeof(Shipping_02Recipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(MachinistTableObject), recipe: this);
        }
        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }
}