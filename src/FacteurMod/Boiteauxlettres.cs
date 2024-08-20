// Le Village - La boite aux lettre

using Eco.Core.Items;
using Eco.Gameplay.Components;
using Eco.Gameplay.Components.Auth;
using Eco.Gameplay.Components.Storage;
using Eco.Gameplay.Items;
using Eco.Gameplay.Items.Recipes;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Occupancy;
using Eco.Gameplay.Players;
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
//    [RequireComponent(typeof(PublicStorageComponent))]
    [RequireComponent(typeof(PropertyAuthComponent))]
//  [RequireComponent(typeof(StatusComponent))]
    [RequireComponent(typeof(BalComponent))]

    [Tag("Usable")]
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Boite aux lettres Item")]
    public partial class BoiteAuxLettresObject : WorldObject
    {
/*
       //Pour gérer le stockage
        public PublicStorageComponent Storage { get; set; }

        //Pour gerer le status enabled/disabled et informer le joueur
        private StatusElement status;
*/
        public override LocString DisplayName => Localizer.DoStr("Boite aux lettres");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            //base.Initialize();

            this.ModsPreInitialize();

            this.ModsPostInitialize();

            /*
            //Gestion du stockage - emplacements et restrictions
            Storage = this.GetComponent<PublicStorageComponent>();
            Storage.Initialize(20);
            Storage.Inventory.AddInvRestriction(new SpecificItemTypesRestriction(new System.Type[] { typeof(LettreItem) }));
            Storage.Inventory.OnChanged.Add(CheckStorage);

            //Gestion du status dans l'onglet du même nom
            status = GetComponent<StatusComponent>().CreateStatusElement(50);*/
        }
        /*
        public void CheckStorage(User user) 
        {
            if (Storage.Inventory.IsEmpty)
            {
                status.SetStatusMessage(false, Localizer.DoStr("La boite est vide. Il n'y a pas de courrier."));
            }
            else 
            {
                status.SetStatusMessage(true, Localizer.DoStr("Il y a du courrier !"));
            }
        }*/
        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Boite aux lettres")]
    [LocDescription("Le facteur est-il passé ? J'espère que ce n'est pas une facture !")]
    [Ecopedia("Crafted Objects", "Storage", createAsSubPage: true)]
    [Weight(500)] // Defines how heavy Boite aux lettre is.
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

                ingredients: new List<IngredientElement>
                {
                    new("WoodBoard", 10, typeof(CarpentrySkill), typeof(CarpentryLavishResourcesTalent)), //noloc
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<BoiteAuxLettresItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            
            this.ExperienceOnCraft = 3;
            this.LaborInCalories = CreateLaborInCaloriesValue(600, typeof(CarpentrySkill));
            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(BoiteAuxLettresRecipe), start: 8, skillType: typeof(CarpentryFocusedSpeedTalent), typeof(CarpentryParallelSpeedTalent));

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Boite aux Lettres"), recipeType: typeof(BoiteAuxLettresRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(CarpentryTableObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }

}
