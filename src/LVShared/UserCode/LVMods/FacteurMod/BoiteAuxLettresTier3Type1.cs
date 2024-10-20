// Le Village - La boite aux lettre Tier3

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
using System.Collections.Generic;

namespace Village.Eco.Mods.FacteurMod
{
    [Serialized]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(BalComponent))]
    [RequireComponent(typeof(PublicStorageComponent))]

    [Tag("Usable")]
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Boite aux lettres Tier3 Item")]
    public partial class BoiteAuxLettresTier3Object : WorldObject
    {
        public override LocString DisplayName => Localizer.DoStr("Boite aux lettres Tier3");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }
        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();

        protected override void PostInitialize()
        {
            base.PostInitialize();

            var storage = this.GetComponent<PublicStorageComponent>();
            storage.Initialize(5);  //Nombre d'emplacement de lettres dans la boite
        }
    }

    [Serialized]
    [LocDisplayName("Boite aux lettres Tier3")]
    [LocDescription("Le facteur est-il passé ? J'espère que ce n'est pas une facture !")]
    [Ecopedia("Crafted Objects", "Storage", createAsSubPage: true)]
    [Weight(500)] // Defines how heavy Boite aux lettre is.
    public partial class BoiteAuxLettresTier3Item : WorldObjectItem<BoiteAuxLettresTier3Object>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
    }

    [RequiresSkill(typeof(CarpentrySkill), 1)]
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Boite aux Lettres Tier3 Item")]
    public partial class BoiteAuxLettresTier3Recipe : RecipeFamily
    {
        public BoiteAuxLettresTier3Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "BoiteAuxLettresTier3",  //noloc
                displayName: Localizer.DoStr("Boite aux Lettres Tier3"),

                ingredients: new List<IngredientElement>
                {
                    new("WoodBoard", 10, typeof(CarpentrySkill), typeof(CarpentryLavishResourcesTalent)), //noloc
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<BoiteAuxLettresTier3Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            
            this.ExperienceOnCraft = 3;
            this.LaborInCalories = CreateLaborInCaloriesValue(600, typeof(CarpentrySkill));
            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(BoiteAuxLettresTier3Recipe), start: 8, skillType: typeof(CarpentryFocusedSpeedTalent), typeof(CarpentryParallelSpeedTalent));

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Boite aux Lettres Tier3"), recipeType: typeof(BoiteAuxLettresTier3Recipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(CarpentryTableObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }


}
