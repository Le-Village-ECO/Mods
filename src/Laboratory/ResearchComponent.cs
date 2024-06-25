// Le Village - Researh Component
// Ce composant génère de la recherche utilisable ensuite pour découvrir les spécialités
// 

using Eco.Core.Utils;
using Eco.Gameplay.Components;
using Eco.Gameplay.Components.Storage;
using Eco.Gameplay.Housing;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Players;
using Eco.Gameplay.Property;
using Eco.Gameplay.Rooms;
using Eco.Shared.IoC;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using Eco.Shared.Utils;
using Eco.Simulation.Settings;
using System;
using System.Linq;

namespace Village.Eco.Mods.Laboratory
{
    [Serialized]
    [RequireComponent(typeof(StatusComponent))]
    [RequireComponent(typeof(PublicStorageComponent))]
    [RequireComponent(typeof(MustBeOwnedComponent))]
    public class ResearchComponent : WorldObjectComponent
    {
        // craft time tracking
        private double passiveCraftTime;
        private double timeSinceLastCraft = 0;

        //pour gerer le status enabled/disabled et informer le joueur
        private StatusElement status;
        LocString SuccessStatus => Localizer.Do($"Le recherche est en cours à raison de 1 {Item.Get(itemType).MarkedUpName} toutes les {(passiveCraftTime / CheckRoom()):F0} secondes.");
        LocString FailStatus => Localizer.Do($"Le recherche est à l'arrêt : Stockage {Item.Get(itemType).MarkedUpName} plein !");

        //article fabriqué
        private Type itemType;

        //le stockage
        private PublicStorageComponent publicStorage;
        bool shutdownFromFullInv;

        //Initialise le component
        public void Initialize(double passiveCraftTime, int slots, Type itemType)
        {
            // Initialise le WorldObjectComponent
            base.Initialize();
            //Récupère les paramètres passés lors de l'initialisation
            this.passiveCraftTime = passiveCraftTime;
            this.itemType = itemType;
            // Gestion du statut de l'objet
            status = Parent.GetComponent<StatusComponent>().CreateStatusElement(50);
            // Initialisation du stockage
            publicStorage = Parent.GetComponent<PublicStorageComponent>();
            publicStorage.Initialize(slots);
            publicStorage.Inventory.AddInvRestriction(new TagRestriction("Science"));
            publicStorage.Storage.OnChanged.Add(TurnOnIfRoom);
        }

        public override void Tick() => Tick(ServiceHolder<IWorldObjectManager>.Obj.TickDeltaTime * EcoDef.Obj.TimeMult);
        public void Tick(float deltaTime)
        {
            if (Parent.Enabled)
            {
                if (IsDuplicatedInDeed())
                {
                    status.SetStatusMessage(false, Localizer.Do($"OK duplicate"), Localizer.Do($"duplicate PAS BON !"));
                    return;
                }

                if (timeSinceLastCraft < (passiveCraftTime / CheckRoom()))
                {
                    timeSinceLastCraft += deltaTime;
                    return;
                }

                //Mise a jour du statut
                status.SetStatusMessage(Research(), SuccessStatus, FailStatus);

                //var val = RoomData.Obj.GetEnclosedRoomForWorldObject(this.Parent).RoomValue.Value;
                //status.Message = Localizer.NL($"\nResearch Value");  //En cours de test... (homefurnishingvalue.cs)
                //status.Message = Localizer.NL($"\n{val}");  //En cours de test... (homefurnishingvalue.cs)  
            }
        }
        public bool Research()
        {
            //Tente d'ajouter l'article dans le stockage
            var result = publicStorage.Inventory.TryAddItem(itemType);

            if (result)
            {
                //Remise à 0 du compteur de temps
                timeSinceLastCraft = 0;
            }
            else
            {
                //Stockage plein
                Parent.GetComponent<OnOffComponent>().On = false;
                shutdownFromFullInv = true;
            }

            return result;
        }
        public void TurnOnIfRoom(User user)
        {
            //Si changement dans le stockage, tente de réactiver le component
            if (shutdownFromFullInv) 
            {
                Parent.GetComponent<OnOffComponent>().On = true;
                shutdownFromFullInv = false;
            }
        }
        public float CheckRoom() 
        {
            //Récupération de la valeur de la pièce
            var room = RoomData.Obj.GetEnclosedRoomForWorldObject(this.Parent);
            var roomVal = room.RoomValue.Value;
            return roomVal;
        }
        public bool IsDuplicatedInDeed() 
        {
            // Voir PropertyValue.cs (42) & ParallelProcessing.cs
            var room = RoomData.Obj.GetEnclosedRoomForWorldObject(this.Parent);
            var roomDeed = room.RoomDeed;
            var nbResearchComponent = room.RoomDeed.Rooms.SelectMany(r => r.RoomStats.ContainedWorldObjects).Select(x => x.GetComponent<ResearchComponent>()).Count(x => x is ResearchComponent);

            Log.WriteLine(Localizer.Do($"Nb Research Component = {nbResearchComponent}"));

            if (nbResearchComponent > 1) return true;

            return false;
        }
    }
}
