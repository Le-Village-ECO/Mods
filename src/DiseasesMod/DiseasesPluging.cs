using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Gameplay.Civics.GameValues.Values.Stats;
using Eco.Gameplay.Minimap;
using Eco.Gameplay.Players;
using Eco.Gameplay.Systems.Messaging.Chat.Commands;
using Eco.Shared.Localization;
using Eco.Shared.States;

namespace Village.Eco.Mods.Diseases
{
    public class DiseasesPlugin : IInitializablePlugin, IModKitPlugin
    {
        public void Initialize(TimedTask timer)
        {
            UserManager.NewUserJoinedEvent.Add(user =>
            {
                //user.Stomach.ChangedEvent.Add(Disease);
            });

            UserManager.NewUserJoinedEvent.Add(NewUserJoinedEvent);
        }

        public DiseasesPlugin() { }

        public static void Disease(User user) 
        {

        }
        public static void NewUserJoinedEvent(User user)
        {
            // Ajoute la spécialité au niveau max à la 1ère connexion
            var skill = user.Skillset.GetOrAddSkill(typeof(DiseasesSkill));
            skill.ForceSetLevel(user, skill.MaxLevel);

            // Ajouter les talents de bonne santé
            user.Talentset.LearnTalent(typeof(Healthy1Talent));
            user.Talentset.LearnTalent(typeof(Healthy2Talent));
            user.Talentset.LearnTalent(typeof(Healthy3Talent));
        }

        #region Malus

        // Joueur malade
        public static void Vomit(User user)
        {
            // Code repris de FoodChatCommands.cs (ClearStomach) donc a retravailler
            var stomach = user.Player.User.Stomach;
            stomach.ClearCalories(user.Player);
            stomach.Contents.RemoveAll(entry => true, out var removedFood);
            foreach (StomachEntry entry in removedFood) Stomach.FoodContentUpdatedEvent?.Invoke(user, entry.Food.GetType());
            stomach.RecalcAverageNutrients();
            user.Player.MsgLocStr("Bad elk meat?");
        }

        // Joueur épuisé
        public static void Exhaust(User user, int cal = 100000)
        {
            // Code repris de FoodChatCommands.cs (Work) donc a retravailler
            //int useCalories = 500;
            user.Stomach.BurnCalories(cal, false);
        }

        public static void SleepEmote(User user) 
        {
            // Code repris de UserCommands.cs "sleep"
            user.Player?.CheckEmotes(user.Client, AnimationEmote.Lie, ExpressionEmote.Sleep);
        }

        #endregion
        public string GetCategory() => "LeVillageMods";
        public override string ToString() => Localizer.DoStr("Diseases Plugin");
        public string GetStatus() => "Active";
    }

    #region ChatCommands
    [ChatCommandHandler]
    public static partial class DiseasesCommands
    {	
        [ChatCommand("Montre les commandes mod diseases", ChatAuthorizationLevel.Admin)]
        public static void Diseases() { }

        [ChatSubCommand("Diseases", "Recupere les stats d'un joueur", ChatAuthorizationLevel.Admin)]
        public static void UserStats(User user, int type, User target)
        {
            var UserStat = target.ModifiedStats.GetStat((UserStatType)type);
            user.OkBoxLocStr($"Stat {(UserStatType)type}: {UserStat.GetValue(target)}");
        }

        [ChatSubCommand("Diseases", "Recupere les stats d'un joueur", ChatAuthorizationLevel.Admin)]
        public static void Plague(User user, User target)
        {

            user.Talentset.UnLearnTalent(typeof(Healthy2Talent));
            user.Talentset.UnLearnTalent(typeof(Healthy3Talent));

            user.Talentset.LearnTalent(typeof(SlowMvtTalent));
            user.Talentset.LearnTalent(typeof(VomitTalent));

            user.OkBoxLocStr($"Vous avez la peste ! :-(");
        }

        [ChatSubCommand("Diseases", "Recupere les stats d'un joueur", ChatAuthorizationLevel.Admin)]
        public static void HealPlague(User user, User target)
        {
            user.Talentset.UnLearnTalent(typeof(SlowMvtTalent));
            user.Talentset.UnLearnTalent(typeof(VomitTalent));

            user.Talentset.LearnTalent(typeof(Healthy2Talent));
            user.Talentset.LearnTalent(typeof(Healthy3Talent));

            user.OkBoxLocStr($"Vous avez été soigné de la peste ;-)");
        }

        [ChatSubCommand("Diseases", "Emote dodo", ChatAuthorizationLevel.Admin)]
        public static void Dodo(User user, User target) => DiseasesPlugin.SleepEmote(target);

        [ChatSubCommand("Diseases", "Emote dodo", ChatAuthorizationLevel.Admin)]
        public static void Vomit(User user, User target) => DiseasesPlugin.Vomit(target);

        [ChatSubCommand("Diseases", "Emote dodo", ChatAuthorizationLevel.Admin)]
        public static void Exhaust(User user, int cal, User target) => DiseasesPlugin.Exhaust(target, cal);

    }

        #endregion
    }
