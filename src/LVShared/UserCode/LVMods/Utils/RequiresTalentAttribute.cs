using Eco.Gameplay.Players;
using System;

namespace Village.Eco.Mods.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RequiresTalentAttribute : Attribute
    {
        public Type TalentType { get; set; }

        public RequiresTalentAttribute(Type requiredTalentType) 
        {
            if (requiredTalentType == null)
                throw new ArgumentNullException("Cannot require a null skill");

            TalentType = requiredTalentType;
        }

        public bool IsMet(Player player) => IsMet(player.User);
        public bool IsMet(User user) => user.Talentset.HasTalent(this.TalentType);

    }
}
