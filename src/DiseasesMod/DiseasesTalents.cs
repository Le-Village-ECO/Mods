using Eco.Gameplay.Players;
using Eco.Gameplay.Skills;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using System;

namespace Village.Eco.Mods.Diseases
{
    #region HealthyX
    public partial class Healthy1Talent : Talent
    {
        public override bool Base => true;
        public override void OnLearned(User user) => base.OnLearned(user);
        public override void OnUnLearned(User user) => base.OnUnLearned(user);
    }
    public partial class Healthy2Talent : Talent
    {
        public override bool Base => true;
        public override void OnLearned(User user) => base.OnLearned(user);
        public override void OnUnLearned(User user) => base.OnUnLearned(user);
    }
    public partial class Healthy3Talent : Talent
    {
        public override bool Base => true;
        public override void OnLearned(User user) => base.OnLearned(user);
        public override void OnUnLearned(User user) => base.OnUnLearned(user);
    }
    public partial class Healthy4Talent : Talent
    {
        public override bool Base => true;
        public override void OnLearned(User user) => base.OnLearned(user);
        public override void OnUnLearned(User user) => base.OnUnLearned(user);
    }

    [Serialized,LocDisplayName("Bonne santé : Pas de maux de tête."),LocDescription("Aucun symptôme, tout va bien !")]
    public partial class DiseasesHealthy1TalentGroup : TalentGroup
    {
        public DiseasesHealthy1TalentGroup()
        {
            Talents = new Type[]
            {
                typeof(Healthy1Talent),
            };
            this.OwningSkill = typeof(DiseasesSkill);
            this.Level = 1;
        }
    }
    [Serialized, LocDisplayName("Bonne santé : Articulations OK."), LocDescription("Aucun symptôme, tout va bien !")]
    public partial class DiseasesHealthy2TalentGroup : TalentGroup
    {
        public DiseasesHealthy2TalentGroup()
        {
            Talents = new Type[]
            {
                typeof(Healthy2Talent),
            };
            this.OwningSkill = typeof(DiseasesSkill);
            this.Level = 2;
        }
    }
    [Serialized, LocDisplayName("Bonne santé : Estomac OK"), LocDescription("Aucun symptôme, tout va bien !")]
    public partial class DiseasesHealthy3TalentGroup : TalentGroup
    {
        public DiseasesHealthy3TalentGroup()
        {
            Talents = new Type[]
            {
                typeof(Healthy3Talent),
            };
            this.OwningSkill = typeof(DiseasesSkill);
            this.Level = 3;
        }
    }
    [Serialized, LocDisplayName("Bonne santé : Digestion OK"), LocDescription("Aucun symptôme, tout va bien !")]
    public partial class DiseasesHealthy4TalentGroup : TalentGroup
    {
        public DiseasesHealthy4TalentGroup()
        {
            Talents = new Type[]
            {
                typeof(Healthy4Talent),
            };
            this.OwningSkill = typeof(DiseasesSkill);
            this.Level = 4;
        }
    }

    [Serialized]
    public partial class DiseasesHealthy1Talent : Healthy1Talent
    {
        public override bool Base { get { return false; } }
        public override Type TalentGroupType { get { return typeof(DiseasesHealthy1TalentGroup); } }
        public DiseasesHealthy1Talent()
        {
            this.Value = 1;  //useless
        }
    }
    [Serialized]
    public partial class DiseasesHealthy2Talent : Healthy2Talent
    {
        public override bool Base { get { return false; } }
        public override Type TalentGroupType { get { return typeof(DiseasesHealthy2TalentGroup); } }
        public DiseasesHealthy2Talent()
        {
            this.Value = 1;  //useless
        }
    }
    [Serialized]
    public partial class DiseasesHealthy3Talent : Healthy3Talent
    {
        public override bool Base { get { return false; } }
        public override Type TalentGroupType { get { return typeof(DiseasesHealthy3TalentGroup); } }
        public DiseasesHealthy3Talent()
        {
            this.Value = 1;  //useless
        }
    }
    [Serialized]
    public partial class DiseasesHealthy4Talent : Healthy4Talent
    {
        public override bool Base { get { return false; } }
        public override Type TalentGroupType { get { return typeof(DiseasesHealthy4TalentGroup); } }
        public DiseasesHealthy4Talent()
        {
            this.Value = 1;  //useless
        }
    }
    #endregion

