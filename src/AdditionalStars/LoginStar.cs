using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Gameplay.Players;
using Eco.Simulation;
using Eco.Simulation.Time;
using System;

namespace AdditionalStars
{
    public class LoginStar : IModKitPlugin, IInitializablePlugin
    {

        public static void GiveXPAtFirstConnection(User user)
        {
            float worldDayAtFirstConnection = (float)WorldTime.Day * EcoSim.Obj.EcoDef.TimeMult;
            float initXP = user.UserXP.SkillRate * worldDayAtFirstConnection;
            user.UserXP.AddExperience(initXP);
        }

        public void Initialize(TimedTask timer)
        {
            UserManager.NewUserJoinedEvent.Add (user => GiveXPAtFirstConnection(user));
        }
    public string GetStatus() => String.Empty;
    public string GetCategory() => String.Empty;
    }

}
