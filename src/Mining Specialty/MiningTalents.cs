using Eco.Gameplay.GameActions;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Players;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Localization;
using Eco.Shared.Math;
using Eco.Shared.Networking;
using Eco.Shared.Serialization;
using Eco.Shared.Utils;
using Eco.Shared.Voxel;
using System.Collections.Generic;
using System.Linq;
using System;
using Eco.Gameplay.Systems.Messaging.Notifications;

namespace Village.Eco.Mods.MiningSpecialty
{
    #region Coron
    public partial class CoronTalent : Talent
    {
        public override bool Base => true;
        public override void OnLearned(User user)
        {
            base.OnLearned(user);
        }
        public override void OnUnLearned(User user)
        {
            base.OnUnLearned(user);
        }

        public readonly int PickUpRange = 4;
        public override void RegisterTalent(User user)
        {
            base.RegisterTalent(user);
            user.OnPickupingObject.Add(this.ApplyAction);
        }
        public override void UnRegisterTalent(User user)
        {
            base.UnRegisterTalent(user);
            user.OnPickupingObject.Remove(this.ApplyAction);
        }
        void ApplyAction(User user, INetObject target, INetObject tool, GameActionPack pack) 
        {
            //Utilisation des mains uniquement
            if (tool != null) return;
            if (target is RubbleObject rubble) this.ApplyTalent(user, rubble, pack);
        }
        void ApplyTalent(User user, RubbleObject target, GameActionPack pack)
        {
            if (!(target is IRepresentsItem representsItem)) return;

            var itemType = representsItem.RepresentedItemType;
            // max stack size minus currently picking item
            var numToTake = Item.GetMaxStackSize(itemType) - 1;
            if (numToTake <= 0) return;

            // Le Village - réutilisation du code du talent SweepingHands avec ajout d'un contrôle sur le type de block
            //NotificationManager.ServerMessageToAllLoc($"itemType : {itemType.Name}");
            if (itemType.Name != "CoalItem") return;

            var carrying = user.Carrying;
            if (!carrying.Empty())
            {
                // ReSharper disable once PossibleNullReferenceException because Empty checked
                if (carrying.Item.Type != itemType || carrying.Quantity >= numToTake) return;

                // adjust to currently carrying item count
                numToTake -= carrying.Quantity;
            }

            // Get not breakable rubble around the target one and group them by their plot position.
            var originPlotPos = target.Position.XZi().ToPlotPos();
            var nearbyRubbleGroups = NetObjectManager.Default.GetObjectsWithin(target.Position, this.PickUpRange)
                                                     .OfType<RubbleObject>()
                                                     .Where(x => x != target && !x.IsBreakable && x is IRepresentsItem rubbleRepresentsItem && rubbleRepresentsItem.RepresentedItemType == itemType)
                                                     .GroupBy(x => x.Position.XZi().ToPlotPos()).ToList();

            // Exexute PickupRubbles for each rubble in current plot
            var currentPlotData = nearbyRubbleGroups.FirstOrDefault(x => x.Key == originPlotPos);
            if (currentPlotData != null) this.CollectRubblesOnPlot(currentPlotData.ToList(), user, pack, itemType, numToTake);

            //todo implement CollectRubblesOnPlot execution after Auth refactor to be able to check auth before adding actions to pack
        }

        // Executes PickupRubbles action on rubble list without notifying (to omit loops for talent reuse) and returns success count
        private int CollectRubblesOnPlot(List<RubbleObject> rubbles, User user, GameActionPack pack, Type itemType, int numToTake)
        {
            var numTaken = 0;

            // Process rubble within the same plot (it will have equal auth requirements and can be done inside the original pack).
            foreach (var rubble in rubbles)
            {
                // Try to add rubble-related stuff to the pack and increment the counter if succeeded.
                if (pack.PickupRubbles(user.Player, user.Inventory, rubble.SingleItemAsEnumerable(), itemType, notificate: false)) numTaken++;

                // No need to continue if it already has required amount.
                if (numTaken == numToTake) break;
            }

            return numTaken;
        }
    }

    [Serialized]
    [LocDisplayName("Mineur de fond : Mining")]
    [LocDescription("Au nord, c'était les corons !")]
    public partial class MiningCoronTalentGroup : TalentGroup
    {
        public MiningCoronTalentGroup()
        {
            Talents = new Type[]
            {
                typeof(MiningCoronTalent),
            };
            this.OwningSkill = typeof(MiningSkill);
            this.Level = 7;
        }
    }

    [Serialized]
    public partial class MiningCoronTalent : CoronTalent
    {
        public override bool Base { get { return false; } }
        public override Type TalentGroupType { get { return typeof(MiningCoronTalentGroup); } }
        public MiningCoronTalent()
        {
            this.Value = 1;
        }
    }
    #endregion

    #region Chercheur d'or
    public partial class GoldRusherTalent : Talent
    {
        public override bool Base => true;
        public override void OnLearned(User user)
        {
            base.OnLearned(user);
        }
        public override void OnUnLearned(User user)
        {
            base.OnUnLearned(user);
        }
    }

    [Serialized]
    [LocDisplayName("Chercheur d'or : Mining")]
    [LocDescription("Tout ce qui brille est bon à prendre !")]
    public partial class MiningGoldrusherTalentGroup : TalentGroup
    {
        public MiningGoldrusherTalentGroup()
        {
            Talents = new Type[]
            {
                typeof(MiningGoldRusherTalent),
            };
            this.OwningSkill = typeof(MiningSkill);
            this.Level = 7;
        }
    }

    [Serialized]
    public partial class MiningGoldRusherTalent : GoldRusherTalent
    {
        public override bool Base { get { return false; } }
        public override Type TalentGroupType { get { return typeof(MiningGoldrusherTalentGroup); } }
        public MiningGoldRusherTalent()
        {
            this.Value = 1;
        }
    }
    #endregion
}
