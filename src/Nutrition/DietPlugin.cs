// Le village - Plugin pour gérer la spécialité Diététique
// TODO - Ajouter une logique, paramétrable, pour choisir de prendre en compte soit toutes les étoiles gagnées soit uniquement celles utilisées
// TODO - Limite actuelle du déclencheur : Ne considère pas le changement d'étoile

using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Gameplay.DynamicValues;
using Eco.Gameplay.Items;
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
            //En cas de changement de SkillRate d'un joueur, mettre à jour son niveau de diététique
            //Note : Ce n'est pas un "event" .NET mais une construction SLG
            //Cet événement est global mais passe le User en paramètre
            UserXP.UserSkillRateChangedEvent.Add(SkillLevel);

            // Technique de "Reflexion" pour modifier toutes les recettes
            SkillModifiedValue smv_time = new(1f, DietSkill.MultiplicativeStrategy, typeof(DietSkill), typeof(Player), Localizer.DoStr("Temps de fabrication"), DynamicValueType.Speed);
            foreach (RecipeFamily recipe in RecipeManager.AllRecipeFamilies)
            {
                recipe.SetPropertyByName("CraftMinutes", new MultiDynamicValue(MultiDynamicOps.Multiply, smv_time, recipe.CraftMinutes));
            }
            // Alimente ToolTip de la spécialité - TODO cela ne fonctionne pas toujours... cela semble être lié au PlayerDefaults.cs
            SkillModifiedValueManager.AddSkillBenefit(typeof(DietSkill), smv_time);

            UserManager.NewUserJoinedEvent.Add(NewUserJoinedEvent);
            UserManager.OnUserLoggedIn.Add(OnUserLoggedIn);
            UserManager.OnUserLoggedOut.Add(OnUserLoggedOut);

        }
        public static void NewUserJoinedEvent(User user) 
        {
            // Ajoute la spécialité au niveau 1 à la 1ère connexion
            var skill = user.Skillset.GetOrAddSkill(typeof(DietSkill));
            skill.ForceSetLevel(user, 1);
        }
        public static void OnUserLoggedIn(User user) 
        {
            if (user.Talentset.HasTalent<DietStackSizeTalent>()) 
            {
                var carryInventory = user.Inventory.Carried;

                if (!carryInventory.Restrictions.Any(restriction => restriction is MultiplierInventoryRestriction))
                {
                    carryInventory.AddInvRestriction(new MultiplierInventoryRestriction(DietStackSizeTalent.STACK_SIZE));
                }
            }
        }
        public static void OnUserLoggedOut(User user)
        {
        }

        public static int CalcLevel(User user)
        {
            //Récupération de la configuration
            var tiers = LVConfigurePlugin.Config.DietTiers;
            float gap = LVConfigurePlugin.Config.DietTiersGap;
            bool logActive = LVConfigurePlugin.Config.DietDebug;

            var skill = user.Skillset.GetSkill(typeof(DietSkill));
            var skillRate = user.Stomach.NutrientSkillRate();
            int stars = user.UserXP.TotalStarsEarned; //Ne prend pas en compte l'étoile de départ
            var palier = stars < tiers.Length ? tiers[stars] : tiers.Last(); //Récupère la valeur palier de SkillRate en fonction du nombre d'étoile

            //Log de contrôle
            if (logActive) Logger.SendLog(Criticity.Info,"Diet" ,$"Niveau de {skill.Name} = {skill.Level} / Skill Rate = {skillRate} / Total stars = {stars} / Palier = {palier}");

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
                if (logActive) Logger.SendLog(Criticity.Info, "Diet", $"Delta = {delta} / multiple = {multiple} / arrondi = {arrondi} / resultat = {resultat}");

                return resultat;
            }
        }
        public static void SkillLevel(User user) 
        {
            //Récupération de la configuration
            bool logActive = LVConfigurePlugin.Config.DietDebug;

            var skill = user.Skillset.GetSkill(typeof(DietSkill));

            int level = CalcLevel(user);

            // Comme le déclencheur est une mise à jour du SkillRate en général, il se peut que le niveau calculé reste inchangé
            // Donc si aucun changement de niveau nécessaire, ne rien faire
            if (level == skill.Level) return;

            // Si estomac vide, on évite une mise à jour du niveau de la spécialité car elle tombera forcément au niveau 1
            // Cela évite aussi de perdre le talent
            if (user.Stomach.Calories == 0f) return;

            skill.ForceSetLevel(user, level);
            if (skill.Talents != null) skill.ResetTalents(user); //Remise à 0 du talent si ce dernier avait été pris
            user.Skillset.RefreshSkills();

            //Log de contrôle
            if (logActive) Logger.SendLog(Criticity.Info, "Diet", $"Changement de **{skill.Name}** : **{skill.Level}**");
        }

        [ChatCommand("Diet Info", ChatAuthorizationLevel.User)]
        public static void DietInfo(User user) 
        {
            var tiers = LVConfigurePlugin.Config.DietTiers;
            int stars = user.UserXP.TotalStarsEarned;
            var palier = stars < tiers.Length ? tiers[stars] : tiers.Last();
            var roundSkillRate = Math.Round(user.Stomach.NutrientSkillRate());

            string title;
            title = "Informations concernant ta diététique";

            string message;
            message = $"Voici les paramètres définis pour avoir le <b>niveau maximum en fonction du nombre d'étoiles gagnées</b> :\n";
            for (int  i = 0; i < tiers.Length; i++) 
            {
                message += $"Avec {i} étoile(s), il faut avoir {tiers[i]} en bonus de nourriture.\n";
            }
            message += $"Au delà, les autres étoiles ne demandent pas plus que {tiers.Last()}.\n\n";
            message += $"Tu as gagné <b>{stars} étoiles</b> depuis le début de la partie.\n";
            message += $"Ton bonus de nourriture est actuellement de <b>{roundSkillRate}</b> (arrondi à l'entier le plus proche).\n";
            message += $"Donc pour avoir le niveau <b>maximum de diététique</b>, il te faut un bonus de nourriture d'<b>au moins {palier}</b>.\n\n";
            message += $"Tu peux forcer la mise à jour de la spécialité diététique avec la commande de chat suivante :\n";
            message += $"<b>/ForceDiet</b>\n\n";
            message += $"Pour avoir tes goûts culinaires ou d'un autre joueur, tu peux faire la commande de chat suivante :\n";
            message += $"<b>/Nourriture</b> <i>(optionel : nom du joueur)</i>\n";

            MessageManager.SendWelcomeMsg(user, title, message);
        }

        [ChatCommand("Force Diet", ChatAuthorizationLevel.User)]
        public static void ForceDiet(User user)
        {
            var skill = user.Skillset.GetSkill(typeof(DietSkill));
            int oldLevel = skill.Level;
            int newLevel = CalcLevel(user);
            skill.ForceSetLevel(user, newLevel);
            user.Skillset.RefreshSkills();

            string message;
            message = $"Niveau de diététique mis à jour : {oldLevel} -> {newLevel}\n";
            user.OkBoxLocStr(message);

            Logger.SendLog(Criticity.Info, "Diet", $"DIET MOD - Le joueur **{user.Player.DisplayName}** a forcé la mise à jour de diététique : {oldLevel} -> {newLevel}");
        }

        [ChatCommand("Nourriture", ChatAuthorizationLevel.User)]
        public static void Nourriture(User user, User target = null)
        {
            target ??= user; //Si pas de cible alors soi-même

            var buds = target.Stomach.TasteBuds;

            // Liste des préférences culinaires - Favorite & Worst sont traités séparemment
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

            // Affichage particulier pour Favorite et Worst
            message = TextLoc.BoldLoc($"Son plat {LocTaste(ItemTaste.TastePreference.Favorite)} : ") + $"{(buds.FavoriteDiscovered ? buds.Favorite.MarkedUpName : "inconnu")}\n\n";
            message += TextLoc.BoldLoc($"Son plat {LocTaste(ItemTaste.TastePreference.Worst)} : ") + $"{(buds.WorstDiscovered ? buds.Worst.MarkedUpName : "inconnu")}\n\n";

            // Boucle sur l'ensemble des préférences culinaires hors Favorite & Worst
            foreach (var preference in preferences)
            {
                message += TextLoc.BoldLoc($"La nourriture qu'il trouve {LocTaste(preference)} :\n");
                foreach (var food in buds.FoodToTaste.Where(x => x.Value.Discovered && x.Value.Preference == preference))
                {
                    message += $"{Item.Get(food.Key).MarkedUpName}\n";
                }
                message += "\n";
            }

            // Affichage de la pop-up de type fenêtre de taille modifiable avec contour bois
            MessageManager.SendWelcomeMsg(user, title, message);
        }

        // Gestion de la localisation (comprendre la traduction) des préférences culinaires avec une couleur associée
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

        public string GetCategory() => "LeVillageMods";
        public override string ToString() => Localizer.DoStr("Diet Plugin");
        public string GetStatus() => "Active";
    }
}
