namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Components.Storage;
    using Eco.Gameplay.Objects;
    using Eco.Shared.Math;
    using Eco.Shared.SharedTypes;

    [RequireComponent(typeof(PublicStorageComponent))]
    [RequireComponent(typeof(StockpileComponent))]
    [RequireComponent(typeof(WorldStockpileComponent))]
    public partial class PaletteObject : WorldObject
    {
        public static readonly Vector3i DefaultDim = new Vector3i(2, 5, 2);
        public override InteractionTargetPriority TargetPriority => InteractionTargetPriority.Medium;

        protected override void OnCreatePostInitialize()
        {
            base.OnCreatePostInitialize();
            StockpileComponent.ClearPlacementArea(this.Creator, this.Position3i, DefaultDim, this.Rotation);
        }

        protected override void PostInitialize()
        {
            base.PostInitialize();

            this.GetComponent<StockpileComponent>().Initialize(DefaultDim);

            var storage = this.GetComponent<PublicStorageComponent>();
            storage.Initialize(DefaultDim.x * DefaultDim.z);
            storage.Storage.AddInvRestriction(new StockpileStackRestriction(DefaultDim.y)); // limit stack sizes to the y-height of the LumberStockpile

            this.GetComponent<LinkComponent>().Initialize(12);
        }
    }
}