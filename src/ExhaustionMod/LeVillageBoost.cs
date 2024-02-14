// Le Village
// Boisson énergisante de 2h

using Eco.Core.Items;
using Eco.Gameplay.Items;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using System.ComponentModel;
using Eco.Mods.TechTree;

namespace Village.Eco.Mods.ExhaustionMod
{
    [Serialized]
    [LocDisplayName("Le Village Boost")]
    [LocDescription("Redonne 2h d'énergie en cas d'épuisement. Il n'y a pas mieux que Le Village !")]  //Description détaillée.
    //[Weight(10000)]  //Défini le poids.
    [Category("Food")]
    [Tag("Boost")]
    //[Ecopedia("Food", "Boost", createAsSubPage: true)]  //Page ECOpedia - TODO ajouter icône tag
    public partial class LVBGreenItem : ExhaustionBoost
    {
        public override float BoostTime => 2f; //2h de boost
    }
}