    #region HardLabor
    public partial class HardLaborTalent : Talent
    {
        //public override bool Base => true;
        public override void OnLearned(User user)
        {
            base.OnLearned(user);
            //user.CalorieRateMultiplier = 2;
        }
        public override void OnUnLearned(User user)
        {
            base.OnUnLearned(user);
        }
    }

    [Serialized]
    [LocDisplayName("Symptôme : Fièvre")]
    [LocDescription("+10% calories fabrication")]
    public partial class DiseasesHardLaborTalentGroup : TalentGroup
    {
        public DiseasesHardLaborTalentGroup()
        {
            Talents = new Type[]
            {
                typeof(DiseasesHardLaborTalent),
            };
            this.OwningSkill = typeof(DiseasesSkill);
            this.Level = 1;
        }
    }

    [Serialized]
    public partial class DiseasesHardLaborTalent : HardLaborTalent
    {
        public override bool Base { get { return false; } }
        public override Type TalentGroupType { get { return typeof(DiseasesHardLaborTalentGroup); } }
        public DiseasesHardLaborTalent()
        {
            this.Value = 1.1f;  //+10%
        }
    }
    #endregion

    #region SlowMvt
    public partial class SlowMvtTalent : Talent
    {
        //public override bool Base => true;
        public override void OnLearned(User user)
        {
            base.OnLearned(user);
            user.ChangedMovementSpeed();
        }
        public override void OnUnLearned(User user)
        {
            base.OnUnLearned(user);
            user.ChangedMovementSpeed();
        }
    }

    [Serialized]
    [LocDisplayName("Symptôme : Déplacement lent")]
    [LocDescription("-10% vitesse déplacement")]
    public partial class DiseasesSlowMvtTalentGroup : TalentGroup
    {
        public DiseasesSlowMvtTalentGroup()
        {
            Talents = new Type[]
            {
                typeof(DiseasesSlowMvtTalent),
            };
            this.OwningSkill = typeof(DiseasesSkill);
            this.Level = 2;
        }
    }

    [Serialized]
    public partial class DiseasesSlowMvtTalent : SlowMvtTalent
    {
        public override bool Base { get { return false; } }
        public override Type TalentGroupType { get { return typeof(DiseasesSlowMvtTalentGroup); } }
        public DiseasesSlowMvtTalent()
        {
            this.Value = -1.5f;  //-50%
        }
    }
    #endregion
    
    #region Vomit
    public partial class VomitTalent : Talent
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
    [LocDisplayName("Symptôme : Mal au coeur")]
    [LocDescription("Je ne me sens pas bien")]
    public partial class DiseasesVomitTalentGroup : TalentGroup
    {
        public DiseasesVomitTalentGroup()
        {
            Talents = new Type[]
            {
                typeof(DiseasesVomitTalent),
            };
            this.OwningSkill = typeof(DiseasesSkill);
            this.Level = 3;
        }
    }

    [Serialized]
    public partial class DiseasesVomitTalent : VomitTalent
    {
        public override bool Base { get { return false; } }
        public override Type TalentGroupType { get { return typeof(DiseasesVomitTalentGroup); } }
        public DiseasesVomitTalent()
        {
            this.Value = 1;  //useless
        }
    }
    #endregion

    #region Digestion
    public partial class DigestTalent : Talent
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
    [LocDisplayName("Symptôme : Mal au coeur")]
    [LocDescription("Je ne me sens pas bien")]
    public partial class DiseasesDigestTalentGroup : TalentGroup
    {
        public DiseasesDigestTalentGroup()
        {
            Talents = new Type[]
            {
                typeof(DiseasesDigestTalent),
            };
            this.OwningSkill = typeof(DiseasesSkill);
            this.Level = 4;
        }
    }

    [Serialized]
    public partial class DiseasesDigestTalent : VomitTalent
    {
        public override bool Base { get { return false; } }
        public override Type TalentGroupType { get { return typeof(DiseasesDigestTalentGroup); } }
        public DiseasesDigestTalent()
        {
            this.Value = 500;  //useless
        }
    }
    #endregion
}
