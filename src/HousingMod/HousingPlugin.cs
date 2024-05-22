using Eco.Core.Plugins.Interfaces;
using Eco.Gameplay.Players;

namespace Village.Eco.Mods.HousingMod
{
    public class HousingPlugin : IModInit
    {
        public static void PostInitialize()
        {
            UserManager.NewUserJoinedEvent.Add(user =>
            {
                var skill = user.Skillset.GetOrAddSkill(typeof(HousingSkill));
                skill.ForceSetLevel(user, 1);
            });
        }
    }
}
