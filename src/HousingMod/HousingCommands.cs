using Eco.Gameplay.Players;
using Eco.Gameplay.Property;
using Eco.Gameplay.Systems.Messaging.Chat.Commands;
using Eco.Shared.Localization;

namespace Village.Eco.Mods.HousingMod
{
    [ChatCommandHandler]
    public static partial class DietCommands
    {
        [ChatCommand("LVHousing", ChatAuthorizationLevel.Admin)]
        public static void LVHousing() { }

        [ChatSubCommand("LVHousing", "TestTick", ChatAuthorizationLevel.Admin)]
        public static void TestTick(User user)
        {
            // Récupération du niveau de la spécialité
            int level = user.Skillset.GetSkill(typeof(HousingSkill)).Level;

            // Récupération de la valeur de la résidence
            var residencyValue = user.ResidencyPropertyValue.Value;

            user.Skillset.AddExperience(typeof(HousingSkill), 20, Localizer.DoStr("Test du Tick.")); //Gain exp. sera multiplié par le bonus exp.
        }

        [ChatSubCommand("LVHousing", "InfoResidency", ChatAuthorizationLevel.Admin)]
        public static void InfoResidency(User user)
        {

            var value = user.ResidencyPropertyValue.Value;
            var rooms = user.ResidencyPropertyValue.Rooms;
            var deed = user.GetResidencyHouse();

            user.Player.Msg(Localizer.Format($"value : {value}"));
            user.Player.Msg(Localizer.Format($"roomsums : {rooms}"));
            user.Player.Msg(Localizer.Format($"deed : {deed}"));
        }
    }
}
