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

namespace Village.Eco.Mods.Laboratory
{
    [Serialized]
    [RequireComponent(typeof(OnOffComponent))]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(MinimapComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(ResearchComponent))]
    [RequireRoomContainment]
    [RequireRoomVolume(24)]
    [RequireRoomMaterialTier(0.8f)]
    [Tag("Usable")]
    [Ecopedia("Work Stations", "Researching", subPageName: "Advanced Research Table Item")]
    public partial class AdvancedResearchTableObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(AdvancedResearchTableItem);
        public override LocString DisplayName => Localizer.DoStr("Advanced Research Table");
        public override TableTextureMode TableTexture => TableTextureMode.Metal;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<MinimapComponent>().SetCategory(Localizer.DoStr("Research"));
            this.GetComponent<HousingComponent>().HomeValue = AdvancedResearchTableItem.homeValue;
            this.GetComponent<ResearchComponent>().Initialize(5, 2, typeof(ScienceItem));
            this.ModsPostInitialize();
        }

        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Advanced Research Table")]
    [LocDescription("An advanced table for researching new technologies and skills.")]
    [IconGroup("World Object Minimap")]
    [Ecopedia("Work Stations", "Researching", createAsSubPage: true)]
    [Tag("Housing")]
    [Weight(2000)] // Defines how heavy AdvancedResearchTable is.
    public partial class AdvancedResearchTableItem : WorldObjectItem<AdvancedResearchTableObject>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new()
        {
            ObjectName = typeof(AdvancedResearchTableObject).UILink(),
            //UniqueObjectName = Localizer.DoStr("Laboratoire"),  //En cours de test...
            //DiminishingMultiplierAcrossFullProperty = 0f,  //En cours de test...
            Category = HousingConfig.GetRoomCategory("Research Centre"),
            BaseValue = 1,
            //TypeForRoomLimit = Localizer.DoStr("Research Table"),
            //DiminishingReturnMultiplier = 0f
        };

        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }
}
