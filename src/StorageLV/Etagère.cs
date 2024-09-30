// MOD créé par Plex.
// Dernière mise à jour du mod : 24/09/24

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
    [RequireComponent(typeof(PublicStorageComponent))]
    [Tag("Usable")]
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Petite Etagère Simple")]
    public partial class Petite_Simple_EtagereObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Petite_Simple_EtagereItem);
        public override LocString DisplayName => Localizer.DoStr("Petite Etagère Simple");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;
        static Petite_Simple_EtagereObject()

        {
            var BlockOccupancyList = new List<BlockOccupancy>
            {


            new BlockOccupancy(new Vector3i(0, 0, 0)),
            new BlockOccupancy(new Vector3i(0, 1, 0)),
            };

            AddOccupancy<Petite_Simple_EtagereObject>(BlockOccupancyList);




        }




        protected override void Initialize()
        {
            this.ModsPreInitialize();
            var storage = this.GetComponent<PublicStorageComponent>();
            this.GetComponent<PublicStorageComponent>().Initialize(8, 5000000); // Le poids maximal "5000000", Le nombre d'emplacement "8" autorisé dans le stockage.
            storage.Storage.AddInvRestriction(new NotCarriedRestriction());
            this.ModsPostInitialize();
        }


        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Petite Etagère Simple")]
    [LocDescription("Besoin d’un rangement sans prise de tête ? La Petite Étagère Simple est là pour empiler vos fournitures.")]
    [Ecopedia("Crafted Objects", "Storage", createAsSubPage: true)]
    [Weight(1000)]
    [MaxStackSize(10)]
    public partial class Petite_Simple_EtagereItem : WorldObjectItem<Petite_Simple_EtagereObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));

    }

    [RequiresSkill(typeof(MechanicsSkill), 1)]
    public partial class Petite_Simple_EtagereRecipe : RecipeFamily
    {
        public Petite_Simple_EtagereRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Petite Etagère Simple",  //noloc
                displayName: Localizer.DoStr("Petite Etagère Simple"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(NailItem), 5, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)), // Recette personnalisable : "5" Clous.
                    new IngredientElement(typeof(IronBarItem), 5, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)), // Recette personnalisable : "5" Lingot de fer.
                    new IngredientElement("WoodBoard", 10, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)), // Recette personnalisable : "10" Planches en bois.
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Petite_Simple_EtagereItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 25;

            this.LaborInCalories = CreateLaborInCaloriesValue(250, typeof(MechanicsSkill)); // Quantité de calories "250" consommées pour la fabrication.

            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(Petite_Simple_EtagereRecipe), start: 3, skillType: typeof(MechanicsSkill), typeof(MechanicsFocusedSpeedTalent), typeof(MechanicsParallelSpeedTalent)); // Durée de fabrication de l'objet : 3 minutes.

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Petite Etagère Simple"), recipeType: typeof(Petite_Simple_EtagereRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(MachinistTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }




    // ________________________________  Petite Etagère Double _______________________________________________


    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(PublicStorageComponent))]
    [Tag("Usable")]
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Petite Etagère Double")]
    public partial class Petite_Double_EtagereObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Petite_Double_EtagereItem);
        public override LocString DisplayName => Localizer.DoStr("Petite Etagère Double");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;
        static Petite_Double_EtagereObject()

        {
            var BlockOccupancyList = new List<BlockOccupancy>
            {


            new BlockOccupancy(new Vector3i(0, 0, 0)),
            new BlockOccupancy(new Vector3i(0, 1, 0)),
            new BlockOccupancy(new Vector3i(1, 0, 0)),
            new BlockOccupancy(new Vector3i(1, 1, 0)),
            };

            AddOccupancy<Petite_Double_EtagereObject>(BlockOccupancyList);




        }




        protected override void Initialize()
        {
            this.ModsPreInitialize();
            var storage = this.GetComponent<PublicStorageComponent>();
            this.GetComponent<PublicStorageComponent>().Initialize(16, 5000000); // Le poids maximal "5000000", Le nombre d'emplacement "16" autorisé dans le stockage.
            storage.Storage.AddInvRestriction(new NotCarriedRestriction());
            this.ModsPostInitialize();
        }


        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Petite Etagère Double")]
    [LocDescription("Deux fois plus d’étagères, deux fois plus de bazar bien rangé ! La Petite Étagère Double : parce que parfois, une seule ne suffit pas pour caser tout ce bric-à-brac.")]
    [Ecopedia("Crafted Objects", "Storage", createAsSubPage: true)]
    [Weight(1000)]
    [MaxStackSize(10)]
    public partial class Petite_Double_EtagereItem : WorldObjectItem<Petite_Double_EtagereObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));

    }

    [RequiresSkill(typeof(MechanicsSkill), 2)]
    public partial class Petite_Double_EtagereRecipe : RecipeFamily
    {
        public Petite_Double_EtagereRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Petite Etagère Double",  //noloc
                displayName: Localizer.DoStr("Petite Etagère Double"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(NailItem), 10, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)), // Recette personnalisable : "10" Clous.
                    new IngredientElement(typeof(IronBarItem), 10, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)), // Recette personnalisable : "10" Lingot de fer.
                    new IngredientElement("WoodBoard", 20, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)), // Recette personnalisable : "20" Planches en bois.
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Petite_Double_EtagereItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 25;

            this.LaborInCalories = CreateLaborInCaloriesValue(350, typeof(MechanicsSkill)); // Quantité de calories "350" consommées pour la fabrication.

            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(Petite_Double_EtagereRecipe), start: 4, skillType: typeof(MechanicsSkill), typeof(MechanicsFocusedSpeedTalent), typeof(MechanicsParallelSpeedTalent)); // Durée de fabrication de l'objet : 4 minutes.

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Petite Etagère Double"), recipeType: typeof(Petite_Double_EtagereRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(MachinistTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }



    // ________________________________  Grande Etagère Simple _______________________________________________


    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(PublicStorageComponent))]
    [Tag("Usable")]
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Grande Etagère Simple")]
    public partial class Grande_Simple_EtagereObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Grande_Simple_EtagereItem);
        public override LocString DisplayName => Localizer.DoStr("Grande Etagère Simple");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;
        static Grande_Simple_EtagereObject()

        {
            var BlockOccupancyList = new List<BlockOccupancy>
            {


            new BlockOccupancy(new Vector3i(0, 0, 0)),
            new BlockOccupancy(new Vector3i(0, 0, 1)),
            new BlockOccupancy(new Vector3i(0, 1, 0)),
            new BlockOccupancy(new Vector3i(0, 1, 1)),
            new BlockOccupancy(new Vector3i(0, 2, 0)),
            new BlockOccupancy(new Vector3i(0, 2, 1)),
            new BlockOccupancy(new Vector3i(0, 3, 0)),
            new BlockOccupancy(new Vector3i(0, 3, 1)),
            new BlockOccupancy(new Vector3i(1, 0, 0)),
            new BlockOccupancy(new Vector3i(1, 0, 1)),
            new BlockOccupancy(new Vector3i(1, 1, 0)),
            new BlockOccupancy(new Vector3i(1, 1, 1)),
            new BlockOccupancy(new Vector3i(1, 2, 0)),
            new BlockOccupancy(new Vector3i(1, 2, 1)),
            new BlockOccupancy(new Vector3i(1, 3, 0)),
            new BlockOccupancy(new Vector3i(1, 3, 1)),
            };

            AddOccupancy<Grande_Simple_EtagereObject>(BlockOccupancyList);




        }




        protected override void Initialize()
        {
            this.ModsPreInitialize();
            var storage = this.GetComponent<PublicStorageComponent>();
            this.GetComponent<PublicStorageComponent>().Initialize(32, 5000000); // Le poids maximal "5000000", Le nombre d'emplacement "32" autorisé dans le stockage.
            storage.Storage.AddInvRestriction(new NotCarriedRestriction());
            this.ModsPostInitialize();
        }


        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Grande Etagère Simple")]
    [LocDescription("Plus grande, plus robuste, mais toujours simple ! La Grande Étagère Simple est là pour gérer les grosses affaires. Parce que même les objets encombrants ont besoin d'un petit coin bien rangé.")]
    [Ecopedia("Crafted Objects", "Storage", createAsSubPage: true)]
    [Weight(1000)]
    [MaxStackSize(10)]
    public partial class Grande_Simple_EtagereItem : WorldObjectItem<Grande_Simple_EtagereObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));

    }

    [RequiresSkill(typeof(MechanicsSkill), 3)]
    public partial class Grande_Simple_EtagereRecipe : RecipeFamily
    {
        public Grande_Simple_EtagereRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Grande Etagère Simple",  //noloc
                displayName: Localizer.DoStr("Grande Etagère Simple"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(NailItem), 30, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)), // Recette personnalisable : "30" Clous.
                    new IngredientElement(typeof(IronBarItem), 30, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)), // Recette personnalisable : "30" Lingot de fer.
                    new IngredientElement("WoodBoard", 50, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)), // Recette personnalisable : "50" Planches en bois.
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Grande_Simple_EtagereItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 25;

            this.LaborInCalories = CreateLaborInCaloriesValue(500, typeof(MechanicsSkill)); // Quantité de calories "500" consommées pour la fabrication.

            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(Grande_Simple_EtagereRecipe), start: 6, skillType: typeof(MechanicsSkill), typeof(MechanicsFocusedSpeedTalent), typeof(MechanicsParallelSpeedTalent)); // Durée de fabrication de l'objet : 6 minutes.

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Grande Etagère Simple"), recipeType: typeof(Grande_Simple_EtagereRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(MachinistTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }



    // ________________________________  Grande Etagère Double _______________________________________________


    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(PublicStorageComponent))]
    [Tag("Usable")]
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Grande Etagère Double")]
    public partial class Grande_Double_EtagereObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Grande_Double_EtagereItem);
        public override LocString DisplayName => Localizer.DoStr("Grande Etagère Double");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;
        static Grande_Double_EtagereObject()

        {
            var BlockOccupancyList = new List<BlockOccupancy>
            {


            new BlockOccupancy(new Vector3i(0, 0, 0)),
            new BlockOccupancy(new Vector3i(0, 0, 1)),
            new BlockOccupancy(new Vector3i(0, 1, 0)),
            new BlockOccupancy(new Vector3i(0, 1, 1)),
            new BlockOccupancy(new Vector3i(0, 2, 0)),
            new BlockOccupancy(new Vector3i(0, 2, 1)),
            new BlockOccupancy(new Vector3i(0, 3, 0)),
            new BlockOccupancy(new Vector3i(0, 3, 1)),
            new BlockOccupancy(new Vector3i(1, 0, 0)),
            new BlockOccupancy(new Vector3i(1, 0, 1)),
            new BlockOccupancy(new Vector3i(1, 1, 0)),
            new BlockOccupancy(new Vector3i(1, 1, 1)),
            new BlockOccupancy(new Vector3i(1, 2, 0)),
            new BlockOccupancy(new Vector3i(1, 2, 1)),
            new BlockOccupancy(new Vector3i(1, 3, 0)),
            new BlockOccupancy(new Vector3i(1, 3, 1)),
            new BlockOccupancy(new Vector3i(2, 0, 0)),
            new BlockOccupancy(new Vector3i(2, 0, 1)),
            new BlockOccupancy(new Vector3i(2, 1, 0)),
            new BlockOccupancy(new Vector3i(2, 1, 1)),
            new BlockOccupancy(new Vector3i(2, 2, 0)),
            new BlockOccupancy(new Vector3i(2, 2, 1)),
            new BlockOccupancy(new Vector3i(2, 3, 0)),
            new BlockOccupancy(new Vector3i(2, 3, 1)),
            new BlockOccupancy(new Vector3i(3, 0, 0)),
            new BlockOccupancy(new Vector3i(3, 0, 1)),
            new BlockOccupancy(new Vector3i(3, 1, 0)),
            new BlockOccupancy(new Vector3i(3, 1, 1)),
            new BlockOccupancy(new Vector3i(3, 2, 0)),
            new BlockOccupancy(new Vector3i(3, 2, 1)),
            new BlockOccupancy(new Vector3i(3, 3, 0)),
            new BlockOccupancy(new Vector3i(3, 3, 1)),
            };

            AddOccupancy<Grande_Double_EtagereObject>(BlockOccupancyList);




        }




        protected override void Initialize()
        {
            this.ModsPreInitialize();
            var storage = this.GetComponent<PublicStorageComponent>();
            this.GetComponent<PublicStorageComponent>().Initialize(64, 5000000); // Le poids maximal "5000000", Le nombre d'emplacement "64" autorisé dans le stockage.
            storage.Storage.AddInvRestriction(new NotCarriedRestriction());
            this.ModsPostInitialize();
        }


        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Grande Etagère Double")]
    [LocDescription("Quand une seule grande étagère ne suffit pas, optez pour la Grande Étagère Double ! Deux fois plus de place pour empiler joyeusement votre inventaire... ou votre chaos bien organisé.")]
    [Ecopedia("Crafted Objects", "Storage", createAsSubPage: true)]
    [Weight(1000)]
    [MaxStackSize(10)]
    public partial class Grande_Double_EtagereItem : WorldObjectItem<Grande_Double_EtagereObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));

    }

    [RequiresSkill(typeof(MechanicsSkill), 4)]
    public partial class Grande_Double_EtagereRecipe : RecipeFamily
    {
        public Grande_Double_EtagereRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Grande Etagère Double",  //noloc
                displayName: Localizer.DoStr("Grande Etagère Double"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(NailItem), 60, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)), // Recette personnalisable : "60" Clous.
                    new IngredientElement(typeof(IronBarItem), 60, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)), // Recette personnalisable : "60" Lingot de fer.
                    new IngredientElement("WoodBoard", 100, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)), // Recette personnalisable : "100" Planches en bois.
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Grande_Double_EtagereItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 25;

            this.LaborInCalories = CreateLaborInCaloriesValue(800, typeof(MechanicsSkill)); // Quantité de calories "800" consommées pour la fabrication.

            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(Grande_Double_EtagereRecipe), start: 10, skillType: typeof(MechanicsSkill), typeof(MechanicsFocusedSpeedTalent), typeof(MechanicsParallelSpeedTalent)); // Durée de fabrication de l'objet : 10 minutes.

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Grande Etagère Double"), recipeType: typeof(Grande_Double_EtagereRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(MachinistTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }

}