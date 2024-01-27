// Le Village

using Eco.Gameplay.Items;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;

namespace Eco.Mods.TechTree
{
    [Serialized]
    [LocDisplayName("BzhCola Boost")]
    [LocDescription("Redonne 2h d'énergie en cas d'épuisement. Vive la blanche hermine !")]  //Description détaillée.
    public partial class LVBPurpleItem : ExhaustionBoost 
    {
        public override float BoostTime => 2f;
    }
}
