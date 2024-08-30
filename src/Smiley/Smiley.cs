// MOD Smiley par Plex 
// Mise à jour le 30/08/2024

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



    // ______________________________________________________ Smiley01 ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Grinche-Molle")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley01Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley01Item);
        public override LocString DisplayName => Localizer.DoStr("Grinche-Molle");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Grinche-Molle")]
    [LocDescription("Ce sourire exagéré cache peut-être une légère envie de grincer des dents.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(150)] 
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley01Item : WorldObjectItem<Smiley01Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }


    [Ecopedia("Housing Objects", "Smiley", subPageName: "Grinche-Molle")]
    public partial class Smiley01Recipe : RecipeFamily
    {
        public Smiley01Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Grinche-Molle",  
                displayName: Localizer.DoStr("Grinche-Molle"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)), 
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley01Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2; 

            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Grinche-Molle"), recipeType: typeof(Smiley01Recipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }




    // ______________________________________________________ Smiley02 ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Content Lumineux")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley02Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley02Item);
        public override LocString DisplayName => Localizer.DoStr("Content Lumineux");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Content Lumineux")]
    [LocDescription("Un sourire avec de grands yeux ouverts, symbole de bonheur pur.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(150)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley02Item : WorldObjectItem<Smiley02Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }


    [Ecopedia("Housing Objects", "Smiley", subPageName: "Content Lumineux")]
    public partial class Smiley02Recipe : RecipeFamily
    {
        public Smiley02Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Grinche-Molle",
                displayName: Localizer.DoStr("Content Lumineux"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley02Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2;

            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Content Lumineux"), recipeType: typeof(Smiley02Recipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }




    // ______________________________________________________ Smiley03 ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Sourire Doux")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley03Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley03Item);
        public override LocString DisplayName => Localizer.DoStr("Sourire Doux");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Sourire Doux")]
    [LocDescription("Un sourire discret et apaisant, transmettant sérénité.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(150)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley03Item : WorldObjectItem<Smiley03Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }


    [Ecopedia("Housing Objects", "Smiley", subPageName: "Sourire Doux")]
    public partial class Smiley03Recipe : RecipeFamily
    {
        public Smiley03Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Sourire Doux",
                displayName: Localizer.DoStr("Sourire Doux"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley03Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2;

            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Sourire Doux"), recipeType: typeof(Smiley03Recipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }



    // ______________________________________________________ Smiley04 ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Ouvr'Oeil")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley04Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley04Item);
        public override LocString DisplayName => Localizer.DoStr("Ouvr'Oeil");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Ouvr'Oeil")]
    [LocDescription("Quand t'es tellement content que tes yeux jouent à cache-cache.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(150)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley04Item : WorldObjectItem<Smiley04Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }


    [Ecopedia("Housing Objects", "Smiley", subPageName: "Ouvr'Oeil")]
    public partial class Smiley04Recipe : RecipeFamily
    {
        public Smiley04Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Ouvr'Oeil",
                displayName: Localizer.DoStr("Ouvr'Oeil"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley04Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2;

            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Ouvr'Oeil"), recipeType: typeof(Smiley04Recipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }




    // ______________________________________________________ Smiley05 ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Lol-Mania")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley05Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley05Item);
        public override LocString DisplayName => Localizer.DoStr("Lol-Mania");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Lol-Mania")]
    [LocDescription("Le rire qui te fait presque oublier de respirer.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(150)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley05Item : WorldObjectItem<Smiley05Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }


    [Ecopedia("Housing Objects", "Smiley", subPageName: "Lol-Mania")]
    public partial class Smiley05Recipe : RecipeFamily
    {
        public Smiley05Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Lol-Mania",
                displayName: Localizer.DoStr("Lol-Mania"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley05Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2;

            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Lol-Mania"), recipeType: typeof(Smiley05Recipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }




    // ______________________________________________________ Smiley06 ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Sueurtitude")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley06Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley06Item);
        public override LocString DisplayName => Localizer.DoStr("Sueurtitude");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Sueurtitude")]
    [LocDescription("Quand t'es heureux mais que la pression monte, comme un examen surprise.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(150)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley06Item : WorldObjectItem<Smiley06Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }


    [Ecopedia("Housing Objects", "Smiley", subPageName: "Sueurtitude")]
    public partial class Smiley06Recipe : RecipeFamily
    {
        public Smiley06Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Sueurtitude",
                displayName: Localizer.DoStr("Sueurtitude"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley06Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2;

            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Sueurtitude"), recipeType: typeof(Smiley06Recipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }





    // ______________________________________________________ Smiley07 ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Fontaine de Rire")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley07Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley07Item);
        public override LocString DisplayName => Localizer.DoStr("Fontaine de Rire");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Fontaine de Rire")]
    [LocDescription("Tu rigoles tellement que tu pourrais remplir une piscine avec tes larmes.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(150)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley07Item : WorldObjectItem<Smiley07Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }


    [Ecopedia("Housing Objects", "Smiley", subPageName: "Fontaine de Rire")]
    public partial class Smiley07Recipe : RecipeFamily
    {
        public Smiley07Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Fontaine de Rire",
                displayName: Localizer.DoStr("Fontaine de Rire"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley07Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2;

            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Fontaine de Rire"), recipeType: typeof(Smiley07Recipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }




    // ______________________________________________________ Smiley08 ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Larmes de Joie")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley08Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley08Item);
        public override LocString DisplayName => Localizer.DoStr("Larmes de Joie");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Larmes de Joie")]
    [LocDescription("Rire aux éclats jusqu'aux larmes, pour les situations hilarantes.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(150)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley08Item : WorldObjectItem<Smiley08Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }


    [Ecopedia("Housing Objects", "Smiley", subPageName: "Larmes de Joie")]
    public partial class Smiley08Recipe : RecipeFamily
    {
        public Smiley08Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Larmes de Joie",
                displayName: Localizer.DoStr("Larmes de Joie"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley08Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2;

            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Larmes de Joie"), recipeType: typeof(Smiley08Recipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }





    // ______________________________________________________ Smiley09 ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Blushy-Cool")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley09Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley09Item);
        public override LocString DisplayName => Localizer.DoStr("Blushy-Cool");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Blushy-Cool")]
    [LocDescription("Le sourire qui dit \"merci, mais arrête, je vais rougir encore plus\".")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(150)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley09Item : WorldObjectItem<Smiley09Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }


    [Ecopedia("Housing Objects", "Smiley", subPageName: "Blushy-Cool")]
    public partial class Smiley09Recipe : RecipeFamily
    {
        public Smiley09Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Blushy-Cool",
                displayName: Localizer.DoStr("Blushy-Cool"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley09Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2;

            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Blushy-Cool"), recipeType: typeof(Smiley09Recipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }





    // ______________________________________________________ Smiley10 ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Timide Heureux")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley10Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley10Item);
        public override LocString DisplayName => Localizer.DoStr("Timide Heureux");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Timide Heureux")]
    [LocDescription("Un sourire modeste avec des joues rougissantes, idéal pour exprimer la timidité.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(150)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley10Item : WorldObjectItem<Smiley10Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }


    [Ecopedia("Housing Objects", "Smiley", subPageName: "Timide Heureux")]
    public partial class Smiley10Recipe : RecipeFamily
    {
        public Smiley10Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Timide Heureux",
                displayName: Localizer.DoStr("Timide Heureux"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley10Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2;

            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Timide Heureux"), recipeType: typeof(Smiley10Recipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }






    // ______________________________________________________ Smiley11 ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Saint Sourire")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley11Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley11Item);
        public override LocString DisplayName => Localizer.DoStr("Saint Sourire");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Saint Sourire")]
    [LocDescription("Tu viens de faire quelque chose de bien... ou de bien suspect !")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(150)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley11Item : WorldObjectItem<Smiley11Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }


    [Ecopedia("Housing Objects", "Smiley", subPageName: "Saint Sourire")]
    public partial class Smiley11Recipe : RecipeFamily
    {
        public Smiley11Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Saint Sourire",
                displayName: Localizer.DoStr("Saint Sourire"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley11Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2;

            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Saint Sourire"), recipeType: typeof(Smiley11Recipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }




    // ______________________________________________________ Smiley12 ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smile-Basique")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley12Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley12Item);
        public override LocString DisplayName => Localizer.DoStr("Smile-Basique");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Smile-Basique")]
    [LocDescription("Le sourire passe-partout pour les moments où tout va... moyen.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(150)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley12Item : WorldObjectItem<Smiley12Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }


    [Ecopedia("Housing Objects", "Smiley", subPageName: "Smile-Basique")]
    public partial class Smiley12Recipe : RecipeFamily
    {
        public Smiley12Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Smile-Basique",
                displayName: Localizer.DoStr("Smile-Basique"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley12Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2;

            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Smile-Basique"), recipeType: typeof(Smiley12Recipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }





    // ______________________________________________________ Smiley13 ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "CalmePlat")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley13Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley13Item);
        public override LocString DisplayName => Localizer.DoStr("CalmePlat");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("CalmePlat")]
    [LocDescription("Le visage d'un pro du zen, même face à une montagne de boulot.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(150)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley13Item : WorldObjectItem<Smiley13Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }


    [Ecopedia("Housing Objects", "Smiley", subPageName: "CalmePlat")]
    public partial class Smiley13Recipe : RecipeFamily
    {
        public Smiley13Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "CalmePlat",
                displayName: Localizer.DoStr("CalmePlat"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley13Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2;

            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("CalmePlat"), recipeType: typeof(Smiley13Recipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }





    // ______________________________________________________ Smiley14 ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Clin d'Oeil-coquin")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley14Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley14Item);
        public override LocString DisplayName => Localizer.DoStr("Clin d'Oeil-coquin");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Clin d'Oeil-coquin")]
    [LocDescription("Un petit clin d'œil pour dire \"Je sais ce que tu sais\".")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(150)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley14Item : WorldObjectItem<Smiley14Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }


    [Ecopedia("Housing Objects", "Smiley", subPageName: "Clin d'Oeil-coquin")]
    public partial class Smiley14Recipe : RecipeFamily
    {
        public Smiley14Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Clin d'Oeil-coquin",
                displayName: Localizer.DoStr("Clin d'Oeil-coquin"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley14Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2;

            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Clin d'Oeil-coquin"), recipeType: typeof(Smiley14Recipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }





    // ______________________________________________________ Smiley15 ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Repos-Face")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley15Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley15Item);
        public override LocString DisplayName => Localizer.DoStr("Repos-Face");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Repos-Face")]
    [LocDescription("Tellement détendu que t'es à deux doigts de t'endormir.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(150)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley15Item : WorldObjectItem<Smiley15Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }


    [Ecopedia("Housing Objects", "Smiley", subPageName: "Repos-Face")]
    public partial class Smiley15Recipe : RecipeFamily
    {
        public Smiley15Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Repos-Face",
                displayName: Localizer.DoStr("Repos-Face"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley15Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2;

            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Repos-Face"), recipeType: typeof(Smiley15Recipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }




    // ______________________________________________________ Smiley16 ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Love-Strike")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley16Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley16Item);
        public override LocString DisplayName => Localizer.DoStr("Love-Strike");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Love-Strike")]
    [LocDescription("Le coup de foudre au premier regard, façon smiley.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(150)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley16Item : WorldObjectItem<Smiley16Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }


    [Ecopedia("Housing Objects", "Smiley", subPageName: "Love-Strike")]
    public partial class Smiley16Recipe : RecipeFamily
    {
        public Smiley16Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Love-Strike",
                displayName: Localizer.DoStr("Love-Strike"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley16Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2;

            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Love-Strike"), recipeType: typeof(Smiley16Recipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }





    // ______________________________________________________ Smiley17 ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Bisou Magique")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley17Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley17Item);
        public override LocString DisplayName => Localizer.DoStr("Bisou Magique");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Bisou Magique")]
    [LocDescription("Envoie un baiser avec un petit supplément de cœurs pour l’effet romantique.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(150)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley17Item : WorldObjectItem<Smiley17Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }


    [Ecopedia("Housing Objects", "Smiley", subPageName: "Bisou Magique")]
    public partial class Smiley17Recipe : RecipeFamily
    {
        public Smiley17Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Bisou Magique",
                displayName: Localizer.DoStr("Bisou Magique"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley17Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2;

            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Bisou Magique"), recipeType: typeof(Smiley17Recipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }





    // ______________________________________________________ Smiley18 ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Relaxé")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley18Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley18Item);
        public override LocString DisplayName => Localizer.DoStr("Relaxé");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Relaxé")]
    [LocDescription("Un visage détendu, pour les moments où vous vous sentez totalement à l'aise.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(150)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley18Item : WorldObjectItem<Smiley18Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }


    [Ecopedia("Housing Objects", "Smiley", subPageName: "Relaxé")]
    public partial class Smiley18Recipe : RecipeFamily
    {
        public Smiley18Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Relaxé",
                displayName: Localizer.DoStr("Relaxé"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley18Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2;

            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Relaxé"), recipeType: typeof(Smiley18Recipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }





    // ______________________________________________________ Smiley19 ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Monsieur Monocle")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley19Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley19Item);
        public override LocString DisplayName => Localizer.DoStr("Monsieur Monocle");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Monsieur Monocle")]
    [LocDescription("L’élégance à l’état pur, même quand t’as rien à dire d’intelligent.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(150)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley19Item : WorldObjectItem<Smiley19Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }


    [Ecopedia("Housing Objects", "Smiley", subPageName: "Monsieur Monocle")]
    public partial class Smiley19Recipe : RecipeFamily
    {
        public Smiley19Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Monsieur Monocle",
                displayName: Localizer.DoStr("Monsieur Monocle"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley19Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2;

            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Monsieur Monocle"), recipeType: typeof(Smiley19Recipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }





    // ______________________________________________________ Smiley20 ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Happy Lapin")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley20Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley20Item);
        public override LocString DisplayName => Localizer.DoStr("Happy Lapin");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Happy Lapin")]
    [LocDescription("Un sourire innocent avec un air de lapin geek... on craque !")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(150)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley20Item : WorldObjectItem<Smiley20Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }


    [Ecopedia("Housing Objects", "Smiley", subPageName: "Happy Lapin")]
    public partial class Smiley20Recipe : RecipeFamily
    {
        public Smiley20Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Happy Lapin",
                displayName: Localizer.DoStr("Happy Lapin"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley20Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2;

            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Happy Lapin"), recipeType: typeof(Smiley20Recipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }





    // ______________________________________________________ Smiley21 ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Chill-Glasses")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley21Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley21Item);
        public override LocString DisplayName => Localizer.DoStr("Chill-Glasses");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Chill-Glasses")]
    [LocDescription("Le sourire de celui qui est trop détendu pour se prendre la tête.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(150)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley21Item : WorldObjectItem<Smiley21Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }


    [Ecopedia("Housing Objects", "Smiley", subPageName: "Chill-Glasses")]
    public partial class Smiley21Recipe : RecipeFamily
    {
        public Smiley21Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Chill-Glasses",
                displayName: Localizer.DoStr("Chill-Glasses"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley21Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2;

            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Chill-Glasses"), recipeType: typeof(Smiley21Recipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }





    // ______________________________________________________ Smiley22 ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Starstruck")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley22Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley22Item);
        public override LocString DisplayName => Localizer.DoStr("Starstruck");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Starstruck")]
    [LocDescription("Les yeux en étoiles, c’est le coup de cœur assuré !")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(150)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley22Item : WorldObjectItem<Smiley22Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }


    [Ecopedia("Housing Objects", "Smiley", subPageName: "Starstruck")]
    public partial class Smiley22Recipe : RecipeFamily
    {
        public Smiley22Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Starstruck",
                displayName: Localizer.DoStr("Starstruck"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley22Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2;

            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Starstruck"), recipeType: typeof(Smiley22Recipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }





    // ______________________________________________________ Smiley23 ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Langue de Feu")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley23Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley23Item);
        public override LocString DisplayName => Localizer.DoStr("Langue de Feu");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Langue de Feu")]
    [LocDescription("Le smiley qui montre sa langue, parce que t’es un peu provoc' aujourd'hui.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(150)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley23Item : WorldObjectItem<Smiley23Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }


    [Ecopedia("Housing Objects", "Smiley", subPageName: "Langue de Feu")]
    public partial class Smiley23Recipe : RecipeFamily
    {
        public Smiley23Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Langue de Feu",
                displayName: Localizer.DoStr("Langue de Feu"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley23Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2;

            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Langue de Feu"), recipeType: typeof(Smiley23Recipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }





    // ______________________________________________________ Smiley24 ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Smiley", subPageName: "Lèche-Babines")]
    [Tag(nameof(SurfaceTags.HasTableSurface))]
    public partial class Smiley24Object : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Smiley24Item);
        public override LocString DisplayName => Localizer.DoStr("Lèche-Babines");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Lèche-Babines")]
    [LocDescription("Un sourire avec la langue dehors, pour les moments gourmands ou malicieux.")]
    [Ecopedia("Housing Objects", "Smiley", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(150)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Smiley24Item : WorldObjectItem<Smiley24Object>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }


    [Ecopedia("Housing Objects", "Smiley", subPageName: "Lèche-Babines")]
    public partial class Smiley24Recipe : RecipeFamily
    {
        public Smiley24Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Lèche-Babines",
                displayName: Localizer.DoStr("Lèche-Babines"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("WoodBoard", 5,typeof(Skill)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<Smiley24Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2;

            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Lèche-Babines"), recipeType: typeof(Smiley24Recipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(SmileyTableObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }



    // ______________________________________________________ SmileyTable ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(OnOffComponent))]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(MinimapComponent))]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(CraftingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [Tag("Usable")]
    [Ecopedia("Work Stations", "Craft Tables", subPageName: "Table à Sourires")]
    public partial class SmileyTableObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(SmileyTableItem);
        public override LocString DisplayName => Localizer.DoStr("Table à Sourires");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<MinimapComponent>().SetCategory(Localizer.DoStr("Crafting"));
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Table à Sourires")]
    [LocDescription("L'atelier ultime pour concocter des smileys avec plus de personnalité que ta playlist de mèmes préférée.")]
    [IconGroup("World Object Minimap")]
    [Ecopedia("Work Stations", "Craft Tables", createAsSubPage: true)]
    [Weight(1000)] 
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class SmileyTableItem : WorldObjectItem<SmileyTableObject>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }


    [Ecopedia("Work Stations", "Craft Tables", subPageName: "Table à Sourires")]
    public partial class SmileyTableRecipe : RecipeFamily
    {
        public SmileyTableRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Table à Sourires",  
                displayName: Localizer.DoStr("Table à Sourires"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("HewnLog", 15,typeof(Skill)), 
                    new IngredientElement(typeof(IronBarItem), 2, typeof(Skill)), 
                    new IngredientElement(typeof (NailItem), 20, typeof(Skill)), 
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<SmileyTableItem>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.LaborInCalories = CreateLaborInCaloriesValue(30);

            this.CraftMinutes = CreateCraftTimeValue(0.5f);

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Table à Sourires"), recipeType: typeof(SmileyTableRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(WorkbenchObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }

}

