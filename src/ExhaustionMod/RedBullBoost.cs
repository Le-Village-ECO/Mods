// Le Village
// Boisson énergisante de 30min

using Eco.Core.Items;
using Eco.Gameplay.Items;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using System.ComponentModel;

namespace Village.Eco.Mods.ExhaustionMod
{
    [Serialized]
    [LocDisplayName("RedBull Boost")]
    [LocDescription("Redonne 30 min d'énergie en cas d'épuisement : Vous donne des ailes !")]  //Description détaillée.
    //[Weight(10000)]  //Défini le poids.
    [Category("Food")]
    [Tag("Boost")]
    [Ecopedia("Food", "Boost", createAsSubPage: true)]  //Page ECOpedia
    public partial class LVBRedItem : ExhaustionBoost
    {
        public override float BoostTime => 0.5f; //30min de boost
    }
}
