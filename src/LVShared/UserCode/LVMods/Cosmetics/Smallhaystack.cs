//Le Village - Petit tas de Paille

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
    using System.ComponentModel;

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [Tag("Decoration")]
    [Ecopedia("Decoration", "Décoration pour Ingals", subPageName: "Petit tas de paille")]
    public partial class SmallhaystackObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(SmallhaystackItem);
        public override LocString DisplayName => Localizer.DoStr("Petit tas de paille");
        public override TableTextureMode TableTexture => TableTextureMode.Stone;
        static SmallhaystackObject()
        {
            var BlockOccupancyList = new List<BlockOccupancy>
            {
             new BlockOccupancy(new Vector3i(0, 0, 0)),

            };
            AddOccupancy<SmallhaystackObject>(BlockOccupancyList);
        }
        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Petit tas de paille")]
    [LocDescription("Petit tas de paille spécialement fait pour les Ingals.")]
    [Ecopedia("Decoration", "Décoration pour Ingals", createAsSubPage: true)]
    [Tag("Decoration")]
    [Weight(2000)]
    public partial class SmallhaystackItem : WorldObjectItem<SmallhaystackObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
    }

    //[RequiresSkill(typeof(FarmingSkill), 1)]
    //[Ecopedia("Decoration", "Décoration pour Ingals", subPageName: "Petit tas de paille")]
    //public partial class SmallhaystackRecipe : RecipeFamily
    //{
    //    public SmallhaystackRecipe()
    //    {
    //        var recipe = new Recipe();
    //        recipe.Init(
    //            name: "Petit tas de paille",  //noloc
    //            displayName: Localizer.DoStr("Petit tas de paille"),
    //            ingredients: new List<IngredientElement>
    //            {
    //                new IngredientElement(typeof(WheatItem), 6, typeof(FarmingSkill)),
    //            },

    //            items: new List<CraftingElement>
    //            {
    //                new CraftingElement<SmallhaystackItem>()
    //            });
    //        this.Recipes = new List<Recipe> { recipe };

    //        this.LaborInCalories = CreateLaborInCaloriesValue(180, typeof(FarmingSkill));
    //        this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(SmallhaystackRecipe), start: 2, skillType: typeof(FarmingSkill), typeof(FarmingFocusedSpeedTalent));

    //        this.ModsPreInitialize();
    //        this.Initialize(displayText: Localizer.DoStr("Petit tas de paille"), recipeType: typeof(SmallhaystackRecipe));
    //        this.ModsPostInitialize();

    //        CraftingComponent.AddRecipe(tableType: typeof(FarmersTableObject), recipe: this);
    //    }
    //    partial void ModsPreInitialize();
    //    partial void ModsPostInitialize();
    //}
}
