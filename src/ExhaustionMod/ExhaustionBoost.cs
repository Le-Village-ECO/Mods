// Le Village
// Ce mod ajoute un objet "redbull" qui est un consommable pour récupérer 1h de jeu une fois épuisé.
// Il va de pair avec l'activation de la fonctionnalité "Exhaution" de ECO.
// Aucune recette associée, il doit se donner par commande admin ou, par exemple, dans le sac de départ.
//TODO : Eventuellement ajouter une pop-up de confirmation avant de consommer dans le cas du contrôle journalier
//TODO : Ajouter l'icône Boost (tag) dans Unity

using Eco.Core;
using Eco.Gameplay.Items;
using Eco.Gameplay.Players;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using Eco.Simulation.Time;
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
        //Paramètre(s)
        public virtual float BoostTime => 1.0f; //Valeur par défaut pour la durée du boost
        public virtual bool CheckDate => false; //True si on doit vérifier l'utilisation 1 fois par jour
        public const double DailyBoostCooldown = 1; //Delai entre 2 utilisations de boost journalier

        public override string OnUsed(Player player, ItemStack itemStack)
        {
            string message;

            if (player.User.ExhaustionMonitor.IsExhausted == false)  //On bloque si le joueur n'est pas épuisé
            {
                message = Localizer.Do($"Vous n'êtes pas encore épuisé... Alors n'abusez pas des boissons énergisantes !");
                player.ErrorLocStr(message);
            }
            else if(CheckDate) //Boost avec un controle d'utilisation journalier
            {
                //Recuperation des donnees du joueur
                var plugin = PluginManager.GetPlugin<PlayersDataPlugin>();
                var playerData = plugin.GetPlayerDataOrDefault(player);

                var daysSinceLastBoost = WorldTime.Day - playerData.LastDailyBoost;
                if (playerData.LastDailyBoost > 0 && daysSinceLastBoost < DailyBoostCooldown)
                {
                    var timeUntilUnspecializing = DailyBoostCooldown - daysSinceLastBoost;
                    message = Localizer.Do($"Vous devez attendre {timeUntilUnspecializing} jour(s) avant de reprendre {itemStack}.");
                    player.ErrorLocStr(message);
                }
                else
                {
                    //Mise a jour des donnees du joueur
                    playerData.LastDailyBoost = WorldTime.Day;
                    plugin.AddOrSetPlayerData(player, playerData);
                }
            }
            else
            {
                //Ajoute temps de jeu supplémentaire
                player.User.ExhaustionMonitor.Energize(BoostTime);

                // Supprime l'objet de l'inventaire après utilisation
                var inventory = new Inventory[] { player.User.Inventory, itemStack.Parent }.Distinct();
                using (var changes = InventoryChangeSet.New(inventory, player.User))
                {
                    changes.ModifyStack(itemStack, -1);
                    changes.Apply();
                }
            }

            return base.OnUsed(player, itemStack);
        }
    }
}
