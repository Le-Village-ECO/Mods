using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Gameplay.Players;
using Eco.Shared.Localization;

namespace Village.Eco.Mods.Diseases
{
    public class DiseasesPlugin : IInitializablePlugin, IModKitPlugin
    {
        public void Initialize(TimedTask timer)
        {
            UserManager.NewUserJoinedEvent.Add(user =>
            {
                user.Stomach.ChangedEvent.Add(Disease);
            });
        }

        public DiseasesPlugin() { }

        public static void Disease(User user) 
        {

        }

        public string GetCategory() => "LeVillageMods";
        public override string ToString() => Localizer.DoStr("Diseases Plugin");
        public string GetStatus() => "Active";
    }
}
