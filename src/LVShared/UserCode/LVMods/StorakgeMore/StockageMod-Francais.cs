// MOD créé par Plex : Modèle 3D et Code.
// Dernière mise à jour du mod : 28/09/24

// Certains paramètres de stockage peuvent être modifiés. Des annotations sont disponibles pour vous indiquer les modifications possibles.
// Merci de ne pas retirer la section "Registered Mod" du code, car elle permet de recevoir une rémunération de la part de Strange Loop Games lors de son utilisation sur un serveur en ligne.

using Eco.Core.Controller;
using Eco.Core.Items;
using Eco.Core.Plugins.Interfaces;
using Eco.Gameplay.Components;
using Eco.Gameplay.Components.Auth;
using Eco.Gameplay.Components.Storage;
using Eco.Gameplay.Items;
using Eco.Gameplay.Items.Recipes;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Occupancy;
using Eco.Gameplay.Skills;
using Eco.Gameplay.Systems.NewTooltip;
using Eco.Shared.Items;
using Eco.Shared.Localization;
using Eco.Shared.Math;
using Eco.Shared.Serialization;
using Eco.Shared.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Eco.Mods.TechTree
{
    public class StockageMod : IModInit
    {
        public static ModRegistration Register() => new()
        {
            ModName = "StockageMod",
            ModDescription = "Le mod de stockage ajoute 11 nouveaux éléments de stockage au jeu. Certains sont inédits et spécialement conçus pour des métiers spécifiques. En plus de leur fonctionnalité, ils apportent également une variété esthétique au jeu.",
            ModDisplayName = "Stockage Mod",
        };
    }

    #region BigBag
    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(ModularStockpileComponent))]
    [RequireComponent(typeof(PublicStorageComponent))]
    [Tag("Usable")]
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Big Bag")]
    public partial class BigBagObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(BigBagItem);
        public override LocString DisplayName => Localizer.DoStr("Big Bag");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;
        static BigBagObject()
        {
            var BlockOccupancyList = new List<BlockOccupancy>
            {
            new BlockOccupancy(new Vector3i(0, 0, 0)),
            new BlockOccupancy(new Vector3i(0, 0, 1)),
            new BlockOccupancy(new Vector3i(0, 1, 0)),
            new BlockOccupancy(new Vector3i(0, 1, 1)),
            new BlockOccupancy(new Vector3i(1, 0, 0)),
            new BlockOccupancy(new Vector3i(1, 0, 1)),
            new BlockOccupancy(new Vector3i(1, 1, 0)),
            new BlockOccupancy(new Vector3i(1, 1, 1)),
            };
            AddOccupancy<BigBagObject>(BlockOccupancyList);
        }
        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<StockpileComponent>().Initialize(new Vector3i(2, 1, 2));
            PublicStorageComponent component = base.GetComponent<PublicStorageComponent>(null);
            this.GetComponent<LinkComponent>().Initialize(12);
            component.Initialize(4);  //SLB 8 à 4 slots
            component.Storage.AddInvRestriction(new BigBagObject.InventoryMultiply());
            component.Inventory.AddInvRestriction(new TagRestriction(new string[] // Les tags autorisées à être utilisées dans le stockage.
            {
                "Silica",
                "Excavatable",
                "CrushedRock",
            }));
            this.ModsPostInitialize();
        }
        public class InventoryMultiply : InventoryRestriction
        {
            public override LocString Message
            {
                get
                {
                    return Localizer.DoStr("Inventory Full");
                }
            }
            public override int MaxAccepted(Item item, int currentQuantity)
            {
                /*
                if (item.MaxStackSize > 1)
                {
                    //return item.MaxStackSize * 4;  //SLB
                    return item.MaxStackSize;  //SLB
                }
                if (!TagUtils.Tags(item).Any((Tag x) => x.Name == "Tools"))
                {
                    return 20;
                }
                return 1;*/ //SLB
                return 2;
            }

            public override bool SurpassStackSize
            {
                get
                {
                    return true;
                }
            }
        }
        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Big Bag")]
    [LocDescription("Le sac de rangement ultime pour vos pierres, terre, sable et autres débris. Assez grand pour stocker une montagne, mais assez humble pour rester au sol. Remplissez-le sans modération... sauf si vous tenez à votre dos !")]
    [Ecopedia("Crafted Objects", "Storage", createAsSubPage: true)]
    [Weight(1000)]
    [MaxStackSize(10)]
    public partial class BigBagItem : WorldObjectItem<BigBagObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));

    }

    [RequiresSkill(typeof(TailoringSkill), 2)]
    public partial class BigBagRecipe : RecipeFamily
    {
        public BigBagRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Big Bag",  
                displayName: Localizer.DoStr("Big Bag"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("Fabric", 20, typeof(TailoringSkill)), // Recette personnalisable : "20" tissu en lin.
                    new IngredientElement(typeof(LeatherHideItem), 10, typeof(TailoringSkill)), // Recette personnalisable : "10" Cuir.
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<BigBagItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 3; 

            this.LaborInCalories = CreateLaborInCaloriesValue(300, typeof(TailoringSkill)); // Quantité de calories consommées pour la fabrication.

            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(BigBagRecipe), start: 2, skillType: typeof(TailoringSkill)); // Durée de fabrication de l'objet : 2 minutes.

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Big Bag"), recipeType: typeof(BigBagRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(TailoringTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }
    #endregion

    #region Palette
    // PaletteStockage_________________________________________________________________
    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(ModularStockpileComponent))]
    [RequireComponent(typeof(PublicStorageComponent))]
    [Tag("Usable")]
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Palette")]
    public partial class PaletteObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(PaletteItem);
        public override LocString DisplayName => Localizer.DoStr("Palette");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;
        static PaletteObject()
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
            new BlockOccupancy(new Vector3i(0, 4, 0)),
            new BlockOccupancy(new Vector3i(0, 4, 1)),
            new BlockOccupancy(new Vector3i(1, 0, 0)),
            new BlockOccupancy(new Vector3i(1, 0, 1)),
            new BlockOccupancy(new Vector3i(1, 1, 0)),
            new BlockOccupancy(new Vector3i(1, 1, 1)),
            new BlockOccupancy(new Vector3i(1, 2, 0)),
            new BlockOccupancy(new Vector3i(1, 2, 1)),
            new BlockOccupancy(new Vector3i(1, 3, 0)),
            new BlockOccupancy(new Vector3i(1, 3, 1)),
            new BlockOccupancy(new Vector3i(1, 4, 0)),
            new BlockOccupancy(new Vector3i(1, 4, 1)),
            };
            AddOccupancy<PaletteObject>(BlockOccupancyList);
        }

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<StockpileComponent>().Initialize(new Vector3i(2, 4, 2));
            PublicStorageComponent component = base.GetComponent<PublicStorageComponent>(null);
            this.GetComponent<LinkComponent>().Initialize(12);
            component.Initialize(4);  //SLB 8 à 4
            component.Storage.AddInvRestriction(new PaletteObject.InventoryMultiply());
            component.Inventory.AddInvRestriction(new TagRestriction(new string[] // Les tags autorisées à être utilisées dans le stockage.
            {
                "Constructable",
                "Metal",
            }));
            this.ModsPostInitialize();
        }


        public class InventoryMultiply : InventoryRestriction
        {
            public override LocString Message
            {
                get
                {
                    return Localizer.DoStr("Inventory Full");
                }
            }
            public override int MaxAccepted(Item item, int currentQuantity)
            {
                if (item.MaxStackSize > 1)
                {
                    //return item.MaxStackSize * 4;  //SLB
                    return item.MaxStackSize;  //SLB
                }
                if (!TagUtils.Tags(item).Any((Tag x) => x.Name == "Tools"))
                {
                    return 20;
                }
                return 1;
            }

            public override bool SurpassStackSize
            {
                get
                {
                    return true;
                }
            }
        }
        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Palette")]
    [LocDescription("Parfaite pour empiler tout ce qui construit, sauf vos rêves ! Compacte, carrée, et prête à accueillir vos briques, planches et béton... sans se plaindre !")]
    [Ecopedia("Crafted Objects", "Storage", createAsSubPage: true)]
    [Weight(1000)]
    [MaxStackSize(10)]
    public partial class PaletteItem : WorldObjectItem<PaletteObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
    }

    [RequiresSkill(typeof(CarpentrySkill), 1)]
    public partial class PaletteRecipe : RecipeFamily
    {
        public PaletteRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Palette",
                displayName: Localizer.DoStr("Palette"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 15, typeof(CarpentrySkill), typeof(CarpentryLavishResourcesTalent)), // Recette personnalisable : "15" Planches.
                },


                items: new List<CraftingElement>
                {
                    new CraftingElement<PaletteItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2;

            this.LaborInCalories = CreateLaborInCaloriesValue(250, typeof(CarpentrySkill)); // Quantité de calories "250" consommées pour la fabrication.
            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(PaletteRecipe), start: 2, skillType: typeof(CarpentrySkill), typeof(CarpentryFocusedSpeedTalent), typeof(CarpentryParallelSpeedTalent)); // Durée de fabrication de l'objet : 2 minutes.

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Palette"), recipeType: typeof(PaletteRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(CarpentryTableObject), recipe: this);
        }
        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }
    #endregion

    #region StockageOutils
    // StockageOutils_________________________________________________________________________
    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(ModularStockpileComponent))]
    [RequireComponent(typeof(PublicStorageComponent))]
    [Tag("Usable")]
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Support à Outils Mural")]
    public partial class StockageOutilsObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(StockageOutilsItem);
        public override LocString DisplayName => Localizer.DoStr("Support à Outils Mural");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;
            protected override void Initialize()
            {
            this.ModsPreInitialize();
            this.GetComponent<StockpileComponent>().Initialize(new Vector3i(6, 3, 2));
            PublicStorageComponent component = base.GetComponent<PublicStorageComponent>(null);
            this.GetComponent<LinkComponent>().Initialize(12);
            component.Initialize(32);
            component.Storage.AddInvRestriction(new StockageOutilsObject.InventoryMultiply());
            component.Inventory.AddInvRestriction(new TagRestriction(new string[] // Les tags autorisées à être utilisées dans le stockage.
            {
                "Tool",
            }));
            this.ModsPostInitialize();
            }

        public class InventoryMultiply : InventoryRestriction
        {
            public override LocString Message
            {
                get
                {
                    return Localizer.DoStr("Inventory Full");
                }
            }

            public override int MaxAccepted(Item item, int currentQuantity)
            {
                if (item.MaxStackSize > 1)
                {
                    return item.MaxStackSize * 3;
                }
                if (!TagUtils.Tags(item).Any((Tag x) => x.Name == "Tools"))
                {
                    return 20;
                }
                return 1;
            }

            public override bool SurpassStackSize
            {
                get
                {
                    return true;
                }
            }
        }
        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Support à Outils Mural")]
    [LocDescription("Gagnez de la place en rangeant vos outils avec style. Le désordre, c'est fini !")]
    [Ecopedia("Crafted Objects", "Storage", createAsSubPage: true)]
    [Weight(1000)]
    [MaxStackSize(10)]
    public partial class StockageOutilsItem : WorldObjectItem<StockageOutilsObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }

    [RequiresSkill(typeof(CarpentrySkill), 1)]
    public partial class StockageOutilsRecipe : RecipeFamily
    {
        public StockageOutilsRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Support à Outils Mural",
                displayName: Localizer.DoStr("Support à Outils Mural"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("HewnLog", 5, typeof(CarpentrySkill), typeof(CarpentryLavishResourcesTalent)), // Recette personnalisable : "5" Hewnlog.
                    new IngredientElement(typeof(IronBarItem), 10, typeof(BlacksmithSkill)), // Recette personnalisable : "10" IronBar.
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<StockageOutilsItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2;

            this.LaborInCalories = CreateLaborInCaloriesValue(250, typeof(CarpentrySkill)); // Quantité de calories "250" consommées pour la fabrication.

            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(PaletteRecipe), start: 2, skillType: typeof(CarpentrySkill), typeof(CarpentryFocusedSpeedTalent), typeof(CarpentryParallelSpeedTalent)); // Durée de fabrication de l'objet : 2 minutes.

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Support à Outils Mural"), recipeType: typeof(StockageOutilsRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(CarpentryTableObject), recipe: this);
        }
        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }
    #endregion

    #region Petite_Simple_Etagere
    // Petite Etagère Simple
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
        PublicStorageComponent component = base.GetComponent<PublicStorageComponent>(null);
        this.GetComponent<LinkComponent>().Initialize(12);
        component.Initialize(16); //SLB 8 à 16
        component.Storage.AddInvRestriction(new Petite_Simple_EtagereObject.InventoryMultiply());
        component.Storage.AddInvRestriction(new NotCarriedRestriction());
        this.ModsPostInitialize();
        }

        public class InventoryMultiply : InventoryRestriction
        {
            public override LocString Message
            {
                get
                {
                    return Localizer.DoStr("Inventory Full");
                }
            }

            public override int MaxAccepted(Item item, int currentQuantity)
            {
                if (item.MaxStackSize > 1)
                {
                    return item.MaxStackSize * 1;  //SLB 2 à 1
                }
                if (!TagUtils.Tags(item).Any((Tag x) => x.Name == "Tools"))
                {
                    return 20;
                }
                return 1;
            }
            public override bool SurpassStackSize
            {
                get
                {
                    return true;
                }
            }
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
    #endregion

    #region Petite_Double_Etagere
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
        PublicStorageComponent component = base.GetComponent<PublicStorageComponent>(null);
        this.GetComponent<LinkComponent>().Initialize(12);
        component.Initialize(32); //SLB 16 à 32
        component.Storage.AddInvRestriction(new Petite_Double_EtagereObject.InventoryMultiply());
        component.Storage.AddInvRestriction(new NotCarriedRestriction());
        this.ModsPostInitialize();
        }

        public class InventoryMultiply : InventoryRestriction
        {
            public override LocString Message
            {
                get
                {
                    return Localizer.DoStr("Inventory Full");
                }
            }
            public override int MaxAccepted(Item item, int currentQuantity)
            {
                if (item.MaxStackSize > 1)
                {
                    return item.MaxStackSize * 1;  //SLB 2 à 1
                }
                if (!TagUtils.Tags(item).Any((Tag x) => x.Name == "Tools"))
                {
                    return 20;
                }
                return 1;
            }
            public override bool SurpassStackSize
            {
                get
                {
                    return true;
                }
            }
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
    #endregion

    #region Grande_Simple_Etagere
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
        PublicStorageComponent component = base.GetComponent<PublicStorageComponent>(null);
        this.GetComponent<LinkComponent>().Initialize(12);
        component.Initialize(64); //SLB 32 à 64
        component.Storage.AddInvRestriction(new Grande_Simple_EtagereObject.InventoryMultiply());
        component.Storage.AddInvRestriction(new NotCarriedRestriction());
        this.ModsPostInitialize();
        }

        public class InventoryMultiply : InventoryRestriction
        {
            public override LocString Message
            {
                get
                {
                    return Localizer.DoStr("Inventory Full");
                }
            }

            public override int MaxAccepted(Item item, int currentQuantity)
            {
                if (item.MaxStackSize > 1)
                {
                    return item.MaxStackSize * 1;  //SLB 2 à 1
                }
                if (!TagUtils.Tags(item).Any((Tag x) => x.Name == "Tools"))
                {
                    return 20;
                }
                return 1;
            }
            public override bool SurpassStackSize
            {
                get
                {
                    return true;
                }
            }
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
    #endregion

    #region Grande_Double_Etagere
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
        PublicStorageComponent component = base.GetComponent<PublicStorageComponent>(null);
        this.GetComponent<LinkComponent>().Initialize(12);
        component.Initialize(128); // SLB 64 à 128
        component.Storage.AddInvRestriction(new Grande_Double_EtagereObject.InventoryMultiply());
        component.Storage.AddInvRestriction(new NotCarriedRestriction());
        this.ModsPostInitialize();
        }
        public class InventoryMultiply : InventoryRestriction
        {
            public override LocString Message
            {
                get
                {
                    return Localizer.DoStr("Inventory Full");
                }
            }
            public override int MaxAccepted(Item item, int currentQuantity)
            {
                if (item.MaxStackSize > 1)
                {
                    return item.MaxStackSize * 1;  //SLB 2 à 1
                }
                if (!TagUtils.Tags(item).Any((Tag x) => x.Name == "Tools"))
                {
                    return 20;
                }
                return 1;
            }

            public override bool SurpassStackSize
            {
                get
                {
                    return true;
                }
            }
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
    #endregion

    #region Shipping_01
    // Shipping_01
    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(CustomTextComponent))]
    [RequireComponent(typeof(PublicStorageComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [Tag("Usable")]
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Container Bleu 5m")]
    public partial class Shipping_01Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Shipping_01Item);
        public override LocString DisplayName => Localizer.DoStr("Container Bleu 5m");
        public override TableTextureMode TableTexture => TableTextureMode.Metal;
        static Shipping_01Object()
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
            };
            AddOccupancy<Shipping_01Object>(BlockOccupancyList);
        }
        protected override void Initialize()
        {
        this.ModsPreInitialize();
        this.GetComponent<CustomTextComponent>().Initialize(700);
        this.GetComponent<LinkComponent>().Initialize(12); // Maximum connection distance with nearby storage units: 12 meters.
        PublicStorageComponent component = base.GetComponent<PublicStorageComponent>(null);
        component.Initialize(10); //SLB 32 à 10
        component.Storage.AddInvRestriction(new Shipping_01Object.InventoryMultiply());
        this.ModsPostInitialize();
        }
        public class InventoryMultiply : InventoryRestriction
        {
            public override LocString Message
            {
                get
                {
                    return Localizer.DoStr("Inventory Full");
                }
            }

            public override int MaxAccepted(Item item, int currentQuantity)
            {
                if (item.MaxStackSize > 1)
                {
                    return item.MaxStackSize * 2; //SLB 3 à 2
                }
                if (!TagUtils.Tags(item).Any((Tag x) => x.Name == "Tools"))
                {
                    return 20;
                }
                return 1;
            }

            public override bool SurpassStackSize
            {
                get
                {
                    return true;
                }
            }
        }
        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Container Bleu 5m")]
    [LocDescription("Assez grand pour ranger tous vos objets... ou simplement tout le bazar que vous ne savez plus où mettre ! Résistant, spacieux et de couleur bleue pour ne jamais le perdre de vue.")]
    [Ecopedia("Crafted Objects", "Storage", createAsSubPage: true)]
    [Weight(20000)]
    [MaxStackSize(10)]
    public partial class Shipping_01Item : WorldObjectItem<Shipping_01Object>
    {

    }

    [Ecopedia("Crafted Objects", "Storage", subPageName: "Container Bleu 5m")]
    [RequiresSkill(typeof(MechanicsSkill), 5)]
    public partial class Shipping_01Recipe : RecipeFamily
    {
        public Shipping_01Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Container Bleu 5m",  //noloc
                displayName: Localizer.DoStr("Container Bleu 5m"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronPlateItem), 100, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)), // Customizable recipe: "100" Iron Plates.
                    new IngredientElement(typeof(ScrewsItem), 50, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)), // Customizable recipe: "50" Screws.
                    new IngredientElement(typeof(IronBarItem), 30, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)), // Customizable recipe: "30" Iron Ingots.
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Shipping_01Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 25;

            this.LaborInCalories = CreateLaborInCaloriesValue(400, typeof(MechanicsSkill)); // Amount of calories consumed for crafting: "250".

            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(Shipping_01Recipe), start: 10, skillType: typeof(MechanicsSkill), typeof(MechanicsFocusedSpeedTalent), typeof(MechanicsParallelSpeedTalent)); // Durée de fabrication de l'objet : 10 minutes.

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Container Bleu 5m"), recipeType: typeof(Shipping_01Recipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(MachinistTableObject), recipe: this);
        }
        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }
    #endregion

    #region Shipping_02
    // Shipping_02 ________________________
    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(CustomTextComponent))]
    [RequireComponent(typeof(PublicStorageComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [Tag("Usable")]
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Container Vert 10m")]
    public partial class Shipping_02Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Shipping_02Item);
        public override LocString DisplayName => Localizer.DoStr("Container Vert 10m");
        public override TableTextureMode TableTexture => TableTextureMode.Metal;
        static Shipping_02Object()
        {
            var BlockOccupancyList = new List<BlockOccupancy>
            {
            new BlockOccupancy(new Vector3i(0, 0, 0)),
            new BlockOccupancy(new Vector3i(0, 0, 1)),
            new BlockOccupancy(new Vector3i(0, 0, 2)),
            new BlockOccupancy(new Vector3i(0, 0, 3)),
            new BlockOccupancy(new Vector3i(0, 0, 4)),
            new BlockOccupancy(new Vector3i(0, 0, 5)),
            new BlockOccupancy(new Vector3i(0, 0, 6)),
            new BlockOccupancy(new Vector3i(0, 0, 7)),
            new BlockOccupancy(new Vector3i(0, 0, 8)),
            new BlockOccupancy(new Vector3i(0, 0, 9)),
            new BlockOccupancy(new Vector3i(0, 1, 0)),
            new BlockOccupancy(new Vector3i(0, 1, 1)),
            new BlockOccupancy(new Vector3i(0, 1, 2)),
            new BlockOccupancy(new Vector3i(0, 1, 3)),
            new BlockOccupancy(new Vector3i(0, 1, 4)),
            new BlockOccupancy(new Vector3i(0, 1, 5)),
            new BlockOccupancy(new Vector3i(0, 1, 6)),
            new BlockOccupancy(new Vector3i(0, 1, 7)),
            new BlockOccupancy(new Vector3i(0, 1, 8)),
            new BlockOccupancy(new Vector3i(0, 1, 9)),
            new BlockOccupancy(new Vector3i(1, 0, 0)),
            new BlockOccupancy(new Vector3i(1, 0, 1)),
            new BlockOccupancy(new Vector3i(1, 0, 2)),
            new BlockOccupancy(new Vector3i(1, 0, 3)),
            new BlockOccupancy(new Vector3i(1, 0, 4)),
            new BlockOccupancy(new Vector3i(1, 0, 5)),
            new BlockOccupancy(new Vector3i(1, 0, 6)),
            new BlockOccupancy(new Vector3i(1, 0, 7)),
            new BlockOccupancy(new Vector3i(1, 0, 8)),
            new BlockOccupancy(new Vector3i(1, 0, 9)),
            new BlockOccupancy(new Vector3i(1, 1, 0)),
            new BlockOccupancy(new Vector3i(1, 1, 1)),
            new BlockOccupancy(new Vector3i(1, 1, 2)),
            new BlockOccupancy(new Vector3i(1, 1, 3)),
            new BlockOccupancy(new Vector3i(1, 1, 4)),
            new BlockOccupancy(new Vector3i(1, 1, 5)),
            new BlockOccupancy(new Vector3i(1, 1, 6)),
            new BlockOccupancy(new Vector3i(1, 1, 7)),
            new BlockOccupancy(new Vector3i(1, 1, 8)),
            new BlockOccupancy(new Vector3i(1, 1, 9)),
            };
            AddOccupancy<Shipping_02Object>(BlockOccupancyList);
        }
        protected override void Initialize()
        {
        this.ModsPreInitialize();
        this.GetComponent<CustomTextComponent>().Initialize(700);
        this.GetComponent<LinkComponent>().Initialize(12); // Maximum connection distance with nearby storage units: 12 meters.
        PublicStorageComponent component = base.GetComponent<PublicStorageComponent>(null);
        component.Initialize(20); // SLB 64 à 20
        component.Storage.AddInvRestriction(new Shipping_02Object.InventoryMultiply());
        this.ModsPostInitialize();
        }
        public class InventoryMultiply : InventoryRestriction
        {
            public override LocString Message
            {
                get
                {
                    return Localizer.DoStr("Inventory Full");
                }
            }
            public override int MaxAccepted(Item item, int currentQuantity)
            {
                if (item.MaxStackSize > 1)
                {
                    return item.MaxStackSize * 2;  //SLB 3 à 2
                }
                if (!TagUtils.Tags(item).Any((Tag x) => x.Name == "Tools"))
                {
                    return 20;
                }
                return 1;
            }
            public override bool SurpassStackSize
            {
                get
                {
                    return true;
                }
            }
        }
        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Container Vert 10m")]
    [LocDescription("Assez grand pour cacher une forêt... ou tout simplement tout ce que vous ne savez plus où mettre. En vert, pour la touche écolo, bien sûr !")]
    [Ecopedia("Crafted Objects", "Storage", createAsSubPage: true)]
    [Weight(20000)]
    [MaxStackSize(10)]
    public partial class Shipping_02Item : WorldObjectItem<Shipping_02Object>
    {

    }

    [Ecopedia("Crafted Objects", "Storage", subPageName: "Container Vert 10m")]
    [RequiresSkill(typeof(MechanicsSkill), 6)]
    public partial class Shipping_02Recipe : RecipeFamily
    {
        public Shipping_02Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Container Vert 10m",  //noloc
                displayName: Localizer.DoStr("Container Vert 10m"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronPlateItem), 200, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)), // Customizable recipe: "200" Iron Plates.
                    new IngredientElement(typeof(ScrewsItem), 100, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)), // Customizable recipe: "100" Screws.
                    new IngredientElement(typeof(IronBarItem), 60, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)), // Customizable recipe: "60" Iron Ingots.
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Shipping_02Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 50;

            this.LaborInCalories = CreateLaborInCaloriesValue(500, typeof(MechanicsSkill)); // Amount of calories consumed for crafting: "500".

            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(Shipping_02Recipe), start: 15, skillType: typeof(MechanicsSkill), typeof(MechanicsFocusedSpeedTalent), typeof(MechanicsParallelSpeedTalent)); // Durée de fabrication de l'objet : 15 minutes.

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Container Vert 10m"), recipeType: typeof(Shipping_02Recipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(MachinistTableObject), recipe: this);
        }
        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }
    #endregion

    #region Shipping_03
    //Shipping_03____________________________________________
    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(CustomTextComponent))]
    [RequireComponent(typeof(PublicStorageComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [Tag("Usable")]
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Container Rouge 12m")]
    public partial class Shipping_03Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Shipping_03Item);
        public override LocString DisplayName => Localizer.DoStr("Container Rouge 12m");
        public override TableTextureMode TableTexture => TableTextureMode.Metal;
        static Shipping_03Object()
        {
            var BlockOccupancyList = new List<BlockOccupancy>
            {
            new BlockOccupancy(new Vector3i(0, 0, 0)),
            new BlockOccupancy(new Vector3i(0, 0, 1)),
            new BlockOccupancy(new Vector3i(0, 0, 2)),
            new BlockOccupancy(new Vector3i(0, 0, 3)),
            new BlockOccupancy(new Vector3i(0, 0, 4)),
            new BlockOccupancy(new Vector3i(0, 0, 5)),
            new BlockOccupancy(new Vector3i(0, 0, 6)),
            new BlockOccupancy(new Vector3i(0, 0, 7)),
            new BlockOccupancy(new Vector3i(0, 0, 8)),
            new BlockOccupancy(new Vector3i(0, 0, 9)),
            new BlockOccupancy(new Vector3i(0, 0, 10)),
            new BlockOccupancy(new Vector3i(0, 0, 11)),
            new BlockOccupancy(new Vector3i(0, 1, 0)),
            new BlockOccupancy(new Vector3i(0, 1, 1)),
            new BlockOccupancy(new Vector3i(0, 1, 2)),
            new BlockOccupancy(new Vector3i(0, 1, 3)),
            new BlockOccupancy(new Vector3i(0, 1, 4)),
            new BlockOccupancy(new Vector3i(0, 1, 5)),
            new BlockOccupancy(new Vector3i(0, 1, 6)),
            new BlockOccupancy(new Vector3i(0, 1, 7)),
            new BlockOccupancy(new Vector3i(0, 1, 8)),
            new BlockOccupancy(new Vector3i(0, 1, 9)),
            new BlockOccupancy(new Vector3i(0, 1, 10)),
            new BlockOccupancy(new Vector3i(0, 1, 11)),
            new BlockOccupancy(new Vector3i(0, 2, 0)),
            new BlockOccupancy(new Vector3i(0, 2, 1)),
            new BlockOccupancy(new Vector3i(0, 2, 2)),
            new BlockOccupancy(new Vector3i(0, 2, 3)),
            new BlockOccupancy(new Vector3i(0, 2, 4)),
            new BlockOccupancy(new Vector3i(0, 2, 5)),
            new BlockOccupancy(new Vector3i(0, 2, 6)),
            new BlockOccupancy(new Vector3i(0, 2, 7)),
            new BlockOccupancy(new Vector3i(0, 2, 8)),
            new BlockOccupancy(new Vector3i(0, 2, 9)),
            new BlockOccupancy(new Vector3i(0, 2, 10)),
            new BlockOccupancy(new Vector3i(0, 2, 11)),
            new BlockOccupancy(new Vector3i(1, 0, 0)),
            new BlockOccupancy(new Vector3i(1, 0, 1)),
            new BlockOccupancy(new Vector3i(1, 0, 2)),
            new BlockOccupancy(new Vector3i(1, 0, 3)),
            new BlockOccupancy(new Vector3i(1, 0, 4)),
            new BlockOccupancy(new Vector3i(1, 0, 5)),
            new BlockOccupancy(new Vector3i(1, 0, 6)),
            new BlockOccupancy(new Vector3i(1, 0, 7)),
            new BlockOccupancy(new Vector3i(1, 0, 8)),
            new BlockOccupancy(new Vector3i(1, 0, 9)),
            new BlockOccupancy(new Vector3i(1, 0, 10)),
            new BlockOccupancy(new Vector3i(1, 0, 11)),
            new BlockOccupancy(new Vector3i(1, 1, 0)),
            new BlockOccupancy(new Vector3i(1, 1, 1)),
            new BlockOccupancy(new Vector3i(1, 1, 2)),
            new BlockOccupancy(new Vector3i(1, 1, 3)),
            new BlockOccupancy(new Vector3i(1, 1, 4)),
            new BlockOccupancy(new Vector3i(1, 1, 5)),
            new BlockOccupancy(new Vector3i(1, 1, 6)),
            new BlockOccupancy(new Vector3i(1, 1, 7)),
            new BlockOccupancy(new Vector3i(1, 1, 8)),
            new BlockOccupancy(new Vector3i(1, 1, 9)),
            new BlockOccupancy(new Vector3i(1, 1, 10)),
            new BlockOccupancy(new Vector3i(1, 1, 11)),
            new BlockOccupancy(new Vector3i(1, 2, 0)),
            new BlockOccupancy(new Vector3i(1, 2, 1)),
            new BlockOccupancy(new Vector3i(1, 2, 2)),
            new BlockOccupancy(new Vector3i(1, 2, 3)),
            new BlockOccupancy(new Vector3i(1, 2, 4)),
            new BlockOccupancy(new Vector3i(1, 2, 5)),
            new BlockOccupancy(new Vector3i(1, 2, 6)),
            new BlockOccupancy(new Vector3i(1, 2, 7)),
            new BlockOccupancy(new Vector3i(1, 2, 8)),
            new BlockOccupancy(new Vector3i(1, 2, 9)),
            new BlockOccupancy(new Vector3i(1, 2, 10)),
            new BlockOccupancy(new Vector3i(1, 2, 11)),
            new BlockOccupancy(new Vector3i(2, 0, 0)),
            new BlockOccupancy(new Vector3i(2, 0, 1)),
            new BlockOccupancy(new Vector3i(2, 0, 2)),
            new BlockOccupancy(new Vector3i(2, 0, 3)),
            new BlockOccupancy(new Vector3i(2, 0, 4)),
            new BlockOccupancy(new Vector3i(2, 0, 5)),
            new BlockOccupancy(new Vector3i(2, 0, 6)),
            new BlockOccupancy(new Vector3i(2, 0, 7)),
            new BlockOccupancy(new Vector3i(2, 0, 8)),
            new BlockOccupancy(new Vector3i(2, 0, 9)),
            new BlockOccupancy(new Vector3i(2, 0, 10)),
            new BlockOccupancy(new Vector3i(2, 0, 11)),
            new BlockOccupancy(new Vector3i(2, 1, 0)),
            new BlockOccupancy(new Vector3i(2, 1, 1)),
            new BlockOccupancy(new Vector3i(2, 1, 2)),
            new BlockOccupancy(new Vector3i(2, 1, 3)),
            new BlockOccupancy(new Vector3i(2, 1, 4)),
            new BlockOccupancy(new Vector3i(2, 1, 5)),
            new BlockOccupancy(new Vector3i(2, 1, 6)),
            new BlockOccupancy(new Vector3i(2, 1, 7)),
            new BlockOccupancy(new Vector3i(2, 1, 8)),
            new BlockOccupancy(new Vector3i(2, 1, 9)),
            new BlockOccupancy(new Vector3i(2, 1, 10)),
            new BlockOccupancy(new Vector3i(2, 1, 11)),
            new BlockOccupancy(new Vector3i(2, 2, 0)),
            new BlockOccupancy(new Vector3i(2, 2, 1)),
            new BlockOccupancy(new Vector3i(2, 2, 2)),
            new BlockOccupancy(new Vector3i(2, 2, 3)),
            new BlockOccupancy(new Vector3i(2, 2, 4)),
            new BlockOccupancy(new Vector3i(2, 2, 5)),
            new BlockOccupancy(new Vector3i(2, 2, 6)),
            new BlockOccupancy(new Vector3i(2, 2, 7)),
            new BlockOccupancy(new Vector3i(2, 2, 8)),
            new BlockOccupancy(new Vector3i(2, 2, 9)),
            new BlockOccupancy(new Vector3i(2, 2, 10)),
            new BlockOccupancy(new Vector3i(2, 2, 11)),
            };
            AddOccupancy<Shipping_03Object>(BlockOccupancyList);
        }
        protected override void Initialize()
        {
        this.ModsPreInitialize();
        this.GetComponent<CustomTextComponent>().Initialize(700);
        this.GetComponent<LinkComponent>().Initialize(12); // Maximum connection distance with nearby storage units: 12 meters.
        PublicStorageComponent component = base.GetComponent<PublicStorageComponent>(null);
        component.Initialize(36); //SLB 128 à 36
        component.Storage.AddInvRestriction(new Shipping_03Object.InventoryMultiply());
        this.ModsPostInitialize();
        }

        public class InventoryMultiply : InventoryRestriction
        {
            public override LocString Message
            {
                get
                {
                    return Localizer.DoStr("Inventory Full");
                }
            }

            public override int MaxAccepted(Item item, int currentQuantity)
            {
                if (item.MaxStackSize > 1)
                {
                    return item.MaxStackSize * 3;
                }
                if (!TagUtils.Tags(item).Any((Tag x) => x.Name == "Tools"))
                {
                    return 20;
                }
                return 1;
            }

            public override bool SurpassStackSize
            {
                get
                {
                    return true;
                }
            }
        }
        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Container Rouge 12m")]
    [LocDescription("Tellement grand que même un mammouth s'y sentirait à l'aise ! Rouge vif pour rappeler que vous stockez du sérieux... ou juste beaucoup trop de trucs.")]
    [Ecopedia("Crafted Objects", "Storage", createAsSubPage: true)]
    [Weight(20000)]
    [MaxStackSize(10)]
    public partial class Shipping_03Item : WorldObjectItem<Shipping_03Object>
    {

    }

    [Ecopedia("Crafted Objects", "Storage", subPageName: "Container Rouge 12m")]
    [RequiresSkill(typeof(MechanicsSkill), 7)]
    public partial class Shipping_03Recipe : RecipeFamily
    {
        public Shipping_03Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Container Rouge 12m",  //noloc
                displayName: Localizer.DoStr("Container Rouge 12m"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronPlateItem), 300, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)), // Recette personnalisable : "300" Plaques de fer.
                    new IngredientElement(typeof(ScrewsItem), 200, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)), // Recette personnalisable : "200" Vis.
                    new IngredientElement(typeof(IronBarItem), 120, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)), // Recette personnalisable : "120" Lingot de fer.
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Shipping_03Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 75;

            this.LaborInCalories = CreateLaborInCaloriesValue(700, typeof(MechanicsSkill)); // Quantité de calories "300" consommées pour la fabrication.

            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(Shipping_03Recipe), start: 20, skillType: typeof(MechanicsSkill), typeof(MechanicsFocusedSpeedTalent), typeof(MechanicsParallelSpeedTalent)); // Durée de fabrication de l'objet : 20 minutes.

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Container Rouge 12m"), recipeType: typeof(Shipping_03Recipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(MachinistTableObject), recipe: this);
        }
        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }
    #endregion

    #region PetitStockageWood
    // Petit Stockage Wood
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
        this.GetComponent<StockpileComponent>().Initialize(new Vector3i(4, 2, 2));
        this.GetComponent<LinkComponent>().Initialize(12); // Maximum connection distance with nearby storage units: 12 meters.
        PublicStorageComponent component = base.GetComponent<PublicStorageComponent>(null);
        component.Initialize(8); // SLB 18 à 8
        component.Storage.AddInvRestriction(new PetitStockageWoodObject.InventoryMultiply());
        component.Inventory.AddInvRestriction(new TagRestriction(new string[] // Les tags autorisées "Wood" à être utilisées dans le stockage.
        {
                "Wood",
        }));
        this.ModsPostInitialize();
        }
        public class InventoryMultiply : InventoryRestriction
        {
            public override LocString Message
            {
                get
                {
                    return Localizer.DoStr("Inventory Full");
                }
            }

            public override int MaxAccepted(Item item, int currentQuantity)
            {
                /*if (item.MaxStackSize > 1)
                {
                    return item.MaxStackSize * 2;
                }
                if (!TagUtils.Tags(item).Any((Tag x) => x.Name == "Tools"))
                {
                    return 20;
                }
                return 1;*/ //SLB
                return 8;
            }

            public override bool SurpassStackSize
            {
                get
                {
                    return true;
                }
            }
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
    #endregion

    #region MoyenStockageWood
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
        this.GetComponent<StockpileComponent>().Initialize(new Vector3i(6, 3, 2));
        this.GetComponent<LinkComponent>().Initialize(12); // Maximum connection distance with nearby storage units: 12 meters.
        PublicStorageComponent component = base.GetComponent<PublicStorageComponent>(null);
        component.Initialize(12); //SLB 24 à 12
        component.Storage.AddInvRestriction(new MoyenStockageWoodObject.InventoryMultiply());
        component.Inventory.AddInvRestriction(new TagRestriction(new string[] // Les tags autorisées "Wood" à être utilisées dans le stockage.
        {
                "Wood",
        }));
        this.ModsPostInitialize();
        }

        public class InventoryMultiply : InventoryRestriction
        {
            public override LocString Message
            {
                get
                {
                    return Localizer.DoStr("Inventory Full");
                }
            }

            public override int MaxAccepted(Item item, int currentQuantity)
            {
                /*if (item.MaxStackSize > 1)
                {
                    return item.MaxStackSize * 3;
                }
                if (!TagUtils.Tags(item).Any((Tag x) => x.Name == "Tools"))
                {
                    return 20;
                }
                return 1;*/ //SLB
                return 12;
            }
            public override bool SurpassStackSize
            {
                get
                {
                    return true;
                }
            }
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
                    new IngredientElement("HewnLog", 40, typeof(CarpentrySkill), typeof(CarpentryLavishResourcesTalent)), // Recette personnalisable : "30" HewnLog.
                    new IngredientElement("WoodBoard", 30, typeof(CarpentrySkill), typeof(CarpentryLavishResourcesTalent)), // Recette personnalisable : "20" WoodBoard.
                    new IngredientElement("Wood", 30, typeof(CarpentrySkill), typeof(CarpentryLavishResourcesTalent)), // Recette personnalisable : "20" Wood.
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
    #endregion

    #region GrandStockageWood
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
        this.GetComponent<StockpileComponent>().Initialize(new Vector3i(3, 3, 3));
        this.GetComponent<LinkComponent>().Initialize(12); // Maximum connection distance with nearby storage units: 12 meters.
        PublicStorageComponent component = base.GetComponent<PublicStorageComponent>(null);
        component.Initialize(9); //SLB 32 à 9
        component.Storage.AddInvRestriction(new GrandStockageWoodObject.InventoryMultiply());
        component.Inventory.AddInvRestriction(new TagRestriction(new string[] // Les tags autorisées "Wood" à être utilisées dans le stockage.
        {
                "Wood",
        }));
        this.ModsPostInitialize();
        }
        public class InventoryMultiply : InventoryRestriction
        {
            public override LocString Message
            {
                get
                {
                    return Localizer.DoStr("Inventory Full");
                }
            }
            public override int MaxAccepted(Item item, int currentQuantity)
            {
                /*if (item.MaxStackSize > 1)
                {
                    return item.MaxStackSize * 4;
                }
                if (!TagUtils.Tags(item).Any((Tag x) => x.Name == "Tools"))
                {
                    return 20;
                }
                return 1;*/  //SLB
                return 12;
            }

            public override bool SurpassStackSize
            {
                get
                {
                    return true;
                }
            }
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
                    new IngredientElement("Lumber", 60, typeof(CarpentrySkill), typeof(CarpentryLavishResourcesTalent)), // Recette personnalisable : "50" Lumber.
                    new IngredientElement("WoodBoard", 40, typeof(CarpentrySkill), typeof(CarpentryLavishResourcesTalent)), // Recette personnalisable : "20" WoodBoard.
                    new IngredientElement("Wood", 40, typeof(CarpentrySkill), typeof(CarpentryLavishResourcesTalent)), // Recette personnalisable : "20" Wood.
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
    #endregion

    #region Tags
    // Icon Tag CrushedRock
    [Serialized]
        [Category("Blocks")]
        [LocDisplayName("CrushedRock")]
        public partial class CrushedRockItem : Item
        {
            public override LocString DisplayName => Localizer.DoStr("CrushedRock");
        }

    // Icon Tag Silica
    [Serialized]
    [LocDisplayName("Silica")]
    public partial class SilicaItem : Item
    {
        public override LocString DisplayName => Localizer.DoStr("Silica");
    }

    // Icon Tag Excavatable
    [Serialized]
    [Category("Blocks")]
    [LocDisplayName("Excavatable")]
    public partial class ExcavatableItem : Item
    {
        public override LocString DisplayName => Localizer.DoStr("Excavatable");
    }

    // Icon Tag Excavatable
    [Serialized]
    [Category("Blocks")]
    [LocDisplayName("Constructable")]
    public partial class ConstructableItem : Item
    {
        public override LocString DisplayName => Localizer.DoStr("Constructable");
    }
    #endregion
}
