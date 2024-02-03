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
using Eco.Core.Items;
using Eco.Gameplay.Items;
using Eco.Gameplay.Players;
using Eco.Gameplay.Skills;
using Eco.Gameplay.Systems.TextLinks;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using Eco.Shared.Services;
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
                message = Localizer.Do($"Impossible d'oublier {skill.UILink()} tant que des tâches l'utilisant sont en cours.");
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
                message = Localizer.Do($"Vous devez attendre {timeUntilUnspecializing} jours avant d'oublier {skill.UILink()}.");
                player.ErrorLocStr(message);

                return;
            }

            //Une confirmation finale du joueur est indispensable et obligatoire
            if (await player.User.ConfirmBoxLoc($"Etes-vous sûr de vouloir abandonner {skill.UILink()} ?") is false) return;

            //Oubli de la specialite + recuperation de(s) etoile(s)
            await player.User.Skillset.Reset(SkillType, false);
            player.User.UserXP.AddStars(skill.Tier);
            player.User.MailLoc($"Vous avez récupéré {skill.Tier} étoile(s)", NotificationCategory.Skills);

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
        }
    }

    public abstract class UnSkillScroll<TSkill> : UnSkillScroll where TSkill : Skill, new()
    {
        public override Type SkillType => typeof(TSkill);
    }
}