// Le Village - Table de recherche T2

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
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Property;
    using Eco.Gameplay.Skills;
    using Eco.Gameplay.Systems.TextLinks;
    using Eco.Gameplay.Pipes.LiquidComponents;
    using Eco.Gameplay.Pipes.Gases;
    using Eco.Gameplay.Systems.Tooltip;
    using Eco.Shared;
    using Eco.Shared.Math;
    using Eco.Shared.Localization;
    using Eco.Shared.Serialization;
    using Eco.Shared.Utils;
    using Eco.Shared.View;
    using Eco.Shared.Items;
    using Eco.Gameplay.Pipes;
    using Eco.World.Blocks;
    using Eco.Gameplay.Housing.PropertyValues;
    using Eco.Gameplay.Civics.Objects;
    using Eco.Gameplay.Settlements;
    using Eco.Gameplay.Systems.NewTooltip;
    using Eco.Core.Controller;

    [Serialized]
    [RequireComponent(typeof(OnOffComponent))]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(MinimapComponent))]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(CraftingComponent))]
    [RequireComponent(typeof(SolidAttachedSurfaceRequirementComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireRoomContainment]
    [RequireRoomVolume(25)]
    [RequireRoomMaterialTier(1.8f, typeof(CampfireCookingLavishReqTalent), typeof(CampfireCookingFrugalReqTalent))]
    [Ecopedia("Work Stations", "Researching", subPageName: "Research Table T2 Item")]
    public partial class ResearchTableT2Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(ResearchTableT2Item);
        public override LocString DisplayName => Localizer.DoStr("Research Table T2");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<MinimapComponent>().SetCategory(Localizer.DoStr("Research"));
            this.ModsPostInitialize();
        }

        static ResearchTableT2Object() => WorldObject.AddOccupancy<ResearchTableT2Object>(new List<BlockOccupancy>()
    {
      new BlockOccupancy(new Vector3i(0, 0, -2)),
      new BlockOccupancy(new Vector3i(0, 0, -1)),
      new BlockOccupancy(new Vector3i(0, 0, 0)),
      new BlockOccupancy(new Vector3i(0, 0, 1)),
      new BlockOccupancy(new Vector3i(-1, 0, -2)),
      new BlockOccupancy(new Vector3i(-1, 0, -1)),
      new BlockOccupancy(new Vector3i(-1, 0, 0)),
      new BlockOccupancy(new Vector3i(-1, 0, 1)),
      new BlockOccupancy(new Vector3i(0, 1, -2)),
      new BlockOccupancy(new Vector3i(0, 1, -1)),
      new BlockOccupancy(new Vector3i(0, 1, 0)),
      new BlockOccupancy(new Vector3i(0, 1, 1)),
      new BlockOccupancy(new Vector3i(-1, 1, -2)),
      new BlockOccupancy(new Vector3i(-1, 1, -1)),
      new BlockOccupancy(new Vector3i(-1, 1, 0)),
      new BlockOccupancy(new Vector3i(-1, 1, 1))
    });

        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Research Table T2")]
    [Ecopedia("Work Stations", "Researching", createAsSubPage: true)]
    public partial class ResearchTableT2Item : WorldObjectItem<ResearchTableT2Object>, IPersistentData
    {
        public override LocString DisplayDescription => Localizer.DoStr("A basic table for researching new technologies and skills.");


        public override DirectionAxisFlags RequiresSurfaceOnSides { get; } = 0
                    | DirectionAxisFlags.Down
                ;

        [Serialized, SyncToView, TooltipChildren, NewTooltipChildren(CacheAs.Instance)] public object PersistentData { get; set; }
    }

    /// <summary>
    /// <para>Server side recipe definition for "ResearchTableT2".</para>
    /// <para>More information about RecipeFamily objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.RecipeFamily.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>
    [Ecopedia("Work Stations", "Researching", subPageName: "Research Table T2 Item")]
    public partial class ResearchTableT2Recipe : RecipeFamily
    {
        public ResearchTableT2Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "ResearchTableT2",  //noloc
                displayName: Localizer.DoStr("Research Table T2"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronBarItem), 3),
                    new IngredientElement("Fabric", 3),
                    new IngredientElement("Wood", 3), //noloc
                    new IngredientElement(typeof(BookTier1Item), 1),
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<ResearchTableT2Item>()
                });
            this.Recipes = new List<Recipe> { recipe };

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(250);

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(5);

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Research Table T2"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Research Table T2"), recipeType: typeof(ResearchTableT2Recipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(WorkbenchObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }
}
