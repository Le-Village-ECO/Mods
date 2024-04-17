using Eco.Gameplay.DynamicValues;
using Eco.Gameplay.Players;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Village.Eco.Mods.Core;

namespace Village.Eco.Mods.Nutrition
{
    #region Talent1
    public partial class Test1Talent : Talent
    {
        public override bool Base => true;
    }

    [Serialized]
    [LocDisplayName("Talent 1 : Test")]
    [LocDescription("En cours de développement...")]
    public partial class DietTest1TalentGroup : TalentGroup
    {
        public DietTest1TalentGroup()
        {
            Talents = new Type[]
            {
                typeof(DietTest1Talent),
            };
            this.OwningSkill = typeof(DietSkill);
            this.Level = 5;
        }
    }

    [Serialized]
    public partial class DietTest1Talent : Test1Talent
    {
        public override bool Base { get { return false; } }
        public override Type TalentGroupType { get { return typeof(DietTest1TalentGroup); } }
        public DietTest1Talent()
        {
            this.Value = 1;
        }
    }
    #endregion

    #region Talent2
    public partial class Test2Talent : Talent
    {
        public override bool Base => true;
    }
    public partial class Test2Talent
    {
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
    [LocDisplayName("Talent 2 : 5kg")]
    [LocDescription("En cours de développement...")]
    public partial class DietTest2TalentGroup : TalentGroup
    {
        public DietTest2TalentGroup()
        {
            Talents = new Type[]
            {
                typeof(DietTest2Talent),
            };
            this.OwningSkill = typeof(DietSkill);
            this.Level = 5;
        }
    }

    [Serialized]
    public partial class DietTest2Talent : Test2Talent
    {
        public override bool Base { get { return false; } }
        public override Type TalentGroupType { get { return typeof(DietTest2TalentGroup); } }
        public DietTest2Talent()
        {
            this.Value = 5000;
        }
    }
    #endregion
}
