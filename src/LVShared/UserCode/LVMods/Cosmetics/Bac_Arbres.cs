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



    // ______________________________________________________ Box_Arbre_Palm ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Outdoor", subPageName: "Palmier Box")]
    public partial class Box_Arbre_PalmObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Box_Arbre_PalmItem);
        public override LocString DisplayName => Localizer.DoStr("Palmier Box");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<HousingComponent>().HomeValue = Box_Arbre_PalmItem.homeValue;
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Palmier Box")]
    [LocDescription("Besoin de vacances ? Ce palmier en caisse vous apporte l'ambiance tropicale, sans le sable entre les orteils !")]
    [Ecopedia("Housing Objects", "Outdoor", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(5000)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Box_Arbre_PalmItem : WorldObjectItem<Box_Arbre_PalmObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName = typeof(Box_Arbre_PalmObject).UILink(),
            Category = HousingConfig.GetRoomCategory("Outdoor"),
            BaseValue = 6,
            TypeForRoomLimit = Localizer.DoStr("Statue"),
            DiminishingReturnMultiplier = 0.3f

        };

    }

    [RequiresSkill(typeof(FarmingSkill), 1)]
    [Ecopedia("Housing Objects", "Outdoor", subPageName: "Palmier Box")]
    public partial class Box_Arbre_PalmRecipe : RecipeFamily
    {
        public Box_Arbre_PalmRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Palmier Box",
                displayName: Localizer.DoStr("Palmier Box"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(PalmSeedItem), 1, typeof(FarmingSkill), typeof(FarmingLavishResourcesTalent)),
                    new IngredientElement(typeof(HeliconiaSeedItem), 2, typeof(FarmingSkill), typeof(FarmingLavishResourcesTalent)),
                    new IngredientElement(typeof(BoxItem), 1, typeof(FarmingSkill), typeof(FarmingLavishResourcesTalent)),
                    new IngredientElement(typeof(DirtItem), 4, typeof(FarmingSkill), typeof(FarmingLavishResourcesTalent)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Box_Arbre_PalmItem>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.ExperienceOnCraft = 4;

            this.LaborInCalories = CreateLaborInCaloriesValue(60, typeof(FarmingSkill));

            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(Box_Arbre_PalmRecipe), start: 2, skillType: typeof(FarmingSkill), typeof(FarmingFocusedSpeedTalent), typeof(FarmingParallelSpeedTalent));

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Palmier Box"), recipeType: typeof(Box_Arbre_PalmRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(FarmersTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }





    // ______________________________________________________ Box_Arbre_Oak ______________________________________________________ \\


    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Outdoor", subPageName: "Chêne Box")]
    public partial class Box_Arbre_OakObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Box_Arbre_OakItem);
        public override LocString DisplayName => Localizer.DoStr("Chêne Box");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<HousingComponent>().HomeValue = Box_Arbre_OakItem.homeValue;
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Chêne Box")]
    [LocDescription("Avec ce chêne en boîte, la nature vient à vous, sans les glands... mais avec style !")]
    [Ecopedia("Housing Objects", "Outdoor", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(5000)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Box_Arbre_OakItem : WorldObjectItem<Box_Arbre_OakObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName = typeof(Box_Arbre_OakObject).UILink(),
            Category = HousingConfig.GetRoomCategory("Outdoor"),
            BaseValue = 6,
            TypeForRoomLimit = Localizer.DoStr("Statue"),
            DiminishingReturnMultiplier = 0.3f

        };

    }

    [RequiresSkill(typeof(FarmingSkill), 1)]
    [Ecopedia("Housing Objects", "Outdoor", subPageName: "Chêne Box")]
    public partial class Box_Arbre_OakRecipe : RecipeFamily
    {
        public Box_Arbre_OakRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Chêne Box",
                displayName: Localizer.DoStr("Chêne Box"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(AcornItem), 1, typeof(FarmingSkill), typeof(FarmingLavishResourcesTalent)),
                    new IngredientElement(typeof(HeliconiaSeedItem), 2, typeof(FarmingSkill), typeof(FarmingLavishResourcesTalent)),
                    new IngredientElement(typeof(BoxItem), 1, typeof(FarmingSkill), typeof(FarmingLavishResourcesTalent)),
                    new IngredientElement(typeof(DirtItem), 4, typeof(FarmingSkill), typeof(FarmingLavishResourcesTalent)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Box_Arbre_OakItem>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.ExperienceOnCraft = 4;

            this.LaborInCalories = CreateLaborInCaloriesValue(60, typeof(FarmingSkill));

            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(Box_Arbre_OakRecipe), start: 2, skillType: typeof(FarmingSkill), typeof(FarmingFocusedSpeedTalent), typeof(FarmingParallelSpeedTalent));

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Chêne Box"), recipeType: typeof(Box_Arbre_OakRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(FarmersTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }







    // ______________________________________________________ Box_Arbre_Redwood ______________________________________________________ \\


    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Outdoor", subPageName: "Séquoia Box")]
    public partial class Box_Arbre_RedwoodObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Box_Arbre_RedwoodItem);
        public override LocString DisplayName => Localizer.DoStr("Séquoia Box");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<HousingComponent>().HomeValue = Box_Arbre_RedwoodItem.homeValue;
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Séquoia Box")]
    [LocDescription("Ce redwood en pot rêve de s'enraciner... mais pour l'instant, il se contente d'un déménagement en caisse express !")]
    [Ecopedia("Housing Objects", "Outdoor", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(5000)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Box_Arbre_RedwoodItem : WorldObjectItem<Box_Arbre_RedwoodObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName = typeof(Box_Arbre_RedwoodObject).UILink(),
            Category = HousingConfig.GetRoomCategory("Outdoor"),
            BaseValue = 6,
            TypeForRoomLimit = Localizer.DoStr("Statue"),
            DiminishingReturnMultiplier = 0.3f

        };

    }

    [RequiresSkill(typeof(FarmingSkill), 1)]
    [Ecopedia("Housing Objects", "Outdoor", subPageName: "Séquoia Box")]
    public partial class Box_Arbre_RedwoodRecipe : RecipeFamily
    {
        public Box_Arbre_RedwoodRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Séquoia Box",
                displayName: Localizer.DoStr("Séquoia Box"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(RedwoodSeedItem), 1, typeof(FarmingSkill), typeof(FarmingLavishResourcesTalent)),
                    new IngredientElement(typeof(HeliconiaSeedItem), 2, typeof(FarmingSkill), typeof(FarmingLavishResourcesTalent)),
                    new IngredientElement(typeof(BoxItem), 1, typeof(FarmingSkill), typeof(FarmingLavishResourcesTalent)),
                    new IngredientElement(typeof(DirtItem), 4, typeof(FarmingSkill), typeof(FarmingLavishResourcesTalent)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Box_Arbre_RedwoodItem>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.ExperienceOnCraft = 4;

            this.LaborInCalories = CreateLaborInCaloriesValue(60, typeof(FarmingSkill));

            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(Box_Arbre_RedwoodRecipe), start: 2, skillType: typeof(FarmingSkill), typeof(FarmingFocusedSpeedTalent), typeof(FarmingParallelSpeedTalent));

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Séquoia Box"), recipeType: typeof(Box_Arbre_RedwoodRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(FarmersTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }







    // ______________________________________________________ Box_Arbre_Birch ______________________________________________________ \\


    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Outdoor", subPageName: "Bouleau Box")]
    public partial class Box_Arbre_BirchObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Box_Arbre_BirchItem);
        public override LocString DisplayName => Localizer.DoStr("Bouleau Box");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<HousingComponent>().HomeValue = Box_Arbre_BirchItem.homeValue;
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Bouleau Box")]
    [LocDescription("Le bouleau qui rêve d'être un bonsaï... mais avec une caisse XXL, parce que le style, ça compte aussi pour les arbres !")]
    [Ecopedia("Housing Objects", "Outdoor", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(5000)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Box_Arbre_BirchItem : WorldObjectItem<Box_Arbre_BirchObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName = typeof(Box_Arbre_BirchObject).UILink(),
            Category = HousingConfig.GetRoomCategory("Outdoor"),
            BaseValue = 6,
            TypeForRoomLimit = Localizer.DoStr("Statue"),
            DiminishingReturnMultiplier = 0.3f

        };

    }

    [RequiresSkill(typeof(FarmingSkill), 1)]
    [Ecopedia("Housing Objects", "Outdoor", subPageName: "Bouleau Box")]
    public partial class Box_Arbre_BirchRecipe : RecipeFamily
    {
        public Box_Arbre_BirchRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Bouleau Box",
                displayName: Localizer.DoStr("Bouleau Box"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(BirchSeedItem), 1, typeof(FarmingSkill), typeof(FarmingLavishResourcesTalent)),
                    new IngredientElement(typeof(HeliconiaSeedItem), 2, typeof(FarmingSkill), typeof(FarmingLavishResourcesTalent)),
                    new IngredientElement(typeof(BoxItem), 1, typeof(FarmingSkill), typeof(FarmingLavishResourcesTalent)),
                    new IngredientElement(typeof(DirtItem), 4, typeof(FarmingSkill), typeof(FarmingLavishResourcesTalent)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Box_Arbre_BirchItem>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.ExperienceOnCraft = 4;

            this.LaborInCalories = CreateLaborInCaloriesValue(60, typeof(FarmingSkill));

            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(Box_Arbre_BirchRecipe), start: 2, skillType: typeof(FarmingSkill), typeof(FarmingFocusedSpeedTalent), typeof(FarmingParallelSpeedTalent));

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Bouleau Box"), recipeType: typeof(Box_Arbre_BirchRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(FarmersTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }




    // ______________________________________________________ Box_Arbre_Spruce ______________________________________________________ \\


    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Outdoor", subPageName: "Épicéa Box")]
    public partial class Box_Arbre_SpruceObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Box_Arbre_SpruceItem);
        public override LocString DisplayName => Localizer.DoStr("Épicéa Box");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<HousingComponent>().HomeValue = Box_Arbre_SpruceItem.homeValue;
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Épicéa Box")]
    [LocDescription("Parce qu’un sapin dans une caisse en bois, c’est la nature version portable. Qui a dit qu’on ne pouvait pas transporter une forêt ?")]
    [Ecopedia("Housing Objects", "Outdoor", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(5000)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Box_Arbre_SpruceItem : WorldObjectItem<Box_Arbre_SpruceObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName = typeof(Box_Arbre_SpruceObject).UILink(),
            Category = HousingConfig.GetRoomCategory("Outdoor"),
            BaseValue = 6,
            TypeForRoomLimit = Localizer.DoStr("Statue"),
            DiminishingReturnMultiplier = 0.3f

        };

    }

    [RequiresSkill(typeof(FarmingSkill), 1)]
    [Ecopedia("Housing Objects", "Outdoor", subPageName: "Épicéa Box")]
    public partial class Box_Arbre_SpruceRecipe : RecipeFamily
    {
        public Box_Arbre_SpruceRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Épicéa Box",
                displayName: Localizer.DoStr("Épicéa Box"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(SpruceSeedItem), 1, typeof(FarmingSkill), typeof(FarmingLavishResourcesTalent)),
                    new IngredientElement(typeof(HeliconiaSeedItem), 2, typeof(FarmingSkill), typeof(FarmingLavishResourcesTalent)),
                    new IngredientElement(typeof(BoxItem), 1, typeof(FarmingSkill), typeof(FarmingLavishResourcesTalent)),
                    new IngredientElement(typeof(DirtItem), 4, typeof(FarmingSkill), typeof(FarmingLavishResourcesTalent)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Box_Arbre_SpruceItem>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.ExperienceOnCraft = 4;

            this.LaborInCalories = CreateLaborInCaloriesValue(60, typeof(FarmingSkill));

            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(Box_Arbre_SpruceRecipe), start: 2, skillType: typeof(FarmingSkill), typeof(FarmingFocusedSpeedTalent), typeof(FarmingParallelSpeedTalent));

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Épicéa Box"), recipeType: typeof(Box_Arbre_SpruceRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(FarmersTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }




    // ______________________________________________________ Box_Arbre_Cactus ______________________________________________________ \\


    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Outdoor", subPageName: "Cactus Box")]
    public partial class Box_Arbre_CactusObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Box_Arbre_CactusItem);
        public override LocString DisplayName => Localizer.DoStr("Cactus Box");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<HousingComponent>().HomeValue = Box_Arbre_CactusItem.homeValue;
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Cactus Box")]
    [LocDescription("Un cactus, bien planté dans une caisse, parce qu’un pot classique, c’était trop simple... et trop fragile !")]
    [Ecopedia("Housing Objects", "Outdoor", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(5000)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Box_Arbre_CactusItem : WorldObjectItem<Box_Arbre_CactusObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName = typeof(Box_Arbre_CactusObject).UILink(),
            Category = HousingConfig.GetRoomCategory("Outdoor"),
            BaseValue = 6,
            TypeForRoomLimit = Localizer.DoStr("Statue"),
            DiminishingReturnMultiplier = 0.3f

        };

    }

    [RequiresSkill(typeof(FarmingSkill), 1)]
    [Ecopedia("Housing Objects", "Outdoor", subPageName: "Cactus Box")]
    public partial class Box_Arbre_CactusRecipe : RecipeFamily
    {
        public Box_Arbre_CactusRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Cactus Box",  
                displayName: Localizer.DoStr("Cactus Box"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(SaguaroSeedItem), 1, typeof(FarmingSkill), typeof(FarmingLavishResourcesTalent)),
                    new IngredientElement(typeof(HeliconiaSeedItem), 2, typeof(FarmingSkill), typeof(FarmingLavishResourcesTalent)),
                    new IngredientElement(typeof(BoxItem), 1, typeof(FarmingSkill), typeof(FarmingLavishResourcesTalent)),
                    new IngredientElement(typeof(DirtItem), 4, typeof(FarmingSkill), typeof(FarmingLavishResourcesTalent)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Box_Arbre_CactusItem>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.ExperienceOnCraft = 4;

            this.LaborInCalories = CreateLaborInCaloriesValue(60, typeof(FarmingSkill));

            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(Box_Arbre_CactusRecipe), start: 2, skillType: typeof(FarmingSkill), typeof(FarmingFocusedSpeedTalent), typeof(FarmingParallelSpeedTalent));

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Cactus Box"), recipeType: typeof(Box_Arbre_CactusRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(FarmersTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }


    // ______________________________________________________ Box_Item ______________________________________________________ \\


    [RequiresSkill(typeof(LoggingSkill), 1)]
    [ForceCreateView]
    [Ecopedia("Items", "Products", subPageName: "Box Item")]
    public partial class BoxRecipe : Recipe
    {
        public BoxRecipe()
        {
            this.Init(
                name: "Box",  
                displayName: Localizer.DoStr("Box"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 12, typeof(LoggingSkill)), 
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<BoxItem>()
                });
           
            this.ModsPostInitialize();
            CraftingComponent.AddTagProduct(typeof(CarpentryTableObject), typeof(BoxRecipe), this);
        }


        partial void ModsPostInitialize();
    }

    [Serialized] 
    [LocDisplayName("Box")] 
    [Weight(500)] 
    [Tag("Currency")]
    [Currency] 
    [Ecopedia("Items", "Products", createAsSubPage: true)]
    [LocDescription("Caisse en bois, utilisée pour la fabrication de bacs à fleurs.")] 
    public partial class BoxItem : Item
    {


    }
}