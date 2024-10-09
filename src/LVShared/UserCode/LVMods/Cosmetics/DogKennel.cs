//Le Village - La niche !

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
    [Ecopedia("Decoration", "Décoration standard", subPageName: "Niche pour chien")]
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
    [LocDescription("Une petite niche pour chien... ou pour celui qui veut.")]
    [Ecopedia("Decoration", "Décoration standart", createAsSubPage: true)]
    [Tag("Decoration")]
    [Weight(2000)]  
    public partial class DogKennelItem : WorldObjectItem<DogKennelObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
    }

    [RequiresSkill(typeof(CarpentrySkill), 2)]
    [Ecopedia("Decoration", "Décoration standart", subPageName: "Niche pour chien")]
    public partial class DogKennelRecipe : RecipeFamily
    {
        public DogKennelRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Niche pour chien",  //noloc
                displayName: Localizer.DoStr("Niche pour chien"),
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 6, typeof(CarpentrySkill), typeof(CarpentryLavishResourcesTalent)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<DogKennelItem>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.LaborInCalories = CreateLaborInCaloriesValue(180, typeof(CarpentrySkill));
            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(DogKennelRecipe), start: 2, skillType: typeof(CarpentrySkill), typeof(CarpentryFocusedSpeedTalent));

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Niche pour chien"), recipeType: typeof(DogKennelRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(CarpentryTableObject), recipe: this);
        }
        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }
}
