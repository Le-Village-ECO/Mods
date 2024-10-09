// Le Village - Gestionnaire de messages
// Types et formats de message :
// 1- Message d'accueil à la 1ère connexion du joueur
// 2- Message d'avertissement pour prévenir tous les joueurs en ligne d'une relance serveur
// 3- Message d'information envoyé dans la boite au lettre de tous les joueurs
// TODO - Ajouter un décompte pour la relance serveur (voir Animal.cs tick() et Tickable.cs)

using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils.Logging;
using Eco.Gameplay.Players;
using Eco.Gameplay.Systems.Messaging.Mail;
using Eco.Gameplay.Systems.Messaging.Notifications;
using Eco.Shared.Localization;
using Eco.Shared.Utils;
using System;
using System.Collections.Concurrent;
using System.Globalization;

namespace Village.Eco.Mods.Core
{
    public class MessageManager : IModKitPlugin
    {
        #region Accueil
        public struct WelcomeMsg 
        {
            public Player Player;
            public User User;
            public string Title; 
            public string Message;
            public string Category;

            public WelcomeMsg(User user, string title, string message) 
            {
                Player = user.Player;
                User = user;
                Title = title;
                Message = message;
                Category = "Welcome";
            }

            public WelcomeMsg(Player player, string title, string message) 
            {
                Player = player;
                User = player.User;
                Title = title;
                Message = message;
                Category = "Welcome";
            }
        }
        public static bool Send(WelcomeMsg Content)
        {
            Content.Player.OpenInfoPanel(Content.Title, Content.Message, Content.Category);
            return true;
        }
        public static bool SendWelcomeMsg(User user, string title, string message) => Send(new WelcomeMsg(user, title, message));
        public static bool SendWelcomeMsg(Player player, string title, string message) => Send(new WelcomeMsg(player, title, message));
        #endregion

        #region Relance serveur
        // Ceci envoie une notification à tous les joueurs en ligne ainsi qu'un message serveur
        public static bool RebootWarning(string message)
        {
            try
            {
                foreach (var user in PlayerUtils.OnlineUsers)
                {
                    user.Player.MsgLocStr(Text.Warning(message));
                    user.Player.OkBoxLocStr(message);
                    NotificationManager.ServerMessageToAll(Localizer.DoStr(message));
                }
                return true;
            }
            catch (Exception error)
            {
                //Log
                var log = NLogManager.GetLogWriter("LeVillageMods");
                log.WriteError($"Failed to send RebootWarning to all users", error);
                return false;
            }
        }
        #endregion

        #region Mail
        public struct Mail
        {
            public string Content;
            public string Tag;
            public Mail(string content)
            {
                Content = content;
                Tag = "Notifications";
            }
        }
        public static bool Send(Mail Message)
        {
            try
            {
                var mailMessage = new MailMessage(Message.Content, Message.Tag);
                foreach (var user in PlayerUtils.AllUsers)
                {
                    user.Mailbox.Add(mailMessage, !user.IsOnline);
                }
                return true;
            }
            catch (Exception error)
            {
                //Log
                var log = NLogManager.GetLogWriter("LeVillageMods");
                log.WriteError($"Failed to send mail to all users", error);
                return false;
            }
        }

        public static bool SendMail(string content) => Send(new Mail(content));
        #endregion

        public string GetCategory() => "LeVillageMods";
        public override string ToString() => Localizer.DoStr("Message Manager");
        public string GetStatus() => "Active";
    }
}
