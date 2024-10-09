//Le Village - Grand tas de Paille

namespace Eco.Mods.TechTree
{
    using Eco.Core.Items;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Components.Auth;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Items.Recipes;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Occupancy;
    using Eco.Gameplay.Skills;
    using Eco.Shared.Items;
    using Eco.Shared.Localization;
    using Eco.Shared.Math;
    using Eco.Shared.Serialization;
    using System;
    using System.Collections.Generic;

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [Tag("Decoration")]
    [Ecopedia("Decoration", "Décoration pour Ingals", subPageName: "Grand tas de paille")]
    public partial class LargehaystackObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(LargehaystackItem);
        public override LocString DisplayName => Localizer.DoStr("Grand tas de paille");
        public override TableTextureMode TableTexture => TableTextureMode.Stone;
        static LargehaystackObject()
        {
            var BlockOccupancyList = new List<BlockOccupancy>
            {
            new BlockOccupancy(new Vector3i(0, 0, 0)),
            new BlockOccupancy(new Vector3i(0, 0, 1)),
            new BlockOccupancy(new Vector3i(0, 1, 0)),
            new BlockOccupancy(new Vector3i(0, 1, 1)),

            };
            AddOccupancy<LargehaystackObject>(BlockOccupancyList);
        }
        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Grand tas de paille")]
    [LocDescription("Grand tas de paille spécialement fait pour les Ingals.")]
    [Ecopedia("Decoration", "Décoration pour Ingals", createAsSubPage: true)]
    [Tag("Decoration")]
    [Weight(2000)]
    public partial class LargehaystackItem : WorldObjectItem<LargehaystackObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
    }

    //[RequiresSkill(typeof(FarmingSkill), 5)]
    //[Ecopedia("Decoration", "Décoration pour Ingals", subPageName: "Grand tas de paille")]
    //public partial class LargehaystackRecipe : RecipeFamily
    //{
    //    public LargehaystackRecipe()
    //    {
    //        var recipe = new Recipe();
    //        recipe.Init(
    //            name: "Grand tas de paille",  //noloc
    //            displayName: Localizer.DoStr("Grand tas de paille"),
    //            ingredients: new List<IngredientElement>
    //            {
    //                new IngredientElement(typeof(WheatItem), 24, typeof(FarmingSkill)),
    //            },

    //            items: new List<CraftingElement>
    //            {
    //                new CraftingElement<LargehaystackItem>()
    //            });
    //        this.Recipes = new List<Recipe> { recipe };

    //        this.LaborInCalories = CreateLaborInCaloriesValue(180, typeof(FarmingSkill));
    //        this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(LargehaystackRecipe), start: 2, skillType: typeof(FarmingSkill), typeof(FarmingFocusedSpeedTalent));

    //        this.ModsPreInitialize();
    //        this.Initialize(displayText: Localizer.DoStr("Grand tas de paille"), recipeType: typeof(LargehaystackRecipe));
    //        this.ModsPostInitialize();

    //        CraftingComponent.AddRecipe(tableType: typeof(FarmersTableObject), recipe: this);
    //    }
    //    partial void ModsPreInitialize();
    //    partial void ModsPostInitialize();
    //}
}