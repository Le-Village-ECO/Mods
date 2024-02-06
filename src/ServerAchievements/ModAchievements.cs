// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using System.Collections.Generic;
    using Eco.Core.Plugins.Interfaces;
    using Eco.Gameplay.Achievements;
    using Eco.Gameplay.Players;
    using Eco.Shared.Localization;
    using Eco.Shared.Utils;
    using Eco.Simulation.Time;

    //User defined achievements example. These are triggered by calling `AchievementManager.UnlockAchievement(name, displayname, description)
    //The client will attempt to find an icon that matches 'name' and use that for displaying it.
    //Modded achievements wont appear on Steam, but can appear on other Eco servers that support the same named achievement.
    public class ModAchievements : IContainsAchievements
    {
        public static IEnumerable<AchievementDefinition> MakeAchievements()
        {
            yield return AchievementDefinition.CreateAchievementDefinition(Localizer.DoStr("Existence"), Localizer.DoStr("You used a mod that had a custom achievement (this one)."), SetupExistenceAchievement, true);
        }

        static void SetupExistenceAchievement(AchievementDefinition def)
        {
            //This example watches the OnUserLoggedIn event, and triggers the achievement on any user who logs in.
            //The achievement system will detect achievements already earned on this server and ignore them, so it's ok to do things extra.
            //'Acheive' accepts a function to construct the achievement message, which is only called in the achievement is actually being added
            //(if it doesn't already exist, that is).
            UserManager.OnUserLoggedIn.Add(user => 
            { 
                def.TriggerAchievementProgress(user, () => Localizer.Do($"You logged in at {TimeFormatter.FormatSpanColor(WorldTime.Seconds)} after server start!"));
            });
        }
    }
}
