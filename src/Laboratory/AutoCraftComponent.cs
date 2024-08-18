// Le village - Auto Craft Component
// Component pour les WorldObject qui permet de fabriquer un objet en automatique toutes les X secondes et le stock directement et uniquement dans le WorldObject
// TODO - Afficher un % de progression de la fabrication dans le statut.

using Eco.Core.Utils;
using Eco.Gameplay.Components;
using Eco.Gameplay.Components.Storage;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Systems.Messaging.Notifications;
using Eco.Shared.IoC;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using Eco.Shared.Utils;
using Eco.Simulation.Settings;
using Eco.Simulation.Time;
using System;

namespace Village.Eco.Mods.OilField
{
    [Serialized]
    [Priority(-2)]
    [RequireComponent(typeof(PublicStorageComponent))]
    [RequireComponent(typeof(StatusComponent))]
    [RequireComponent(typeof(MustBeOwnedComponent))]
    public class AutoCraftComponent : WorldObjectComponent
    {
        // craft time tracking
        private double passiveCraftTime;
        private double timeSinceLastCraft = 0;

        //pour gerer le status enabled/disabled et informer le joueur
        private StatusElement status;

        //article fabriqué
        private Type itemType;

        //Initialise le component
        public void Initialize(double passiveCraftTime, int slots, Type itemType)
        {
            // Initialise le WorldObjectComponent
            base.Initialize();
            //Récupère les paramètres passés lors de l'initialisation
            this.passiveCraftTime = passiveCraftTime;
            this.itemType = itemType;
            // Gestion du statut de l'objet
            status = Parent.GetComponent<StatusComponent>().CreateStatusElement();
            // Initialisation du stockage
            Parent.GetComponent<PublicStorageComponent>().Initialize(slots);
        }

        LocString DisplayStatus => Localizer.Do($"Auto Crafter est en fonctionnement a raison de 1 {itemType.Name} toutes les {passiveCraftTime} secondes.");

        public override void Tick() => Tick(ServiceHolder<IWorldObjectManager>.Obj.TickDeltaTime * EcoDef.Obj.TimeMult);
        public void Tick(float deltaTime)
        {
            if (timeSinceLastCraft == 0) timeSinceLastCraft = WorldTime.Seconds; //Initialisation du tracker

            if (Parent.Enabled)
            {
                var inv = Parent.GetComponent<PublicStorageComponent>().Inventory;
                //var deltaTime = WorldTime.Seconds - timeSinceLastCraft;

                if (deltaTime >= passiveCraftTime)
                {
                    NotificationManager.ServerMessageToAllLoc($"Tick = {WorldTime.Seconds} / Last = {timeSinceLastCraft} / Delta = {deltaTime}");

                    //Tente d'ajouter l'article dans le stockage
                    //if (!inv.TryAddItem<CompostItem>())
                    if (!inv.TryAddItem(itemType))
                    {
                        Parent.GetComponent<OnOffComponent>().On = false; //desactivation de l'objet
                        status.SetStatusMessage(false, Localizer.DoStr("Il n'y a plus de place !")); //Mise a jour du statut
                        NotificationManager.ServerMessageToAllLoc($"C'est plein !"); //log
                    }
                    else
                    {
                        timeSinceLastCraft = WorldTime.Seconds;  //Copie du temps pour le prochain tick
                        status.SetStatusMessage(true, DisplayStatus); //Mise a jour du statut
                    }

                }
                else NotificationManager.ServerMessageToAllLoc($"Delta = {deltaTime}"); //log
            }
        }
    }
}
