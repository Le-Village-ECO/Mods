using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Gameplay.Players;
using Eco.Gameplay.Skills;
using Eco.Gameplay.Systems.Messaging.Chat.Commands;
using Eco.Gameplay.Systems.Messaging.Notifications;
using Eco.Shared.Localization;
using Eco.Shared.States;
using System;

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
        public static void UserStats(User user, int type, User target = null)
        {
            target ??= user;

            var UserStat = target.ModifiedStats.GetStat((UserStatType)type);
            target.OkBoxLocStr($"Stat {(UserStatType)type}: {UserStat.GetValue(target)}");
        }

        [ChatSubCommand("Diseases", "Attrape la peste", ChatAuthorizationLevel.Admin)]
        public static void Plague(User user, User target = null)
        {
            target ??= user;

            RemoveTalent(target, typeof(Healthy2Talent));
            RemoveTalent(target, typeof(Healthy3Talent));

            AddTalent(target, typeof(SlowMvtTalent));
            AddTalent(target, typeof(VomitTalent));

            target.OkBoxLocStr($"Vous avez la peste ! :-(");
        }

        [ChatSubCommand("Diseases", "Soigne la peste", ChatAuthorizationLevel.Admin)]
        public static void HealPlague(User user, User target = null)
        {
            target ??= user;

            RemoveTalent(target, typeof(SlowMvtTalent));
            RemoveTalent(target, typeof(VomitTalent));

            AddTalent(target, typeof(Healthy2Talent));
            AddTalent(target, typeof(Healthy3Talent));

            target.OkBoxLocStr($"Vous avez été soigné de la peste ;-)");
        }

        public static void RemoveTalent(User target, Type talentType)
        {
            if (target.Talentset.HasTalent(talentType)) 
            {
                target.Talentset.UnLearnTalent(talentType);
                NotificationManager.ServerMessageToAllLoc($"Remove {talentType}");
            }
        }

        public static void AddTalent(User target, Type talentType)
        {
            if (!target.Talentset.HasTalent(talentType))
            {
                target.Talentset.LearnTalent(talentType);
                NotificationManager.ServerMessageToAllLoc($"Add {talentType}");
            }
        }

        [ChatSubCommand("Diseases", "Emote dodo", ChatAuthorizationLevel.Admin)]
        public static void Dodo(User user, User target) => DiseasesPlugin.SleepEmote(target);

        [ChatSubCommand("Diseases", "Vomit", ChatAuthorizationLevel.Admin)]
        public static void Vomit(User user, User target) => DiseasesPlugin.Vomit(target);

        [ChatSubCommand("Diseases", "Exhaust", ChatAuthorizationLevel.Admin)]
        public static void Exhaust(User user, int cal, User target) => DiseasesPlugin.Exhaust(target, cal);

    }

        #endregion
    }
