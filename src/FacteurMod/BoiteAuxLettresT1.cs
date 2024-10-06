// Le Village - La boite aux lettre Tiers 1

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
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Boite aux lettres Tier 1 Item")]
    public partial class BoiteAuxLettresTier1Object : WorldObject
    {
        public override LocString DisplayName => Localizer.DoStr("Boite aux lettres Tier 1");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            var storage = this.GetComponent<PublicStorageComponent>();
            storage.Initialize(4);
            storage.Inventory.AddInvRestriction(new SpecificItemTypesRestriction(new System.Type[] { typeof(LettreItem) }));
            this.ModsPostInitialize();
        }
        static BoiteAuxLettresTier1Object()
        {
            var BlockOccupancyList = new List<BlockOccupancy>
            {
            new BlockOccupancy(new Vector3i(0, 0, 0)),
            new BlockOccupancy(new Vector3i(0, 1, 0)),
            };
            AddOccupancy<BoiteAuxLettresTier1Object>(BlockOccupancyList);
        }

        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Boite aux lettres Tier 1")]
    [LocDescription("Le facteur est-il passé ? J'espère que ce n'est pas une facture !")]
    [Ecopedia("Crafted Objects", "Storage", createAsSubPage: true)]
    [Weight(500)] // Defines how heavy Boite aux lettre is.
    public partial class BoiteAuxLettresTier1Item : WorldObjectItem<BoiteAuxLettresObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));


    }

    [RequiresSkill(typeof(SurvivalistSkill), 1)]
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Boite aux Lettres Item")]
    public partial class BoiteAuxLettresTier1Recipe : RecipeFamily
    {
        public BoiteAuxLettresTier1Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "BoiteAuxLettresTier1",  //noloc
                displayName: Localizer.DoStr("Boite aux Lettres"),

                ingredients: new List<IngredientElement>
                {
                    new("Wood", 10, typeof(SurvivalistSkill)), //noloc
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<BoiteAuxLettresTier1Item>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.ExperienceOnCraft = 3;
            this.LaborInCalories = CreateLaborInCaloriesValue(600, typeof(SurvivalistSkill));
            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(BoiteAuxLettresTier1Item), start: 8, skillType: typeof(SurvivalistSkill));
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Boite aux Lettres Tier 1"), recipeType: typeof(BoiteAuxLettresRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(WorkbenchObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }
}
