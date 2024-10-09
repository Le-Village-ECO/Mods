/* Le Village
Commandes de chat pour le mod UnSkill :
Unskill             - liste de commandes
Unskill reset       - Met à 0 le timer d'un joueur (admin)
Unskill checktimer  - Info sur le dernier abandon
Unskill checkplayer - Info sur le dernier abandon d'un joueur (admin)
Unskill conditions  - Info générale sur UnSkill
*/

using Eco.Core;
using Eco.Gameplay.Aliases;
using Eco.Gameplay.Players;
using Eco.Gameplay.Skills;
using Eco.Gameplay.Systems.Messaging.Chat.Commands;
using Eco.Shared.Localization;
using Eco.Shared.Services;
using Eco.Shared.Utils;
using Eco.Simulation.Time;
using System;
using Village.Eco.Mods.Core;
using static Eco.Shared.Utils.TimeFormatter;

namespace Village.Eco.Mods.UnSkillScroll
{
    [ChatCommandHandler]
    public static class CommandsUnSkill
    {
        [ChatCommand("Commandes pour Oublier une spécialité", ChatAuthorizationLevel.User)]
        public static void UnSkill(User user) { }

        [ChatSubCommand("UnSkill", "Remettre à 0 le délai", ChatAuthorizationLevel.Admin)]
        public static void Reset(User user, User targetUser)
        {
            int timer = 0;
            string message;

            //Recuperation des donnees du joueur
            var plugin = PluginManager.GetPlugin<PlayersDataPlugin>();
            var playerData = plugin.GetUserDataOrDefault(targetUser);

            //Mise a jour des donnees du joueur
            playerData.LastUnspecializingDay = timer;
            plugin.AddOrSetUserData(targetUser, playerData);

            //Message d'info de confirmation à l'admin
            message = Localizer.Do($"Le joueur {targetUser.Name} - Nouveau timer {playerData.LastUnspecializingDay}.");
            user.OkBoxLocStr(message);
            //Message mail de confirmation au joueur
            message = Localizer.Do($"Votre timer pour abandonné une spécialité a été réinitialisé !");
            targetUser.MailLocStr(message, NotificationCategory.Skills);

        }

        [ChatSubCommand("UnSkill", "Consulter le délai", ChatAuthorizationLevel.User)]
        public static void CheckTimer(User user)
        {
            string message;

            //Recuperation des donnees du joueur
            var plugin = PluginManager.GetPlugin<PlayersDataPlugin>();
            var playerData = plugin.GetUserDataOrDefault(user);

            //Durée entre maintenant et la dernière utilisation
            var daysSinceLastUnspecializing = WorldTime.Day - playerData.LastUnspecializingDay;
            var duration = TimeSpan.FromDays(daysSinceLastUnspecializing);

            message = Localizer.Do($"La dernière fois que vous avez oublié une spécialité remonte à {TextLoc.BoldLocStr(TimeFormatter.FormatSpan(duration, Rounding.ShowTwoBiggest, false))}");
            user.Player.OkBoxLocStr(message);
        }

        [ChatSubCommand("UnSkill", "Consulter le délai", ChatAuthorizationLevel.Admin)]
        public static void CheckPlayer(User user, User targetUser = null)
        {
            var currentUser = targetUser != null ? targetUser : user;

            //Recuperation des donnees du joueur cible
            var plugin = PluginManager.GetPlugin<PlayersDataPlugin>();
            var playerData = plugin.GetUserDataOrDefault(currentUser);
            var lastUnspecializing = playerData.LastUnspecializingDay;
            var daysSinceLastUnspecializing = WorldTime.Day - lastUnspecializing;
            int day = (int)daysSinceLastUnspecializing;  //La partie entière = Jours
            int hour = (int)((daysSinceLastUnspecializing - day) * 24);  //La partie décimale x 24 = Heures

            string message;
            message = Localizer.Do($"Dernier abandon de spécialité du joueur {currentUser.Name}, il y a {day} jour(s) et {hour} heure(s).");
            user.Player.OkBoxLocStr(message);
        }

        [ChatSubCommand("UnSkill", "Afficher les prérequis pour oublier une spécialité", ChatAuthorizationLevel.User)]
        public static void Conditions(User user)
        {
            LocString header;
            LocString message;
            LocString button;
            
            header = Localizer.Do($"Abandon d'une spécialité");
            message = Localizer.Do($"Il faut remplir les conditions suivantes :");
            message += "\r\n";
            message += Localizer.Do($"(1) Avoir la spécialité au niveau maximum");
            message += "\r\n";
            message += Localizer.Do($"(2) Ne pas avoir lancé de fabrication avec cette spécialité");
            message += "\r\n";
            message += Localizer.Do($"(3) Ne pas avoir déjà abandonné une spécialité au cours des dernières 48h");
            message += "\r\n";
            message += "\r\n";
            message += Localizer.Do($"Enfin, il faut fabriquer le parchemin d'oubli correspondant à la spécialité voulue et l'utiliser.");
            message += "\r\n";
            message += Localizer.Do($"Notez qu'il se fabrique au niveau 7 depuis une table de recherche.");
            button = Localizer.Do($"J'ai compris !");

            user.Player.LargeInfoBox(header, message, button);
        }
    }
}
