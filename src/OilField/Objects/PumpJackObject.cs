// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.
namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Players;
    using Eco.Shared.Localization;
    using Eco.Shared.Math;
    using Eco.Shared.Utils;
    using Eco.Simulation.WorldLayers;
    using Eco.Gameplay.Systems.NewTooltip;
    using Eco.Shared.Items;

    public partial class PumpJackObject : WorldObject
    {
        public static float Radius { get { return 3.0f; } }

        public void OnCraftingComplete()
        {
            var newSpeed = 0.0f;
            WorldLayerManager.Obj.GetLayer(LayerNames.Oilfield).ApplyRadius(this.Position.XZi(), Radius, (x, val) =>
            {
                var newVal = val - ((1 - (Vector2.Distance(x, this.Position.XZ()) / Radius)) * 0.05f);
                newSpeed += newVal;
                return newVal;
            });
        }
    }

    public partial class PumpJackItem : WorldObjectItem<PumpJackObject>
    {
        //Cache disabled because tooltip depends on user position, but we need it on server because the tooltip needs WorldLayerManager.
        [NewTooltip(CacheAs.Disabled, 120)]
        public static LocString OilTooltip(User user)
        {
            var player = user.Player;
            var layer = WorldLayerManager.Obj.GetLayer(LayerNames.Oilfield);
            var pos = player.User.Position.XZi();
            float value = 0.0f;
            layer.ForRadius(layer.WorldPosToLayerPos(pos), PumpJackObject.Radius, (x, val) => value += val);
            return new TooltipSection(Localizer.DoStr("Oil Amount"), new LocString(Text.Num(value)));
        }
    }
}
