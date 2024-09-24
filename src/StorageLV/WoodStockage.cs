// MOD créé par Plex : Modèle 3D et Code.
// Dernière mise à jour du mod : 23/09/24

// Certains paramètres de stockage peuvent être modifiés. Des annotations sont disponibles pour vous indiquer les modifications possibles.
// Merci de ne pas retirer la section "Registered Mod" du code, car elle permet de recevoir une rémunération de la part de Strange Loop Games lors de son utilisation sur un serveur en ligne.

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
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(ModularStockpileComponent))]
    [RequireComponent(typeof(PublicStorageComponent))]
    [Tag("Usable")]
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Petit Stockage à bois")]
    public partial class PetitStockageWoodObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(PetitStockageWoodItem);
        public override LocString DisplayName => Localizer.DoStr("Petit Stockage à bois");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;
        static PetitStockageWoodObject()

        {
            var BlockOccupancyList = new List<BlockOccupancy>
            {


            new BlockOccupancy(new Vector3i(0, 0, 0)),
            new BlockOccupancy(new Vector3i(0, 0, 1)),
            new BlockOccupancy(new Vector3i(0, 0, 2)),
            new BlockOccupancy(new Vector3i(0, 1, 0)),
            new BlockOccupancy(new Vector3i(0, 1, 1)),
            new BlockOccupancy(new Vector3i(0, 1, 2)),
            new BlockOccupancy(new Vector3i(0, 2, 0)),
            new BlockOccupancy(new Vector3i(0, 2, 1)),
            new BlockOccupancy(new Vector3i(0, 2, 2)),
            new BlockOccupancy(new Vector3i(1, 0, 0)),
            new BlockOccupancy(new Vector3i(1, 0, 1)),
            new BlockOccupancy(new Vector3i(1, 0, 2)),
            new BlockOccupancy(new Vector3i(1, 1, 0)),
            new BlockOccupancy(new Vector3i(1, 1, 1)),
            new BlockOccupancy(new Vector3i(1, 1, 2)),
            new BlockOccupancy(new Vector3i(1, 2, 0)),
            new BlockOccupancy(new Vector3i(1, 2, 1)),
            new BlockOccupancy(new Vector3i(1, 2, 2)),
            new BlockOccupancy(new Vector3i(2, 0, 0)),
            new BlockOccupancy(new Vector3i(2, 0, 1)),
            new BlockOccupancy(new Vector3i(2, 0, 2)),
            new BlockOccupancy(new Vector3i(2, 1, 0)),
            new BlockOccupancy(new Vector3i(2, 1, 1)),
            new BlockOccupancy(new Vector3i(2, 1, 2)),
            new BlockOccupancy(new Vector3i(2, 2, 0)),
            new BlockOccupancy(new Vector3i(2, 2, 1)),
            new BlockOccupancy(new Vector3i(2, 2, 2)),
            new BlockOccupancy(new Vector3i(3, 0, 0)),
            new BlockOccupancy(new Vector3i(3, 0, 1)),
            new BlockOccupancy(new Vector3i(3, 0, 2)),
            new BlockOccupancy(new Vector3i(3, 1, 0)),
            new BlockOccupancy(new Vector3i(3, 1, 1)),
            new BlockOccupancy(new Vector3i(3, 1, 2)),
            new BlockOccupancy(new Vector3i(3, 2, 0)),
            new BlockOccupancy(new Vector3i(3, 2, 1)),
            new BlockOccupancy(new Vector3i(3, 2, 2)),
            new BlockOccupancy(new Vector3i(4, 0, 0)),
            new BlockOccupancy(new Vector3i(4, 0, 1)),
            new BlockOccupancy(new Vector3i(4, 0, 2)),
            new BlockOccupancy(new Vector3i(4, 1, 0)),
            new BlockOccupancy(new Vector3i(4, 1, 1)),
            new BlockOccupancy(new Vector3i(4, 1, 2)),
            new BlockOccupancy(new Vector3i(4, 2, 0)),
            new BlockOccupancy(new Vector3i(4, 2, 1)),
            new BlockOccupancy(new Vector3i(4, 2, 2)),
            };

            AddOccupancy<PetitStockageWoodObject>(BlockOccupancyList);




        }




        protected override void Initialize()
        {
            this.ModsPreInitialize();
            var storage = this.GetComponent<PublicStorageComponent>();
            this.GetComponent<StockpileComponent>().Initialize(new Vector3i(4, 2, 2));
            this.GetComponent<PublicStorageComponent>().Initialize(18, 5000000); // Le poids maximal "5000000", Le nombre d'emplacement "18" autorisé dans le stockage.
            this.GetComponent<LinkComponent>().Initialize(12); // Distance maximale de connexion avec les autres stockages à proximité : 12 mètres.
            storage.Storage.AddInvRestriction(new StackLimitRestriction(30)); // La quantité maximale "30" d'articles empilables dans un emplacement.
            storage.Inventory.AddInvRestriction(new TagRestriction(new string[] // Les tags autorisées "Wood" à être utilisées dans le stockage.
            {
                "Wood",
            }));
            this.ModsPostInitialize();
        }


        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Petit Stockage à bois")]
    [LocDescription("Parce que même les bûches méritent un toit avant de finir en planches !")]
    [Ecopedia("Crafted Objects", "Storage", createAsSubPage: true)]
    [Weight(1000)]
    [MaxStackSize(10)]
    public partial class PetitStockageWoodItem : WorldObjectItem<PetitStockageWoodObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));

    }


    [RequiresSkill(typeof(CarpentrySkill), 1)]
    public partial class PetitStockageWoodRecipe : RecipeFamily
    {
        public PetitStockageWoodRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Petit Stockage à bois",
                displayName: Localizer.DoStr("Petit Stockage à bois"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("HewnLog", 15, typeof(CarpentrySkill), typeof(CarpentryLavishResourcesTalent)), // Recette personnalisable : "15" HewnLog.
                    new IngredientElement("WoodBoard", 10, typeof(CarpentrySkill), typeof(CarpentryLavishResourcesTalent)), // Recette personnalisable : "10" WoodBoard.
                    new IngredientElement("Wood", 10, typeof(CarpentrySkill), typeof(CarpentryLavishResourcesTalent)), // Recette personnalisable : "10" Wood.
                },


                items: new List<CraftingElement>
                {
                    new CraftingElement<PetitStockageWoodItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2;

            this.LaborInCalories = CreateLaborInCaloriesValue(250, typeof(CarpentrySkill)); // Quantité de calories "250" consommées pour la fabrication.

            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(PetitStockageWoodRecipe), start: 2, skillType: typeof(CarpentrySkill), typeof(CarpentryFocusedSpeedTalent), typeof(CarpentryParallelSpeedTalent)); // Durée de fabrication de l'objet : 2 minutes.

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Petit Stockage à bois"), recipeType: typeof(PetitStockageWoodRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(CarpentryTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }


    // MoyenStockage_____________________________________________________________________

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(ModularStockpileComponent))]
    [RequireComponent(typeof(PublicStorageComponent))]
    [Tag("Usable")]
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Moyen stockage à bois")]
    public partial class MoyenStockageWoodObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(MoyenStockageWoodItem);
        public override LocString DisplayName => Localizer.DoStr("Moyen stockage à bois");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;
        static MoyenStockageWoodObject()

        {
            var BlockOccupancyList = new List<BlockOccupancy>
            {

            new BlockOccupancy(new Vector3i(0, 0, 0)),
            new BlockOccupancy(new Vector3i(0, 0, 1)),
            new BlockOccupancy(new Vector3i(0, 0, 2)),
            new BlockOccupancy(new Vector3i(0, 1, 0)),
            new BlockOccupancy(new Vector3i(0, 1, 1)),
            new BlockOccupancy(new Vector3i(0, 1, 2)),
            new BlockOccupancy(new Vector3i(0, 2, 0)),
            new BlockOccupancy(new Vector3i(0, 2, 1)),
            new BlockOccupancy(new Vector3i(0, 2, 2)),
            new BlockOccupancy(new Vector3i(0, 3, 0)),
            new BlockOccupancy(new Vector3i(0, 3, 1)),
            new BlockOccupancy(new Vector3i(0, 3, 2)),
            new BlockOccupancy(new Vector3i(1, 0, 0)),
            new BlockOccupancy(new Vector3i(1, 0, 1)),
            new BlockOccupancy(new Vector3i(1, 0, 2)),
            new BlockOccupancy(new Vector3i(1, 1, 0)),
            new BlockOccupancy(new Vector3i(1, 1, 1)),
            new BlockOccupancy(new Vector3i(1, 1, 2)),
            new BlockOccupancy(new Vector3i(1, 2, 0)),
            new BlockOccupancy(new Vector3i(1, 2, 1)),
            new BlockOccupancy(new Vector3i(1, 2, 2)),
            new BlockOccupancy(new Vector3i(1, 3, 0)),
            new BlockOccupancy(new Vector3i(1, 3, 1)),
            new BlockOccupancy(new Vector3i(1, 3, 2)),
            new BlockOccupancy(new Vector3i(2, 0, 0)),
            new BlockOccupancy(new Vector3i(2, 0, 1)),
            new BlockOccupancy(new Vector3i(2, 0, 2)),
            new BlockOccupancy(new Vector3i(2, 1, 0)),
            new BlockOccupancy(new Vector3i(2, 1, 1)),
            new BlockOccupancy(new Vector3i(2, 1, 2)),
            new BlockOccupancy(new Vector3i(2, 2, 0)),
            new BlockOccupancy(new Vector3i(2, 2, 1)),
            new BlockOccupancy(new Vector3i(2, 2, 2)),
            new BlockOccupancy(new Vector3i(2, 3, 0)),
            new BlockOccupancy(new Vector3i(2, 3, 1)),
            new BlockOccupancy(new Vector3i(2, 3, 2)),
            new BlockOccupancy(new Vector3i(3, 0, 0)),
            new BlockOccupancy(new Vector3i(3, 0, 1)),
            new BlockOccupancy(new Vector3i(3, 0, 2)),
            new BlockOccupancy(new Vector3i(3, 1, 0)),
            new BlockOccupancy(new Vector3i(3, 1, 1)),
            new BlockOccupancy(new Vector3i(3, 1, 2)),
            new BlockOccupancy(new Vector3i(3, 2, 0)),
            new BlockOccupancy(new Vector3i(3, 2, 1)),
            new BlockOccupancy(new Vector3i(3, 2, 2)),
            new BlockOccupancy(new Vector3i(3, 3, 0)),
            new BlockOccupancy(new Vector3i(3, 3, 1)),
            new BlockOccupancy(new Vector3i(3, 3, 2)),
            new BlockOccupancy(new Vector3i(4, 0, 0)),
            new BlockOccupancy(new Vector3i(4, 0, 1)),
            new BlockOccupancy(new Vector3i(4, 0, 2)),
            new BlockOccupancy(new Vector3i(4, 1, 0)),
            new BlockOccupancy(new Vector3i(4, 1, 1)),
            new BlockOccupancy(new Vector3i(4, 1, 2)),
            new BlockOccupancy(new Vector3i(4, 2, 0)),
            new BlockOccupancy(new Vector3i(4, 2, 1)),
            new BlockOccupancy(new Vector3i(4, 2, 2)),
            new BlockOccupancy(new Vector3i(4, 3, 0)),
            new BlockOccupancy(new Vector3i(4, 3, 1)),
            new BlockOccupancy(new Vector3i(4, 3, 2)),
            new BlockOccupancy(new Vector3i(5, 0, 0)),
            new BlockOccupancy(new Vector3i(5, 0, 1)),
            new BlockOccupancy(new Vector3i(5, 0, 2)),
            new BlockOccupancy(new Vector3i(5, 1, 0)),
            new BlockOccupancy(new Vector3i(5, 1, 1)),
            new BlockOccupancy(new Vector3i(5, 1, 2)),
            new BlockOccupancy(new Vector3i(5, 2, 0)),
            new BlockOccupancy(new Vector3i(5, 2, 1)),
            new BlockOccupancy(new Vector3i(5, 2, 2)),
            new BlockOccupancy(new Vector3i(5, 3, 0)),
            new BlockOccupancy(new Vector3i(5, 3, 1)),
            new BlockOccupancy(new Vector3i(5, 3, 2)),
            new BlockOccupancy(new Vector3i(6, 0, 0)),
            new BlockOccupancy(new Vector3i(6, 0, 1)),
            new BlockOccupancy(new Vector3i(6, 0, 2)),
            new BlockOccupancy(new Vector3i(6, 1, 0)),
            new BlockOccupancy(new Vector3i(6, 1, 1)),
            new BlockOccupancy(new Vector3i(6, 1, 2)),
            new BlockOccupancy(new Vector3i(6, 2, 0)),
            new BlockOccupancy(new Vector3i(6, 2, 1)),
            new BlockOccupancy(new Vector3i(6, 2, 2)),
            new BlockOccupancy(new Vector3i(6, 3, 0)),
            new BlockOccupancy(new Vector3i(6, 3, 1)),
            new BlockOccupancy(new Vector3i(6, 3, 2)),
            };

            AddOccupancy<MoyenStockageWoodObject>(BlockOccupancyList);




        }




        protected override void Initialize()
        {
            this.ModsPreInitialize();
            var storage = this.GetComponent<PublicStorageComponent>();
            this.GetComponent<StockpileComponent>().Initialize(new Vector3i(6, 3, 2));
            this.GetComponent<PublicStorageComponent>().Initialize(24, 5000000); // Le poids maximal "5000000", Le nombre d'emplacement "24" autorisé dans le stockage.
            this.GetComponent<LinkComponent>().Initialize(12); // Distance maximale de connexion avec les autres stockages à proximité : 12 mètres.
            storage.Storage.AddInvRestriction(new StackLimitRestriction(40)); // La quantité maximale "40" d'articles empilables dans un emplacement.
            storage.Inventory.AddInvRestriction(new TagRestriction(new string[] // Les tags autorisées "Wood" à être utilisées dans le stockage.
            {
                "Wood",
            }));
            this.ModsPostInitialize();
        }


        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Moyen stockage à bois")]
    [LocDescription("Besoin de ranger tout ce bois qui traîne ? La moyenne pergola est là pour sauver ta cabane du chaos ! Empile ton bois avec classe (et un soupçon d’organisation, promis).")]
    [Ecopedia("Crafted Objects", "Storage", createAsSubPage: true)]
    [Weight(1000)]
    [MaxStackSize(10)]
    public partial class MoyenStockageWoodItem : WorldObjectItem<MoyenStockageWoodObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));

    }


    [RequiresSkill(typeof(CarpentrySkill), 2)]
    public partial class MoyenStockageWoodRecipe : RecipeFamily
    {
        public MoyenStockageWoodRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Moyen stockage à bois",
                displayName: Localizer.DoStr("Moyen stockage à bois"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("HewnLog", 10, typeof(CarpentrySkill), typeof(CarpentryLavishResourcesTalent)), // Recette personnalisable : "10" HewnLog.
                    new IngredientElement(typeof(IronBarItem), 12, typeof(BlacksmithSkill)), // Recette personnalisable : "12" IronBar.
                    new IngredientElement(typeof(BrickItem), 10, typeof(BlacksmithSkill)), // Recette personnalisable : "10" Brick.
                },


                items: new List<CraftingElement>
                {
                    new CraftingElement<MoyenStockageWoodItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2;

            this.LaborInCalories = CreateLaborInCaloriesValue(350, typeof(CarpentrySkill)); // Quantité de calories "350" consommées pour la fabrication.

            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(PetitStockageWoodRecipe), start: 5, skillType: typeof(CarpentrySkill), typeof(CarpentryFocusedSpeedTalent), typeof(CarpentryParallelSpeedTalent)); // Durée de fabrication de l'objet : 5 minutes.

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Moyen stockage à bois"), recipeType: typeof(MoyenStockageWoodRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(CarpentryTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }


    // GrandStockage________________________________________________________________


    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(ModularStockpileComponent))]
    [RequireComponent(typeof(PublicStorageComponent))]
    [Tag("Usable")]
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Grand Stockage à bois")]
    public partial class GrandStockageWoodObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(GrandStockageWoodItem);
        public override LocString DisplayName => Localizer.DoStr("Grand Stockage à bois");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;
        static GrandStockageWoodObject()

        {
            var BlockOccupancyList = new List<BlockOccupancy>
            {

            new BlockOccupancy(new Vector3i(0, 0, 0)),
            new BlockOccupancy(new Vector3i(0, 0, 1)),
            new BlockOccupancy(new Vector3i(0, 0, 2)),
            new BlockOccupancy(new Vector3i(0, 0, 3)),
            new BlockOccupancy(new Vector3i(0, 0, 4)),
            new BlockOccupancy(new Vector3i(0, 1, 0)),
            new BlockOccupancy(new Vector3i(0, 1, 1)),
            new BlockOccupancy(new Vector3i(0, 1, 2)),
            new BlockOccupancy(new Vector3i(0, 1, 3)),
            new BlockOccupancy(new Vector3i(0, 1, 4)),
            new BlockOccupancy(new Vector3i(0, 2, 0)),
            new BlockOccupancy(new Vector3i(0, 2, 1)),
            new BlockOccupancy(new Vector3i(0, 2, 2)),
            new BlockOccupancy(new Vector3i(0, 2, 3)),
            new BlockOccupancy(new Vector3i(0, 2, 4)),
            new BlockOccupancy(new Vector3i(0, 3, 0)),
            new BlockOccupancy(new Vector3i(0, 3, 1)),
            new BlockOccupancy(new Vector3i(0, 3, 2)),
            new BlockOccupancy(new Vector3i(0, 3, 3)),
            new BlockOccupancy(new Vector3i(0, 3, 4)),
            new BlockOccupancy(new Vector3i(0, 4, 0)),
            new BlockOccupancy(new Vector3i(0, 4, 1)),
            new BlockOccupancy(new Vector3i(0, 4, 2)),
            new BlockOccupancy(new Vector3i(0, 4, 3)),
            new BlockOccupancy(new Vector3i(0, 4, 4)),
            new BlockOccupancy(new Vector3i(1, 0, 0)),
            new BlockOccupancy(new Vector3i(1, 0, 1)),
            new BlockOccupancy(new Vector3i(1, 0, 2)),
            new BlockOccupancy(new Vector3i(1, 0, 3)),
            new BlockOccupancy(new Vector3i(1, 0, 4)),
            new BlockOccupancy(new Vector3i(1, 1, 0)),
            new BlockOccupancy(new Vector3i(1, 1, 1)),
            new BlockOccupancy(new Vector3i(1, 1, 2)),
            new BlockOccupancy(new Vector3i(1, 1, 3)),
            new BlockOccupancy(new Vector3i(1, 1, 4)),
            new BlockOccupancy(new Vector3i(1, 2, 0)),
            new BlockOccupancy(new Vector3i(1, 2, 1)),
            new BlockOccupancy(new Vector3i(1, 2, 2)),
            new BlockOccupancy(new Vector3i(1, 2, 3)),
            new BlockOccupancy(new Vector3i(1, 2, 4)),
            new BlockOccupancy(new Vector3i(1, 3, 0)),
            new BlockOccupancy(new Vector3i(1, 3, 1)),
            new BlockOccupancy(new Vector3i(1, 3, 2)),
            new BlockOccupancy(new Vector3i(1, 3, 3)),
            new BlockOccupancy(new Vector3i(1, 3, 4)),
            new BlockOccupancy(new Vector3i(1, 4, 0)),
            new BlockOccupancy(new Vector3i(1, 4, 1)),
            new BlockOccupancy(new Vector3i(1, 4, 2)),
            new BlockOccupancy(new Vector3i(1, 4, 3)),
            new BlockOccupancy(new Vector3i(1, 4, 4)),
            new BlockOccupancy(new Vector3i(2, 0, 0)),
            new BlockOccupancy(new Vector3i(2, 0, 1)),
            new BlockOccupancy(new Vector3i(2, 0, 2)),
            new BlockOccupancy(new Vector3i(2, 0, 3)),
            new BlockOccupancy(new Vector3i(2, 0, 4)),
            new BlockOccupancy(new Vector3i(2, 1, 0)),
            new BlockOccupancy(new Vector3i(2, 1, 1)),
            new BlockOccupancy(new Vector3i(2, 1, 2)),
            new BlockOccupancy(new Vector3i(2, 1, 3)),
            new BlockOccupancy(new Vector3i(2, 1, 4)),
            new BlockOccupancy(new Vector3i(2, 2, 0)),
            new BlockOccupancy(new Vector3i(2, 2, 1)),
            new BlockOccupancy(new Vector3i(2, 2, 2)),
            new BlockOccupancy(new Vector3i(2, 2, 3)),
            new BlockOccupancy(new Vector3i(2, 2, 4)),
            new BlockOccupancy(new Vector3i(2, 3, 0)),
            new BlockOccupancy(new Vector3i(2, 3, 1)),
            new BlockOccupancy(new Vector3i(2, 3, 2)),
            new BlockOccupancy(new Vector3i(2, 3, 3)),
            new BlockOccupancy(new Vector3i(2, 3, 4)),
            new BlockOccupancy(new Vector3i(2, 4, 0)),
            new BlockOccupancy(new Vector3i(2, 4, 1)),
            new BlockOccupancy(new Vector3i(2, 4, 2)),
            new BlockOccupancy(new Vector3i(2, 4, 3)),
            new BlockOccupancy(new Vector3i(2, 4, 4)),
            new BlockOccupancy(new Vector3i(3, 0, 0)),
            new BlockOccupancy(new Vector3i(3, 0, 1)),
            new BlockOccupancy(new Vector3i(3, 0, 2)),
            new BlockOccupancy(new Vector3i(3, 0, 3)),
            new BlockOccupancy(new Vector3i(3, 0, 4)),
            new BlockOccupancy(new Vector3i(3, 1, 0)),
            new BlockOccupancy(new Vector3i(3, 1, 1)),
            new BlockOccupancy(new Vector3i(3, 1, 2)),
            new BlockOccupancy(new Vector3i(3, 1, 3)),
            new BlockOccupancy(new Vector3i(3, 1, 4)),
            new BlockOccupancy(new Vector3i(3, 2, 0)),
            new BlockOccupancy(new Vector3i(3, 2, 1)),
            new BlockOccupancy(new Vector3i(3, 2, 2)),
            new BlockOccupancy(new Vector3i(3, 2, 3)),
            new BlockOccupancy(new Vector3i(3, 2, 4)),
            new BlockOccupancy(new Vector3i(3, 3, 0)),
            new BlockOccupancy(new Vector3i(3, 3, 1)),
            new BlockOccupancy(new Vector3i(3, 3, 2)),
            new BlockOccupancy(new Vector3i(3, 3, 3)),
            new BlockOccupancy(new Vector3i(3, 3, 4)),
            new BlockOccupancy(new Vector3i(3, 4, 0)),
            new BlockOccupancy(new Vector3i(3, 4, 1)),
            new BlockOccupancy(new Vector3i(3, 4, 2)),
            new BlockOccupancy(new Vector3i(3, 4, 3)),
            new BlockOccupancy(new Vector3i(3, 4, 4)),
            new BlockOccupancy(new Vector3i(4, 0, 0)),
            new BlockOccupancy(new Vector3i(4, 0, 1)),
            new BlockOccupancy(new Vector3i(4, 0, 2)),
            new BlockOccupancy(new Vector3i(4, 0, 3)),
            new BlockOccupancy(new Vector3i(4, 0, 4)),
            new BlockOccupancy(new Vector3i(4, 1, 0)),
            new BlockOccupancy(new Vector3i(4, 1, 1)),
            new BlockOccupancy(new Vector3i(4, 1, 2)),
            new BlockOccupancy(new Vector3i(4, 1, 3)),
            new BlockOccupancy(new Vector3i(4, 1, 4)),
            new BlockOccupancy(new Vector3i(4, 2, 0)),
            new BlockOccupancy(new Vector3i(4, 2, 1)),
            new BlockOccupancy(new Vector3i(4, 2, 2)),
            new BlockOccupancy(new Vector3i(4, 2, 3)),
            new BlockOccupancy(new Vector3i(4, 2, 4)),
            new BlockOccupancy(new Vector3i(4, 3, 0)),
            new BlockOccupancy(new Vector3i(4, 3, 1)),
            new BlockOccupancy(new Vector3i(4, 3, 2)),
            new BlockOccupancy(new Vector3i(4, 3, 3)),
            new BlockOccupancy(new Vector3i(4, 3, 4)),
            new BlockOccupancy(new Vector3i(4, 4, 0)),
            new BlockOccupancy(new Vector3i(4, 4, 1)),
            new BlockOccupancy(new Vector3i(4, 4, 2)),
            new BlockOccupancy(new Vector3i(4, 4, 3)),
            new BlockOccupancy(new Vector3i(4, 4, 4)),
            };

            AddOccupancy<GrandStockageWoodObject>(BlockOccupancyList);




        }




        protected override void Initialize()
        {
            this.ModsPreInitialize();
            var storage = this.GetComponent<PublicStorageComponent>();
            this.GetComponent<StockpileComponent>().Initialize(new Vector3i(3, 3, 3));
            this.GetComponent<PublicStorageComponent>().Initialize(32, 5000000); // Le poids maximal "5000000", Le nombre d'emplacement "32" autorisé dans le stockage.
            this.GetComponent<LinkComponent>().Initialize(12); // Distance maximale de connexion avec les autres stockages à proximité : 12 mètres.
            storage.Storage.AddInvRestriction(new StackLimitRestriction(50)); // La quantité maximale "50" d'articles empilables dans un emplacement.
            storage.Inventory.AddInvRestriction(new TagRestriction(new string[] // Les tags autorisées "Wood" à être utilisées dans le stockage.
            {
                "Wood",
            }));
            this.ModsPostInitialize();
        }


        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Grand Stockage à bois")]
    [LocDescription("Parce que jeter des bûches partout, c'est bon pour les castors, mais pas pour toi. Range tes précieux bouts de forêt dans cette GrandStockageWood élégante, et impressionne tes amis bûcherons avec ta maîtrise de l'art du rangement !")]
    [Ecopedia("Crafted Objects", "Storage", createAsSubPage: true)]
    [Weight(1000)]
    [MaxStackSize(10)]
    public partial class GrandStockageWoodItem : WorldObjectItem<GrandStockageWoodObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));

    }


    [RequiresSkill(typeof(CarpentrySkill), 3)]
    public partial class GrandStockageWoodRecipe : RecipeFamily
    {
        public GrandStockageWoodRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Grand Stockage à bois",
                displayName: Localizer.DoStr("Grand Stockage à bois"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("HewnLog", 30, typeof(CarpentrySkill), typeof(CarpentryLavishResourcesTalent)), // Recette personnalisable : "30" HewnLog.
                    new IngredientElement("WoodBoard", 20, typeof(CarpentrySkill), typeof(CarpentryLavishResourcesTalent)), // Recette personnalisable : "20" WoodBoard.
                    new IngredientElement("Wood", 20, typeof(CarpentrySkill), typeof(CarpentryLavishResourcesTalent)), // Recette personnalisable : "20" Wood.
                },


                items: new List<CraftingElement>
                {
                    new CraftingElement<GrandStockageWoodItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2;

            this.LaborInCalories = CreateLaborInCaloriesValue(450, typeof(CarpentrySkill)); // Quantité de calories "450" consommées pour la fabrication.

            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(PetitStockageWoodRecipe), start: 10, skillType: typeof(CarpentrySkill), typeof(CarpentryFocusedSpeedTalent), typeof(CarpentryParallelSpeedTalent)); // Durée de fabrication de l'objet : 10 minutes.

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Grand Stockage à bois"), recipeType: typeof(GrandStockageWoodRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(CarpentryTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }

}
