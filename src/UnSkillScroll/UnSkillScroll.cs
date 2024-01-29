// Le Village

using System;
using System.Linq;
using System.Threading.Tasks;
using Eco.Core;
using Eco.Gameplay.Items;
using Eco.Gameplay.Players;
using Eco.Gameplay.Skills;
using Eco.Gameplay.Systems.TextLinks;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using Eco.Shared.Services;
using Eco.Simulation.Time;

namespace Village.Eco.Mods.UnSkillScroll
{
    [Serialized]
    public abstract class UnSkillScroll : Item  //Item generique avec lien vers Unity3D
    {
        public const double RefundSpecialtyDaysCooldown = 2;
        public abstract Type SkillType { get; }  //Recuperation de la specialite definie dans le parchemin (1 pour chaque spe.)

        public override string OnUsed(Player player, ItemStack itemStack)
        {
            Task.Run(() => OnUsedAsync(player, itemStack));

            return base.OnUsed(player, itemStack);
        }

        public async Task OnUsedAsync(Player player, ItemStack itemStack)
        {
            string message;
            var skill = player.User.Skillset.GetSkill(SkillType);
            if (skill == null)
            {
                message = Localizer.Do($"Vous n'avez pas cette spécialité.");
                player.ErrorLocStr(message);

                return;
            }

            if (skill.Level != skill.MaxLevel)
            {
                message = Localizer.Do($"Vous devez avoir le niveau maximum de {skill.UILink()}.");
                player.ErrorLocStr(message);

                return;
            }

            var hasWorkOrders = player.User.GetWatchedWorkOrders.Any(workOrder =>
                workOrder.Recipe!.RequiredSkills.Any(a => a.SkillType == SkillType));
            if (hasWorkOrders)
            {
                message = Localizer.Do($"Impossible d'oublier {skill.UILink()} tant que des tâches l'utilisant sont en cours.");
                player.ErrorLocStr(message);

                return;
            }

            var plugin = PluginManager.GetPlugin<PlayersDataPlugin>();
            var playerData = plugin.GetPlayerDataOrDefault(player);

            var daysSinceLastUnspecializing = WorldTime.Day - playerData.LastUnspecializingDay;
            if (playerData.LastUnspecializingDay > 0 && daysSinceLastUnspecializing < RefundSpecialtyDaysCooldown)
            {
                var timeUntilUnspecializing = RefundSpecialtyDaysCooldown - daysSinceLastUnspecializing;
                message = Localizer.Do($"Vous devez attendre {timeUntilUnspecializing} jours avant d'oublier {skill.UILink()}.");
                player.ErrorLocStr(message);

                return;
            }

            if (await player.User.ConfirmBoxLoc($"Etes-vous sûr de vouloir abandonner {skill.UILink()} ?") is false) return;

            await player.User.Skillset.Reset(SkillType, false);
            player.User.UserXP.AddStars(skill.Tier);
            player.User.MailLoc($"Vous avez récupéré {skill.Tier} étoile(s)", NotificationCategory.Skills);

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