using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Core.Utils.Logging;
using Eco.Gameplay.DynamicValues;
using Eco.Gameplay.GameActions;
using Eco.Gameplay.Items;
using Eco.Gameplay.Items.Recipes;
using Eco.Gameplay.Players;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Localization;
using Eco.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Village.Eco.Mods.Nutrition
{
    public class DietPlugin : IInitializablePlugin, IModKitPlugin
    {
        public void Initialize(TimedTask timer) 
        {
            var log = NLogManager.GetLogWriter("LeVillageMods");

            float tier0 = 0f;
            float tier1 = 20f;
            float tier2 = 32f;
            float tier3 = 44f;

            Stomach.FoodContentUpdatedEvent.Add((user, foodtype) =>
            {
                var skill = user.Skillset.GetSkill(typeof(DietSkill));

                //Log
                log.Write($"Niveau de **{skill.Name}** : **{skill.Level}**");
                log.Write($"Skill Rate : **{user.Stomach.NutrientSkillRate()}**");

                if (user.Stomach.NutrientSkillRate() >= tier1 && skill.Level < 3 )
                {
                    //user.Skillset.LevelUp(typeof(DietSkill));
                    //user.Skillset.LevelUp(typeof(HuntingSkill));

                    //Log
                    log.Write($"Changement de **{skill.Name}** : **{skill.Level}**");
                }
            });

            SkillModifiedValue smv_time = new SkillModifiedValue(1f, DietSkill.MultiplicativeStrategy, typeof(DietSkill), Localizer.DoStr("Temps de fabrication"), DynamicValueType.Speed);
            foreach (RecipeFamily recipe in RecipeManager.AllRecipeFamilies)
            {
                recipe.SetPropertyByName("CraftMinutes", new MultiDynamicValue(MultiDynamicOps.Multiply, smv_time, recipe.CraftMinutes));
            }
            // Alimente ToolTip de la spécialité
            SkillModifiedValueManager.AddSkillBenefit(typeof(DietSkill), smv_time);

        }
        public string GetCategory() => "LeVillageMods";
        public override string ToString() => Localizer.DoStr("Diet");
        public string GetStatus() => "Active";
    }
}
