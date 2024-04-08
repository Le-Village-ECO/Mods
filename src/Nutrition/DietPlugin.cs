// Le village - Plugin pour gérer la spécialité Diététique
// TODO - Ajouter une logique, paramétrable, pour choisir de prendre en compte soit toutes les étoiles gagnées soit uniquement celles utilisées
// TODO - Limite actuelle du déclencheur : Ne considère pas le changement d'étoile et utilise la valeur SkillRate avant la redétermination après avoir mangé un aliment

using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Core.Utils.Logging;
using Eco.Gameplay.DynamicValues;
using Eco.Gameplay.Items.Recipes;
using Eco.Gameplay.Players;
using Eco.Gameplay.Systems.Messaging.Chat.Commands;
using Eco.Shared.Localization;
using Eco.Shared.Utils;
using System;
using System.Linq;
using Village.Eco.Mods.Core;

namespace Village.Eco.Mods.Nutrition
{
    [LocDisplayName("Diet Plugin")]
    [ChatCommandHandler]
    public class DietPlugin : IInitializablePlugin, IModKitPlugin
    {
        public void Initialize(TimedTask timer)
        {
            //Récupération de la configuration
            bool logActive = LVConfigurePlugin.Config.DietDebug;

            //Initialisation de la log
            var log = NLogManager.GetLogWriter("LeVillageMods");
            
            Stomach.FoodContentUpdatedEvent.Add((user, foodtype) =>
            {
                //Log de contrôle
                if (logActive)
                {
                    log.Write($"******* DIET LOG DEBUT *******");
                    log.Write($"**{foodtype.Name}**");
                }

                SkillLevel(user);

                //Log de contrôle
                if (logActive) log.Write($"******* DIET LOG FIN *******");
            });

            // Technique de "Reflexion" pour modifier toutes les recettes
            SkillModifiedValue smv_time = new(1f, DietSkill.MultiplicativeStrategy, typeof(DietSkill), Localizer.DoStr("Temps de fabrication"), DynamicValueType.Speed);
            foreach (RecipeFamily recipe in RecipeManager.AllRecipeFamilies)
            {
                recipe.SetPropertyByName("CraftMinutes", new MultiDynamicValue(MultiDynamicOps.Multiply, smv_time, recipe.CraftMinutes));
            }
            // Alimente ToolTip de la spécialité - TODO cela ne fonctionne pas toujours...
            SkillModifiedValueManager.AddSkillBenefit(typeof(DietSkill), smv_time);
        }

        public static int CalcLevel(User user) 
        {
            //Initialisation de la log
            var log = NLogManager.GetLogWriter("LeVillageMods");

            //Récupération de la configuration
            var tiers = LVConfigurePlugin.Config.DietTiers;
            float gap = LVConfigurePlugin.Config.DietTiersGap;
            bool logActive = LVConfigurePlugin.Config.DietDebug;

            var skill = user.Skillset.GetSkill(typeof(DietSkill));
            var skillRate = user.Stomach.NutrientSkillRate();
            int stars = user.UserXP.TotalStarsEarned; //Ne prend pas en compte l'étoile de départ
            var palier = stars < tiers.Length ? tiers[stars] : tiers.Last(); //Récupère la valeur palier de SkillRate en fonction du nombre d'étoile

            //Log de contrôle
            if (logActive) log.Write($"Niveau de {skill.Name} = {skill.Level} / Skill Rate = {skillRate} / Total stars = {stars} / Palier = {palier}");

            if (skillRate >= palier)
            {
                return skill.MaxLevel; //On ne peut avoir plus que le niveau maximum de la spécialité
            }
            else 
            {
                var delta = (palier - skillRate) / palier * 100; //% d'écart entre le palier et le SkillRate du joueur
                var multiple = delta / gap; //Combien de fois le "gap" (paramètre plugin) dans l'écart
                int arrondi = Convert.ToInt32(Math.Ceiling(multiple)); //arrondi supérieur au format INT32
                int resultat = (skill.MaxLevel - arrondi < 1) ? 1 : (skill.MaxLevel - arrondi); //On ne peut pas descendre en dessous du niveau 1 de la spécialité

                //Log de contrôle
                if (logActive) log.Write($"Delta = {delta} / multiple = {multiple} / arrondi = {arrondi} / resultat = {resultat}");

                return resultat;
            }
        }
        public static void SkillLevel(User user) 
        {
            //Initialisation de la log
            var log = NLogManager.GetLogWriter("LeVillageMods");
            bool logActive = LVConfigurePlugin.Config.DietDebug;

            var skill = user.Skillset.GetSkill(typeof(DietSkill));

            skill.ForceSetLevel(user, CalcLevel(user));
            user.Skillset.RefreshSkills();

            //user.Skillset.LevelUp(typeof(DietSkill)); // Affichage du message de gain d'un niveau

            //Log de contrôle
            if (logActive) log.Write($"Changement de **{skill.Name}** : **{skill.Level}**");
        }

        [ChatCommand("Diet Info", ChatAuthorizationLevel.User)]
        public static void DietInfo(User user) 
        {
            var tiers = LVConfigurePlugin.Config.DietTiers;
            int stars = user.UserXP.TotalStarsEarned;
            var palier = stars < tiers.Length ? tiers[stars] : tiers.Last();
            var roundSkillRate = Math.Round(user.Stomach.NutrientSkillRate());

            string message;
            message = $"Tu as gagné {stars} étoiles depuis le début de la partie.\n";
            message += $"Ton bonus de nourriture est actuellement de {roundSkillRate} (arrondi à l'entier le plus proche).\n";
            message += $"Pour avoir le niveau maximum de diététique, il te faut un bonus de nourriture d'au moins {palier}.\n\n";
            message += $"Tu peux forcer la mise à jour de la spécialité diététique avec la commande de chat suivante : /ForceDiet\n";

            MessageManager.SendWelcomeMsg(user, "Voici les informations concernant ta diététique", message);
        }

        [ChatCommand("Force Diet", ChatAuthorizationLevel.User)]
        public static void ForceDiet(User user)
        {
            //Initialisation de la log
            var log = NLogManager.GetLogWriter("LeVillageMods");

            var skill = user.Skillset.GetSkill(typeof(DietSkill));
            int oldLevel = skill.Level;
            int newLevel = CalcLevel(user);
            skill.ForceSetLevel(user, newLevel);
            user.Skillset.RefreshSkills();

            string message;
            message = $"Niveau de diététique mis à jour : {oldLevel} -> {newLevel}\n";
            user.OkBoxLocStr(message);

            log.Write($"DIET MOD - Le joueur **{user}** a forcé la mise à jour de diététique : {oldLevel} -> {newLevel}");
        }
        public string GetCategory() => "LeVillageMods";
        public override string ToString() => Localizer.DoStr("Diet Plugin");
        public string GetStatus() => "Active";
    }
}
