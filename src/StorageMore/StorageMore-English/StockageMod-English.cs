// MOD created by Plex: 3D Model and Code.
// Last update of the mod: 09/28/24

// Some storage parameters can be modified. Annotations are available to indicate possible changes.
// Please do not remove the "Registered Mod" section of the code, as it allows receiving compensation from Strange Loop Games when used on an online server.



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
    using Eco.Core.Plugins.Interfaces;
    using Eco.Mods.Organisms;
    using Eco.Mods.TechTree;
    using System.Diagnostics;
    using System.Linq;

    public class StockageMod : IModInit
    {
        public static ModRegistration Register() => new()
        {
            ModName = "StockageMod",
            ModDescription = "Le mod de stockage ajoute 11 nouveaux éléments de stockage au jeu. Certains sont inédits et spécialement conçus pour des métiers spécifiques. En plus de leur fonctionnalité, ils apportent également une variété esthétique au jeu.",
            ModDisplayName = "Stockage Mod",
        };
    }


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
            component.Initialize(8);
            component.Storage.AddInvRestriction(new BigBagObject.InventoryMultiply());
            component.Inventory.AddInvRestriction(new TagRestriction(new string[] // The tags allowed to be used in storage.

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
                if (item.MaxStackSize > 1)
                {
                    return item.MaxStackSize * 4;
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
    [LocDisplayName("Big Bag")]
    [LocDescription("The ultimate storage bag for your stones, dirt, sand, and other debris. Big enough to store a mountain, but humble enough to stay grounded. Fill it up without moderation... unless you care about your back!")]
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
                    new IngredientElement("Fabric", 20, typeof(TailoringSkill)), // Customizable recipe: "20" linen fabric.
                    new IngredientElement(typeof(LeatherHideItem), 10, typeof(TailoringSkill)), // Customizable recipe: "10" Leather.
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<BigBagItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 3;

            this.LaborInCalories = CreateLaborInCaloriesValue(300, typeof(TailoringSkill)); // Amount of calories consumed for crafting.

            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(BigBagRecipe), start: 2, skillType: typeof(TailoringSkill)); // Crafting time for the item: 2 minutes.

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Big Bag"), recipeType: typeof(BigBagRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(TailoringTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }


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
            component.Initialize(8);
            component.Storage.AddInvRestriction(new PaletteObject.InventoryMultiply());
            component.Inventory.AddInvRestriction(new TagRestriction(new string[] // The tags allowed to be used in storage.
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
                    return item.MaxStackSize * 4;
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
    [LocDescription("Perfect for stacking everything that builds, except your dreams! Compact, square, and ready to hold your bricks, boards, and concrete... without complaining!")]
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
                    new IngredientElement("WoodBoard", 15, typeof(CarpentrySkill), typeof(CarpentryLavishResourcesTalent)), // Customizable recipe: "15" Boards.
                },


                items: new List<CraftingElement>
                {
                    new CraftingElement<PaletteItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2;

            this.LaborInCalories = CreateLaborInCaloriesValue(250, typeof(CarpentrySkill)); // Amount of calories "250" consumed for crafting.
            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(PaletteRecipe), start: 2, skillType: typeof(CarpentrySkill), typeof(CarpentryFocusedSpeedTalent), typeof(CarpentryParallelSpeedTalent)); // Crafting time for the item: 2 minutes.

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Palette"), recipeType: typeof(PaletteRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(CarpentryTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }



    // Wall Tool Rack_________________________________________________________________________


    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(ModularStockpileComponent))]
    [RequireComponent(typeof(PublicStorageComponent))]
    [Tag("Usable")]
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Wall Tool Rack")]
    public partial class StockageOutilsObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(StockageOutilsItem);
        public override LocString DisplayName => Localizer.DoStr("Wall Tool Rack");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<StockpileComponent>().Initialize(new Vector3i(6, 3, 2));
            PublicStorageComponent component = base.GetComponent<PublicStorageComponent>(null);
            this.GetComponent<LinkComponent>().Initialize(12);
            component.Initialize(32);
            component.Storage.AddInvRestriction(new StockageOutilsObject.InventoryMultiply());
            component.Inventory.AddInvRestriction(new TagRestriction(new string[] // The tags allowed to be used in storage.
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
    [LocDisplayName("Wall Tool Rackl")]
    [LocDescription("Save space by storing your tools in style. No more clutter!")]
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
                name: "Wall Tool Rack",
                displayName: Localizer.DoStr("Wall Tool Rack"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("HewnLog", 5, typeof(CarpentrySkill), typeof(CarpentryLavishResourcesTalent)), // Customizable recipe: "5" Hewnlog.
                    new IngredientElement(typeof(IronBarItem), 10, typeof(BlacksmithSkill)), // Customizable recipe: "10" IronBar.
                },


                items: new List<CraftingElement>
                {
                    new CraftingElement<StockageOutilsItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2;

            this.LaborInCalories = CreateLaborInCaloriesValue(250, typeof(CarpentrySkill)); // Amount of calories "250" consumed for crafting.

            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(PaletteRecipe), start: 2, skillType: typeof(CarpentrySkill), typeof(CarpentryFocusedSpeedTalent), typeof(CarpentryParallelSpeedTalent)); // Crafting time for the item: 2 minutes.

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Wall Tool Rack"), recipeType: typeof(StockageOutilsRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(CarpentryTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }



    // Small Simple Shelf


    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(PublicStorageComponent))]
    [Tag("Usable")]
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Small Simple Shelf")]
    public partial class Petite_Simple_EtagereObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Petite_Simple_EtagereItem);
        public override LocString DisplayName => Localizer.DoStr("Small Simple Shelf");
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
            component.Initialize(8); // The number of slots "8" allowed in storage.
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
                    return item.MaxStackSize * 2;
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
    [LocDisplayName("Small Simple Shelf")]
    [LocDescription("Need hassle-free storage? The Small Simple Shelf is here to stack your supplies.")]
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
                name: "Small Simple Shelf",  
                displayName: Localizer.DoStr("Small Simple Shelf"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(NailItem), 5, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)), // Customizable recipe: "5" Nails.
                    new IngredientElement(typeof(IronBarItem), 5, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)), // Customizable recipe: "5" Iron Ingot.
                    new IngredientElement("WoodBoard", 10, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)), // Customizable recipe: "10" Wooden Boards.
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Petite_Simple_EtagereItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 25;

            this.LaborInCalories = CreateLaborInCaloriesValue(250, typeof(MechanicsSkill)); // Amount of calories "250" consumed for crafting.

            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(Petite_Simple_EtagereRecipe), start: 3, skillType: typeof(MechanicsSkill), typeof(MechanicsFocusedSpeedTalent), typeof(MechanicsParallelSpeedTalent)); // Durée de fabrication de l'objet : 3 minutes.

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Small Simple Shelf"), recipeType: typeof(Petite_Simple_EtagereRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(MachinistTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }




    // ________________________________  Small Double Shelf _______________________________________________


    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(PublicStorageComponent))]
    [Tag("Usable")]
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Small Double Shelf")]
    public partial class Petite_Double_EtagereObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Petite_Double_EtagereItem);
        public override LocString DisplayName => Localizer.DoStr("Small Double Shelf");
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
            component.Initialize(16); // The number of slots "16" allowed in storage.
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
                    return item.MaxStackSize * 2;
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
    [LocDisplayName("Small Double Shelf")]
    [LocDescription("Twice the shelves, twice the neatly organized clutter! The Small Double Shelf: because sometimes, one is just not enough to fit all that odds and ends.")]
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
                name: "Small Double Shelf", 
                displayName: Localizer.DoStr("Small Double Shelf"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(NailItem), 10, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)), // Customizable recipe: "10" Nails.
                    new IngredientElement(typeof(IronBarItem), 10, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)), // Customizable recipe: "10" Iron Ingots.
                    new IngredientElement("WoodBoard", 20, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)), // Customizable recipe: "20" Wooden Boards.
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Petite_Double_EtagereItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 25;

            this.LaborInCalories = CreateLaborInCaloriesValue(350, typeof(MechanicsSkill)); // Amount of calories "350" consumed for crafting.

            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(Petite_Double_EtagereRecipe), start: 4, skillType: typeof(MechanicsSkill), typeof(MechanicsFocusedSpeedTalent), typeof(MechanicsParallelSpeedTalent)); // Crafting time for the item: 4 minutes.

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Small Double Shelf"), recipeType: typeof(Petite_Double_EtagereRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(MachinistTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }



    // ________________________________  Large Simple Shelf _______________________________________________


    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(PublicStorageComponent))]
    [Tag("Usable")]
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Large Simple Shelf")]
    public partial class Grande_Simple_EtagereObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Grande_Simple_EtagereItem);
        public override LocString DisplayName => Localizer.DoStr("Large Simple Shelf");
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
            component.Initialize(32); // The number of slots "32" allowed in storage.
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
                    return item.MaxStackSize * 2;
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
    [LocDisplayName("Large Simple Shelf")]
    [LocDescription("Bigger, sturdier, but still simple! The Large Simple Shelf is here to handle the big stuff. Because even bulky items need a little neatly organized corner.")]
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
                name: "Large Simple Shelf",  
                displayName: Localizer.DoStr("Large Simple Shelf"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(NailItem), 30, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)), // Customizable recipe: "30" Nails.
                    new IngredientElement(typeof(IronBarItem), 30, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)), // Customizable recipe: "30" Iron Ingots.
                    new IngredientElement("WoodBoard", 50, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)), // Customizable recipe: "50" Wooden Boards.
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Grande_Simple_EtagereItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 25;

            this.LaborInCalories = CreateLaborInCaloriesValue(500, typeof(MechanicsSkill)); // Amount of calories "500" consumed for crafting.

            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(Grande_Simple_EtagereRecipe), start: 6, skillType: typeof(MechanicsSkill), typeof(MechanicsFocusedSpeedTalent), typeof(MechanicsParallelSpeedTalent)); // Crafting time for the item: 6 minutes.

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Large Simple Shelf"), recipeType: typeof(Grande_Simple_EtagereRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(MachinistTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }



    // ________________________________  Large Double Shelf _______________________________________________


    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(PublicStorageComponent))]
    [Tag("Usable")]
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Large Double Shelf")]
    public partial class Grande_Double_EtagereObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Grande_Double_EtagereItem);
        public override LocString DisplayName => Localizer.DoStr("Large Double Shelf");
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
            component.Initialize(64); // The number of slots "64" allowed in storage.
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
                    return item.MaxStackSize * 2;
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
    [LocDisplayName("Large Double Shelf")]
    [LocDescription("When one large shelf isn’t enough, go for the Large Double Shelf! Twice the space to happily stack your inventory... or your well-organized chaos.")]
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
                name: "Large Double Shelf",  
                displayName: Localizer.DoStr("Large Double Shelf"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(NailItem), 60, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)), // Customizable recipe: "60" Nails.
                    new IngredientElement(typeof(IronBarItem), 60, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)), // Customizable recipe: "60" Iron Ingots.
                    new IngredientElement("WoodBoard", 100, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)), // Customizable recipe: "100" Wooden Boards.
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Grande_Double_EtagereItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 25;

            this.LaborInCalories = CreateLaborInCaloriesValue(800, typeof(MechanicsSkill)); // Amount of calories "800" consumed for crafting.

            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(Grande_Double_EtagereRecipe), start: 10, skillType: typeof(MechanicsSkill), typeof(MechanicsFocusedSpeedTalent), typeof(MechanicsParallelSpeedTalent)); // Crafting time for the item: 10 minutes.

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Large Double Shelf"), recipeType: typeof(Grande_Double_EtagereRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(MachinistTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }




    // Shipping_01



    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(CustomTextComponent))]
    [RequireComponent(typeof(PublicStorageComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [Tag("Usable")]
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Blue Container 5m")]
    public partial class Shipping_01Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Shipping_01Item);
        public override LocString DisplayName => Localizer.DoStr("Blue Container 5m");
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
            component.Initialize(32); // The number of slots "32" allowed in storage.
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
    [LocDisplayName("Blue Container 5m")]
    [LocDescription("Large enough to store all your items... or just all the clutter you don't know what to do with! Durable, spacious, and blue in color so you never lose sight of it.")]
    [Ecopedia("Crafted Objects", "Storage", createAsSubPage: true)]
    [Weight(20000)]
    [MaxStackSize(10)]
    public partial class Shipping_01Item : WorldObjectItem<Shipping_01Object>
    {

    }

    [Ecopedia("Crafted Objects", "Storage", subPageName: "Blue Container 5m")]
    [RequiresSkill(typeof(MechanicsSkill), 5)]
    public partial class Shipping_01Recipe : RecipeFamily
    {
        public Shipping_01Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Blue Container 5m", 
                displayName: Localizer.DoStr("Blue Container 5m"),

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
            this.Initialize(displayText: Localizer.DoStr("Blue Container 5m"), recipeType: typeof(Shipping_01Recipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(MachinistTableObject), recipe: this);
        }
        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }


    // Shipping_02 ________________________

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(CustomTextComponent))]
    [RequireComponent(typeof(PublicStorageComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [Tag("Usable")]
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Green Container 10m")]
    public partial class Shipping_02Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Shipping_02Item);
        public override LocString DisplayName => Localizer.DoStr("Green Container 10m");
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
            component.Initialize(64); // Le nombre d'emplacement "64" autorisé dans le stockage.
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
    [LocDisplayName("Green Container 10m")]
    [LocDescription("Large enough to hide a forest... or simply everything you don't know where to put. In green, for that eco-friendly touch, of course!")]
    [Ecopedia("Crafted Objects", "Storage", createAsSubPage: true)]
    [Weight(20000)]
    [MaxStackSize(10)]
    public partial class Shipping_02Item : WorldObjectItem<Shipping_02Object>
    {

    }

    [Ecopedia("Crafted Objects", "Storage", subPageName: "Green Container 10m")]
    [RequiresSkill(typeof(MechanicsSkill), 6)]
    public partial class Shipping_02Recipe : RecipeFamily
    {
        public Shipping_02Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Green Container 10m",  //noloc
                displayName: Localizer.DoStr("Green Container 10m"),

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
            this.Initialize(displayText: Localizer.DoStr("Green Container 10m"), recipeType: typeof(Shipping_02Recipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(MachinistTableObject), recipe: this);
        }
        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    //Shipping_03____________________________________________


    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(CustomTextComponent))]
    [RequireComponent(typeof(PublicStorageComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [Tag("Usable")]
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Red Container 12m")]
    public partial class Shipping_03Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Shipping_03Item);
        public override LocString DisplayName => Localizer.DoStr("Red Container 12m");
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
            component.Initialize(128); // The number of slots "128" allowed in storage.
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
    [LocDisplayName("Red Container 12m")]
    [LocDescription("So big that even a mammoth would feel at home! Bright red to remind you that you're storing something serious... or just way too much stuff.")]
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
                name: "Red Container 12m",  //noloc
                displayName: Localizer.DoStr("Red Container 12m"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronPlateItem), 300, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)), // Customizable recipe: "300" Iron Plates.
                    new IngredientElement(typeof(ScrewsItem), 200, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)), // Customizable recipe: "200" Screws.
                    new IngredientElement(typeof(IronBarItem), 120, typeof(MechanicsSkill), typeof(MechanicsLavishResourcesTalent)), // Customizable recipe: "120" Iron Ingots.
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Shipping_03Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 75;

            this.LaborInCalories = CreateLaborInCaloriesValue(700, typeof(MechanicsSkill)); // Amount of calories "300" consumed for crafting.

            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(Shipping_03Recipe), start: 20, skillType: typeof(MechanicsSkill), typeof(MechanicsFocusedSpeedTalent), typeof(MechanicsParallelSpeedTalent)); // Durée de fabrication de l'objet : 20 minutes.

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Red Container 12m"), recipeType: typeof(Shipping_03Recipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(MachinistTableObject), recipe: this);
        }
        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }


    // Small Wood Storage

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(ModularStockpileComponent))]
    [RequireComponent(typeof(PublicStorageComponent))]
    [Tag("Usable")]
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Small Wood Storage")]
    public partial class PetitStockageWoodObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(PetitStockageWoodItem);
        public override LocString DisplayName => Localizer.DoStr("Small Wood Storage");
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
            component.Initialize(18); // The number of slots "18" allowed in storage.
            component.Storage.AddInvRestriction(new PetitStockageWoodObject.InventoryMultiply());
            component.Inventory.AddInvRestriction(new TagRestriction(new string[] // The allowed tags "Wood" to be used in storage.
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
                if (item.MaxStackSize > 1)
                {
                    return item.MaxStackSize * 2;
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
    [LocDisplayName("Small Wood Storage")]
    [LocDescription("Because even logs deserve a roof before they end up as boards!")]
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
                name: "Small Wood Storage",
                displayName: Localizer.DoStr("Small Wood Storage"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("HewnLog", 15, typeof(CarpentrySkill), typeof(CarpentryLavishResourcesTalent)), // Customizable recipe: "15" HewnLogs.
                    new IngredientElement("WoodBoard", 10, typeof(CarpentrySkill), typeof(CarpentryLavishResourcesTalent)), // Customizable recipe: "10" WoodBoards.
                    new IngredientElement("Wood", 10, typeof(CarpentrySkill), typeof(CarpentryLavishResourcesTalent)), // Customizable recipe: "10" Wood.
                },


                items: new List<CraftingElement>
                {
                    new CraftingElement<PetitStockageWoodItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2;

            this.LaborInCalories = CreateLaborInCaloriesValue(250, typeof(CarpentrySkill)); // Amount of calories "250" consumed for crafting.

            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(PetitStockageWoodRecipe), start: 2, skillType: typeof(CarpentrySkill), typeof(CarpentryFocusedSpeedTalent), typeof(CarpentryParallelSpeedTalent)); // Durée de fabrication de l'objet : 2 minutes.

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Small Wood Storage"), recipeType: typeof(PetitStockageWoodRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(CarpentryTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }


    // Medium Wood Storage_____________________________________________________________________

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(ModularStockpileComponent))]
    [RequireComponent(typeof(PublicStorageComponent))]
    [Tag("Usable")]
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Medium Wood Storage")]
    public partial class MoyenStockageWoodObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(MoyenStockageWoodItem);
        public override LocString DisplayName => Localizer.DoStr("Medium Wood Storage");
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
            component.Initialize(24); // Le nombre d'emplacement "24" autorisé dans le stockage.
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
    [LocDisplayName("Medium Wood Storage")]
    [LocDescription("Need to store all that wood lying around? The medium pergola is here to save your shed from chaos! Stack your wood in style (with a touch of organization, we promise).")]
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
                name: "Medium Wood Storage",
                displayName: Localizer.DoStr("Medium Wood Storage"),

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
            this.Initialize(displayText: Localizer.DoStr("Medium Wood Storage"), recipeType: typeof(MoyenStockageWoodRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(CarpentryTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }


    // Large Wood Storage________________________________________________________________


    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(ModularStockpileComponent))]
    [RequireComponent(typeof(PublicStorageComponent))]
    [Tag("Usable")]
    [Ecopedia("Crafted Objects", "Storage", subPageName: "Large Wood Storage")]
    public partial class GrandStockageWoodObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(GrandStockageWoodItem);
        public override LocString DisplayName => Localizer.DoStr("Large Wood Storage");
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
            component.Initialize(32); // Le nombre d'emplacement "32" autorisé dans le stockage.
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
                if (item.MaxStackSize > 1)
                {
                    return item.MaxStackSize * 4;
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
    [LocDisplayName("Large Wood Storage")]
    [LocDescription("Because tossing logs around is great for beavers, but not for you. Store your precious forest pieces in this elegant Large Stockage Wood, and impress your lumberjack friends with your mastery of the art of organization!")]
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
                name: "Large Wood Storage",
                displayName: Localizer.DoStr("Large Wood Storage"),

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
            this.Initialize(displayText: Localizer.DoStr("Large Wood Storage"), recipeType: typeof(GrandStockageWoodRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(CarpentryTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }






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



}