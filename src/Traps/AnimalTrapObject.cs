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
            //Paramètres de stockage (slots, poids)
            this.GetComponent<PublicStorageComponent>().Initialize(1, 2000); //1 FoxCarcass vaut 2kg
            this.GetComponent<PublicStorageComponent>().Inventory.AddInvRestriction(new SpecificItemTypesRestriction(new System.Type[] { typeof(FoxCarcassItem) }));
            this.GetComponent<PublicStorageComponent>().Inventory.AddInvRestriction(new StackLimitRestriction(1));
            this.GetComponent<AnimalTrapComponent>().Initialize(new List<string>() { "Fox" });
            //this.GetComponent<AnimalTrapComponent>().FailStatusMessage = Localizer.DoStr("Wooden fish traps must be placed underwater in fresh water to function.");
            //this.GetComponent<AnimalTrapComponent>().EnabledTest = this.OutOfWaterTest;
            this.GetComponent<AnimalTrapComponent>().UpdateEnabled();

            SpeciesLayeredCatchPlugin.Obj.AddLayeredCatcher(this, new FoxCatcher(null, this));
        }
        /*public bool OutOfWaterTest(Vector3i pos)
        {
            var blockAbove = World.GetBlock(pos + Vector3i.Up); // Get the block above the trap

            if (blockAbove is IWaterBlock waterBlock && !waterBlock.PipeSupplied) return false; // Above block IS water and NOT from a pipe
            else return true;
        }*/
    }

    [RequireComponent(typeof(AnimalTrapComponent))]
    public partial class SmallAnimalTrapObject : WorldObject
    {
        protected override void PostInitialize()
        {
            base.PostInitialize();
            this.GetComponent<PublicStorageComponent>().Initialize(1, 2000);
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
            this.GetComponent<PublicStorageComponent>().Initialize(1, 2000);
            this.GetComponent<PublicStorageComponent>().Inventory.AddInvRestriction(new SpecificItemTypesRestriction(new System.Type[] { typeof(TurkeyCarcassItem) }));
            this.GetComponent<PublicStorageComponent>().Inventory.AddInvRestriction(new StackLimitRestriction(1));
            this.GetComponent<AnimalTrapComponent>().Initialize(new List<string>() { "Turkey" });
            this.GetComponent<AnimalTrapComponent>().UpdateEnabled();

            SpeciesLayeredCatchPlugin.Obj.AddLayeredCatcher(this, new TurkeyCatcher(null, this));
        }
    }
}
