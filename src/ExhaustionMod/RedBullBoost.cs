// Le Village
// Boisson énergisante de 1h

using Eco.Gameplay.Items;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using Eco.Mods.TechTree;
using Eco.Core.Items;
using System.ComponentModel;

namespace Village.Eco.Mods.ExhaustionMod
{
    [Serialized]
    [LocDisplayName("RedBull Boost")]
    [LocDescription("Redonne 1h d'énergie en cas d'épuisement. Vous donne des ailes !")]  //Description détaillée.
    //[Weight(10000)]  //Défini le poids.
    [Category("Food")]
    //[Tag("Boost")] - TODO ajouter icône tag
    //[Ecopedia("Food", "Boost", createAsSubPage: true)]  //Page ECOpedia - TODO ajouter icône tag
    public partial class LVBRedItem : ExhaustionBoost
    {
        public override float BoostTime => 1.0f; //1h de boost
    }
}
