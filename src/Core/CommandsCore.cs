// Le Village
// TODO : Pouvoir consulter toutes les valeurs d'un joueur
// TODO : Pouvoir modifier la valeur d'un paramètre d'un joueur
// TODO : Commande invisible, ajouter la minimap

using Eco.Core;
using Eco.Core.Utils;
using Eco.Core.Utils.Logging;
using Eco.Gameplay.Minimap;
using Eco.Gameplay.Players;
using Eco.Gameplay.Systems.Messaging.Chat.Commands;
using Eco.Shared.Localization;

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
            if (user.IsInvisible) MinimapManager.Obj.Objects.Remove(user.Player.MinimapObject);
            else MinimapManager.Obj.Objects.Add(user.Player.MinimapObject);
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
            string playerID = targetUser.SteamId?.IfEmpty(targetUser.SlgId) ?? targetUser.SlgId;
            user.Player.Msg(Localizer.Format($"Joueur {targetUser}  = ID {playerID}"));

        }

        //Sous-commande : Afficher les configurations
        [ChatSubCommand("LVCore", "Afficher les configurations", ChatAuthorizationLevel.Admin)]
        public static void Config(User user)
        {
            user.Player.MsgLoc($"Configuration **SkillTierCost** : {LVConfigurePlugin.Config.SkillTierCost}");
        }
    }

}
