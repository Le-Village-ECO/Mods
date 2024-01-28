// Le Village

namespace Eco.Mods
{
	using System;
    using System.Linq;
    using Eco.Core.Plugins.Interfaces;
    using Eco.Core.Tests;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Players;
	using Eco.Gameplay.Skills;	
    using Eco.Gameplay.Systems.Messaging.Chat.Commands;
    using Eco.Gameplay.Systems.Messaging.Notifications;
    using Eco.Gameplay.Systems.TextLinks;
    using Eco.Gameplay.Utils;
    using Eco.Shared.Localization;
    using Eco.Shared.Services;
    using Eco.Shared.Utils;
    using Eco.Gameplay.Components;

    [ChatCommandHandler]
    public static class LeVillageCommands
    {
		
        public static Type? SkillTypeByName(User user, string skillName)
        {
            var existingSkill = CommandsUtil.ClosestMatchingEntity(user, skillName.Trim(), SkillTree.AllSkillTrees, x => x.StaticSkill.Name, x => x.StaticSkill.DisplayName);
            return existingSkill != null ? existingSkill.StaticSkill.Type : null;
        }

//Commande générale du chat		
        [ChatCommand("Les commandes du Village.", ChatAuthorizationLevel.User)]
        public static void LeVillage(User user) { }

//Sous-commande pour des tests (admin)
        [ChatSubCommand("LeVillage", "Affichage du test", ChatAuthorizationLevel.Admin)]
        public static void Test(User user) => user.Player.Msg(Localizer.Format("C'est un test"));

//Sous-commande pour des tests2 (admin)
        [ChatSubCommand("LeVillage", "Affichage du test 2", ChatAuthorizationLevel.Admin)]
        public static void Test2(User user)
        {
            //var selection = user.OptionBox("test","oui,non");
            //player.InfoBoxLoc($"test");
        }

//Sous-commande pour ajouter tous les kits en main (admin)
        [ChatSubCommand("LeVillage", "Kits", ChatAuthorizationLevel.Admin)]
        public static void Kits(User user)
        {
            //var selection = user.OptionBox("test","oui,non");
            //player.InfoBoxLoc($"test");
        }

        //Sous-commande pour l'abandon de spécialité (user)
        [ChatSubCommand("LeVillage", "Enlever une spécialité", "Oublier", ChatAuthorizationLevel.User)]
        public static void Oublier(User user, string specialtyName)
        {
            var skillType = SkillTypeByName(user, specialtyName);     //Convertir le nom de la spécialité de la commande
            if (skillType == null) return;  //Vérifier que le nom existe
			var skill = user.Skillset[skillType];                     //Récupération du nom technique de la spécialité
//            int stars = 1;                                            //On ne rend qu'une seule étoile
            int stars = skill.Tier;                                   //On rend autant d'étoiles que le tier de la spécialité

            if (skill.Level == skill.MaxLevel)  //Si le niveau de la spécialisation est égale au niveau maximum
            {
                user.Skillset.Reset(skillType);
                user.UserXP.AddStars(stars);

                string message;
                message = Localizer.Do($"Vous avez oublié {specialtyName}.");
                message += "\r\n";
                message += Localizer.Do($"Vous avez récupéré {stars} étoile(s).");

                user.Player.InfoBoxLocStr(message);
				NotificationManager.ServerMessageToPlayerLocStr(message, user);
            }
            else //Notifier le joueur que le niveau n'est pas au maximum
            {
                string message;
				message = Localizer.Do($"Vous devez avoir le niveau maximum de {specialtyName}.");
				
				user.Player.InfoBoxLocStr(message);
				NotificationManager.ServerMessageToPlayerLocStr(message, user);
            }
        }	
	}
}
