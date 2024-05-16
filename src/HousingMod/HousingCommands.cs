using Eco.Gameplay.Players;
using Eco.Gameplay.Systems.Messaging.Chat.Commands;

namespace Village.Eco.Mods.HousingMod
{
    [ChatCommandHandler]
    public static partial class DietCommands
    {
        [ChatCommand("LVHousing", ChatAuthorizationLevel.Admin)]
        public static void LVHousing() { }

        [ChatSubCommand("LVHousing", "Test1", ChatAuthorizationLevel.Admin)]
        public static void Test1(User user)
        {

        }
    }
}
