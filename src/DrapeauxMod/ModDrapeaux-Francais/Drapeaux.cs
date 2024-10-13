// MOD créé par Plex : Modèle 3D et Code.
// Dernière mise à jour du mod : 10/10/24

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






    // ______________________________________________________ Drapeau ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau")]
    public partial class DrapeauObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(DrapeauItem);
        public override LocString DisplayName => Localizer.DoStr("Drapeau");
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
    [LocDisplayName("Drapeau")]
    [LocDescription("Parfait pour décorer votre maison ou signaler aux voisins que c'est vous qui avez fait les meilleurs croissants du quartier.")]
    [Ecopedia("Housing Objects", "Drapeau", createAsSubPage: true)]
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
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau")]
    public partial class DrapeauRecipe : RecipeFamily
    {
        public DrapeauRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Drapeau",
                displayName: Localizer.DoStr("Drapeau"),

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



    // ______________________________________________________ Drapeau_Francais ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Francais")]
    public partial class Drapeau_FrancaisObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_FrancaisItem);
        public override LocString DisplayName => Localizer.DoStr("Drapeau_Francais");
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
    [LocDisplayName("Drapeau_Francais")]
    [LocDescription("Parfait pour décorer votre maison ou signaler aux voisins que c'est vous qui avez fait les meilleurs croissants du quartier.")]
    [Ecopedia("Housing Objects", "Drapeau", createAsSubPage: true)]
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
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Francais")]
    public partial class Drapeau_FrancaisRecipe : Recipe
    {
        public Drapeau_FrancaisRecipe()
        {
            this.Init(
                name: "Drapeau_Francais",
                displayName: Localizer.DoStr("Drapeau_Francais"),


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




    // ______________________________________________________ Drapeau_Allemand ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Allemand")]
    public partial class Drapeau_AllemandObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_AllemandItem);
        public override LocString DisplayName => Localizer.DoStr("Drapeau_Allemand");
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
    [LocDisplayName("Drapeau_Allemand")]
    [LocDescription("Ce drapeau allemand sur mât est là pour vous ! Garantie : il se plante droit comme une bière bien servie.")]
    [Ecopedia("Housing Objects", "Drapeau", createAsSubPage: true)]
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
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Allemand")]
    public partial class Drapeau_AllemandRecipe : Recipe
    {
        public Drapeau_AllemandRecipe()
        {
            this.Init(
                name: "Drapeau_Allemand",
                displayName: Localizer.DoStr("Drapeau_Allemand"),


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



    // ______________________________________________________ Drapeau_Quebec ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Quebec")]
    public partial class Drapeau_QuebecObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_QuebecItem);
        public override LocString DisplayName => Localizer.DoStr("Drapeau_Quebec");
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
    [LocDisplayName("Drapeau_Quebec")]
    [LocDescription("Drapeau du Québec sur mât. Parce qu'il n'y a rien de mieux qu'un brin de 'tabarnak' dans votre ville !")]
    [Ecopedia("Housing Objects", "Drapeau", createAsSubPage: true)]
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
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Quebec")]
    public partial class Drapeau_QuebecRecipe : Recipe
    {
        public Drapeau_QuebecRecipe()
        {
            this.Init(
                name: "Drapeau_Quebec",
                displayName: Localizer.DoStr("Drapeau_Quebec"),


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





    // ______________________________________________________ Drapeau_Angleterre ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Angleterre")]
    public partial class Drapeau_AngleterreObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_AngleterreItem);
        public override LocString DisplayName => Localizer.DoStr("Drapeau_Angleterre");
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
    [LocDisplayName("Drapeau_Angleterre")]
    [LocDescription("Drapeau anglais sur mât. Parfait pour une déco soignée, avec une tasse de thé et un 'God Save the Style' ")]
    [Ecopedia("Housing Objects", "Drapeau", createAsSubPage: true)]
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
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Angleterre")]
    public partial class Drapeau_AngleterreRecipe : Recipe
    {
        public Drapeau_AngleterreRecipe()
        {
            this.Init(
                name: "Drapeau_Angleterre",
                displayName: Localizer.DoStr("Drapeau_Angleterre"),


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





    // ______________________________________________________ Drapeau_Espagne ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Espagne")]
    public partial class Drapeau_EspagneObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_EspagneItem);
        public override LocString DisplayName => Localizer.DoStr("Drapeau_Espagne");
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
    [LocDisplayName("Drapeau_Espagne")]
    [LocDescription("Drapeau espagnol sur mât. Parfait pour une ambiance caliente... même sans les castagnettes !")]
    [Ecopedia("Housing Objects", "Drapeau", createAsSubPage: true)]
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
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Espagne")]
    public partial class Drapeau_EspagneRecipe : Recipe
    {
        public Drapeau_EspagneRecipe()
        {
            this.Init(
                name: "Drapeau_Espagne",
                displayName: Localizer.DoStr("Drapeau_Espagne"),


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




    // ______________________________________________________ Drapeau_Italie ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Italie")]
    public partial class Drapeau_ItalieObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_ItalieItem);
        public override LocString DisplayName => Localizer.DoStr("Drapeau_Italie");
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
    [LocDisplayName("Drapeau_Italie")]
    [LocDescription("Drapeau italien sur mât. Parfait pour dire 'Mamma mia' à votre style... sans même toucher à la pizza !")]
    [Ecopedia("Housing Objects", "Drapeau", createAsSubPage: true)]
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
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Italie")]
    public partial class Drapeau_ItalieRecipe : Recipe
    {
        public Drapeau_ItalieRecipe()
        {
            this.Init(
                name: "Drapeau_Italie",
                displayName: Localizer.DoStr("Drapeau_Italie"),


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







    // ______________________________________________________ Drapeau_Suisse ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Suisse")]
    public partial class Drapeau_SuisseObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_SuisseItem);
        public override LocString DisplayName => Localizer.DoStr("Drapeau_Suisse");
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
    [LocDisplayName("Drapeau_Suisse")]
    [LocDescription("Drapeau Suisse sur mât. Parfait pour une ambiance neutre... mais toujours classe, comme une montre bien réglée !")]
    [Ecopedia("Housing Objects", "Drapeau", createAsSubPage: true)]
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
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Suisse")]
    public partial class Drapeau_SuisseRecipe : Recipe
    {
        public Drapeau_SuisseRecipe()
        {
            this.Init(
                name: "Drapeau_Suisse",
                displayName: Localizer.DoStr("Drapeau_Suisse"),


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





    // ______________________________________________________ Drapeau_Belgique ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Belgique")]
    public partial class Drapeau_BelgiqueObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_BelgiqueItem);
        public override LocString DisplayName => Localizer.DoStr("Drapeau_Belgique");
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
    [LocDisplayName("Drapeau_Belgique")]
    [LocDescription("Drapeau belge sur mât. Parfait pour une ambiance qui frôle la perfection... comme une gaufre bien croustillante !")]
    [Ecopedia("Housing Objects", "Drapeau", createAsSubPage: true)]
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
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Belgique")]
    public partial class Drapeau_BelgiqueRecipe : Recipe
    {
        public Drapeau_BelgiqueRecipe()
        {
            this.Init(
                name: "Drapeau_Belgique",
                displayName: Localizer.DoStr("Drapeau_Belgique"),


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





    // ______________________________________________________ Drapeau_Canada ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Canada")]
    public partial class Drapeau_CanadaObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_CanadaItem);
        public override LocString DisplayName => Localizer.DoStr("Drapeau_Canada");
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
    [LocDisplayName("Drapeau_Canada")]
    [LocDescription("Drapeau du Canada sur mât. Parfait pour une ambiance chill, avec un soupçon de sirop d'érable dans l'air !")]
    [Ecopedia("Housing Objects", "Drapeau", createAsSubPage: true)]
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
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Canada")]
    public partial class Drapeau_CanadaRecipe : Recipe
    {
        public Drapeau_CanadaRecipe()
        {
            this.Init(
                name: "Drapeau_Canada",
                displayName: Localizer.DoStr("Drapeau_Canada"),


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






    // ______________________________________________________ Drapeau_Maroc ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Maroc")]
    public partial class Drapeau_MarocObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_MarocItem);
        public override LocString DisplayName => Localizer.DoStr("Drapeau_Maroc");
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
    [LocDisplayName("Drapeau_Maroc")]
    [LocDescription("Drapeau du Maroc sur mât. Parfait pour une ambiance chaleureuse, sans même avoir besoin d'un thé à la menthe !")]
    [Ecopedia("Housing Objects", "Drapeau", createAsSubPage: true)]
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
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Maroc")]
    public partial class Drapeau_MarocRecipe : Recipe
    {
        public Drapeau_MarocRecipe()
        {
            this.Init(
                name: "Drapeau_Maroc",
                displayName: Localizer.DoStr("Drapeau_Maroc"),


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





    // ______________________________________________________ Drapeau_Danemark ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Danemark")]
    public partial class Drapeau_DanemarkObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_DanemarkItem);
        public override LocString DisplayName => Localizer.DoStr("Drapeau_Danemark");
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
    [LocDisplayName("Drapeau_Danemark")]
    [LocDescription("Drapeau du Danemark sur mât. Parfait pour une ambiance cosy... presque aussi hygge qu'une soirée au coin du feu !")]
    [Ecopedia("Housing Objects", "Drapeau", createAsSubPage: true)]
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
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Danemark")]
    public partial class Drapeau_DanemarkRecipe : Recipe
    {
        public Drapeau_DanemarkRecipe()
        {
            this.Init(
                name: "Drapeau_Danemark",
                displayName: Localizer.DoStr("Drapeau_Danemark"),


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





    // ______________________________________________________ Drapeau_Turquie ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Turquie")]
    public partial class Drapeau_TurquieObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_TurquieItem);
        public override LocString DisplayName => Localizer.DoStr("Drapeau_Turquie");
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
    [LocDisplayName("Drapeau_Turquie")]
    [LocDescription("Drapeau de la Turquie sur mât. Parfait pour une ambiance qui brille... comme un baklava bien doré !")]
    [Ecopedia("Housing Objects", "Drapeau", createAsSubPage: true)]
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
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Turquie")]
    public partial class Drapeau_TurquieRecipe : Recipe
    {
        public Drapeau_TurquieRecipe()
        {
            this.Init(
                name: "Drapeau_Turquie",
                displayName: Localizer.DoStr("Drapeau_Turquie"),


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




    // ______________________________________________________ Drapeau_Algerie ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Algerie")]
    public partial class Drapeau_AlgerieObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_AlgerieItem);
        public override LocString DisplayName => Localizer.DoStr("Drapeau_Algerie");
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
    [LocDisplayName("Drapeau_Algerie")]
    [LocDescription("Drapeau de l'Algerie sur mât. Parfait pour une ambiance vivante... sans avoir à danser le raï dans votre salon !")]
    [Ecopedia("Housing Objects", "Drapeau", createAsSubPage: true)]
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
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Algerie")]
    public partial class Drapeau_AlgerieRecipe : Recipe
    {
        public Drapeau_AlgerieRecipe()
        {
            this.Init(
                name: "Drapeau_Algerie",
                displayName: Localizer.DoStr("Drapeau_Algerie"),


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





    // ______________________________________________________ Drapeau_Autriche ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Autriche")]
    public partial class Drapeau_AutricheObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_AutricheItem);
        public override LocString DisplayName => Localizer.DoStr("Drapeau_Autriche");
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
    [LocDisplayName("Drapeau_Autriche")]
    [LocDescription("Drapeau de l'Autriche sur mât. Parfait pour une ambiance qui fait yodeler de joie... sans même monter sur une montagne !")]
    [Ecopedia("Housing Objects", "Drapeau", createAsSubPage: true)]
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
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Autriche")]
    public partial class Drapeau_AutricheRecipe : Recipe
    {
        public Drapeau_AutricheRecipe()
        {
            this.Init(
                name: "Drapeau_Autriche",
                displayName: Localizer.DoStr("Drapeau_Autriche"),


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






    // ______________________________________________________ Drapeau_Finlande ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Finlande")]
    public partial class Drapeau_FinlandeObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_FinlandeItem);
        public override LocString DisplayName => Localizer.DoStr("Drapeau_Finlande");
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
    [LocDisplayName("Drapeau_Finlande")]
    [LocDescription("Drapeau de la Finlande sur mât. Parfait pour une ambiance tranquille, avec un soupçon de sauna et des aurores boréales !")]
    [Ecopedia("Housing Objects", "Drapeau", createAsSubPage: true)]
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
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Finlande")]
    public partial class Drapeau_FinlandeRecipe : Recipe
    {
        public Drapeau_FinlandeRecipe()
        {
            this.Init(
                name: "Drapeau_Finlande",
                displayName: Localizer.DoStr("Drapeau_Finlande"),


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





    // ______________________________________________________ Drapeau_Irlande ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Irlande")]
    public partial class Drapeau_IrlandeObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_IrlandeItem);
        public override LocString DisplayName => Localizer.DoStr("Drapeau_Irlande");
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
    [LocDisplayName("Drapeau_Irlande")]
    [LocDescription("Drapeau de l'Irlande sur mât. Parfait pour une ambiance joyeuse, sans même avoir à chercher le pot d'or au bout de l'arc-en-ciel !")]
    [Ecopedia("Housing Objects", "Drapeau", createAsSubPage: true)]
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
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Irlande")]
    public partial class Drapeau_IrlandeRecipe : Recipe
    {
        public Drapeau_IrlandeRecipe()
        {
            this.Init(
                name: "Drapeau_Irlande",
                displayName: Localizer.DoStr("Drapeau_Irlande"),


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





    // ______________________________________________________ Drapeau_Pologne ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Pologne")]
    public partial class Drapeau_PologneObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_PologneItem);
        public override LocString DisplayName => Localizer.DoStr("Drapeau_Pologne");
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
    [LocDisplayName("Drapeau_Pologne")]
    [LocDescription("Drapeau de la Pologne sur mât. Parfait pour une ambiance élégante, sans même devoir faire un tour dans les montagnes !")]
    [Ecopedia("Housing Objects", "Drapeau", createAsSubPage: true)]
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
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Pologne")]
    public partial class Drapeau_PologneRecipe : Recipe
    {
        public Drapeau_PologneRecipe()
        {
            this.Init(
                name: "Drapeau_Pologne",
                displayName: Localizer.DoStr("Drapeau_Pologne"),


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





    // ______________________________________________________ Drapeau_Russie ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Russie")]
    public partial class Drapeau_RussieObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_RussieItem);
        public override LocString DisplayName => Localizer.DoStr("Drapeau_Russie");
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
    [LocDisplayName("Drapeau_Russie")]
    [LocDescription("Drapeau de la Russie sur mât. Parfait pour une ambiance majestueuse, sans avoir à traverser la Sibérie !")]
    [Ecopedia("Housing Objects", "Drapeau", createAsSubPage: true)]
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
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Russie")]
    public partial class Drapeau_RussieRecipe : Recipe
    {
        public Drapeau_RussieRecipe()
        {
            this.Init(
                name: "Drapeau_Russie",
                displayName: Localizer.DoStr("Drapeau_Russie"),


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






    // ______________________________________________________ Drapeau_Amerique ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Amerique")]
    public partial class Drapeau_AmeriqueObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_AmeriqueItem);
        public override LocString DisplayName => Localizer.DoStr("Drapeau_Amerique");
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
    [LocDisplayName("Drapeau_Amerique")]
    [LocDescription("Drapeau Américain sur mât. Parfait pour une ambiance étoilée, sans avoir à faire le tour des 50 états !")]
    [Ecopedia("Housing Objects", "Drapeau", createAsSubPage: true)]
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
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Amerique")]
    public partial class Drapeau_AmeriqueRecipe : Recipe
    {
        public Drapeau_AmeriqueRecipe()
        {
            this.Init(
                name: "Drapeau_Amerique",
                displayName: Localizer.DoStr("Drapeau_Amerique"),


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







    // ______________________________________________________ Drapeau_Tunisie ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Tunisie")]
    public partial class Drapeau_TunisieObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_TunisieItem);
        public override LocString DisplayName => Localizer.DoStr("Drapeau_Tunisie");
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
    [LocDisplayName("Drapeau_Tunisie")]
    [LocDescription("Drapeau de la Tunisie sur mât. Parfait pour une ambiance ensoleillée, sans même avoir besoin de sable !")]
    [Ecopedia("Housing Objects", "Drapeau", createAsSubPage: true)]
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
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_Tunisie")]
    public partial class Drapeau_TunisieRecipe : Recipe
    {
        public Drapeau_TunisieRecipe()
        {
            this.Init(
                name: "Drapeau_Tunisie",
                displayName: Localizer.DoStr("Drapeau_Tunisie"),


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
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_LesFranquois")]
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
    [Ecopedia("Housing Objects", "Drapeau", createAsSubPage: true)]
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
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_LesFranquois")]
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
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_LeVillage")]
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
    [Ecopedia("Housing Objects", "Drapeau", createAsSubPage: true)]
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
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau_LeVillage")]
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



    // ______________________________________________________ Drapeau_Australie ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau Australie")]
    public partial class Drapeau_AustralieObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_AustralieItem);
        public override LocString DisplayName => Localizer.DoStr("Drapeau Australie");
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
    [LocDisplayName("Drapeau Australie")]
    [LocDescription("Apportez un souffle australien à votre déco avec ce drapeau sur mât. Parfait pour une ambiance Down Under... sans avoir à croiser de kangourous !")]
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
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau Australie")]
    public partial class Drapeau_AustralieRecipe : Recipe
    {
        public Drapeau_AustralieRecipe()
        {
            this.Init(
                name: "Drapeau Australie",
                displayName: Localizer.DoStr("Drapeau Australie"),


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




    // ______________________________________________________ Drapeau_Norvege ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau Norvège")]
    public partial class Drapeau_NorvegeObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_NorvegeItem);
        public override LocString DisplayName => Localizer.DoStr("Drapeau Norvège");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<HousingComponent>().HomeValue = Drapeau_NorvegeItem.homeValue;
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Drapeau Norvège")]
    [LocDescription("Ajoutez une touche viking à votre déco avec ce drapeau norvégien sur mât. Parfait pour une ambiance nordique... sans avoir à sortir le drakkar !")]
    [Ecopedia("Housing Objects", "Drapeau", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(5000)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Drapeau_NorvegeItem : WorldObjectItem<Drapeau_NorvegeObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName = typeof(Drapeau_NorvegeObject).UILink(),
            Category = HousingConfig.GetRoomCategory("Outdoor"),
            BaseValue = 6,
            TypeForRoomLimit = Localizer.DoStr("Statue"),
            DiminishingReturnMultiplier = 0.3f

        };

    }

    [RequiresSkill(typeof(TailoringSkill), 1)]
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau Norvège")]
    public partial class Drapeau_NorvegeRecipe : Recipe
    {
        public Drapeau_NorvegeRecipe()
        {
            this.Init(
                name: "Drapeau Norvège",
                displayName: Localizer.DoStr("Drapeau Norvège"),


                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronBarItem), 3, typeof(TailoringSkill)),
                    new IngredientElement(typeof(LinenFabricItem), 10, typeof(TailoringSkill)),
                },


                items: new List<CraftingElement>
                {
                    new CraftingElement<Drapeau_NorvegeItem>()
                });

            this.ModsPostInitialize();
            CraftingComponent.AddTagProduct(typeof(TailoringTableObject), typeof(DrapeauRecipe), this);
        }

        partial void ModsPostInitialize();
    }




    // ______________________________________________________ Drapeau_Suede ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau Suède")]
    public partial class Drapeau_SuedeObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_SuedeItem);
        public override LocString DisplayName => Localizer.DoStr("Drapeau Suède");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<HousingComponent>().HomeValue = Drapeau_SuedeItem.homeValue;
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Drapeau Suède")]
    [LocDescription("Apportez un brin de fraîcheur suédoise à votre déco avec ce drapeau sur mât. Parfait pour une ambiance scandinave... sans avoir à monter un meuble en kit !")]
    [Ecopedia("Housing Objects", "Drapeau", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(5000)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Drapeau_SuedeItem : WorldObjectItem<Drapeau_SuedeObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName = typeof(Drapeau_SuedeObject).UILink(),
            Category = HousingConfig.GetRoomCategory("Outdoor"),
            BaseValue = 6,
            TypeForRoomLimit = Localizer.DoStr("Statue"),
            DiminishingReturnMultiplier = 0.3f

        };

    }

    [RequiresSkill(typeof(TailoringSkill), 1)]
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau Suède")]
    public partial class Drapeau_SuedeRecipe : Recipe
    {
        public Drapeau_SuedeRecipe()
        {
            this.Init(
                name: "Drapeau Suède",
                displayName: Localizer.DoStr("Drapeau Suède"),


                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronBarItem), 3, typeof(TailoringSkill)),
                    new IngredientElement(typeof(LinenFabricItem), 10, typeof(TailoringSkill)),
                },


                items: new List<CraftingElement>
                {
                    new CraftingElement<Drapeau_SuedeItem>()
                });

            this.ModsPostInitialize();
            CraftingComponent.AddTagProduct(typeof(TailoringTableObject), typeof(DrapeauRecipe), this);
        }

        partial void ModsPostInitialize();
    }



    // ______________________________________________________ Drapeau_Estonie ______________________________________________________ \\

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(PaintableComponent))]
    [RequireRoomVolume(30)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau Estonie")]
    public partial class Drapeau_EstonieObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(Drapeau_EstonieItem);
        public override LocString DisplayName => Localizer.DoStr("Drapeau Estonie");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;


        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<HousingComponent>().HomeValue = Drapeau_EstonieItem.homeValue;
            this.ModsPostInitialize();
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Drapeau Estonie")]
    [LocDescription("Ajoutez une touche baltique à votre déco avec ce drapeau estonien sur mât. Parfait pour une ambiance high-tech... avec un soupçon de nature sauvage !")]
    [Ecopedia("Housing Objects", "Drapeau", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(5000)]
    [Tag(nameof(SurfaceTags.CanBeOnRug))]
    public partial class Drapeau_EstonieItem : WorldObjectItem<Drapeau_EstonieObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName = typeof(Drapeau_EstonieObject).UILink(),
            Category = HousingConfig.GetRoomCategory("Outdoor"),
            BaseValue = 6,
            TypeForRoomLimit = Localizer.DoStr("Statue"),
            DiminishingReturnMultiplier = 0.3f

        };

    }

    [RequiresSkill(typeof(TailoringSkill), 1)]
    [Ecopedia("Housing Objects", "Drapeau", subPageName: "Drapeau Estonie")]
    public partial class Drapeau_EstonieRecipe : Recipe
    {
        public Drapeau_EstonieRecipe()
        {
            this.Init(
                name: "Drapeau Estonie",
                displayName: Localizer.DoStr("Drapeau Estonie"),


                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronBarItem), 3, typeof(TailoringSkill)),
                    new IngredientElement(typeof(LinenFabricItem), 10, typeof(TailoringSkill)),
                },


                items: new List<CraftingElement>
                {
                    new CraftingElement<Drapeau_EstonieItem>()
                });

            this.ModsPostInitialize();
            CraftingComponent.AddTagProduct(typeof(TailoringTableObject), typeof(DrapeauRecipe), this);
        }

        partial void ModsPostInitialize();
    }

}