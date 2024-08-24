using Eco.Core.Items;
using Eco.Gameplay.Items;
using Eco.Gameplay.Players;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Village.Eco.Mods.Cooking
{
    [Serialized]
    [LocDisplayName("Bol en bois")]
    [LocDescription("Un joli bol en bois.")]
    [Ecopedia("Items", "Products", createAsSubPage: true)]
    [Weight(100)]
    public partial class WoodenBowlItem : Item
    {

    }
}
