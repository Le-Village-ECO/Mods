// Le Village
// Ce mod ajoute un objet "redbull" qui est un consommable pour récupérer 1h de jeu une fois épuisé.
// Il va de pair avec l'activation de la fonctionnalité "Exhaution" de ECO.
// Aucune recette associée, il doit se donner par commande admin ou, par exemple, dans le sac de départ.

namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Players;
    using Eco.Shared.Localization;
    using Eco.Shared.Serialization;
    using System.Linq;
    using System.Threading.Tasks;

    [Serialized]
    [LocDisplayName("Red Bull")]
    [LocDescription("Redonne 1h d'énergie en cas d'épuisement. Vous donne des ailes !")]  //Description détaillée.
    public partial class RedBullItem : Item
    {
        public override string OnUsed(Player player, ItemStack itemStack)
        {
            //Paramètre(s)
            int hours = 1;

            //var task = player.User.ConfirmBoxLoc($"Test ConfirmBoxLoc");

            //Task.WaitAll( task );

            //if (task.Result == true)
            //{
            //    player.MsgLoc($"TRUE !!");
            //}
            //else
            //{
            //    player.MsgLoc($"FALSE !!");
            //}

            if (player.User.ExhaustionMonitor.IsExhausted == false)  //On bloque si le joueur n'est pas épuisé
            {
                player.MsgLoc($"Vous n'êtes pas encore épuisé... Alors n'abusez pas des boissons sucrées !");
            }
            else
            {
                //Ajoute (hours) heures de jeu supplémentaire
                player.User.ExhaustionMonitor.Energize(hours);

                // Supprime l'objet après utilisation
                using (var changes = InventoryChangeSet.New(new Inventory[] { player.User.Inventory, itemStack.Parent }.Distinct(), player.User))
                {
                    changes.ModifyStack(itemStack, -1);
                    changes.Apply();
                }
            }

            return base.OnUsed(player, itemStack);
        }

    }

}
