// Le Village

namespace Eco.Mods.TechTree
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Skills;
    using Eco.Gameplay.Systems.Messaging.Notifications;
    using Eco.Gameplay.Systems.TextLinks;
    using Eco.Shared.Localization;
    using Eco.Shared.Serialization;
    using Eco.Shared.Services;

    [Serialized]
    [LocDisplayName("UnSkill Scroll")]
    public abstract class UnSkillScroll : Item  //Item generique avec lien vers Unity3D
    {
        public abstract Type SkillType { get; }  //Recuperation de la specialite definie dans le parchemin (1 pour chaque spe.)

        public override string OnUsed(Player player, ItemStack itemStack)
        {
            var skill = player.User.Skillset.GetSkill(SkillType);
            Task.Run(async () => await player.User.ConfirmBoxLoc($"Etes-vous sûr de vouloir abandonner {skill.UILink()} ?"))
                .ContinueWith(t => { if (t.Result == true) OnConfirmBoxOk(player, itemStack, skill); });

            return base.OnUsed(player, itemStack);
        }

        public void OnConfirmBoxOk(Player player, ItemStack itemStack, Skill skill)
        {
            string message;
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

            player.User.Skillset.Reset(SkillType, false);
            player.User.UserXP.AddStars(skill.Tier);

            //  Supprimer le parchemin apres utilisation avec succes
            var inventory = new Inventory[] { player.User.Inventory, itemStack.Parent }.Distinct();
            using (var changes = InventoryChangeSet.New(inventory, player.User))
            {
                changes.ModifyStack(itemStack, -1);
                changes.Apply();
            }

            player.User.MailLoc($"Vous avez abandonné {skill.UILink()}", NotificationCategory.Skills);
            player.User.MailLoc($"Vous avez récupéré {skill.Tier} étoile(s)", NotificationCategory.Skills);
            NotificationManager.ServerMessageToAll(Localizer.Do($"{player.User.UILink()} a abandonné {skill.UILink()}"), NotificationCategory.Skills);
        }
    }

    public abstract class UnSkillScroll<TSkill> : UnSkillScroll where TSkill : Skill, new()
    {
        public override Type SkillType => typeof(TSkill);
    }
}