//Le Village - Gestion icône tag Boost

using Eco.Core.Items;
using Eco.Gameplay.Items;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using System.ComponentModel;

namespace Village.Eco.Mods.ExhaustionMod
{
    [Serialized]
    [Category("Hidden")]
    [Tag("Boost")]
    [LocDisplayName("Boost")]
    public partial class BoostItem : Item
    {
        public override LocString DisplayNamePlural => Localizer.DoStr("Boost");
    }
}
