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
    [RequireComponent(typeof(OnOffComponent))]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(MinimapComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(LargeDoorUtils))]
    [Tag("Usable")]
    [Tag("LargeDoor")]
    [Ecopedia("Housing Objects", "Doors", subPageName: "Porte de Grange Large")]
    public partial class ShedDoorObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(ShedDoorItem);
        public override LocString DisplayName => Localizer.DoStr("Large Corrugated Steel Door");
        public override TableTextureMode TableTexture => TableTextureMode.Metal;
        public override bool HasTier => true;
        public override int Tier => 3;



        protected override void Initialize()
        {

            this.ModsPreInitialize();
            this.GetComponent<MinimapComponent>().SetCategory(Localizer.DoStr("Doors"));
            this.ModsPostInitialize();
            //protected override void PostInitialize() => LargeDoorUtils.InitializeDoor(this);
    }

        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Porte de Grange Large")]
    [LocDescription("Une porte de Grange Large.")]
    [IconGroup("World Object Minimap")]
    [Tier(4)]
    [Ecopedia("Housing Objects", "Doors", createAsSubPage: true)]
    [Weight(2000)]
    public partial class ShedDoorItem : WorldObjectItem<ShedDoorObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));

    }

    [RequiresSkill(typeof(CarpentrySkill), 3)]
    [Ecopedia("Housing Objects", "Doors", subPageName: "Porte de Grange Large item")]
    public partial class ShedDoorRecipe : RecipeFamily
    {
        public ShedDoorRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "ShedDoor",  //noloc
                displayName: Localizer.DoStr("Porte de Grange Large"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("Lumber", 10, true), //noloc
                    new IngredientElement("WoodBoard", 4, true), //noloc
                    new IngredientElement(typeof(NailItem),8 , true),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<ShedDoorItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2.5f; 

            this.LaborInCalories = CreateLaborInCaloriesValue(480, typeof(CarpentrySkill));

            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(ShedDoorRecipe), start: 8, skillType: typeof(CarpentrySkill), typeof(CarpentryFocusedSpeedTalent), typeof(CarpentryParallelSpeedTalent));

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Porte de Grange Large"), recipeType: typeof(ShedDoorRecipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(SawmillObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }
}
