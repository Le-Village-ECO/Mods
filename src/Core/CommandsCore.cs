// Le Village
// TODO : Forcer la sauvegarde du fichier PlayersData (admin)
// TODO : Pouvoir consulter toutes les valeurs d'un joueur (admin)
// TODO : Pouvoir modifier la valeur d'un paramètre d'un joueur (admin)

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

        //Sous-commande : Forcer la sauvegarde du fichier PlayersData sur le serveur
        [ChatSubCommand("LVCore", "Forcer sauvegarde de PlayersData", ChatAuthorizationLevel.Admin)]
        public static void Force(User user) 
        {
            user.Player.Msg(Localizer.Format("C'est un test1"));
        }
        
        //Sous-commande : Consulter les valeurs d'un joueur dans PlayersData
        [ChatSubCommand("LVCore", "Consulter PlayersData", ChatAuthorizationLevel.Admin)]
        public static void Consult(User user)
        {
            user.Player.Msg(Localizer.Format("C'est un test2"));
        }
        
        //Sous-commande : Modifier une valeur d'un joueur dans PlayersData
        [ChatSubCommand("LVCore", "Modifier PlayersData", ChatAuthorizationLevel.Admin)]
        public static void Change(User user)
        {
            user.Player.Msg(Localizer.Format("C'est un test3"));
        }
    }

}
