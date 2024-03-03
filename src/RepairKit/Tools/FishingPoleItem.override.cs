// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.
namespace Eco.Mods.TechTree
{
    using System;
    using System.ComponentModel;
    using Eco.Core.Controller;
    using Eco.Core.Items;
    using Eco.Gameplay.DynamicValues;
    using Eco.Gameplay.Items;
    using Eco.Shared.Serialization;
    using Eco.Shared.Networking;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Animals;
    using Eco.Gameplay.Utils;
    using Eco.Shared.Localization;
    using Eco.Shared.Math;
    using Vector3 = System.Numerics.Vector3;
    using Eco.Gameplay.Animals.Catchers;
    using Eco.Gameplay.Interactions.Interactors;
    using Eco.Mods.Organisms.SpeciesCatchers;
    using Eco.Shared.Items;
    using Eco.Shared.SharedTypes;
    using Eco.Core.Utils;
    using Eco.Core.Utils.Logging;

    //this is going to be a real item at some point

    [Serialized]
    [LocDisplayName("Fishing Pole")]
    [LocDescription("A wooden pole attached to a line and hook. Used to catch fish from rivers and the ocean.")]
    [Weight(1000)]
    [Category("Tool")]
    [Tag("Tool")]
    [Ecopedia("Items", "Tools", createAsSubPage: true)]
    public partial class FishingPoleItem : ToolItem, IInteractor
    {
        private static IDynamicValue talent                 = new TalentModifiedValue(typeof(FishingPoleItem), typeof(ToolEfficiencyTalent));
        private static IDynamicValue calories               = CreateCalorieValue(20, typeof(HuntingSkill), typeof(WoodenBowItem));
        private static IDynamicValue caloriesBurn           = new MultiDynamicValue(MultiDynamicOps.Multiply, talent, calories);
        private static IDynamicValue exp                    = new ConstantValue(1);
        private static IDynamicValue tier                   = new ConstantValue(0);
        private static IDynamicValue skilledRepairCost      = new ConstantValue(1); //5 to 1

        // Tool overrides
        public override IDynamicValue CaloriesBurn      => caloriesBurn;
        public override Type ExperienceSkill            => typeof(HuntingSkill);
        public override IDynamicValue ExperienceRate    => exp;
        public override IDynamicValue Tier              => tier;
        public override IDynamicValue SkilledRepairCost => skilledRepairCost;
        public override float DurabilityRate            => DurabilityMax / 200f;
        //public override Item RepairItem                 => Item.Get<Item>();
        //public override Tag RepairTag                   => TagManager.Tag("Wood");
        public override Item RepairItem                 => Item.Get<WoodRepairKitItem>();
        public override int FullRepairAmount            => 1; //5 to 1

        // Tool Base Item
        public override ItemCategory  ItemCategory        => ItemCategory.FishingPole;
        public override bool          CanBeUsedWithEmotes => false;
        public LureEntity? Lure { get; private set; }

        static FishingPoleItem() { }

        /// <summary> Creates the LureEntity from a Client-command, assigning its controller, position, and the force to apply at spawn. </summary>
        [RPC]
        public int CastLure(Player player, Vector3 position, Vector3 castForce)
        {
            if(player?.User == null) return 0;

            if (!player.User.ExhaustionMonitor.CheckEnergy(out var result))
            {
                player.Error(result);
                return 0;
            }

            var calories = NeededCalories(player);
            if (calories > player.User.Stomach.Calories)
            {
                player.ErrorLocStr($"You are too hungry to do that.");
                return 0;
            }

            player.User.Stomach.BurnCalories(calories, true);
            UseDurability(1, player);

            this.Lure = new LureEntity   // Set up the new lure here server-side.
            {
                Controller = player,
                Position = position,
                CastForce = castForce,
            };
            this.Lure.SetActiveAndCreate();  // Create the lure and send it out.

            SpeciesLayeredCatchPlugin.Obj.AddLayeredCatcher(this, new FishingPoleCatcher(player.User, this));

            return this.Lure.ID;
        }

        [RPC]
        private void FinalizeCatch(Player player, INetObject target)
        {
            if (target is not AnimalEntity animal) return;

            var xpMessage = Localizer.Do($"fishing {Localizer.A(animal.Species.DisplayName).AppendSpaceIfSet()}");
            AddExperience(player.User, 1, xpMessage);

            animal.Destroy();

            var resourceType = animal.Species.ResourceItemType;
            if (resourceType == null || !player.User.Inventory.TryAddItem(resourceType, player.User).Notify(player)) return;
            animal.Kill();
        }

        [RPC]
        private void Release(INetObject target)
        {
            if (target is AnimalEntity animal) animal.Destroy(); // Returning this call untill better solution is found
            // Future code here for if a caught-fish receives any state change. This space will undo what was done.
        }

        [Interaction(InteractionTrigger.LeftClick, canHoldToTrigger: TriBool.False, authRequired: AccessType.ConsumerAccess, animationDriven: true, Flags = InteractionFlags.NoTargetRequired)]
        public void CastInteraction(Player player, InteractionTriggerInfo trigger, InteractionTarget target) { }
    }

    public class LureEntity : NetEntity
    {
        public INetObjectViewer? Controller { get; set; }
        public Vector3 CastForce;

        public LureEntity() : base("Lure") { }

        [RPC]
        public override void Destroy()
        {
            // Let clients help decide when to destroy the lure.
            base.Destroy();
        }

        public override void SendInitialState(BSONObject bsonObj, INetObjectViewer viewer)
        {
            base.SendInitialState(bsonObj, viewer);
            bsonObj["pos"] = this.Position;
            bsonObj["force"] = this.CastForce;
            if (this.Controller != null && this.Controller is INetObject)
                bsonObj["controller"] = ((INetObject)this.Controller).ID;
        }
        public override void ReceiveUpdate(BSONObject bsonObj)
        {
            // Store the received position if valid.
            if (bsonObj.TryGetValue("pos", out var pos) && pos.Vector3Value.IsFinite()) { this.Position = pos; }

            // If the Lure has no controller or has descended below the world, destroy it.
            bsonObj.TryGetValue("controlled", out var controlled);
            if ((this.Position.Y < -30.0f) || !controlled) { this.Destroy(); }

            base.ReceiveUpdate(bsonObj);
        }

        // Trigger an update to anyone not the owner of this Lure
        public override bool IsUpdated(INetObjectViewer viewer) => this.Controller != viewer;   

        public override void SendUpdate(BSONObject bsonObj, INetObjectViewer viewer)
        {
            bsonObj["pos"] = this.Position;
            base.SendUpdate(bsonObj, viewer);
        }

        public void FishCaught(int fishId) => this.RPC("FishCaught", fishId);
    }
}