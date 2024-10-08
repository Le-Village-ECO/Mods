using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eco.Gameplay.Components;
using Eco.Gameplay.Objects;
using Eco.Core.Controller;
using Eco.Shared.Networking;
using Eco.Shared.Serialization;
using Eco.Gameplay.Players;
using Eco.Gameplay.Interactions.Interactors;
using Eco.Shared.Items;
using Eco.Shared.SharedTypes;
using Eco.Core.Items;
using Eco.Shared.Localization;
using System.ComponentModel;

namespace Village.Eco.Mods.FacteurMod
{
    [Tag("Economy"), Category("Hidden"), NoIcon, Serialized, AutogenClass, LocDisplayName("Paramétrage magasin")]
    public class Component_UI : WorldObjectComponent
    {
        [RPC, Autogen]
        public void LoadProducts(Player player)
        {
            player.MsgLocStr("Test produits");
        }
        
        [RPC, Autogen]
        public void LoadComponents(Player player)
        {
            player.MsgLocStr("Test composants");
        }
        [SyncToView, Autogen, AutoRPC, Serialized] public int MarginPercentage { get; set; }

        [RPC, Autogen] public void CalculatePrice(Player player)
        {
            player.MsgLocStr("Test calcul prix");
        }

        public Component_UI()
        {
            this.MarginPercentage = 10;  //Default value
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        // Pour ajouter des interactions, pas indispensable
        //[Interaction(InteractionTrigger.RightClick, "Calculer les prix de vente", priority: -1, authRequired: AccessType.OwnerAccess)]
        //public void ProductsClick(Player player, InteractionTriggerInfo trigger, InteractionTarget target) => this.CalculatePrice(player);

        //[Interaction(InteractionTrigger.InteractKey, "Calculer les prix de vente", priority: -1, authRequired: AccessType.OwnerAccess)]
        //public void ComponentsClick(Player player, InteractionTriggerInfo trigger, InteractionTarget target) => this.CalculatePrice(player);

        //[Interaction(InteractionTrigger.InteractKey, "Calculer les prix de vente", priority: -1, authRequired: AccessType.OwnerAccess)]
        //public void CalculateClick(Player player, InteractionTriggerInfo trigger, InteractionTarget target) => this.CalculatePrice(player);
    }
}
