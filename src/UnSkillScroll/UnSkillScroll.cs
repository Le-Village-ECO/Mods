// Le Village
// Ce parchemin permet, une fois utilisé, d'oublier une spécialité à condition de :
// 1- Ne plus avoir d'ordre de fabrication en cours dessus
// 2- Etre au niveau maximum de la spécialité (généralement 7)
// 3- Ne pas avoir déjà oublié une spécialité par un parchemin au cours des 2 derniers jours

using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Eco.Core;
using Eco.Core.Controller;
using Eco.Core.Items;
using Eco.Core.Utils;
using Eco.Core.Utils.Logging;
using Eco.Gameplay.Items;
using Eco.Gameplay.GameActions;
using Eco.Gameplay.Players;
using Eco.Gameplay.Skills;
using Eco.Gameplay.Systems.TextLinks;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using Eco.Shared.Services;
using Eco.Shared.Utils;
using Eco.Simulation.Time;
using Village.Eco.Mods.Core;

namespace Village.Eco.Mods.UnSkillScroll
{
    [Serialized]
    [Category("Hidden/Research")]
    [ItemGroup("Skill Scrolls")]
    [Tag("Skill Scrolls")]
    public abstract class UnSkillScroll : Item  //Item generique avec lien vers Unity3D
    {
        //Event d'oubli de la spécialité
        public static ThreadSafeAction<Player, Skill> UnlearnSkillEvent = new();

        public const double RefundSpecialtyDaysCooldown = 2; //Delai entre 2 utilisation de parchemin d'oubli
        public abstract Type SkillType { get; }  //Recuperation de la specialite definie dans le parchemin (1 pour chaque spe.)

        public override string OnUsed(Player player, ItemStack itemStack)
        {
            // Execute la Task en parallele afin de terminer OnUsed
            Task.Run(() => OnUsedAsync(player, itemStack));

            return base.OnUsed(player, itemStack);
        }

        public async Task OnUsedAsync(Player player, ItemStack itemStack)
        {
            string message;
            var skill = player.User.Skillset.GetSkill(SkillType);
            if (skill == null) //Le joueur n'a pas la specialite du parchemin
            {
                message = Localizer.Do($"Vous n'avez pas cette spécialité.");
                player.ErrorLocStr(message);

                return;
            }

            if (skill.Level != skill.MaxLevel) //Le joueur n'est pas au niveau maximum de la specialite
            {
                message = Localizer.Do($"Vous devez avoir le niveau maximum de {skill.UILink()}.");
                player.ErrorLocStr(message);

                return;
            }

            //Recuperation des ordres de travails contenant la specialite
            var hasWorkOrders = player.User.GetWatchedWorkOrders.Any(workOrder =>
                workOrder.Recipe!.RequiredSkills.Any(a => a.SkillType == SkillType));
            //Il ne faut aucun ordre en cours, quelque soit leur statut, demandant la specialite
            if (hasWorkOrders)
            {
                message = Localizer.Do($"Impossible d'oublier {skill.UILink()} tant que des fabrications l'utilisant sont en cours.");
                player.ErrorLocStr(message);

                return;
            }

            //Recuperation des donnees du joueur
            var plugin = PluginManager.GetPlugin<PlayersDataPlugin>();
            var playerData = plugin.GetPlayerDataOrDefault(player);

            var daysSinceLastUnspecializing = WorldTime.Day - playerData.LastUnspecializingDay;
            //Il faut attendre le delai entre 2 oublis de specialite
            if (playerData.LastUnspecializingDay > 0 && daysSinceLastUnspecializing < RefundSpecialtyDaysCooldown)
            {
                var timeUntilUnspecializing = RefundSpecialtyDaysCooldown - daysSinceLastUnspecializing;
                var coolDownDuration = TimeSpan.FromDays(timeUntilUnspecializing);

                //TODO : Revoir le format d'affichage du temps d'attente
                //Voir UserCommands.cs : public static void Now
                message = Localizer.Do($"Vous devez attendre {TimeFormatter.FormatSimple(coolDownDuration)} avant d'oublier {skill.UILink()}.");
                player.ErrorLocStr(message);

                return;
            }

            //Une confirmation finale du joueur est indispensable et obligatoire
            if (await player.User.ConfirmBoxLoc($"Etes-vous sûr de vouloir abandonner {skill.UILink()} ?") is false) return;

            //Récupération des étoiles
            int stars = LVConfigurePlugin.Config.SkillTierCost ? skill.Tier : 1;

            ResetStar(player.User, stars);
            //await player.User.Skillset.Reset(SkillType, false);
            player.User.MailLoc($"You have abandoned {skill.UILink()} specialty", NotificationCategory.Skills);

            player.User.UserXP.AddStars(stars);
            player.User.UserXP.Changed(nameof(UserXP.StarsAvailable));
            player.User.MailLoc($"Vous avez récupéré {stars} étoile(s)", NotificationCategory.Skills);

            //TODO en cours de Warang sur les events
            // Event lié à l'oubli de la spécialité
            UnlearnSkillEvent?.Invoke(player, skill);

            //Mise a jour des donnees du joueur
            playerData.LastUnspecializingDay = WorldTime.Day;
            plugin.AddOrSetPlayerData(player, playerData);

            //  Supprimer le parchemin apres utilisation avec succes
            var inventory = new Inventory[] { player.User.Inventory, itemStack.Parent }.Distinct();
            using (var changes = InventoryChangeSet.New(inventory, player.User))
            {
                changes.ModifyStack(itemStack, -1);
                changes.Apply();
            }

            //Log
            var log = NLogManager.GetLogWriter("LeVillageMods");
            log.Write($"Le joueur **{player.DisplayName}** a oublié **{skill.DisplayName}**.");
        }

        public void ResetStar(User user, int stars)
        {
            //Log
            var log = NLogManager.GetLogWriter("LeVillageMods");
            log.Write($"ResetStar appelé pour l'utilisateur {user.Name}");

            var skill = user.Skillset[SkillType];

            var pack = new GameActionPack();
            pack.AddGameAction(new LoseSpecialty()
            {
                Profession = skill.RootSkillTree.StaticSkill,
                Specialty = skill,
                Citizen = user,
                StarsRefunded = stars
            });
            pack.TryPerform(user);

            skill.ForceSetLevel(user, 0);
            if (skill.Talents != null) skill.ResetTalents(user);
            user.Skillset.UnLearnSkillInSkillset(skill.Type, user);
        }
    }

    public abstract class UnSkillScroll<TSkill> : UnSkillScroll where TSkill : Skill, new()
    {
        public override Type SkillType => typeof(TSkill);
    }
}