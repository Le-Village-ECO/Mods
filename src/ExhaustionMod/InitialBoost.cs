﻿// Le Village
// L'objectif est de donner l'énergie accumulable depuis le lancement du cycle (jour 1) au joueur
// lors de sa 1ère connexion au serveur

using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Gameplay.Players;
using Eco.Simulation.Time;
using Eco.Simulation;
using Eco.Shared.Utils;
using Eco.Gameplay.Systems;
using Eco.Gameplay.Systems.Messaging.Chat.Commands;
using System.Text;

namespace Eco.Mods
{
    [ChatCommandHandler]
    public class InitialBoost : IModKitPlugin, IInitializablePlugin
    {
        //public double ExhaustionAfterSec => TimeUtil.HoursToSeconds(BalanceConfig.Obj?.ExhaustionAfterHours ?? 0); //Récupération de ExhaustionMonitor.cs
        public static float ExhaustionAfterHour => BalanceConfig.Obj?.ExhaustionAfterHours ?? 0; //Hours before exhaustion per day

        //float currentWorldTime = (float)WorldTime.Seconds * EcoSim.Obj.EcoDef.TimeMult; //Récupération ExperienceController.cs
        int currentWorldDay = (int)WorldTime.Day; //On ne garde que la partie entière de la valeur du jour du monde

        float Calcul => ExhaustionAfterHour * currentWorldDay;

        [ChatCommand("ExhaustionMod commands")]
        public static void ExhaustionMod() { }

        [ChatSubCommand("ExhaustionMod", "Affichage du jour du serveur", "WorldDay", ChatAuthorizationLevel.User)]
        public static void WorldDay(User user) 
        {
            StringBuilder sb = new();

            int Calcul = (int)(BalanceConfig.Obj.ExhaustionAfterHours * WorldTime.Day); //On ne garde que la partie entière de 

            sb.AppendLine($"La configuration est : {BalanceConfig.Obj.ExhaustionAfterHours} ");
            sb.AppendLine($"Le jour du serveur est : {(float)WorldTime.Day} ");
            sb.AppendLine($"Calcul : {Calcul} ");

            user.Player.InfoBoxLocStr($"{sb}");
        }

        public void Initialize(TimedTask timer)
        {
            //UserManager.OnUserLoggedIn.Add(user => { user.UserXP.AddStars(1); }); //Test OK

            //UserManager.NewUserJoinedEvent.Add(user => { user.ExhaustionMonitor.Energize(calcul); } );  //a tester
            UserManager.OnUserLoggedIn.Add(user => { user.ExhaustionMonitor.Energize(Calcul); });

        }
        public string GetStatus() => string.Empty;
        public string GetCategory() => "LeVillageMods";
    }

}