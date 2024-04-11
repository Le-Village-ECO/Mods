// Le village - Ensemble de commandes Admin pour faire des tests.
// Les commandes pour les joueurs sont dans DietPlugin.cs

using Eco.Gameplay.DynamicValues;
using Eco.Gameplay.Items;
using Eco.Gameplay.Players;
using Eco.Gameplay.Skills;
using Eco.Gameplay.Systems.Messaging.Chat.Commands;
using Eco.Gameplay.Utils;
using Eco.Shared.Localization;
using Eco.Shared.Utils;
using System;
using System.Linq;
using Village.Eco.Mods.Core;

namespace Village.Eco.Mods.Nutrition
{
    [ChatCommandHandler]
    public static partial class DietCommands
    {
        [ChatCommand("LVDiet", ChatAuthorizationLevel.Admin)]
        public static void LVDiet() { }

        [ChatSubCommand("LVDiet", "test1", ChatAuthorizationLevel.Admin)]
        public static void Test1(User user)
        {
            foreach (var groupFoodTaste in user.Stomach.TasteBuds.FoodToTaste.Where(x => x.Value.Discovered).GroupBy(x => x.Value.Preference))
            {
                user.Player.Msg(Localizer.Format($"Preference : {groupFoodTaste.Key.GetEnumLocDisplayName()}"));
                foreach (var foodTaste in groupFoodTaste)
                {
                    user.Player.Msg(Localizer.Format($"{foodTaste.Key.GetLocDisplayNameColored()}"));
                }
            }
        }

        [ChatSubCommand("LVDiet", "test2", ChatAuthorizationLevel.Admin)]
        public static void Test2(User user, string skillName, int level)
        {
            var skillType = SkillCommands.SkillTypeByName(user, skillName);
            var skill = user.Skillset[skillType];

            skill.ForceSetLevel(user, level);
            user.Skillset.RefreshSkills();

            user.Player.Msg(Localizer.Format($"coucou"));
        }

        [ChatSubCommand("LVDiet", "test3", ChatAuthorizationLevel.Admin)]
        public static void Test3(User user)
        {
            var toto = SkillModifiedValueManager.GetBenefitsFor(new DietSkill());
            foreach (var skillBenefits in toto)
            {
                user.Player.Msg(Localizer.Format($"Skill benefits : {skillBenefits.Key.Name}"));
                user.Player.Msg(Localizer.Format($"Nb skill benefits : {skillBenefits.Value.Count}"));
            }
        }

        [ChatSubCommand("LVDiet", "test4", ChatAuthorizationLevel.Admin)]
        public static void Test4(User user, string foodName)
        {
            var foodItem = CommandsUtil.ClosestMatchingItem<FoodItem>(user, foodName);
            string test = user.Stomach.TasteBuds.GetFoodTaste(foodItem);
            user.Player.Msg(Localizer.Format($"{foodName} = {foodItem} = {test}"));
        }

        [ChatSubCommand("LVDiet", "test5", ChatAuthorizationLevel.Admin)]
        public static void Test5(User user)
        {
            //Récupération de la configuration
            var tiers = LVConfigurePlugin.Config.DietTiers;
            float gap = LVConfigurePlugin.Config.DietTiersGap;

            var skill = user.Skillset.GetSkill(typeof(DietSkill));
            var skillRate = user.Stomach.NutrientSkillRate();
            int stars = user.UserXP.TotalStarsEarned;
            var palier = tiers[stars]; //Récupère la valeur palier de SkillRate en fonction du nombre d'étoile

            user.Player.MsgLocStr($"Niveau de **{skill.Name}** : **{skill.Level}**");
            user.Player.MsgLocStr($"Skill Rate : **{skillRate}**");
            user.Player.MsgLocStr($"Total stars : **{stars}**");
            user.Player.MsgLocStr($"Palier : **{palier}**");

            if (skillRate >= palier)
            {
                user.Player.MsgLocStr($"skillRate >= Palier");
            }
            else
            {
                var delta = (palier - skillRate) / palier * 100;
                var multiple = delta / gap;
                int arrondi = Convert.ToInt32(Math.Ceiling(multiple));
                int resultat = (skill.MaxLevel - arrondi < 1) ? 1 : (skill.MaxLevel - arrondi);

                user.Player.MsgLocStr($"skillRate < Palier");
                user.Player.MsgLocStr($"Delta = {delta} / multiple = {multiple} / arrondi = {arrondi} / resultat = {resultat}");
            }
        }
        [ChatSubCommand("LVDiet", "test6 - Skill count et Stars earned", ChatAuthorizationLevel.Admin)]
        public static void Test6(User user)
        {
            int totStars = user.Skillset.Skills.Count();
            int stars = user.UserXP.TotalStarsEarned;
            user.Player.MsgLocStr($"Skill count **{totStars}** / Stars earned **{stars}**");
        }

        [ChatSubCommand("LVDiet", "test7 - popup bouffe", ChatAuthorizationLevel.Admin)]
        public static void Test7(User user)
        {
            foreach (var groupFoodTaste in user.Stomach.TasteBuds.FoodToTaste.Where(x => x.Value.Discovered).GroupBy(x => x.Value.Preference))
            {
                user.Player.Msg(Localizer.Format($"Preference : {groupFoodTaste.Key.GetEnumLocDisplayName()}"));
                foreach (var foodTaste in groupFoodTaste)
                {
                    user.Player.Msg(Localizer.Format($"{foodTaste.Key.GetLocDisplayNameColored()}"));
                }
            }

        }
    }
}
