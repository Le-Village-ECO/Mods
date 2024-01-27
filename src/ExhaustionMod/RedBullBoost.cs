// Le Village

using Eco.Gameplay.Items;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;

namespace Eco.Mods.TechTree
{
    [Serialized]
    [LocDisplayName("RedBull Boost")]
    [LocDescription("Redonne 1h d'énergie en cas d'épuisement. Vous donne des ailes !")]  //Description détaillée.
    public partial class LVBRedItem : ExhaustionBoost
    {
        public override float BoostTime => 1.0f;
    }
}
