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
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(PaintableComponent))] 
    [Tag("Decoration")]
    [Ecopedia("Decoration", "Décoration standart", subPageName: "Niche pour chien")]
    public partial class DogKennelObject : WorldObject, IRepresentsItem
    {

        public virtual Type RepresentedItemType => typeof(DogKennelItem);
        public override LocString DisplayName => Localizer.DoStr("Niche pour chien");
        public override TableTextureMode TableTexture => TableTextureMode.Stone;

        static DogKennelObject()
        {
            var BlockOccupancyList = new List<BlockOccupancy>
            {
            // DogKennelObject
            new BlockOccupancy(new Vector3i(0, 0, 0)),
            new BlockOccupancy(new Vector3i(0, 0, 1)),
            new BlockOccupancy(new Vector3i(1, 0, 0)),
            new BlockOccupancy(new Vector3i(1, 0, 1)),

            };

            AddOccupancy<DogKennelObject>(BlockOccupancyList);

        }




        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Niche pour chien")]
    [LocDescription("Une petite niche pour chien ... ou pour celui qui veut.")]
    [Ecopedia("Decoration", "Décoration standart", createAsSubPage: true)]
    [Tag("Decoration")]
    [Weight(2000)] 
    public partial class DogKennelItem : WorldObjectItem<DogKennelObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));



    }
    [RequiresSkill(typeof(CarpentrySkill), 2)]
    [ForceCreateView]
    [Ecopedia("Decoration", "Décoration standart", subPageName: "Niche pour chien")]
    public partial class DogKennelRecipe : Recipe
    {
        public DogKennelRecipe()
        {
            this.Init(
                name: "Niche pour chien",  //noloc
                displayName: Localizer.DoStr("Niche pour chien"),
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("Wood", 6, typeof(CarpenterSkill), typeof(CarpentryLavishResourcesTalent)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<DogKennelItem>()
                });
            this.ModsPostInitialize();
            CraftingComponent.AddTagProduct(typeof(CarpentryTableObject), typeof(DogKennelRecipe), this);
        }


        partial void ModsPostInitialize();
    }
}
