using Eco.Core.Items;
using Eco.Gameplay.Components;
using Eco.Gameplay.Components.Auth;
using Eco.Gameplay.Components.Storage;
using Eco.Gameplay.Items;
using Eco.Gameplay.Items.Recipes;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Occupancy;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Items;
using Eco.Shared.Localization;
using Eco.Shared.Math;
using Eco.Shared.Serialization;
using Eco.Shared.SharedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Village.Eco.Mods.Templates
{
    #region Object
    [RequireComponent(typeof(PublicStorageComponent))]
    [RequireComponent(typeof(StockpileComponent))]
    [RequireComponent(typeof(WorldStockpileComponent))]
    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [Tag("Usable")]
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Stockage Template Item")]
    public partial class Shipping_20ftObject : WorldObject, IRepresentsItem
    {
        public static readonly Vector3i DefaultDim = new Vector3i(2, 3, 2); // Taille du stockage dans le monde
        public override TableTextureMode TableTexture => TableTextureMode.Wood;
        protected override void OnCreatePostInitialize()
        {
            base.OnCreatePostInitialize();
            StockpileComponent.ClearPlacementArea(this.Creator, this.Position3i, DefaultDim, this.Rotation);
        }
        protected override void PostInitialize()
        {
            base.PostInitialize();

            this.GetComponent<StockpileComponent>().Initialize(DefaultDim);

            var storage = this.GetComponent<PublicStorageComponent>();
            storage.Initialize(DefaultDim.x * DefaultDim.z);
            storage.Storage.AddInvRestriction(new StockpileStackRestriction(DefaultDim.y)); // limit stack sizes to the y-height of the stockpile
        }

        public override InteractionTargetPriority TargetPriority => InteractionTargetPriority.Medium;
        public virtual Type RepresentedItemType => typeof(TinyStockpileItem);
        public override LocString DisplayName => Localizer.DoStr("Stockage Template");
        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }
        partial void ModsPreInitialize();
        partial void ModsPostInitialize();

    }
    #endregion

    #region Item
    [Serialized]
    [LocDisplayName("Stockage Template")]
    [LocDescription("Description du stockage template.")]
    [Ecopedia("Crafted Objects", "Storage", createAsSubPage: true)]
    [Weight(500)]
    public partial class Shipping_20ftItem : WorldObjectItem<Shipping_20ftObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));

    }
    #endregion

    #region Recipe
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Stockage Template Item")]
    public partial class Shipping_20ftRecipe : RecipeFamily
    {
        public Shipping_20ftRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Shipping_20ft",  //noloc
                displayName: Localizer.DoStr("Stockage Template"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("Wood", 2,typeof(Skill)), //noloc
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Shipping_20ftItem>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.LaborInCalories = CreateLaborInCaloriesValue(5);
            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Stockage Template"), recipeType: typeof(Shipping_20ftRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(WorkbenchObject), recipe: this);
        }
        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }
    #endregion
}