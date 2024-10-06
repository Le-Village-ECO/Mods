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
    [Ecopedia("Crafted Objects", "Storage", subPageName: " Boite aux lettre Tier 3 Item")]
    public partial class BoiteAuxLettresTier3Type1Object : WorldObject
    {
        public override LocString DisplayName => Localizer.DoStr(" Boite aux lettre Tier 3");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            var storage = this.GetComponent<PublicStorageComponent>();
            storage.Initialize(12);
            storage.Inventory.AddInvRestriction(new SpecificItemTypesRestriction(new System.Type[] { typeof(LettreItem) }));
            this.ModsPostInitialize();
        }
        static BoiteAuxLettresTier3Type1Object()
        {
            var BlockOccupancyList = new List<BlockOccupancy>
            {
            new BlockOccupancy(new Vector3i(0, 0, 0)),
            new BlockOccupancy(new Vector3i(0, 1, 0)),
            };
            AddOccupancy<BoiteAuxLettresTier3Type1Object>(BlockOccupancyList);
        }

        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName(" Boite aux lettre Tier 3")]
    [LocDescription("Le facteur est-il passé ? J'espère que ce n'est pas une facture !")]
    [Ecopedia("Crafted Objects", "Storage", createAsSubPage: true)]
    [Weight(500)] // Defines how heavy Boite aux lettre is.
    public partial class BoiteAuxLettresTier3Type1Item : WorldObjectItem<BoiteAuxLettresObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));


    }

    [RequiresSkill(typeof(BlacksmithSkill), 3)]
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Boite aux lettre Tier 2 en bois Item")]
    public partial class BoiteAuxLettresTier3Type1Recipe : RecipeFamily
    {
        public BoiteAuxLettresTier3Type1Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "BoiteAuxLettresTier3Type1",  //noloc
                displayName: Localizer.DoStr("Boite aux lettre Tier 2 en bois"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronBarItem), 16, typeof(BlacksmithSkill)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<BoiteAuxLettresTier3Type1Item>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.ExperienceOnCraft = 3;
            this.LaborInCalories = CreateLaborInCaloriesValue(600, typeof(BlacksmithSkill));
            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(BoiteAuxLettresTier3Type1Recipe), start: 10, skillType: typeof(BlacksmithFocusedSpeedTalent), typeof(BlacksmithParallelSpeedTalent));
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr(" Boite aux lettre Tier 3"), recipeType: typeof(BoiteAuxLettresTier3Type1Recipe));//définie une recette générique
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(BlacksmithTableItem), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }
}
