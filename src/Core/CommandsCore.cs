// Le Village
// TODO : Pouvoir consulter toutes les valeurs d'un joueur
// TODO : Pouvoir modifier la valeur d'un paramètre d'un joueur
// TODO : Commande invisible, ajouter la minimap

using Eco.Core;
using Eco.Core.Utils;
using Eco.Gameplay.Minimap;
using Eco.Gameplay.Players;
using Eco.Gameplay.Systems.Messaging.Chat.Commands;
using Eco.Shared.Localization;

namespace Village.Eco.Mods.Core
{
    [ChatCommandHandler]
    public static class CommandsCore
    {
        //Commandes Admin Core	
        [ChatCommand("Commandes admin du Village.", ChatAuthorizationLevel.Admin)]
        public static void LVCore(User user) { }

        [ChatSubCommand("LVCore", "Make your character invisible", ChatAuthorizationLevel.Admin)]
        public static void Invisible(User user)
        {
            user.IsInvisible = !user.IsInvisible;
            //Voir UserCommands.cs
            //minimap todo: update visibility status
            //if (user.IsInvisible) MinimapManager.RemoveMinimapObject(user.Player);
            //else                  MinimapManager.AddOrUpdateMinimapObject(user.Player);
        }

        //Sous-commande : Forcer la sauvegarde du fichier PlayersData sur le serveur
        [ChatSubCommand("LVCore", "Forcer sauvegarde de PlayersData", ChatAuthorizationLevel.Admin)]
        public static void Force(User user) 
        {
            var plugin = PluginManager.GetPlugin<PlayersDataPlugin>();
            plugin.ShutdownAsync();
            user.Player.Msg(Localizer.Format("Le fichier PlayersData.eco est normalement sauvé sur le serveur"));
        }

        //Sous-commande : Consulter les valeurs d'un joueur dans PlayersData
        [ChatSubCommand("LVCore", "Consulter PlayersData", ChatAuthorizationLevel.Admin)]
        public static void Consult(User user)
        {
            user.Player.Msg(Localizer.Format("Pas encore actif, en cours de dev..."));
        }
        
        //Sous-commande : Modifier une valeur d'un joueur dans PlayersData
        [ChatSubCommand("LVCore", "Modifier PlayersData", ChatAuthorizationLevel.Admin)]
        public static void Change(User user)
        {
            user.Player.Msg(Localizer.Format("Pas encore actif, en cours de dev..."));
        }

        //Sous-commande : Modifier une valeur d'un joueur dans PlayersData
        [ChatSubCommand("LVCore", "Récupérer l'ID d'un joueur", ChatAuthorizationLevel.Admin)]
        public static void PlayerID(User user, User targetUser)
        {
            string playerID = targetUser.SteamId?.IfEmpty(targetUser.SlgId) ?? targetUser.SlgId;
            user.Player.Msg(Localizer.Format($"Joueur {targetUser}  = ID {playerID}"));

        }
    }

}
