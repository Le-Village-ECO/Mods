// Le Village
// Lors de la 1ère connexion : Donner le temps accumulable depuis le lancement du cycle (jour 1)
// Lors d'une connexion : Si pas de flag, donner un bonus de 6h (bidouille pour donner 6h en weekend) -- NE FONCTIONNE PAS

using Eco.Core;
using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Core.Utils.Logging;
using Eco.Gameplay.Items;
using Eco.Gameplay.Players;
using Eco.Gameplay.Systems;
using Eco.Gameplay.Systems.Messaging.Chat.Commands;
using Eco.Shared.Localization;
using Eco.Simulation.Time;
using Village.Eco.Mods.ExhaustionMod;
using System;
using Village.Eco.Mods.Core;

namespace Village.Eco.Mods.ExhaustionMod
{
    [ChatCommandHandler]
    public class InitialBoost : IModKitPlugin, IInitializablePlugin
    {
        public static TimeSpan ExhaustionAfterHour => TimeSpan.FromHours(BalanceConfig.Obj?.ExhaustionAfterHours ?? 0); //Nb heures par jour avant épuisement (Fichier config)

        public static double CurrentWorldDay => Math.Floor(WorldTime.Day); //On ne garde que la partie entière de la valeur du jour du monde

        public static TimeSpan Calcul => ExhaustionAfterHour * CurrentWorldDay;

        public void Initialize(TimedTask timer)
        {
            /* LE CODE SLG SUR CETTE PARTIE C'EST DE LA MERDE !
            //Donne un boost de X heure(s) en fonction d'une variable bool
            UserManager.OnUserLoggedIn.Add(user => 
            {
                //Recuperation des donnees du joueur
                var plugin = PluginManager.GetPlugin<PlayersDataPlugin>();
                var playerData = plugin.GetPlayerDataOrDefault(user.Player);

                int hours = 6;
                //Si jamais boosté et déjà épuisé
                if (!playerData.BoostWE && user.ExhaustionMonitor.IsExhausted)
                {
                    //Vanille
                    user.ExhaustionMonitor.Energize(hours);
                    //Version Bejarid
                    //user.ExhaustionMonitor.AddEnergy(hours);

                    //variable joueur TRUE
                    playerData.BoostWE = true;
                    plugin.AddOrSetPlayerData(user.Player, playerData);
                    
                    //Log
                    var log = NLogManager.GetLogWriter("LeVillageMods");
                    log.Write($"Le joueur **{user.Player.DisplayName}** a reçu son boost WE de {hours} heure(s).");
                }
            });*/

            // Sur l'event de 1ère connexion du joueur, on ajoute un nombre d'heure d'énergie basé sur la calcul
            UserManager.NewUserJoinedEvent.Add(user => 
            { 
                //Vérification configuration
                if (LVConfigurePlugin.Config.ExhaustInitialBoost is true)
                {
                    user.ExhaustionMonitor.Energize(Convert.ToSingle(Calcul.TotalHours));

                    //Log
                    var log = NLogManager.GetLogWriter("LeVillageMods");
                    log.Write($"Le joueur **{user.Player.DisplayName}** s'est connecté pour la 1ère fois avec {Convert.ToSingle(Calcul.TotalHours)}");
                }
            } );
        }

        public string GetCategory() => "LeVillageMods";
        public override string ToString() => Localizer.DoStr("Exhaustion Initial Boost");
        public string GetStatus() => "Active";

    }

}
