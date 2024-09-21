// Le Village - Ajout des WorldObjects pour la définition des pièges pour animaux

using System;
using System.Collections.Generic;
using Eco.Gameplay.Animals.Catchers;
using Eco.Gameplay.Components;
using Eco.Gameplay.Components.Storage;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Mods.Organisms.SpeciesCatchers;
using Eco.Mods.TechTree;
using Eco.Shared.Localization;
using Eco.Shared.Math;
using Eco.World;
using Eco.World.Blocks;

namespace Village.Eco.Mods.Traps
{
    [RequireComponent(typeof(AnimalTrapComponent))]
    public partial class FoxTrapObject : WorldObject
    {
        protected override void PostInitialize()
        {
            base.PostInitialize();
            this.GetComponent<PublicStorageComponent>().Initialize(1, 2000); //1 FoxCarcass vaut 2kg
            this.GetComponent<PublicStorageComponent>().Inventory.AddInvRestriction(new SpecificItemTypesRestriction(new System.Type[] { typeof(FoxCarcassItem) }));
            this.GetComponent<PublicStorageComponent>().Inventory.AddInvRestriction(new StackLimitRestriction(1));
            this.GetComponent<AnimalTrapComponent>().Initialize(new List<string>() { "Fox" });
            this.GetComponent<AnimalTrapComponent>().UpdateEnabled();

            SpeciesLayeredCatchPlugin.Obj.AddLayeredCatcher(this, new FoxCatcher(null, this));
        }
    }

    [RequireComponent(typeof(AnimalTrapComponent))]
    public partial class SmallAnimalTrapObject : WorldObject
    {
        protected override void PostInitialize()
        {
            base.PostInitialize();
            this.GetComponent<PublicStorageComponent>().Initialize(1, 2000); //1 HareCarcass vaut 1kg et 1 AgoutiCarcass vaut 2kg
            this.GetComponent<PublicStorageComponent>().Inventory.AddInvRestriction(new SpecificItemTypesRestriction(new System.Type[] { typeof(HareCarcassItem), typeof(AgoutiCarcassItem) }));
            this.GetComponent<PublicStorageComponent>().Inventory.AddInvRestriction(new StackLimitRestriction(1));
            this.GetComponent<AnimalTrapComponent>().Initialize(new List<string>() { "Hare", "Agouti" });
            this.GetComponent<AnimalTrapComponent>().UpdateEnabled();

            SpeciesLayeredCatchPlugin.Obj.AddLayeredCatcher(this, new SmallAnimalCatcher(null, this));
        }
    }

    [RequireComponent(typeof(AnimalTrapComponent))]
    public partial class TurkeyTrapObject : WorldObject
    {
        protected override void PostInitialize()
        {
            base.PostInitialize();
            this.GetComponent<PublicStorageComponent>().Initialize(4, 4000); //1 TurkeyCarcass vaut 1kg
            this.GetComponent<PublicStorageComponent>().Inventory.AddInvRestriction(new SpecificItemTypesRestriction(new System.Type[] { typeof(TurkeyCarcassItem) }));
            this.GetComponent<PublicStorageComponent>().Inventory.AddInvRestriction(new StackLimitRestriction(1));
            this.GetComponent<AnimalTrapComponent>().Initialize(new List<string>() { "Turkey" });
            this.GetComponent<AnimalTrapComponent>().UpdateEnabled();

            SpeciesLayeredCatchPlugin.Obj.AddLayeredCatcher(this, new TurkeyCatcher(null, this));
        }
    }
}
