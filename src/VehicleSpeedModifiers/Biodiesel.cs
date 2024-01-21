using Eco.Gameplay.Items;
using Eco.Gameplay.Systems.NewTooltip;
using Eco.Gameplay.Systems.TextLinks;
using Eco.Shared.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eco.Mods.TechTree
{
    public partial class BiodieselItem
    {
        public float VehicleSpeedMultiplier { get; } = 1.25f;

        [NewTooltip(Shared.Items.CacheAs.Global, 1)]
        public LocString FuelBenefit => Localizer.DoStr($"Speeds up {Item.Get<TruckItem>().UILinkPlural()} by {(VehicleSpeedMultiplier - 1):P0}. Fuel consumption is increased by the same amount");
    }
}
