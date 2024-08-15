namespace Eco.Mods.TechTree
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Eco.Core.Items;
    using Eco.Gameplay.Auth;
    using Eco.Gameplay.Blocks;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Components.Auth;
    using Eco.Gameplay.DynamicValues;
    using Eco.Gameplay.Economy;
    using Eco.Gameplay.Housing;
    using Eco.Gameplay.Interactions;
    using Eco.Gameplay.Interactions.Interactors;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Items.Recipes;
    using Eco.Gameplay.Modules;
    using Eco.Gameplay.Minimap;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Occupancy;
    using Eco.Gameplay.Placement;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Property;
    using Eco.Gameplay.Skills;
    using Eco.Gameplay.Systems;
    using Eco.Gameplay.Systems.TextLinks;
    using Eco.Gameplay.Systems.NewTooltip;
    using Eco.Shared.IoC;
    using Eco.Shared;
    using Eco.Shared.Math;
    using Eco.Shared.Localization;
    using Eco.Shared.Serialization;
    using Eco.Shared.SharedTypes;
    using Eco.Shared.Utils;
    using Eco.Shared.View;
    using Eco.Shared.Items;
    using Eco.Shared.Networking;
    using Eco.World.Blocks;
    using Eco.Gameplay.Civics.Objects;
    using Eco.Gameplay.Settlements;
    using Eco.Core.Controller;
    using Eco.Core.Utils;
    using Eco.Gameplay.Components.Storage;
    using Eco.Gameplay.GameActions;
    using Village.Eco.Mods.Core;
    using System.Linq;

    [Serialized]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(PublicStorageComponent))]
    [RequireComponent(typeof(PropertyAuthComponent))]

    [Tag("Usable")]
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Boite aux lettres Item")]
    public partial class BoiteAuxLettresObject : WorldObject
    {
        public PublicStorageComponent Storage { get; set; }
        [Serialized] public bool UpLevers { get; set; }


        public override LocString DisplayName => Localizer.DoStr("Boite aux lettres");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            base.Initialize();
            Storage = this.GetComponent<PublicStorageComponent>();
            Storage.Initialize(20);
            //Storage.Storage.AddInvRestriction(new TagRestriction("Carcasse", "Spoiled Food"));
            Logger.SendLog(Criticity.Info, "FacteurMod", $"Has Content initialized");
        }

        [Interaction(InteractionTrigger.RightClick, "Up Lever")]
        public void UpLever(Player context, InteractionTriggerInfo interactionTriggerInfo, InteractionTarget interactionTarget)
        {
            var isAuthorized = ServiceHolder<IAuthManager>.Obj.IsAuthorized(this, context.User);

            if (isAuthorized)
            {
                UpLevers = !UpLevers;
            }
            else
            {
                context.ErrorLocStr("You Are Not Authorized To Do That");
                return;
            }
        }

        public override void Tick()
        {
            base.Tick();
            SetAnimatedState("UpLever", UpLevers);
        }





    }

    [Serialized]
    [LocDisplayName("Boite aux lettres")]
    [LocDescription("Le facteur est-il passé, j'éspère que ce n'est pas une facture. ")]
    [Ecopedia("Crafted Objects", "Storage", createAsSubPage: true)]
    [Weight(500)] // Defines how heavy SmallStockpile is.
    public partial class BoiteAuxLettresItem : WorldObjectItem<BoiteAuxLettresObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));

    }
    [RequiresSkill(typeof(CarpentrySkill), 1)]
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Boite aux Lettres Item")]
    public partial class BoiteAuxLettresRecipe : RecipeFamily
    {
        public BoiteAuxLettresRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "BoiteAuxLettres",  //noloc
                displayName: Localizer.DoStr("Boite aux Lettres"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new("WoodBoard", 10, typeof(CarpentrySkill), typeof(CarpentryLavishResourcesTalent)), //noloc
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<BoiteAuxLettresItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 3; // Defines how much experience is gained when crafted.

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(600, typeof(CarpentrySkill));

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(BoiteAuxLettresRecipe), start: 8, skillType: typeof(CarpentryFocusedSpeedTalent), typeof(CarpentryParallelSpeedTalent));

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Lumber Stockpile"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Boite aux Lettres"), recipeType: typeof(BoiteAuxLettresRecipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(CarpentryTableObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }

}
