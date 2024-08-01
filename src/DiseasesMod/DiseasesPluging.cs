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
                user.Stomach.ChangedEvent.Add();
            });
        }

        public HousingPlugin() { }

        public static void Disease(User user) 
        {

        }

    }
}
