// Le Village - Component pour gérer le on/off, et donc l'animation Unity, de la boite aux lettres

using Eco.Core.Controller;
using Eco.Gameplay.Components;
using Eco.Gameplay.Components.Storage;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Players;
using Eco.Mods.TechTree;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;

namespace Village.Eco.Mods.FacteurMod
{
    [Serialized]
    [RequireComponent(typeof(StatusComponent))]
    [RequireComponent(typeof(PublicStorageComponent))]
    [NoIcon]
    public class BalComponent : WorldObjectComponent
    {
        //Pour gerer le status et informer le joueur
        private StatusElement status;
        private LocString FailedStatus => Localizer.DoStr("La boite est vide. Il n'y a pas de courrier.");
        private LocString SuccessStatus => Localizer.DoStr("Il y a du courrier !");

        //Pour gérer le statut (enabled/disabled) du WorldObjectComponent
        public override bool Enabled => this.hasLetter;
        private bool hasLetter = false; //false par défaut quand la boite est vide

        //Pour gérer le stockage
        private PublicStorageComponent storage;

        public override void Initialize()
        {
            // Initialise le WorldObjectComponent
            base.Initialize();

            //Gestion du status dans l'onglet du même nom
            status = Parent.GetComponent<StatusComponent>().CreateStatusElement(50);
            status.SetStatusMessage(false, FailedStatus);

            //Gestion du stockage - emplacements et restrictions
            storage = Parent.GetComponent<PublicStorageComponent>();
            storage.Initialize(20);
            storage.Inventory.AddInvRestriction(new SpecificItemTypesRestriction(new System.Type[] { typeof(LettreItem) }));
            storage.Inventory.OnChanged.Add(CheckStorage);
        }

        public void CheckStorage(User user)
        {
            status.SetStatusMessage(this.hasLetter = !storage.Inventory.IsEmpty, storage.Inventory.IsEmpty ? FailedStatus : SuccessStatus);
            this.Parent.UpdateEnabledAndOperating(); //Force update object status (obligatoire pour les components qui n'ont pas de tick()
        }
    }
}
