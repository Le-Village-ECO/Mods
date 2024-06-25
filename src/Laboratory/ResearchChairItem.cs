using Eco.Core.Controller;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Occupancy;
using Eco.Gameplay.Systems.NewTooltip;
using Eco.Shared.Items;
using Eco.Shared.Localization;
using Eco.Shared.Math;
using Eco.Shared.Serialization;
using System;

namespace Village.Eco.Mods.Laboratory
{
    [Serialized]
    public partial class ResearchChairObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(ResearchChairItem);
        public override LocString DisplayName => Localizer.DoStr("Research Chair Object");
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
    [LocDisplayName("Research Chair Object")]
    public partial class ResearchChairItem : WorldObjectItem<ResearchChairObject>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }
}