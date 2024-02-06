/* Le Village
Commandes de chat pour le mod Exhaustion :
Exhaustion              - liste de commandes
Exhaustion reset        - Met à 0 le timer d'un joueur (admin)
Exhaustion checktimer   - Info sur le dernier abandon
Exhaustion checkplayer  - Info sur le dernier abandon d'un joueur (admin)
Exhaustion informations   - Info générale sur UnSkill
*/

using Eco.Core;
using Eco.Gameplay.Aliases;
using Eco.Gameplay.Players;
using Eco.Gameplay.Systems.Messaging.Chat.Commands;
using Eco.Shared.Localization;
using Eco.Shared.Services;
using Eco.Simulation.Time;
using Village.Eco.Mods.Core;

namespace Village.Eco.Mods.ExhaustionMod
{
    [ChatCommandHandler]
    public static class CommandsExhaustion
    {
        [ChatCommand("Commandes liées à l'épuisement", ChatAuthorizationLevel.User)]
        public static void Fatigue(User user) { }

        [ChatSubCommand("Fatigue", "Remettre à 0 le délai", ChatAuthorizationLevel.Admin)]
        public static void Reset(User user, User targetUser)
        {
            int timer = 0;
            string message;

            //Recuperation des donnees du joueur
            var plugin = PluginManager.GetPlugin<PlayersDataPlugin>();
            var playerData = plugin.GetUserDataOrDefault(targetUser);

            //Mise a jour des donnees du joueur
            playerData.LastDailyBoost = timer;
            plugin.AddOrSetUserData(targetUser, playerData);

            //Message d'info de confirmation à l'admin
            message = Localizer.Do($"Le joueur {targetUser.Name} - Nouveau timer de boost {playerData.LastDailyBoost}.");
            user.InfoBoxLocStr(message);
            //Message mail de confirmation au joueur
            message = Localizer.Do($"Votre timer pour reprendre un boost journalier a été réinitialisé !");
            targetUser.MailLocStr(message, NotificationCategory.Skills);

        }

        [ChatSubCommand("Fatigue", "Consulter le délai", ChatAuthorizationLevel.User)]
        public static void CheckTimer(User user)
        {
            //Recuperation des donnees du joueur
            var plugin = PluginManager.GetPlugin<PlayersDataPlugin>();
            var playerData = plugin.GetUserDataOrDefault(user);
            var lastUnspecializing = playerData.LastDailyBoost;
            var daysSinceLastUnspecializing = WorldTime.Day - lastUnspecializing;
            int day = (int)daysSinceLastUnspecializing;  //La partie entière = Jours
            int hour = (int)((daysSinceLastUnspecializing - day) * 24);  //La partie décimale x 24 = Heures

            string message;
            if (lastUnspecializing == 0)
            {
                message = Localizer.Do($"Vous pouvez (re)prendre un boost journalier.");
            }
            else
            {
                message = Localizer.Do($"La dernière fois que vous avez un boost journalier remonte à {day} jour(s) et {hour} heure(s).");
            }
            user.Player.InfoBoxLocStr(message);
        }

        [ChatSubCommand("Fatigue", "Consulter le délai", ChatAuthorizationLevel.Admin)]
        public static void CheckPlayer(User user, User targetUser = null)
        {
            var currentUser = targetUser != null ? targetUser : user;

            //Recuperation des donnees du joueur cible
            var plugin = PluginManager.GetPlugin<PlayersDataPlugin>();
            var playerData = plugin.GetUserDataOrDefault(currentUser);
            var lastUnspecializing = playerData.LastDailyBoost;
            var daysSinceLastUnspecializing = WorldTime.Day - lastUnspecializing;
            int day = (int)daysSinceLastUnspecializing;  //La partie entière = Jours
            int hour = (int)((daysSinceLastUnspecializing - day) * 24);  //La partie décimale x 24 = Heures

            //a tester :
            //var calc = TimeFormatter.FormatSpan(WorldTime.Seconds - user.LoginTime); 

            string message;
            message = Localizer.Do($"Dernière prise de boost journalier du joueur {currentUser.Name}, il y a {day} jour(s) et {hour} heure(s).");
            user.Player.InfoBoxLocStr(message);
            user.Player.MsgLocStr(message);
        }

        [ChatSubCommand("Fatigue", "Afficher les informations sur le système d'épuisement", ChatAuthorizationLevel.User)]
        public static void Informations(User user)
        {
            LocString header;
            LocString message;
            LocString button;

            header = Localizer.Do($"Informations sur l'épuisement (ou Exhaustion)");

            message = Localizer.Do($"Le paramétrage serveur : 3h par jour avec report du temps non consommé jusqu'à un maximum de 9h");
            message += "\r\n";
            message += Localizer.Do($"Une fois épuisé, toutes actions consommant des calories est impossible sauf :");
            message += "\r\n";
            message += Localizer.Do($"(1) Les véhicules de transport (camions et bateaux)");
            message += "\r\n";
            message += Localizer.Do($"(2) Les marteaux quelques soient leur utilisation");
            message += "\r\n";
            message += Localizer.Do($"(3) Les déplacements à pied");
            message += "\r\n";
            message += "\r\n";
            message += Localizer.Do($"Chaque jour, si vous êtes épuisé, vous pouvez prendre un boost journalier : café qui se fabrique sur le feu de camp.");
            message += "\r\n";
            message += Localizer.Do($"Les autres boosts sont à usage unique et ne peuvent pas être fabriqué.");
            message += "\r\n";
            message += Localizer.Do($"Enfin, lors de la 1ère connexion au serveur, vous récupérez tout le temps de jeu que vous avez raté depuis le lancement du serveur.");

            button = Localizer.Do($"J'ai compris !");

            user.Player.LargeInfoBox(header, message, button);
        }
    }
}

