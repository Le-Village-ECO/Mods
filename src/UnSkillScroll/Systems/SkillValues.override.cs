// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.
// Le Village - Ajout des Tier 2 et Tier 3 pour "StarsToSelfTeach"

namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Housing;
    using Eco.Core.Items;
    using Eco.Core.Plugins.Interfaces;
    using System.Collections.Generic;
    using System.Linq;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Skills;
    using static Eco.Shared.Services.DifficultyModifiers;

    //Defines the cost of skills and specialty numbers
    public class SkillValues : IModInit
    {
        public static void Initialize()
        {
            //Define each collaboration level for skills
            SkillManager.CollabToSkillSettings.Add(CollaborationPreset.Legacy, new SkillSettings()
            {  
                LevelUps               = new[] { 25, 75, 150, 250, 500, 1000, 2000 },
                OverSpecializationRate = 1f,
                SkillReqs              = new[]
                {
                    new SkillReqs() { Tier = 1f, CanBeEducated = true, CaloriesToLearn = 2000, CaloriesToTeach = 2000, ClassroomTierRequired = 1, StarsToBecomeTeacher = 1, StarsToBeTaught = 1, StarsToSelfTeach = 1, TimeToLearnHours = 5 },
                    new SkillReqs() { Tier = 2f, CanBeEducated = true, CaloriesToLearn = 2000, CaloriesToTeach = 2000, ClassroomTierRequired = 1, StarsToBecomeTeacher = 1, StarsToBeTaught = 1, StarsToSelfTeach = 2, TimeToLearnHours = 5 },  //Le Village
                    new SkillReqs() { Tier = 3f, CanBeEducated = true, CaloriesToLearn = 2000, CaloriesToTeach = 2000, ClassroomTierRequired = 1, StarsToBecomeTeacher = 1, StarsToBeTaught = 1, StarsToSelfTeach = 3, TimeToLearnHours = 5 },  //Le Village
				    new SkillReqs() { Tier = 10f, CanBeEducated = true, CaloriesToLearn = 2000, CaloriesToTeach = 2000, ClassroomTierRequired = 1, StarsToBecomeTeacher = 1, StarsToBeTaught = 1, StarsToSelfTeach = 0, TimeToLearnHours = 5 },  //Le Village (astuce pour la spécialité chercheur : Tier 10 à 0 étoile)
				}
            });
            
            ///////////////////////////////////////////////////////////////
            /////No Collab
            SkillManager.CollabToSkillSettings.Add(CollaborationPreset.NoCollaboration, new SkillSettings()
            {  
                                               //1  2  3   4   5   6   7   8    9   10   11   12   13   14    15
                LevelUps               = new[] { 0, 0, 5, 10, 15, 25, 40, 65, 105, 170, 275, 445, 600, 800, 1000 },
                OverSpecializationRate = 1f,
                SkillReqs              = new[]
                {
                    new SkillReqs() { Tier = 1f,  CanBeEducated = true, CaloriesToLearn = 2000,  CaloriesToTeach = 2000,   ClassroomTierRequired = 1, StarsToBecomeTeacher = 1, StarsToBeTaught = 1, StarsToSelfTeach = 2,  TimeToLearnHours = 5 },
                    new SkillReqs() { Tier = 10f, CanBeEducated = true, CaloriesToLearn = 50000, CaloriesToTeach = 50000,  ClassroomTierRequired = 4, StarsToBecomeTeacher = 5, StarsToBeTaught = 2, StarsToSelfTeach = 10, TimeToLearnHours = 48 },
                }
            });
            ///////////////////////////////////////////////////////////////


            ///////////////////////////////////////////////////////////////
            /////Low Collab
            SkillManager.CollabToSkillSettings.Add(CollaborationPreset.LowCollaboration, new SkillSettings()
            {  
                                               //1  2  3   4   5   6   7   8    9   10   11   12   13   14    15
                LevelUps               = new[] { 0, 0, 5, 10, 15, 25, 40, 65, 105, 170, 275, 445, 600, 800, 1000 },
                OverSpecializationRate = .5f,
                SkillReqs              = new[]
                {
                    new SkillReqs() { Tier = 1f,  CanBeEducated = true, CaloriesToLearn = 2000,  CaloriesToTeach = 2000,   ClassroomTierRequired = 1, StarsToBecomeTeacher = 1, StarsToBeTaught = 1, StarsToSelfTeach = 2,  TimeToLearnHours = 5 },
                    new SkillReqs() { Tier = 10f, CanBeEducated = true, CaloriesToLearn = 50000, CaloriesToTeach = 50000,  ClassroomTierRequired = 4, StarsToBecomeTeacher = 5, StarsToBeTaught = 2, StarsToSelfTeach = 10, TimeToLearnHours = 48 },
                }
            });            ///////////////////////////////////////////////////////////////


            ///////////////////////////////////////////////////////////////
            /////Mid Collab
            SkillManager.CollabToSkillSettings.Add(CollaborationPreset.MediumCollaboration, new SkillSettings()
            {
                //1  2  3   4   5   6   7   8    9   10   11   12   13   14    15
                LevelUps               = new[] { 0, 0, 5, 10, 15, 25, 40, 65, 105, 170, 275, 445, 600, 800, 1000 },
                OverSpecializationRate = .35f,
                SkillReqs              = new[]
               {
                    new SkillReqs() { Tier = 1f,  CanBeEducated = true, CaloriesToLearn = 2000,  CaloriesToTeach = 2000,   ClassroomTierRequired = 1, StarsToBecomeTeacher = 1, StarsToBeTaught = 1, StarsToSelfTeach = 2,  TimeToLearnHours = 5 },
                    new SkillReqs() { Tier = 10f, CanBeEducated = true, CaloriesToLearn = 50000, CaloriesToTeach = 50000,  ClassroomTierRequired = 4, StarsToBecomeTeacher = 5, StarsToBeTaught = 2, StarsToSelfTeach = 10, TimeToLearnHours = 48 },
                }
            });
            ///////////////////////////////////////////////////////////////


            ///////////////////////////////////////////////////////////////
            /////High Collab
             SkillManager.CollabToSkillSettings.Add(CollaborationPreset.HighCollaboration, new SkillSettings()
            {  
                                               //1  2  3   4   5   6   7   8    9   10   11   12   13   14    15
                LevelUps               = new[] { 0, 0, 5, 10, 15, 25, 40, 65, 105, 170, 275, 445, 600, 800, 1000 },
                OverSpecializationRate = .1f,
                SkillReqs              = new[]
                {
                    new SkillReqs() { Tier = 1f,  CanBeEducated = true, CaloriesToLearn = 2000,  CaloriesToTeach = 2000,   ClassroomTierRequired = 1, StarsToBecomeTeacher = 1, StarsToBeTaught = 1, StarsToSelfTeach = 2,  TimeToLearnHours = 5 },
					new SkillReqs() { Tier = 10f, CanBeEducated = true, CaloriesToLearn = 50000, CaloriesToTeach = 50000,  ClassroomTierRequired = 4, StarsToBecomeTeacher = 5, StarsToBeTaught = 2, StarsToSelfTeach = 10, TimeToLearnHours = 48 },
                }
            });
            ///////////////////////////////////////////////////////////////
        }
    }
}
