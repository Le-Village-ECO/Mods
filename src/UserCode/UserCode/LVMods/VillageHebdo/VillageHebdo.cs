using Eco.Core.Controller;
using Eco.Core.Items;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Occupancy;
using Eco.Gameplay.Systems.NewTooltip;
using Eco.Shared.Items;
using Eco.Shared.Localization;
using Eco.Shared.Math;
using Eco.Shared.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LVShared.UserCode.LVMods.LVHebdo
{
    [Serialized]
    [LocDisplayName("Le Village Hebdo")]
    [LocDescription("Les nouvelles fraîches du Village.")]
    [Ecopedia("Items", "Products", createAsSubPage: true)]
    [Weight(50)]
    public partial class VillageHebdoItem : Item
    {

    }
}
