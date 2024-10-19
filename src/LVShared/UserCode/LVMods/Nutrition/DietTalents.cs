using Eco.Gameplay.Items;
using Eco.Gameplay.Players;
using Eco.Gameplay.Skills;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using System;
using System.Linq;

namespace Village.Eco.Mods.Nutrition
{
    #region DietAddWeightTalent
    public partial class AddWeightTalent : Talent
    {
        public override bool Base => true;
        public override void OnLearned(User user)
        {
            base.OnLearned(user);
            user.ChangedCarryWeight();
        }
        public override void OnUnLearned(User user)
        {
            base.OnUnLearned(user);
            user.ChangedCarryWeight();
        }
    }

    [Serialized]
    [LocDisplayName("Extension de sac : Diététique")]
    [LocDescription("Ajoute 15kg au sac à dos et améliore la pelle")]
    public partial class DietAddWeightTalentGroup : TalentGroup
    {
        public DietAddWeightTalentGroup()
        {
            Talents = new Type[]
            {
                typeof( DietAddWeightTalent),
            };
            this.OwningSkill = typeof(DietSkill);
            this.Level = 5;
        }
    }

    [Serialized]
    public partial class DietAddWeightTalent : AddWeightTalent
    {
        public override bool Base { get { return false; } }
        public override Type TalentGroupType { get { return typeof(DietAddWeightTalentGroup); } }
        public DietAddWeightTalent()
        {
            this.Value = 15000;
        }
    }
    #endregion

    #region DietStackSizeTalent
    public class StackSizeTalent : Talent
    {
        public override bool Base => true;
        public override void OnLearned(User user)
        {
            base.OnLearned(user);

            var carryInventory = user.Inventory.Carried;

            if (!carryInventory.Restrictions.Any(restriction => restriction is MultiplierInventoryRestriction))
            {
                carryInventory.AddInvRestriction(new MultiplierInventoryRestriction(DietStackSizeTalent.STACK_SIZE));
            }
        }
        public override void OnUnLearned(User user)
        {
            base.OnUnLearned(user);

            Inventory carryInventory = user.Inventory.Carried;
            carryInventory.RemoveAllRestrictions(restriction => restriction is MultiplierInventoryRestriction);
        }
    }

    [Serialized]
    [LocDisplayName("Force herculéenne : Diététique")]
    [LocDescription("Vous pouvez porter 50% d'objet en plus")]
    public partial class DietStackSizeTalentGroup : TalentGroup
    {
        public DietStackSizeTalentGroup()
        {
            Talents = new Type[]
            {
                typeof(DietStackSizeTalent),
            };
            this.OwningSkill = typeof(DietSkill);
            this.Level = 5;
        }
    }

    [Serialized]
    public partial class DietStackSizeTalent : StackSizeTalent
    {
        public const float STACK_SIZE = 1.5f;

        public override bool Base { get { return false; } }
        public override Type TalentGroupType { get { return typeof(DietStackSizeTalentGroup); } }

    }

    public class MultiplierInventoryRestriction : InventoryRestriction
    {
        private float value;

        public MultiplierInventoryRestriction(float value)
        {
            this.value = value;
        }

        public override LocString Message => new ();

        public override int MaxAccepted(Item item, int currentQuantity)
        {
            return (int)(item.MaxStackSize * value);
        }
        public override bool SurpassStackSize => true;
    }
    #endregion
}
