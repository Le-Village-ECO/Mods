// Le Village
// TODO : Pouvoir remettre à 0 le timer d'un joueur pour unskill à nouveau (admin)
// TODO : Pouvoir consulter son timer pour unskill (player)
// TODO : Afficher les prérequis pour unskill (player)

using Eco.Gameplay.Players;
using Eco.Gameplay.Systems.Messaging.Chat.Commands;
using Eco.Shared.Localization;

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
        public static void Reset(User user)
        {
            user.Player.Msg(Localizer.Format("C'est un test1"));
        }

        //Sous-commande : Pouvoir consulter son timer pour unskill
        [ChatSubCommand("UnSkill", "Consulter le délai", ChatAuthorizationLevel.User)]
        public static void CheckTimer(User user)
        {
            user.Player.Msg(Localizer.Format("C'est un test2"));
        }

        //Sous-commande : Afficher les prérequis pour unskill
        [ChatSubCommand("UnSkill", "Afficher les prérequis pour oublier une spécialité", ChatAuthorizationLevel.User)]
        public static void Prerequisites(User user)
        {
            user.Player.Msg(Localizer.Format("C'est un test3"));
        }
    }
}
