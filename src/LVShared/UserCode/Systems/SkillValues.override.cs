// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.
// Le Village - Suppression du niveau 2 avec 75 pour décaler les niveaux des étoiles à gagner 2 et +

namespace Eco.Mods.TechTree
{
    using Eco.Core.Plugins.Interfaces;
    using Eco.Gameplay.Skills;
    using Eco.Shared.Services;

    //Defines the cost of skills and specialty numbers
    public class SkillValues : IModInit
    {
        public static void Initialize()
        {    
            SkillManager.Settings = new SkillSettings()
            {
                                               //1  2  3   4   5   6   7   8    9   10   11   12   13   14    15
                //LevelUps               = new[] { 25, 75, 150, 250, 500, 1000, 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000 },
                LevelUps = new[] { 25, 150, 250, 500, 1000, 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000 },
            };
        }
    }
}
