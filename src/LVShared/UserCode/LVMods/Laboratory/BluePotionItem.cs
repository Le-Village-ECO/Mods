using Eco.Core.Items;
using Eco.Gameplay.Items;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Village.Eco.Mods.Laboratory
{
    [Serialized]
    [LocDisplayName("Science")]
    [Weight(50)]  //Défini le poids.
    [MaxStackSize(100)]
    [Category("Research")]
    [Tag("Science")]
    [Ecopedia("Items", "Tools", createAsSubPage: true)]  //Page ECOpedia
    [LocDescription("Un objet qui représente une quantité de recherche scientifique")]  //Description détaillée.
    public partial class BluePotionItem : Item
    {

    }
}
