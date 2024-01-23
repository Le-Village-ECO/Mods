// Le Village

namespace Eco.Mods.TechTree
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Eco.Core.Utils;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Skills;
    using Eco.ModKit.Internal;
    using Eco.Shared.Items;
    using Eco.Shared.Localization;
    using Eco.Shared.Serialization;

    [Serialized]
    [LocDisplayName("UnSkill Scroll")]
    public abstract class UnSkillScroll : Item  //Item generique avec lien vers Unity3D
    {
        public abstract Type SkillType { get; }  //Recuperation de la specialite definie dans le parchemin (1 pour chaque spe.)

        public override string OnUsed(Player player, ItemStack itemStack)
        {
            Task.Run(async () => await player.User.ConfirmBoxLoc($"Test ConfirmBoxLoc"))
                .ContinueWith(t => { if (t.Result == true) OnConfirmBoxOk(player); });

            return base.OnUsed(player, itemStack);
        }

        public void OnConfirmBoxOk(Player player)
        {
            var skill = player.User.Skillset[SkillType];  //Infos du SkillType du joueur
            if (skill.Level == skill.MaxLevel)  //Si le niveau de la spécialisation est égale au niveau maximum
            {
                player.User.Skillset.Reset(SkillType, true);
                player.User.UserXP.AddStars(skill.Tier);

                string message;
                message = Localizer.Do($"Vous avez oublié {skill.MarkedUpName}.");
                message += "\r\n";
                message += Localizer.Do($"Vous avez récupéré {skill.Tier} étoile(s).");
                player.OkBoxLocStr(message);
            }
            else
            {
                string message;
                message = Localizer.Do($"Vous devez avoir le niveau maximum de {skill.MarkedUpName}.");
                player.ErrorLocStr(message);
            }

            //pack.PreTests.Add(() => player.User.GetWatchedWorkOrders.Any(workOrder => workOrder.Recipe.SkillsNeeded().Contains(skill.Type)) ? Result.FailLoc($"Cannot unspecialize while haveing a work order in progress that is using that specialization") : Result.Succeeded);

            //Voir WorkPartyManager.cs 
            //var orders = player.User?.GetWatchedWorkOrders.Where(x => (x.ResourcePercentage < 1f || x.LaborPercentage < 1f) && (x.WorkParty == null || x.WorkParty.State > ProposableState.Active))
            //        .OrderByDescending(x => x.CreationTime).Take(2)
            //        .ToList();
            //if (!orders.Any()) { player?.OkBoxLoc($"Trouve pas 1 !"); return null; }

            // var orders2 = player.User?.GetWatchedWorkOrders
            //     .Where(x => x.WorkParty == null && x.Recipe!.SkillsNeeded().Contains(SkillType))
            //     //.OrderByDescending(x=>x.CreationTime).Take(2)
            //     .ToList();
            // if (!orders2.Any())
            // {
            //     player?.OkBoxLoc($"Encore un WO sur {SkillType} / {skill.Type} / {player.User} !!!!!!");
            //     //return null;
            // }
            // else
            // {
            //     player?.OkBoxLoc($"Aucun WO");
            // }

            //  Supprimer le parchemin apres utilisation avec succes
            //            using (var changes = InventoryChangeSet.New(new Inventory[] { user.Inventory, itemStack.Parent }.Distinct(), user))
            //            {
            //                changes.ModifyStack(itemStack, -1);
            //                var descriptionInventoryChanges = changes.DescribeWhatItAdds();
            //            }
        }
    }

    public abstract class UnSkillScroll<TSkill> : UnSkillScroll where TSkill : Skill, new()
    {
        public override Type SkillType => typeof(TSkill);
    }
}