// Le Village

using Eco.Core;
using Eco.Core.Utils;
using Eco.Gameplay.Aliases;
using Eco.Gameplay.Players;
using Eco.Gameplay.Systems.Messaging.Chat.Commands;
using Eco.Shared.Localization;
using Eco.Shared.Services;
using System;
using Village.Eco.Mods.Core;

namespace Village.Eco.Mods.UnSkillScroll
{
    [ChatCommandHandler]
    public static class CommandsUnSkill
    {
        //Commandes Admin Core	
        [ChatCommand("Commandes pour Oublier une spécialité", ChatAuthorizationLevel.User)]
        public static void UnSkill(User user) { }

        //Sous-commande : Pouvoir remettre à 0 le timer d'un joueur pour unskill à nouveau
        [ChatSubCommand("UnSkill", "Remettre à 0 le délai", ChatAuthorizationLevel.Admin)]
        public static void Reset(User user, User targetUser, double timer = 0)
        {
            //Recuperation des donnees du joueur
            var plugin = PluginManager.GetPlugin<PlayersDataPlugin>();
            var playerData = plugin.GetPlayerDataOrDefault(targetUser.Player);
            var lastUnspecializing = playerData.LastUnspecializingDay;

            //Mise a jour des donnees du joueur
            playerData.LastUnspecializingDay = timer;
            plugin.AddOrSetPlayerData(targetUser.Player, playerData);

            string message;
            //Message de confirmation à l'admin
            message = Localizer.Do($"Le joueur {targetUser.Player} - Nouveau timer {lastUnspecializing}.");
            user.OkBoxLocStr(message);
            //Message de confirmation au joueur
            message = Localizer.Do($"Votre timer pour abandonné une spécialité a été remis à {lastUnspecializing}.");
            targetUser.MailLocStr(message, NotificationCategory.Skills);

        }

        //Sous-commande : Pouvoir consulter son propre timer pour unskill
        [ChatSubCommand("UnSkill", "Consulter le délai", ChatAuthorizationLevel.User)]
        public static void CheckTimer(User user)
        {
            //Recuperation des donnees du joueur
            var plugin = PluginManager.GetPlugin<PlayersDataPlugin>();
            var playerData = plugin.GetPlayerDataOrDefault(user.Player);
            var lastUnspecializing = playerData.LastUnspecializingDay;
            int day = (int)lastUnspecializing;  //La partie entière = Jours
            int hour = (int)((lastUnspecializing - day)*24);  //La partie décimale x 24 = Heures

            string message;
            message = Localizer.Do($"La dernière fois que vous avez abandonné une spécialité remonte à {day} jour(s) et {hour} heure(s).");
            user.Player.InfoBoxLocStr(message);
        }

        //Sous-commande : Pouvoir consulter le timer d'un joueur pour unskill
        [ChatSubCommand("UnSkill", "Consulter le délai", ChatAuthorizationLevel.Admin)]
        public static void CheckPlayer(User user, User targetUser = null)
        {
            var currentUser = targetUser != null ? targetUser : user;

            //Recuperation des donnees du joueur cible
            var plugin = PluginManager.GetPlugin<PlayersDataPlugin>();
            var playerData = plugin.GetPlayerDataOrDefault(currentUser.Player);
            var lastUnspecializing = playerData.LastUnspecializingDay;
            int day = (int)lastUnspecializing;  //La partie entière = Jours
            int hour = (int)((lastUnspecializing - day) * 24);  //La partie décimale x 24 = Heures

            //a tester :
            //var calc = TimeFormatter.FormatSpan(WorldTime.Seconds - user.LoginTime); 

            string message;
            message = Localizer.Do($"Dernier abandon de spécialité du joueur {currentUser.Player}, il y a {day} jour(s) et {hour} heure(s).");
            user.Player.InfoBoxLocStr(message);
        }

        //Sous-commande : Afficher les prérequis pour unskill
        [ChatSubCommand("UnSkill", "Afficher les prérequis pour oublier une spécialité", ChatAuthorizationLevel.User)]
        public static void Conditions(User user)
        {
            user.Player.Msg(Localizer.Format("C'est un test4"));

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
            message += Localizer.Do($"(3) Ne pas avoir déjà abandonné une spécialité au cours des 2 derniers jours");
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
