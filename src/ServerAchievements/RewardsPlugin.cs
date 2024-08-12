using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Gameplay.Players;
using Eco.Shared.Localization;
using Eco.Shared.Services;
using Eco.Simulation;

namespace Village.Eco.Mods.ServerAchievements
{
    public class RewardsPlugin : IInitializablePlugin, IModKitPlugin
    {
        public void Initialize(TimedTask timer) 
        {
            PlantSimEvents.TreeFelledEvent.Add( (user, treeSpecies) => Message((User)user) );
            
        }

        public void Message(User user) 
        {
            user.MsgLocStr($"1 arbre", NotificationStyle.InfoBox);
        }

        public string GetCategory() => "LeVillageMods";
        public override string ToString() => Localizer.DoStr("Rewards Plugin");
        public string GetStatus() => "Active";
    }
}
