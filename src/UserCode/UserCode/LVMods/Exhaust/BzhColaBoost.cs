// Le Village
// Boisson énergisante de 1h

using Eco.Core.Items;
using Eco.Gameplay.Items;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using System.ComponentModel;

namespace Village.Eco.Mods.ExhaustionMod
{
    [Serialized]
    [LocDisplayName("BzhCola Boost")]
    [LocDescription("Redonne 1h d'énergie en cas d'épuisement : Vive la blanche hermine !")]  //Description détaillée.
    //[Weight(10000)]  //Défini le poids.
    [Category("Food")]
    [Tag("Boost")]
    [Ecopedia("Food", "Boost", createAsSubPage: true)]  //Page ECOpedia
    public partial class LVBPurpleItem : ExhaustionBoost 
    {
        public override float BoostTime => 1f; //1h de boost
    }
}
