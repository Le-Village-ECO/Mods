// Le Village
// Abstract class pour gérer tous les consommable de type boost qui ajoute de l'énergie

using Eco.Core;
using Eco.Core.Utils;
using Eco.Core.Utils.Logging;
using Eco.Gameplay.Items;
using Eco.Gameplay.Players;
using Eco.Gameplay.Skills;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using Eco.Shared.Utils;
using Eco.Simulation.Time;
using System;
using System.ComponentModel;
using System.Linq;
using Village.Eco.Mods.Core;

namespace Village.Eco.Mods.ExhaustionMod
{
    [Serialized]
    [LocDisplayName("Exhaustion Boost")]
    [LocDescription("Permet de redonner de l'énergie si épuisé")]
    [Category("Hidden")]

    public abstract class ExhaustionBoost : Item
    {
        public static ThreadSafeAction<User> ConsumeEnergyBoost = new();
        //Paramètre(s)
        public virtual float BoostTime => 1.0f; //Valeur par défaut pour la durée du boost
        public virtual bool CheckDate => false; //True si on doit vérifier l'utilisation 1 fois par jour
        public const double DailyBoostCooldown = 1; //Delai entre 2 utilisations de boost journalier

        public override string OnUsed(Player player, ItemStack itemStack)
        {
            string message;
            var boostName = itemStack.Item;

            if (player.User.ExhaustionMonitor.IsExhausted == false)  //On bloque si le joueur n'est pas épuisé
            {
                message = Localizer.Do($"Vous n'êtes pas encore épuisé... Alors n'abusez pas des boissons énergisantes !");
                player.ErrorLocStr(message);

                return base.OnUsed(player, itemStack);
            }

            if(CheckDate) //Boost avec un controle d'utilisation journalier
            {
                //Recuperation des donnees du joueur
                var plugin = PluginManager.GetPlugin<PlayersDataPlugin>();
                var playerData = plugin.GetPlayerDataOrDefault(player);

                //var daysSinceLastBoost = WorldTime.Day - playerData.LastDailyBoost;
                int playerDayInteger = (int) playerData.LastDailyBoost;
                int worldDayInteger = (int) WorldTime.Day;
                //if (playerData.LastDailyBoost > 0 && daysSinceLastBoost < DailyBoostCooldown)
                if (playerData.LastDailyBoost > 0 && playerDayInteger == worldDayInteger)
                {
                    //var timeUntilDailyBoost = DailyBoostCooldown - daysSinceLastBoost;
                    //var coolDownDuration = TimeSpan.FromDays(timeUntilDailyBoost);

                    //Voir UserCommands.cs : public static void Now
                    //message = Localizer.Do($"Vous devez attendre {TimeFormatter.FormatSimple(coolDownDuration)} avant de reprendre {itemStack}.");
                    message = Localizer.Do($"Vous devez attendre 1 jour avant de reprendre {boostName}.");
                    player.ErrorLocStr(message);

                    return base.OnUsed(player, itemStack);
                }
                else
                {
                    //Mise a jour des donnees du joueur
                    playerData.LastDailyBoost = WorldTime.Day;
                    plugin.AddOrSetPlayerData(player, playerData);
                }
            }

            //Ajoute temps de jeu supplémentaire
            player.User.ExhaustionMonitor.Energize(BoostTime);

            // Supprime l'objet de l'inventaire après utilisation
            var inventory = new Inventory[] { player.User.Inventory, itemStack.Parent }.Distinct();
            using (var changes = InventoryChangeSet.New(inventory, player.User))
            {
                changes.ModifyStack(itemStack, -1);
                changes.Apply();
            }

            //Log
            var log = NLogManager.GetLogWriter("LeVillageMods");
            log.Write($"Le joueur **{player.DisplayName}** a utilisé **{boostName}**.");

            //Event
            ConsumeEnergyBoost?.Invoke(player.User);

            return base.OnUsed(player, itemStack);
        }
    }
}
