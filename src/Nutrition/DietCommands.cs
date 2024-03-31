using Eco.Core.Utils.Logging;
using Eco.Gameplay.Items;
using Eco.Gameplay.Players;
using Eco.Gameplay.Skills;
using Eco.Gameplay.Systems.Messaging.Chat.Commands;
using Eco.Gameplay.Systems.Messaging.Notifications;
using Eco.Gameplay.UI;
using Eco.Gameplay.Utils;
using Eco.Shared.Localization;
using System;
//using Village.Eco.Mods.Core;

namespace Village.Eco.Mods.Nutrition
{
    [ChatCommandHandler]
    public static partial class DietCommands
    {
        [ChatCommand("Diet", ChatAuthorizationLevel.Admin)]
        public static void LVDiet() { }

        [ChatSubCommand("LVDiet", "test", ChatAuthorizationLevel.Admin)]
        public static void Test1(User user, string foodName)
        {
            var foodItem = CommandsUtil.ClosestMatchingItem<FoodItem>(user, foodName);
            string test = user.Stomach.TasteBuds.GetFoodTaste(foodItem);
            user.Player.Msg(Localizer.Format($"{foodName} = {foodItem} = {test}"));

                /*foreach (var item in PlayerUtils.Food)
                {
                    user.Player.MsgLocStr($"loop : {item}");
                }
                //return true;*/


        }

        [ChatSubCommand("LVDiet", "test2", ChatAuthorizationLevel.Admin)]
        public static void Test2(User user, string skillName, int level)
        {
            var skillType = SkillCommands.SkillTypeByName(user, skillName);
            var skill = user.Skillset[skillType];

            user.Player.Msg(Localizer.Format($"coucou"));

            //while (skill.Level < skill.MaxLevel)
            while (skill.Level < level)
            {
                var x = skill.Learn(user, true, false, true).Result;
            }
        }

        [ChatSubCommand("LVDiet", "test3", ChatAuthorizationLevel.Admin)]
        public static void Test3(User user, string skillName, int level)
        {
            var skillType = SkillCommands.SkillTypeByName(user, skillName);
            var skill = user.Skillset[skillType];

            user.Player.Msg(Localizer.Format($"coucou"));

            //while (skill.Level < skill.MaxLevel)
        }
    }

    /*public class MethodUtils
    {
        public static List<Item> Food => FoodItem.AllItemsExceptHidden.ToList();
        public static List<Item> Tool => Item.AllItemsExceptHidden.Where(i => i.IsTool).ToList();
    }*/

}
