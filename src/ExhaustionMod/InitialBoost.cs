// Le Village
// L'objectif est de donner l'énergie accumulable depuis le lancement du cycle (jour 1) au joueur
// lors de sa 1ère connexion au serveur
// TODO - Revoir le code par dev c#

using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Gameplay.Players;
using Eco.Simulation.Time;
using Eco.Simulation;
using Eco.Shared.Utils;
using Eco.Gameplay.Systems;
using Eco.Gameplay.Systems.Messaging.Chat.Commands;
using System.Text;
using System;

namespace Village.Eco.Mods.ExhaustionMod
{
    [ChatCommandHandler]
    public class InitialBoost : IModKitPlugin, IInitializablePlugin
    {
        public static TimeSpan ExhaustionAfterHour => TimeSpan.FromHours(BalanceConfig.Obj?.ExhaustionAfterHours ?? 0); //Nb heures par jour avant épuisement (Fichier config)

        public static double CurrentWorldDay => Math.Floor(WorldTime.Day); //On ne garde que la partie entière de la valeur du jour du monde

        public static TimeSpan Calcul => ExhaustionAfterHour * CurrentWorldDay;

        [ChatCommand("ExhaustionMod commands")]
        public static void ExhaustionMod() { }

        [ChatSubCommand("ExhaustionMod", "Affichage du jour du serveur", "WorldDay", ChatAuthorizationLevel.User)]
        public static void WorldDay(User user) 
        {
            StringBuilder sb = new();

            sb.AppendLine($"La configuration est : {BalanceConfig.Obj.ExhaustionAfterHours} ");
            sb.AppendLine($"Le jour du serveur est : {CurrentWorldDay} ");
            sb.AppendLine($"Calcul : {Calcul.TotalHours} heures");

            user.Player.InfoBoxLocStr($"{sb}");
        }

        public void Initialize(TimedTask timer)
        {
            //UserManager.OnUserLoggedIn.Add(user => { user.ExhaustionMonitor.Energize(Calcul); }); //Test OK

            // Sur l'event de 1ère connexion du joueur, on ajoute un nombre d'heure d'énergie basé sur la calcul
            UserManager.NewUserJoinedEvent.Add(user => { user.ExhaustionMonitor.Energize(Convert.ToSingle(Calcul.TotalHours)); } );  //a tester

        }
        public string GetStatus() => string.Empty;
        public string GetCategory() => "LeVillageMods";
    }

}
