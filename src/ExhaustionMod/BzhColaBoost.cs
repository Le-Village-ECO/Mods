// Le Village
// Boisson énergisante de 2h

using Eco.Gameplay.Items;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using Eco.Mods.TechTree;
using Eco.Core.Items;
using System.ComponentModel;

namespace Village.Eco.Mods.ExhaustionMod
{
    [Serialized]
    [LocDisplayName("BzhCola Boost")]
    [LocDescription("Redonne 2h d'énergie en cas d'épuisement. Vive la blanche hermine !")]  //Description détaillée.
    //[Weight(10000)]  //Défini le poids.
    [Category("Food")]
    //[Tag("Boost")] - TODO ajouter icône tag
    //[Ecopedia("Food", "Boost", createAsSubPage: true)]  //Page ECOpedia - TODO ajouter icône tag
    public partial class LVBPurpleItem : ExhaustionBoost 
    {
        public override float BoostTime => 2f; //2h de boost
    }
}
