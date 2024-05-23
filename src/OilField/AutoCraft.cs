using Eco.Core.Controller;
using Eco.Core.Items;
using Eco.Core.Utils;
using Eco.Gameplay.Components;
using Eco.Gameplay.Components.Auth;
using Eco.Gameplay.Components.Storage;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Occupancy;
using Eco.Gameplay.Systems.Messaging.Notifications;
using Eco.Gameplay.Systems.NewTooltip;
using Eco.Mods.TechTree;
using Eco.Shared.Items;
using Eco.Shared.Localization;
using Eco.Shared.Math;
using Eco.Shared.Serialization;
using Eco.Simulation.Time;
using System;

namespace Village.Eco.Mods.OilField
{
    [Serialized]
    [RequireComponent(typeof(OnOffComponent))]
    [RequireComponent(typeof(MinimapComponent))]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(AutoCraftComponent))]
    [Tag("Usable")]
    [Ecopedia("Work Stations", "Craft Tables", subPageName: "Auto Crafter")]
    public partial class AutoCraftObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(AutoCraftItem);
        public override LocString DisplayName => Localizer.DoStr("Auto Crafter");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;
        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<MinimapComponent>().SetCategory(Localizer.DoStr("Crafting"));
            this.GetComponent<AutoCraftComponent>().Initialize(5, 1);
            this.ModsPostInitialize();
        }
        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Auto Crafter")]
    [LocDescription("Fabrication automatique d<objets")]
    [IconGroup("World Object Minimap")]
    [Ecopedia("Work Stations", "Craft Tables", createAsSubPage: true)]
    [Tag("Usable")]
    [Weight(1000)]  //en kg
    public partial class AutoCraftItem : WorldObjectItem<AutoCraftObject>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }
    [Serialized]
    [Priority(-2)]
    [RequireComponent(typeof(PublicStorageComponent))]
    [RequireComponent(typeof(StatusComponent))]
    public class AutoCraftComponent : WorldObjectComponent
    {
        // craft time tracking
        private double passiveCraftTime;
        private double timeSinceLastCraft = 0;

        //pour gerer le status enabled/disabled et informer le joueur
        private StatusElement status;

        //Initialise le component
        public void Initialize(double passiveCraftTime, int slots)
        {
            // Initialise le WorldObjectComponent
            base.Initialize();
            //Recupere les parametres passes lors de l'initialisation
            this.passiveCraftTime = passiveCraftTime;
            // Gestion du statut de l'objet
            status = Parent.GetComponent<StatusComponent>().CreateStatusElement();
            // Initialisation du stockage
            Parent.GetComponent<PublicStorageComponent>().Initialize(slots);
        }

        LocString DisplayStatus => Localizer.Do($"Auto Crafter est en fonctionnement a raison de 1 composte toutes les {passiveCraftTime} secondes.");

        public override void Tick()
        {
            if (timeSinceLastCraft == 0) timeSinceLastCraft = WorldTime.Seconds;

            if (Parent.Operating)
            {
                var inv = Parent.GetComponent<PublicStorageComponent>().Inventory;
                var delta = WorldTime.Seconds - timeSinceLastCraft;

                if (delta >= passiveCraftTime)
                {
                    NotificationManager.ServerMessageToAllLoc($"Tick = {WorldTime.Seconds} / Last = {timeSinceLastCraft} / Delta = {delta}");

                    //Tente d'ajouter l'article dans le stockage
                    if (!inv.TryAddItem<CompostItem>())
                    {
                        //Parent.GetComponent<OnOffComponent>().On = false; //desactivation de l'objet
                        status.SetStatusMessage(false, Localizer.DoStr("Il n'y a plus de place !")); //Mise a jour du statut
                        NotificationManager.ServerMessageToAllLoc($"C'est plein !"); //log
                    }
                    else
                    {
                        timeSinceLastCraft = WorldTime.Seconds;  //Copie du temps pour le prochain tick
                        status.SetStatusMessage(true, DisplayStatus); //Mise a jour du statut
                    }
                    
                }
                else NotificationManager.ServerMessageToAllLoc($"Delta = {delta}"); //log
            }
        }
    }
}
