// MOD created by Plex: 3D Model and Code.
// Last mod update: 09/30/24

// Some storage parameters can be modified. Annotations are available to indicate possible changes.
// Please do not remove the "Registered Mod" section from the code, as it allows for compensation from Strange Loop Games when used on an online server.



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
    using static Eco.Gameplay.Housing.PropertyValues.HomeFurnishingValue;
    using Eco.Gameplay.Items.Recipes;
    using Eco.Core.Plugins.Interfaces;

    public class DrapeauxMod : IModInit
    {
        public static ModRegistration Register() => new()
        {
            ModName = "DrapeauxMod",
            ModDescription = "DrapeauxMod ajoute la possibilité de fabriquer des drapeaux de plusieurs pays directement à partir de la table de couturier dans le jeu ECO. Vous pouvez afficher fièrement les couleurs de votre nation. Si un pays manque ou si vous souhaitez personnaliser un drapeau avec un logo unique, contactez-moi pour des ajouts sur mesure.",
            ModDisplayName = "Drapeaux Mod",
        };
    }




    // ______________________________________________________ Flag ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Flag")]
    public partial class DrapeauObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(DrapeauItem);
        public override LocString DisplayName => Localizer.DoStr("Flag");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<HousingComponent>().HomeValue = DrapeauItem.homeValue;
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Flag")]
    [LocDescription("Perfect for decorating your home or signaling to the neighbors that you are the one who makes the best croissants in the neighborhood.")]
    [Ecopedia("Housing Objects", "Flag", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(5000)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class DrapeauItem : WorldObjectItem<DrapeauObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName = typeof(DrapeauObject).UILink(),
            Category = HousingConfig.GetRoomCategory("Outdoor"),
            BaseValue = 6,
            TypeForRoomLimit = Localizer.DoStr("Statue"),
            DiminishingReturnMultiplier = 0.3f

        };

    }

    [RequiresSkill(typeof(TailoringSkill), 1)]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Flag")]
    public partial class DrapeauRecipe : RecipeFamily
    {
        public DrapeauRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Flag",
                displayName: Localizer.DoStr("Flag"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronBarItem), 20, typeof(TailoringSkill)),
                    new IngredientElement(typeof(LinenFabricItem), 10, typeof(TailoringSkill)),

                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<DrapeauItem>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.ExperienceOnCraft = 4;

            this.LaborInCalories = CreateLaborInCaloriesValue(60, typeof(TailoringSkill));

            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(DrapeauRecipe), start: 1, skillType: typeof(TailoringSkill));

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Drapeau"), recipeType: typeof(DrapeauRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(TailoringTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }



    // ______________________________________________________ French_Flag ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Flag", subPageName: "French_Flag")]
    public partial class Drapeau_FrancaisObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_FrancaisItem);
        public override LocString DisplayName => Localizer.DoStr("French_Flag");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<HousingComponent>().HomeValue = Drapeau_FrancaisItem.homeValue;
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("French_Flag")]
    [LocDescription("Perfect for decorating your home or signaling to the neighbors that you're the one who made the best croissants in the neighborhood.")]
    [Ecopedia("Housing Objects", "Flag", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(5000)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Drapeau_FrancaisItem : WorldObjectItem<Drapeau_FrancaisObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName = typeof(Drapeau_FrancaisObject).UILink(),
            Category = HousingConfig.GetRoomCategory("Outdoor"),
            BaseValue = 6,
            TypeForRoomLimit = Localizer.DoStr("Statue"),
            DiminishingReturnMultiplier = 0.3f

        };

    }

    [RequiresSkill(typeof(TailoringSkill), 1)]
    [Ecopedia("Housing Objects", "Flag", subPageName: "French_Flag")]
    public partial class Drapeau_FrancaisRecipe : Recipe
    {
        public Drapeau_FrancaisRecipe()
        {
            this.Init(
                name: "Drapeau_Francais",
                displayName: Localizer.DoStr("French_Flag"),


                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronBarItem), 3, typeof(TailoringSkill)),
                    new IngredientElement(typeof(LinenFabricItem), 10, typeof(TailoringSkill)),
                },


                items: new List<CraftingElement>
                {
                    new CraftingElement<Drapeau_FrancaisItem>()
                });

            this.ModsPostInitialize();
            CraftingComponent.AddTagProduct(typeof(TailoringTableObject), typeof(DrapeauRecipe), this);
        }

        partial void ModsPostInitialize();
    }




    // ______________________________________________________ German_Flag ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Flag", subPageName: "German_Flag")]
    public partial class Drapeau_AllemandObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_AllemandItem);
        public override LocString DisplayName => Localizer.DoStr("German_Flag");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<HousingComponent>().HomeValue = Drapeau_AllemandItem.homeValue;
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("German_Flag")]
    [LocDescription("This German flag on a mast is here for you! Guaranteed: it stands straight like a well-served beer.")]
    [Ecopedia("Housing Objects", "Flag", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(5000)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Drapeau_AllemandItem : WorldObjectItem<Drapeau_AllemandObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName = typeof(Drapeau_AllemandObject).UILink(),
            Category = HousingConfig.GetRoomCategory("Outdoor"),
            BaseValue = 6,
            TypeForRoomLimit = Localizer.DoStr("Statue"),
            DiminishingReturnMultiplier = 0.3f

        };

    }

    [RequiresSkill(typeof(TailoringSkill), 1)]
    [Ecopedia("Housing Objects", "Flag", subPageName: "German_Flag")]
    public partial class Drapeau_AllemandRecipe : Recipe
    {
        public Drapeau_AllemandRecipe()
        {
            this.Init(
                name: "German_Flag",
                displayName: Localizer.DoStr("German_Flag"),


                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronBarItem), 3, typeof(TailoringSkill)),
                    new IngredientElement(typeof(LinenFabricItem), 10, typeof(TailoringSkill)),
                },


                items: new List<CraftingElement>
                {
                    new CraftingElement<Drapeau_AllemandItem>()
                });

            this.ModsPostInitialize();
            CraftingComponent.AddTagProduct(typeof(TailoringTableObject), typeof(DrapeauRecipe), this);
        }

        partial void ModsPostInitialize();
    }



    // ______________________________________________________ Quebec_Flag ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Quebec_Flag")]
    public partial class Drapeau_QuebecObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_QuebecItem);
        public override LocString DisplayName => Localizer.DoStr("Quebec_Flag");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<HousingComponent>().HomeValue = Drapeau_QuebecItem.homeValue;
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Quebec_Flag")]
    [LocDescription("Quebec flag on a pole. Because there's nothing better than a touch of 'tabarnak' in your town!")]
    [Ecopedia("Housing Objects", "Flag", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(5000)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Drapeau_QuebecItem : WorldObjectItem<Drapeau_QuebecObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName = typeof(Drapeau_QuebecObject).UILink(),
            Category = HousingConfig.GetRoomCategory("Outdoor"),
            BaseValue = 6,
            TypeForRoomLimit = Localizer.DoStr("Statue"),
            DiminishingReturnMultiplier = 0.3f

        };

    }

    [RequiresSkill(typeof(TailoringSkill), 1)]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Quebec_Flag")]
    public partial class Drapeau_QuebecRecipe : Recipe
    {
        public Drapeau_QuebecRecipe()
        {
            this.Init(
                name: "Quebec_Flag",
                displayName: Localizer.DoStr("Quebec_Flag"),


                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronBarItem), 3, typeof(TailoringSkill)),
                    new IngredientElement(typeof(LinenFabricItem), 10, typeof(TailoringSkill)),
                },


                items: new List<CraftingElement>
                {
                    new CraftingElement<Drapeau_QuebecItem>()
                });

            this.ModsPostInitialize();
            CraftingComponent.AddTagProduct(typeof(TailoringTableObject), typeof(DrapeauRecipe), this);
        }

        partial void ModsPostInitialize();
    }





    // ______________________________________________________ Flag_England ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Flag_England")]
    public partial class Drapeau_AngleterreObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_AngleterreItem);
        public override LocString DisplayName => Localizer.DoStr("Flag_England");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<HousingComponent>().HomeValue = Drapeau_AngleterreItem.homeValue;
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Flag_England")]
    [LocDescription("English flag on a pole. Perfect for a neat decor, paired with a cup of tea and a 'God Save the Style.'")]
    [Ecopedia("Housing Objects", "Flag", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(5000)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Drapeau_AngleterreItem : WorldObjectItem<Drapeau_AngleterreObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName = typeof(Drapeau_AngleterreObject).UILink(),
            Category = HousingConfig.GetRoomCategory("Outdoor"),
            BaseValue = 6,
            TypeForRoomLimit = Localizer.DoStr("Statue"),
            DiminishingReturnMultiplier = 0.3f

        };

    }

    [RequiresSkill(typeof(TailoringSkill), 1)]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Flag_England")]
    public partial class Drapeau_AngleterreRecipe : Recipe
    {
        public Drapeau_AngleterreRecipe()
        {
            this.Init(
                name: "Flag_England",
                displayName: Localizer.DoStr("Flag_England"),


                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronBarItem), 3, typeof(TailoringSkill)),
                    new IngredientElement(typeof(LinenFabricItem), 10, typeof(TailoringSkill)),
                },


                items: new List<CraftingElement>
                {
                    new CraftingElement<Drapeau_AngleterreItem>()
                });

            this.ModsPostInitialize();
            CraftingComponent.AddTagProduct(typeof(TailoringTableObject), typeof(DrapeauRecipe), this);
        }

        partial void ModsPostInitialize();
    }





    // ______________________________________________________ Flag_Spain ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Flag_Spain")]
    public partial class Drapeau_EspagneObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_EspagneItem);
        public override LocString DisplayName => Localizer.DoStr("Flag_Spain");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<HousingComponent>().HomeValue = Drapeau_EspagneItem.homeValue;
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Flag_Spain")]
    [LocDescription("Spanish flag on a pole. Perfect for a caliente atmosphere... even without the castanets!")]
    [Ecopedia("Housing Objects", "Flag", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(5000)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Drapeau_EspagneItem : WorldObjectItem<Drapeau_EspagneObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName = typeof(Drapeau_EspagneObject).UILink(),
            Category = HousingConfig.GetRoomCategory("Outdoor"),
            BaseValue = 6,
            TypeForRoomLimit = Localizer.DoStr("Statue"),
            DiminishingReturnMultiplier = 0.3f

        };

    }

    [RequiresSkill(typeof(TailoringSkill), 1)]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Flag_Spain")]
    public partial class Drapeau_EspagneRecipe : Recipe
    {
        public Drapeau_EspagneRecipe()
        {
            this.Init(
                name: "Flag_Spain",
                displayName: Localizer.DoStr("Flag_Spain"),


                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronBarItem), 3, typeof(TailoringSkill)),
                    new IngredientElement(typeof(LinenFabricItem), 10, typeof(TailoringSkill)),
                },


                items: new List<CraftingElement>
                {
                    new CraftingElement<Drapeau_EspagneItem>()
                });

            this.ModsPostInitialize();
            CraftingComponent.AddTagProduct(typeof(TailoringTableObject), typeof(DrapeauRecipe), this);
        }

        partial void ModsPostInitialize();
    }




    // ______________________________________________________ Flag_Italy ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Flag_Italy")]
    public partial class Drapeau_ItalieObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_ItalieItem);
        public override LocString DisplayName => Localizer.DoStr("Flag_Italy");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<HousingComponent>().HomeValue = Drapeau_ItalieItem.homeValue;
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Flag_Italy")]
    [LocDescription("Italian flag on a pole. Perfect for saying \"Mamma mia\" to your style... without even touching the pizza!")]
    [Ecopedia("Housing Objects", "Flag", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(5000)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Drapeau_ItalieItem : WorldObjectItem<Drapeau_ItalieObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName = typeof(Drapeau_ItalieObject).UILink(),
            Category = HousingConfig.GetRoomCategory("Outdoor"),
            BaseValue = 6,
            TypeForRoomLimit = Localizer.DoStr("Statue"),
            DiminishingReturnMultiplier = 0.3f

        };

    }

    [RequiresSkill(typeof(TailoringSkill), 1)]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Flag_Italy")]
    public partial class Drapeau_ItalieRecipe : Recipe
    {
        public Drapeau_ItalieRecipe()
        {
            this.Init(
                name: "Flag_Italy",
                displayName: Localizer.DoStr("Flag_Italy"),


                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronBarItem), 3, typeof(TailoringSkill)),
                    new IngredientElement(typeof(LinenFabricItem), 10, typeof(TailoringSkill)),
                },


                items: new List<CraftingElement>
                {
                    new CraftingElement<Drapeau_ItalieItem>()
                });

            this.ModsPostInitialize();
            CraftingComponent.AddTagProduct(typeof(TailoringTableObject), typeof(DrapeauRecipe), this);
        }

        partial void ModsPostInitialize();
    }







    // ______________________________________________________ Flag_Switzerland ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Flag_Switzerland")]
    public partial class Drapeau_SuisseObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_SuisseItem);
        public override LocString DisplayName => Localizer.DoStr("Flag_Switzerland");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<HousingComponent>().HomeValue = Drapeau_SuisseItem.homeValue;
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Flag_Switzerland")]
    [LocDescription("Swiss flag on a pole. Perfect for a neutral vibe... but always classy, like a well-tuned watch!")]
    [Ecopedia("Housing Objects", "Flag", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(5000)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Drapeau_SuisseItem : WorldObjectItem<Drapeau_SuisseObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName = typeof(Drapeau_SuisseObject).UILink(),
            Category = HousingConfig.GetRoomCategory("Outdoor"),
            BaseValue = 6,
            TypeForRoomLimit = Localizer.DoStr("Statue"),
            DiminishingReturnMultiplier = 0.3f

        };

    }

    [RequiresSkill(typeof(TailoringSkill), 1)]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Flag_Switzerland")]
    public partial class Drapeau_SuisseRecipe : Recipe
    {
        public Drapeau_SuisseRecipe()
        {
            this.Init(
                name: "Flag_Switzerland",
                displayName: Localizer.DoStr("Flag_Switzerland"),


                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronBarItem), 3, typeof(TailoringSkill)),
                    new IngredientElement(typeof(LinenFabricItem), 10, typeof(TailoringSkill)),
                },


                items: new List<CraftingElement>
                {
                    new CraftingElement<Drapeau_SuisseItem>()
                });

            this.ModsPostInitialize();
            CraftingComponent.AddTagProduct(typeof(TailoringTableObject), typeof(DrapeauRecipe), this);
        }

        partial void ModsPostInitialize();
    }





    // ______________________________________________________ Flag_Belgian ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Flag_Belgian")]
    public partial class Drapeau_BelgiqueObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_BelgiqueItem);
        public override LocString DisplayName => Localizer.DoStr("Flag_Belgian");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<HousingComponent>().HomeValue = Drapeau_BelgiqueItem.homeValue;
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Flag_Belgian")]
    [LocDescription("Belgian flag on a pole. Perfect for an atmosphere that flirts with perfection... just like a perfectly crispy waffle!")]
    [Ecopedia("Housing Objects", "Flag", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(5000)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Drapeau_BelgiqueItem : WorldObjectItem<Drapeau_BelgiqueObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName = typeof(Drapeau_BelgiqueObject).UILink(),
            Category = HousingConfig.GetRoomCategory("Outdoor"),
            BaseValue = 6,
            TypeForRoomLimit = Localizer.DoStr("Statue"),
            DiminishingReturnMultiplier = 0.3f

        };

    }

    [RequiresSkill(typeof(TailoringSkill), 1)]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Flag_Belgian")]
    public partial class Drapeau_BelgiqueRecipe : Recipe
    {
        public Drapeau_BelgiqueRecipe()
        {
            this.Init(
                name: "Flag_Belgian",
                displayName: Localizer.DoStr("Flag_Belgian"),


                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronBarItem), 3, typeof(TailoringSkill)),
                    new IngredientElement(typeof(LinenFabricItem), 10, typeof(TailoringSkill)),
                },


                items: new List<CraftingElement>
                {
                    new CraftingElement<Drapeau_BelgiqueItem>()
                });

            this.ModsPostInitialize();
            CraftingComponent.AddTagProduct(typeof(TailoringTableObject), typeof(DrapeauRecipe), this);
        }

        partial void ModsPostInitialize();
    }





    // ______________________________________________________ Flag_Canada ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Flag_Canada")]
    public partial class Drapeau_CanadaObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_CanadaItem);
        public override LocString DisplayName => Localizer.DoStr("Flag_Canada");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<HousingComponent>().HomeValue = Drapeau_CanadaItem.homeValue;
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Flag_Canada")]
    [LocDescription("Flag of Canada on a pole. Perfect for a chill vibe, with a hint of maple syrup in the air!")]
    [Ecopedia("Housing Objects", "Flag", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(5000)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Drapeau_CanadaItem : WorldObjectItem<Drapeau_CanadaObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName = typeof(Drapeau_CanadaObject).UILink(),
            Category = HousingConfig.GetRoomCategory("Outdoor"),
            BaseValue = 6,
            TypeForRoomLimit = Localizer.DoStr("Statue"),
            DiminishingReturnMultiplier = 0.3f

        };

    }

    [RequiresSkill(typeof(TailoringSkill), 1)]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Flag_Canada")]
    public partial class Drapeau_CanadaRecipe : Recipe
    {
        public Drapeau_CanadaRecipe()
        {
            this.Init(
                name: "Flag_Canada",
                displayName: Localizer.DoStr("Flag_Canada"),


                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronBarItem), 3, typeof(TailoringSkill)),
                    new IngredientElement(typeof(LinenFabricItem), 10, typeof(TailoringSkill)),
                },


                items: new List<CraftingElement>
                {
                    new CraftingElement<Drapeau_CanadaItem>()
                });

            this.ModsPostInitialize();
            CraftingComponent.AddTagProduct(typeof(TailoringTableObject), typeof(DrapeauRecipe), this);
        }

        partial void ModsPostInitialize();
    }






    // ______________________________________________________ Flag_Morocco ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Flag_Morocco")]
    public partial class Drapeau_MarocObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_MarocItem);
        public override LocString DisplayName => Localizer.DoStr("Flag_Morocco");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<HousingComponent>().HomeValue = Drapeau_MarocItem.homeValue;
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Flag_Morocco")]
    [LocDescription("Flag of Morocco on a pole. Perfect for a warm atmosphere, without even needing mint tea!")]
    [Ecopedia("Housing Objects", "Flag", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(5000)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Drapeau_MarocItem : WorldObjectItem<Drapeau_MarocObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName = typeof(Drapeau_MarocObject).UILink(),
            Category = HousingConfig.GetRoomCategory("Outdoor"),
            BaseValue = 6,
            TypeForRoomLimit = Localizer.DoStr("Statue"),
            DiminishingReturnMultiplier = 0.3f

        };

    }

    [RequiresSkill(typeof(TailoringSkill), 1)]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Flag_Morocco")]
    public partial class Drapeau_MarocRecipe : Recipe
    {
        public Drapeau_MarocRecipe()
        {
            this.Init(
                name: "Flag_Morocco",
                displayName: Localizer.DoStr("Flag_Morocco"),


                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronBarItem), 3, typeof(TailoringSkill)),
                    new IngredientElement(typeof(LinenFabricItem), 10, typeof(TailoringSkill)),
                },


                items: new List<CraftingElement>
                {
                    new CraftingElement<Drapeau_MarocItem>()
                });

            this.ModsPostInitialize();
            CraftingComponent.AddTagProduct(typeof(TailoringTableObject), typeof(DrapeauRecipe), this);
        }

        partial void ModsPostInitialize();
    }





    // ______________________________________________________ Flag_Denmark ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Flag_Denmark")]
    public partial class Drapeau_DanemarkObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_DanemarkItem);
        public override LocString DisplayName => Localizer.DoStr("Flag_Denmark");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<HousingComponent>().HomeValue = Drapeau_DanemarkItem.homeValue;
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Flag_Denmark")]
    [LocDescription("Flag of Denmark on a pole. Perfect for a cozy atmosphere... almost as hygge as an evening by the fire!")]
    [Ecopedia("Housing Objects", "Flag", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(5000)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Drapeau_DanemarkItem : WorldObjectItem<Drapeau_DanemarkObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName = typeof(Drapeau_DanemarkObject).UILink(),
            Category = HousingConfig.GetRoomCategory("Outdoor"),
            BaseValue = 6,
            TypeForRoomLimit = Localizer.DoStr("Statue"),
            DiminishingReturnMultiplier = 0.3f

        };

    }

    [RequiresSkill(typeof(TailoringSkill), 1)]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Flag_Denmark")]
    public partial class Drapeau_DanemarkRecipe : Recipe
    {
        public Drapeau_DanemarkRecipe()
        {
            this.Init(
                name: "Flag_Denmark",
                displayName: Localizer.DoStr("Flag_Denmark"),


                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronBarItem), 3, typeof(TailoringSkill)),
                    new IngredientElement(typeof(LinenFabricItem), 10, typeof(TailoringSkill)),
                },


                items: new List<CraftingElement>
                {
                    new CraftingElement<Drapeau_DanemarkItem>()
                });

            this.ModsPostInitialize();
            CraftingComponent.AddTagProduct(typeof(TailoringTableObject), typeof(DrapeauRecipe), this);
        }

        partial void ModsPostInitialize();
    }





    // ______________________________________________________ Flag_Turkey ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Flag_Turkey")]
    public partial class Drapeau_TurquieObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_TurquieItem);
        public override LocString DisplayName => Localizer.DoStr("Flag_Turkey");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<HousingComponent>().HomeValue = Drapeau_TurquieItem.homeValue;
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Flag_Turkey")]
    [LocDescription("Flag of Turkey on a pole. Perfect for an ambiance that shines... like a beautifully golden baklava!")]
    [Ecopedia("Housing Objects", "Flag", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(5000)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Drapeau_TurquieItem : WorldObjectItem<Drapeau_TurquieObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName = typeof(Drapeau_TurquieObject).UILink(),
            Category = HousingConfig.GetRoomCategory("Outdoor"),
            BaseValue = 6,
            TypeForRoomLimit = Localizer.DoStr("Statue"),
            DiminishingReturnMultiplier = 0.3f

        };

    }

    [RequiresSkill(typeof(TailoringSkill), 1)]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Flag_Turkey")]
    public partial class Drapeau_TurquieRecipe : Recipe
    {
        public Drapeau_TurquieRecipe()
        {
            this.Init(
                name: "Drapeau_Turquie",
                displayName: Localizer.DoStr("Flag_Turkey"),


                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronBarItem), 3, typeof(TailoringSkill)),
                    new IngredientElement(typeof(LinenFabricItem), 10, typeof(TailoringSkill)),
                },


                items: new List<CraftingElement>
                {
                    new CraftingElement<Drapeau_TurquieItem>()
                });

            this.ModsPostInitialize();
            CraftingComponent.AddTagProduct(typeof(TailoringTableObject), typeof(DrapeauRecipe), this);
        }

        partial void ModsPostInitialize();
    }




    // ______________________________________________________ Flag_Algeria ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Flag_Algeria")]
    public partial class Drapeau_AlgerieObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_AlgerieItem);
        public override LocString DisplayName => Localizer.DoStr("Flag_Algeria");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<HousingComponent>().HomeValue = Drapeau_AlgerieItem.homeValue;
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Flag_Algeria")]
    [LocDescription("Algerian flag on a pole. Perfect for a lively vibe... without having to dance raï in your living room!")]
    [Ecopedia("Housing Objects", "Flag", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(5000)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Drapeau_AlgerieItem : WorldObjectItem<Drapeau_AlgerieObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName = typeof(Drapeau_AlgerieObject).UILink(),
            Category = HousingConfig.GetRoomCategory("Outdoor"),
            BaseValue = 6,
            TypeForRoomLimit = Localizer.DoStr("Statue"),
            DiminishingReturnMultiplier = 0.3f

        };

    }

    [RequiresSkill(typeof(TailoringSkill), 1)]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Flag_Algeria")]
    public partial class Drapeau_AlgerieRecipe : Recipe
    {
        public Drapeau_AlgerieRecipe()
        {
            this.Init(
                name: "Flag_Algeria",
                displayName: Localizer.DoStr("Flag_Algeria"),


                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronBarItem), 3, typeof(TailoringSkill)),
                    new IngredientElement(typeof(LinenFabricItem), 10, typeof(TailoringSkill)),
                },


                items: new List<CraftingElement>
                {
                    new CraftingElement<Drapeau_AlgerieItem>()
                });

            this.ModsPostInitialize();
            CraftingComponent.AddTagProduct(typeof(TailoringTableObject), typeof(DrapeauRecipe), this);
        }

        partial void ModsPostInitialize();
    }





    // ______________________________________________________ Flag_Austrian ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Flag_Austrian")]
    public partial class Drapeau_AutricheObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_AutricheItem);
        public override LocString DisplayName => Localizer.DoStr("Flag_Austrian");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<HousingComponent>().HomeValue = Drapeau_AutricheItem.homeValue;
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Flag_Austrian")]
    [LocDescription("Austrian flag on a pole. Perfect for an atmosphere that makes you yodel with joy... without even climbing a mountain!")]
    [Ecopedia("Housing Objects", "Flag", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(5000)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Drapeau_AutricheItem : WorldObjectItem<Drapeau_AutricheObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName = typeof(Drapeau_AutricheObject).UILink(),
            Category = HousingConfig.GetRoomCategory("Outdoor"),
            BaseValue = 6,
            TypeForRoomLimit = Localizer.DoStr("Statue"),
            DiminishingReturnMultiplier = 0.3f

        };

    }

    [RequiresSkill(typeof(TailoringSkill), 1)]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Flag_Austrian")]
    public partial class Drapeau_AutricheRecipe : Recipe
    {
        public Drapeau_AutricheRecipe()
        {
            this.Init(
                name: "Flag_Austrian",
                displayName: Localizer.DoStr("Flag_Austrian"),


                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronBarItem), 3, typeof(TailoringSkill)),
                    new IngredientElement(typeof(LinenFabricItem), 10, typeof(TailoringSkill)),
                },


                items: new List<CraftingElement>
                {
                    new CraftingElement<Drapeau_AutricheItem>()
                });

            this.ModsPostInitialize();
            CraftingComponent.AddTagProduct(typeof(TailoringTableObject), typeof(DrapeauRecipe), this);
        }

        partial void ModsPostInitialize();
    }






    // ______________________________________________________ Flag_Finland ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Flag_Finland")]
    public partial class Drapeau_FinlandeObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_FinlandeItem);
        public override LocString DisplayName => Localizer.DoStr("Flag_Finland");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<HousingComponent>().HomeValue = Drapeau_FinlandeItem.homeValue;
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Flag_Finland")]
    [LocDescription("Flag of Finland on a pole. Perfect for a calm atmosphere, with a touch of sauna and northern lights!")]
    [Ecopedia("Housing Objects", "Flag", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(5000)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Drapeau_FinlandeItem : WorldObjectItem<Drapeau_FinlandeObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName = typeof(Drapeau_FinlandeObject).UILink(),
            Category = HousingConfig.GetRoomCategory("Outdoor"),
            BaseValue = 6,
            TypeForRoomLimit = Localizer.DoStr("Statue"),
            DiminishingReturnMultiplier = 0.3f

        };

    }

    [RequiresSkill(typeof(TailoringSkill), 1)]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Flag_Finland")]
    public partial class Drapeau_FinlandeRecipe : Recipe
    {
        public Drapeau_FinlandeRecipe()
        {
            this.Init(
                name: "Flag_Finland",
                displayName: Localizer.DoStr("Flag_Finland"),


                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronBarItem), 3, typeof(TailoringSkill)),
                    new IngredientElement(typeof(LinenFabricItem), 10, typeof(TailoringSkill)),
                },


                items: new List<CraftingElement>
                {
                    new CraftingElement<Drapeau_FinlandeItem>()
                });

            this.ModsPostInitialize();
            CraftingComponent.AddTagProduct(typeof(TailoringTableObject), typeof(DrapeauRecipe), this);
        }

        partial void ModsPostInitialize();
    }





    // ______________________________________________________ Flag_Ireland ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Flag_Ireland")]
    public partial class Drapeau_IrlandeObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_IrlandeItem);
        public override LocString DisplayName => Localizer.DoStr("Flag_Ireland");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<HousingComponent>().HomeValue = Drapeau_IrlandeItem.homeValue;
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Flag_Ireland")]
    [LocDescription("Flag of Ireland on a pole. Perfect for a cheerful vibe, without even needing to search for the pot of gold at the end of the rainbow!")]
    [Ecopedia("Housing Objects", "Flag", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(5000)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Drapeau_IrlandeItem : WorldObjectItem<Drapeau_IrlandeObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName = typeof(Drapeau_IrlandeObject).UILink(),
            Category = HousingConfig.GetRoomCategory("Outdoor"),
            BaseValue = 6,
            TypeForRoomLimit = Localizer.DoStr("Statue"),
            DiminishingReturnMultiplier = 0.3f

        };

    }

    [RequiresSkill(typeof(TailoringSkill), 1)]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Flag_Ireland")]
    public partial class Drapeau_IrlandeRecipe : Recipe
    {
        public Drapeau_IrlandeRecipe()
        {
            this.Init(
                name: "Drapeau_Irlande",
                displayName: Localizer.DoStr("Flag_Ireland"),


                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronBarItem), 3, typeof(TailoringSkill)),
                    new IngredientElement(typeof(LinenFabricItem), 10, typeof(TailoringSkill)),
                },


                items: new List<CraftingElement>
                {
                    new CraftingElement<Drapeau_IrlandeItem>()
                });

            this.ModsPostInitialize();
            CraftingComponent.AddTagProduct(typeof(TailoringTableObject), typeof(DrapeauRecipe), this);
        }

        partial void ModsPostInitialize();
    }





    // ______________________________________________________ Flag_Poland ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Flag_Poland")]
    public partial class Drapeau_PologneObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_PologneItem);
        public override LocString DisplayName => Localizer.DoStr("Flag_Poland");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<HousingComponent>().HomeValue = Drapeau_PologneItem.homeValue;
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Flag_Poland")]
    [LocDescription("Flag of Poland on a flagpole. Perfect for an elegant atmosphere, without even needing to take a trip to the mountains!")]
    [Ecopedia("Housing Objects", "Flag", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(5000)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Drapeau_PologneItem : WorldObjectItem<Drapeau_PologneObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName = typeof(Drapeau_PologneObject).UILink(),
            Category = HousingConfig.GetRoomCategory("Outdoor"),
            BaseValue = 6,
            TypeForRoomLimit = Localizer.DoStr("Statue"),
            DiminishingReturnMultiplier = 0.3f

        };

    }

    [RequiresSkill(typeof(TailoringSkill), 1)]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Flag_Poland")]
    public partial class Drapeau_PologneRecipe : Recipe
    {
        public Drapeau_PologneRecipe()
        {
            this.Init(
                name: "Flag_Poland",
                displayName: Localizer.DoStr("Flag_Poland"),


                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronBarItem), 3, typeof(TailoringSkill)),
                    new IngredientElement(typeof(LinenFabricItem), 10, typeof(TailoringSkill)),
                },


                items: new List<CraftingElement>
                {
                    new CraftingElement<Drapeau_PologneItem>()
                });

            this.ModsPostInitialize();
            CraftingComponent.AddTagProduct(typeof(TailoringTableObject), typeof(DrapeauRecipe), this);
        }

        partial void ModsPostInitialize();
    }





    // ______________________________________________________ Flag_Russia ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Flag_Russia")]
    public partial class Drapeau_RussieObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_RussieItem);
        public override LocString DisplayName => Localizer.DoStr("Flag_Russia");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<HousingComponent>().HomeValue = Drapeau_RussieItem.homeValue;
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Flag_Russia")]
    [LocDescription("Flag of Russia on a flagpole. Perfect for a majestic atmosphere, without having to cross Siberia!")]
    [Ecopedia("Housing Objects", "Flag", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(5000)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Drapeau_RussieItem : WorldObjectItem<Drapeau_RussieObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName = typeof(Drapeau_RussieObject).UILink(),
            Category = HousingConfig.GetRoomCategory("Outdoor"),
            BaseValue = 6,
            TypeForRoomLimit = Localizer.DoStr("Statue"),
            DiminishingReturnMultiplier = 0.3f

        };

    }

    [RequiresSkill(typeof(TailoringSkill), 1)]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Flag_Russia")]
    public partial class Drapeau_RussieRecipe : Recipe
    {
        public Drapeau_RussieRecipe()
        {
            this.Init(
                name: "Drapeau_Russie",
                displayName: Localizer.DoStr("Flag_Russia"),


                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronBarItem), 3, typeof(TailoringSkill)),
                    new IngredientElement(typeof(LinenFabricItem), 10, typeof(TailoringSkill)),
                },


                items: new List<CraftingElement>
                {
                    new CraftingElement<Drapeau_RussieItem>()
                });

            this.ModsPostInitialize();
            CraftingComponent.AddTagProduct(typeof(TailoringTableObject), typeof(DrapeauRecipe), this);
        }

        partial void ModsPostInitialize();
    }






    // ______________________________________________________ Flag_America ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Flag_America")]
    public partial class Drapeau_AmeriqueObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_AmeriqueItem);
        public override LocString DisplayName => Localizer.DoStr("Flag_America");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<HousingComponent>().HomeValue = Drapeau_AmeriqueItem.homeValue;
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Flag_America")]
    [LocDescription("American flag on a flagpole. Perfect for a starry atmosphere, without having to tour all 50 states!")]
    [Ecopedia("Housing Objects", "Flag", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(5000)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Drapeau_AmeriqueItem : WorldObjectItem<Drapeau_AmeriqueObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName = typeof(Drapeau_AmeriqueObject).UILink(),
            Category = HousingConfig.GetRoomCategory("Outdoor"),
            BaseValue = 6,
            TypeForRoomLimit = Localizer.DoStr("Statue"),
            DiminishingReturnMultiplier = 0.3f

        };

    }

    [RequiresSkill(typeof(TailoringSkill), 1)]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Flag_America")]
    public partial class Drapeau_AmeriqueRecipe : Recipe
    {
        public Drapeau_AmeriqueRecipe()
        {
            this.Init(
                name: "Flag_America",
                displayName: Localizer.DoStr("Flag_America"),


                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronBarItem), 3, typeof(TailoringSkill)),
                    new IngredientElement(typeof(LinenFabricItem), 10, typeof(TailoringSkill)),
                },


                items: new List<CraftingElement>
                {
                    new CraftingElement<Drapeau_AmeriqueItem>()
                });

            this.ModsPostInitialize();
            CraftingComponent.AddTagProduct(typeof(TailoringTableObject), typeof(DrapeauRecipe), this);
        }

        partial void ModsPostInitialize();
    }







    // ______________________________________________________ Flag_Tunisia ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Flag_Tunisia")]
    public partial class Drapeau_TunisieObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_TunisieItem);
        public override LocString DisplayName => Localizer.DoStr("Flag_Tunisia");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<HousingComponent>().HomeValue = Drapeau_TunisieItem.homeValue;
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Flag_Tunisia")]
    [LocDescription("Flag of Tunisia on a flagpole. Perfect for a sunny atmosphere, without even needing any sand!")]
    [Ecopedia("Housing Objects", "Flag", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(5000)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Drapeau_TunisieItem : WorldObjectItem<Drapeau_TunisieObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName = typeof(Drapeau_TunisieObject).UILink(),
            Category = HousingConfig.GetRoomCategory("Outdoor"),
            BaseValue = 6,
            TypeForRoomLimit = Localizer.DoStr("Statue"),
            DiminishingReturnMultiplier = 0.3f

        };

    }

    [RequiresSkill(typeof(TailoringSkill), 1)]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Flag_Tunisia")]
    public partial class Drapeau_TunisieRecipe : Recipe
    {
        public Drapeau_TunisieRecipe()
        {
            this.Init(
                name: "Flag_Tunisia",
                displayName: Localizer.DoStr("Flag_Tunisia"),


                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronBarItem), 3, typeof(TailoringSkill)),
                    new IngredientElement(typeof(LinenFabricItem), 10, typeof(TailoringSkill)),
                },


                items: new List<CraftingElement>
                {
                    new CraftingElement<Drapeau_TunisieItem>()
                });

            this.ModsPostInitialize();
            CraftingComponent.AddTagProduct(typeof(TailoringTableObject), typeof(DrapeauRecipe), this);
        }

        partial void ModsPostInitialize();
    }







    // ______________________________________________________ Drapeau_LesFranquois ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Drapeau_LesFranquois")]
    public partial class Drapeau_LesFranquoisObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_LesFranquoisItem);
        public override LocString DisplayName => Localizer.DoStr("Drapeau_LesFranquois");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<HousingComponent>().HomeValue = Drapeau_LesFranquoisItem.homeValue;
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Drapeau_LesFranquois")]
    [LocDescription("Drapeau de la communauté Les Franquois, communauté francophone autour du jeu ECO proposant différents serveurs de jeu offrant une expérience à chaque fois unique ! www.lesfranquois.fr")]
    [Ecopedia("Housing Objects", "Flag", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(5000)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Drapeau_LesFranquoisItem : WorldObjectItem<Drapeau_LesFranquoisObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName = typeof(Drapeau_LesFranquoisObject).UILink(),
            Category = HousingConfig.GetRoomCategory("Outdoor"),
            BaseValue = 6,
            TypeForRoomLimit = Localizer.DoStr("Statue"),
            DiminishingReturnMultiplier = 0.3f

        };

    }

    [RequiresSkill(typeof(TailoringSkill), 1)]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Drapeau_LesFranquois")]
    public partial class Drapeau_LesFranquoisRecipe : Recipe
    {
        public Drapeau_LesFranquoisRecipe()
        {
            this.Init(
                name: "Drapeau_LesFranquois",
                displayName: Localizer.DoStr("Drapeau_LesFranquois"),


                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronBarItem), 3, typeof(TailoringSkill)),
                    new IngredientElement(typeof(LinenFabricItem), 10, typeof(TailoringSkill)),
                },


                items: new List<CraftingElement>
                {
                    new CraftingElement<Drapeau_LesFranquoisItem>()
                });

            this.ModsPostInitialize();
            CraftingComponent.AddTagProduct(typeof(TailoringTableObject), typeof(DrapeauRecipe), this);
        }

        partial void ModsPostInitialize();
    }




    // ______________________________________________________ Drapeau_LeVillage ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Drapeau_LeVillage")]
    public partial class Drapeau_LeVillageObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_LeVillageItem);
        public override LocString DisplayName => Localizer.DoStr("Drapeau_LeVillage");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<HousingComponent>().HomeValue = Drapeau_LeVillageItem.homeValue;
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Drapeau_LeVillage")]
    [LocDescription("Le Village est un serveur ECO de longue date, faisant parti du programme de serveurs recommandés par l'équipe de développement (SLG), activement administré par une équipe en étroite relation avec les développeurs du jeu (SLG) et incluant des mods (dont certains sont \"made in Le Village\") tout en faisant en sorte de rester aligné avec les concepts généraux \"vanille\" du jeu.")]
    [Ecopedia("Housing Objects", "Flag", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(5000)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Drapeau_LeVillageItem : WorldObjectItem<Drapeau_LeVillageObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName = typeof(Drapeau_LeVillageObject).UILink(),
            Category = HousingConfig.GetRoomCategory("Outdoor"),
            BaseValue = 6,
            TypeForRoomLimit = Localizer.DoStr("Statue"),
            DiminishingReturnMultiplier = 0.3f

        };

    }

    [RequiresSkill(typeof(TailoringSkill), 1)]
    [Ecopedia("Housing Objects", "Flag", subPageName: "Drapeau_LeVillage")]
    public partial class Drapeau_LeVillageRecipe : Recipe
    {
        public Drapeau_LeVillageRecipe()
        {
            this.Init(
                name: "Drapeau_LeVillage",
                displayName: Localizer.DoStr("Drapeau_LeVillage"),


                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronBarItem), 3, typeof(TailoringSkill)),
                    new IngredientElement(typeof(LinenFabricItem), 10, typeof(TailoringSkill)),
                },


                items: new List<CraftingElement>
                {
                    new CraftingElement<Drapeau_LeVillageItem>()
                });

            this.ModsPostInitialize();
            CraftingComponent.AddTagProduct(typeof(TailoringTableObject), typeof(DrapeauRecipe), this);
        }

        partial void ModsPostInitialize();
    }




    // ______________________________________________________ Flag_Australia ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Flag_Australia")]
    public partial class Drapeau_AustralieObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_AustralieItem);
        public override LocString DisplayName => Localizer.DoStr("Flag_Australia");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<HousingComponent>().HomeValue = Drapeau_AustralieItem.homeValue;
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Flag_Australia")]
    [LocDescription("Bring a breath of Australia to your decor with this flag on a pole. Perfect for a Down Under vibe... without having to bump into any kangaroos!")]
    [Ecopedia("Housing Objects", "Drapeau", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(5000)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Drapeau_AustralieItem : WorldObjectItem<Drapeau_AustralieObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName = typeof(Drapeau_AustralieObject).UILink(),
            Category = HousingConfig.GetRoomCategory("Outdoor"),
            BaseValue = 6,
            TypeForRoomLimit = Localizer.DoStr("Statue"),
            DiminishingReturnMultiplier = 0.3f

        };

    }

    [RequiresSkill(typeof(TailoringSkill), 1)]
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Flag_Australia")]
    public partial class Drapeau_AustralieRecipe : Recipe
    {
        public Drapeau_AustralieRecipe()
        {
            this.Init(
                name: "Flag_Australia",
                displayName: Localizer.DoStr("Flag_Australia"),


                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronBarItem), 3, typeof(TailoringSkill)),
                    new IngredientElement(typeof(LinenFabricItem), 10, typeof(TailoringSkill)),
                },


                items: new List<CraftingElement>
                {
                    new CraftingElement<Drapeau_AustralieItem>()
                });

            this.ModsPostInitialize();
            CraftingComponent.AddTagProduct(typeof(TailoringTableObject), typeof(DrapeauRecipe), this);
        }

        partial void ModsPostInitialize();
    }

}