// Le village - Ensemble de commandes Admin pour faire des tests.
// Les commandes pour les joueurs sont dans DietPlugin.cs

using Eco.Core.Utils;
using Eco.Gameplay.DynamicValues;
using Eco.Gameplay.Items;
using Eco.Gameplay.Players;
using Eco.Gameplay.Skills;
using Eco.Gameplay.Systems.Messaging.Chat.Commands;
using Eco.Gameplay.Utils;
using Eco.Shared.Localization;
using Eco.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Village.Eco.Mods.Core;

namespace Village.Eco.Mods.Nutrition
{
    [ChatCommandHandler]
    public static partial class DietCommands
    {
        [ChatCommand("LVDiet", ChatAuthorizationLevel.Admin)]
        public static void LVDiet() { }

        [ChatSubCommand("LVDiet", "Taste Stomach", ChatAuthorizationLevel.Admin)]
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

        [ChatSubCommand("LVDiet", "Force Skill Level", ChatAuthorizationLevel.Admin)]
        public static void Test2(User user, string skillName, int level)
        {
            var skillType = SkillCommands.SkillTypeByName(user, skillName);
            var skill = user.Skillset[skillType];

            skill.ForceSetLevel(user, level);
            if (skill.Talents != null) skill.ResetTalents(user);
            user.Skillset.RefreshSkills();

            user.Player.Msg(Localizer.Format($"coucou"));
        }

        [ChatSubCommand("LVDiet", "Benefits Diet", ChatAuthorizationLevel.Admin)]
        public static void Test3(User user)
        {
            var smvm = SkillModifiedValueManager.GetBenefitsFor(new DietSkill());
            foreach (var skillBenefits in smvm)
            {
                user.Player.Msg(Localizer.Format($"Skill benefits : {skillBenefits.Key.Name}"));
                user.Player.Msg(Localizer.Format($"Nb skill benefits : {skillBenefits.Value.Count}"));
            }
        }

        [ChatSubCommand("LVDiet", "Taste bouffe", ChatAuthorizationLevel.Admin)]
        public static void Test4(User user, string foodName)
        {
            var foodItem = CommandsUtil.ClosestMatchingItem<FoodItem>(user, foodName);
            string test = user.Stomach.TasteBuds.GetFoodTaste(foodItem);
            user.Player.Msg(Localizer.Format($"{foodName} = {foodItem} = {test}"));
        }

        [ChatSubCommand("LVDiet", "Valeurs DietInfo", ChatAuthorizationLevel.Admin)]
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

        [ChatSubCommand("LVDiet", "test7 - Test Logger", ChatAuthorizationLevel.Admin)]
        public static void Test7(User user, string msg)
        {
            Logger.SendLog(Criticity.Info, "test", $"{msg}");
        }
        [ChatSubCommand("LVDiet", "test8 - popup bouffe sans extreme", ChatAuthorizationLevel.Admin)]
        public static void Test8(User user, User target = null)
        {
            target ??= user;

            var buds = target.Stomach.TasteBuds;

            var preferences = new[]
            {
                ItemTaste.TastePreference.Delicious,
                ItemTaste.TastePreference.Good,
                ItemTaste.TastePreference.Ok,
                ItemTaste.TastePreference.Bad,
                ItemTaste.TastePreference.Horrible
            };

            string title;
            title = $"Les gouts culinaires de {target}";

            string message;

            message = TextLoc.BoldLoc($"Son plat {LocTaste(ItemTaste.TastePreference.Favorite)} : ")+ $"{(buds.FavoriteDiscovered ? buds.Favorite.MarkedUpName : "inconnu")}\n\n";
            message += TextLoc.BoldLoc($"Son plat {LocTaste(ItemTaste.TastePreference.Worst)} : ") + $"{(buds.WorstDiscovered ? buds.Worst.MarkedUpName : "inconnu")}\n\n";

            foreach (var preference in preferences)
            {
                message += TextLoc.BoldLoc($"La nourriture qu'il trouve {LocTaste(preference)} :\n");
                foreach (var food in buds.FoodToTaste.Where(x => x.Value.Discovered && x.Value.Preference == preference))
                {
                    message += $"{Item.Get(food.Key).MarkedUpName}\n";
                }
                message += "\n";
            }

            MessageManager.SendWelcomeMsg(user, title, message);
        }

        public static LocString LocTaste(ItemTaste.TastePreference pref) => pref switch
        {
            ItemTaste.TastePreference.Delicious => TextLoc.ColorLocStr(Color.LightGreen, "Delicious"),
            ItemTaste.TastePreference.Good => TextLoc.ColorLocStr(Color.GreenGrey, "Good"),
            ItemTaste.TastePreference.Ok => TextLoc.ColorLocStr(Color.White, "Ok"),
            ItemTaste.TastePreference.Bad => TextLoc.ColorLocStr(Color.BlueGrey, "Bad"),
            ItemTaste.TastePreference.Horrible => TextLoc.ColorLocStr(Color.Grey, "Horrible"),
            ItemTaste.TastePreference.Worst => TextLoc.ColorLocStr(Color.Red, "Least favorite"),
            ItemTaste.TastePreference.Favorite => TextLoc.ColorLocStr(Color.Green, "Favorite"),
            _ => throw new NotImplementedException(),
        };
    }
}
