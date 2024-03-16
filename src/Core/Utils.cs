using Eco.Core.Utils.Logging;
using Eco.Gameplay.Players;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Village.Eco.Mods.Core
{
    public static class Utils
    {
        // Permet de log la pile d'appels pour analyser et debuguer
        // Il suffit d'appeler LogWithStackTrace
        public static void LogWithStackTrace() 
        {
            var stack = Environment.StackTrace;
            var log = NLogManager.GetLogWriter("LeVillageMods");
            log.Write($"Pile d'appels {stack}");
        }
    }
    public class PlayerUtils
    {
        // Permet d'obtenir la liste de tous les joueurs du serveur
        // Exemple d'utlisation :
        // foreach (var user in PlayerUtils.Users)
        // {
        //     user.Mailbox.Add(mailMessage, !user.IsOnline);
        // }
        public static List<User> AllUsers => UserManager.Users.ToList();

        // Permet d'obtenir la liste des joueurs en ligne
        //public static List<User> OnlineUsers => UserManager.OnlineUsers.ToList(); //pb de package...
        public static List<User> OnlineUsers => UserManager.Users.Where(u => u.IsOnline).ToList();
        
        // Permet d'avoir une liste de joueurs épuisés
        public static List<User> ExhaustedUsers => UserManager.Users.Where(u => u.ExhaustionMonitor.IsExhausted).ToList();
    }
}
