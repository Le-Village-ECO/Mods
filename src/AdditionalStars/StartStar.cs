namespace Eco.Mods
{
    using Eco.Core.Plugins.Interfaces;
    using Eco.Core.Utils;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Skills;
    using Eco.Gameplay.Systems.Chat;
    using System;
    using System.Reflection;
    using System.Text;

    public partial class StartStar : IModKitPlugin, IInitializablePlugin
    {
        private int starsToAdd = 1;  // Modify this value to add stars on first connect to server. 
        
        public void Initialize(TimedTask timer)
        {
            UserManager.NewUserJoinedEvent.Add(user =>
            {
                user.UserXP.StarsAvailable += starsToAdd;
                user.UserXP.TotalStarsEarned += starsToAdd;
            });
        }
        
        public string GetStatus() => string.Empty;

        public string GetCategory() => string.Empty;

    }
    
}
