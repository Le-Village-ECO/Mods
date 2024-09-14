using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Gameplay.Civics.GameValues.Values.Stats;
using Eco.Gameplay.Players;
using Eco.Shared.Localization;

namespace Village.Eco.Mods.Diseases
{
    public class DiseasesPlugin : IInitializablePlugin, IModKitPlugin
    {
        public void Initialize(TimedTask timer)
        {
            UserManager.NewUserJoinedEvent.Add(user =>
            {
                user.Stomach.ChangedEvent.Add(Disease);
            });
        }

        public DiseasesPlugin() { }

        public static void Disease(User user) 
        {

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
        public static void Exhaust(User user)
        {
            // Code repris de FoodChatCommands.cs (Work) donc a retravailler
            int useCalories = 100000;
            user.Stomach.BurnCalories(useCalories, false);
        }

        #endregion
        public string GetCategory() => "LeVillageMods";
        public override string ToString() => Localizer.DoStr("Diseases Plugin");
        public string GetStatus() => "Active";
    }
}
