// Le Village - Double porte de grange en bois

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
    [RequireComponent(typeof(OnOffComponent))]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(MinimapComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(LargeDoorUtils))]
    [Tag("Usable")]
    [Tag("LargeDoor")]
    [Ecopedia("Housing Objects", "Doors", subPageName: "Porte de Grange Large")]
    public partial class BarnDoorObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(BarnDoorItem);
        public override LocString DisplayName => Localizer.DoStr("Large Corrugated Steel Door");
        public override TableTextureMode TableTexture => TableTextureMode.Metal;
        public override bool HasTier => true;
        public override int Tier => 3;
        protected override void PostInitialize() => LargeDoorUtils.InitializeDoor(this);

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<MinimapComponent>().SetCategory(Localizer.DoStr("Doors"));
            this.ModsPostInitialize();
        }
        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Porte de Grange Large")]
    [LocDescription("Une porte de Grange Large.")]
    [IconGroup("World Object Minimap")]
    [Tier(4)]
    [Ecopedia("Housing Objects", "Doors", createAsSubPage: true)]
    [Weight(2000)]
    public partial class BarnDoorItem : WorldObjectItem<BarnDoorObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
    }

    [RequiresSkill(typeof(CarpentrySkill), 3)]
    [Ecopedia("Housing Objects", "Doors", subPageName: "Porte de Grange Large item")]
    public partial class BarnDoorRecipe : RecipeFamily
    {
        public BarnDoorRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "BarnDoor",  //noloc
                displayName: Localizer.DoStr("Porte de Grange Large"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("Lumber", 10, true), //noloc
                    new IngredientElement("WoodBoard", 4, true), //noloc
                    new IngredientElement(typeof(NailItem),8 , true),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<BarnDoorItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2.5f; 

            this.LaborInCalories = CreateLaborInCaloriesValue(480, typeof(CarpentrySkill));

            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(BarnDoorRecipe), start: 8, skillType: typeof(CarpentrySkill), typeof(CarpentryFocusedSpeedTalent), typeof(CarpentryParallelSpeedTalent));

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Porte de Grange Large"), recipeType: typeof(BarnDoorRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(SawmillObject), recipe: this);
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }
}
