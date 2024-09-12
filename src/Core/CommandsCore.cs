// Le Village
// TODO : Pouvoir consulter toutes les valeurs d'un joueur
// TODO : Pouvoir modifier la valeur d'un paramètre d'un joueur
// TODO : Afficher toutes les config
// TODO : Afficher un message complet à partir d'un fichier déposé sur le serveur
// TODO syr le Fly : Sauvegarder le changement de l'activation sur le fichier de config

using Eco.Core;
using Eco.Core.Utils;
using Eco.Core.Utils.Logging;
using Eco.Gameplay.Aliases;
using Eco.Gameplay.EcopediaRoot;
using Eco.Gameplay.Minimap;
using Eco.Gameplay.Players;
using Eco.Gameplay.Systems.Chat;
using Eco.Gameplay.Systems.Messaging.Chat.Commands;
using Eco.ModKit;
using Eco.Shared.Localization;
using Eco.Gameplay.Objects;

namespace Village.Eco.Mods.Core
{
    [ChatCommandHandler]
    public static partial class CommandsCore
    {
        //Commandes Admin Core	
        [ChatCommand("Commandes admin du Village.", ChatAuthorizationLevel.Admin)]
        public static void LVCore() { }

        [ChatSubCommand("LVCore", "Make your character invisible", "Ghost", ChatAuthorizationLevel.Admin)]
        public static void Invisible(User user)
        {
            user.IsInvisible = !user.IsInvisible;
            //Portion de code inspirée de UserCommands.cs avec aussi MinimapManager.cs
            //Cela semble fonctionner !!
            if (user.IsInvisible) MinimapManager.Obj.DeltaHashSetObjects.Remove(user.Player.MinimapObject);
            else MinimapManager.Obj.DeltaHashSetObjects.Add(user.Player.MinimapObject);
        }

        //Sous-commande : Forcer la sauvegarde du fichier PlayersData sur le serveur
        [ChatSubCommand("LVCore", "Forcer sauvegarde de PlayersData.eco", ChatAuthorizationLevel.Admin)]
        public static void Force(User user) 
        {
            var plugin = PluginManager.GetPlugin<PlayersDataPlugin>();
            plugin.ShutdownAsync();
            user.Player.Msg(Localizer.Format("Le fichier PlayersData.eco est normalement sauvé sur le serveur"));
            
			//Log
			var log= NLogManager.GetLogWriter("LeVillageMods");
			log.Write($"Admin {user.Name} force sauvegarde de PlayerData.eco");
		}

		//Sous-commande : Consulter les valeurs d'un joueur dans PlayersData
		[ChatSubCommand("LVCore", "Consulter PlayersData - TODO", ChatAuthorizationLevel.Admin)]
        public static void Consult(User user)
        {
            user.Player.Msg(Localizer.Format("Pas encore actif, en cours de dev..."));
        }
        
        //Sous-commande : Modifier une valeur d'un joueur dans PlayersData
        [ChatSubCommand("LVCore", "Modifier PlayersData - TODO", ChatAuthorizationLevel.Admin)]
        public static void Change(User user)
        {
            user.Player.Msg(Localizer.Format("Pas encore actif, en cours de dev..."));
        }

        //Sous-commande : Récupérer l'ID d'un joueur
        [ChatSubCommand("LVCore", "Récupérer l'ID d'un joueur", ChatAuthorizationLevel.Admin)]
        public static void PlayerID(User user, User targetUser)
        {
            string playerID = targetUser.SteamId?.IfEmpty(targetUser.StrangeId) ?? targetUser.StrangeId;
            user.Player.Msg(Localizer.Format($"Joueur {targetUser}  = ID {playerID}"));

        }

        //Sous-commande : Afficher les configurations
        [ChatSubCommand("LVCore", "Afficher les configurations", ChatAuthorizationLevel.Admin)]
        public static void Config(User user)
        {
            user.Player.MsgLoc($"Configuration **SkillTierCost** : {LVConfigurePlugin.Config.SkillTierCost}");
        }

        //Sous-commande : Annoncer un reboot serveur
        [ChatSubCommand("LVCore", "Reboot", ChatAuthorizationLevel.Admin)]
        public static void Reboot(User user, int timer = 5)
        {
            MessageManager.RebootWarning($"Relance du serveur dans {timer} min(s) !");
            user.Player.MsgLocStr("L'annonce du reboot serveur a été envoyé.");
        }

        //Sous-commande : Envoi d'un message mail type notification à tous les joueurs
        [ChatSubCommand("LVCore", "Mail", ChatAuthorizationLevel.Admin)]
        public static void Mail(User user, string content)
        {
            MessageManager.SendMail(content);
            user.Player.MsgLocStr($"Le mail suivant été envoyé aux joueurs : {content}");
        }

        //Sous-commande : Tests sur les types d'affichage de messages
        [ChatSubCommand("LVCore", "Test Info Panel, en cours de dev...", ChatAuthorizationLevel.Admin)]
        public static void WelcomeMsg(User user, string title, string content)
        {
            //user.Player.OpenInfoPanel("titre", "contenu", "Test");
            MessageManager.SendWelcomeMsg(user, title, content);
        }

        //Pour faciliter les tests Unity - TODO - A tester
        [ChatSubCommand("Reloads the Unity Data Files without needing to reboot the server", "rl-unity", ChatAuthorizationLevel.Admin)]
        public static void ReloadUnityData(User user, IChatClient chatClient)
        {
            ModKitPlugin.ContentSync.RefreshContent();

            chatClient.MsgLocStr("Unity Files Refreshed, Please Re-log to get the new changes.");
        }

        //Pour faciliter les tests ECOPEDIA - TODO - A tester
        [ChatSubCommand("Rebuilds the ecopedia", "rl-ecopedia", ChatAuthorizationLevel.Admin)]
        public static void RebuildEcopedia(User user, IChatClient chatClient)
        {
            EcopediaManager.Build(ModKitPlugin.ModDirectory);
            chatClient.MsgLocStr("The Ecopedia Has been Rebuilt and should be automatically update. Please check the console for any logged issues with rebuilding the ecopedia");
        }

        #region Fly pour tous
        [ChatSubCommand("LVCore","Fly pour tous", ChatAuthorizationLevel.User)]
        public static void UserFly(User user)
        {
            if (LVConfigurePlugin.Config.AllowFlyAll)
                user.Player.ClientRPC("ToggleFly");
            else
                user.Player.ErrorLocStr("Vous n'avez pas l'autorisation pour faire cette commande.");
        }

        [ChatSubCommand("LVCore", "Activer/Désactiver Fly pour tous", ChatAuthorizationLevel.Admin)]
        public static void SwitchUserFly(User user)
        {
            LVConfigurePlugin.Config.AllowFlyAll = !LVConfigurePlugin.Config.AllowFlyAll;
            LVConfigurePlugin.Save();
            user.Player.OkBoxLocStr($"L'activation de UserFly est à : {LVConfigurePlugin.Config.AllowFlyAll}");
        }

        [ChatSubCommand("LVCore", "Vérifier statut Fly pour tous", ChatAuthorizationLevel.User)]
        public static void CheckUserFly(User user)
        {
            user.Player.OkBoxLocStr($"L'activation de UserFly est à : {LVConfigurePlugin.Config.AllowFlyAll}");
        }
        #endregion
    }
}
