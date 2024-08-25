// Le Village - Ajout récupération du bol après consommation du plat
// Note : le "consume" ne fonctionne pas (il manque keyword 'virtual' dans FoodItem)

using Eco.Gameplay.Items;
using Eco.Gameplay.Players;
using Village.Eco.Mods.Cooking;

namespace Eco.Mods.TechTree
{
    public partial class FishStewItem
    {
        public override string OnUsed(Player player, ItemStack itemStack)
        {
            var message = base.OnUsed(player, itemStack);

            if (message != "")
            {
                var result = player.User.Inventory.TryAddItemNonUnique(typeof(WoodenBowlItem));
                if (result.Success) player.User.InfoBoxLocStr("Chanceux ! Tu as récupéré un bol.");
            }
            return message;
        }

        public string Consume(Player player) => OnUsed(player, player.User.Inventory.Toolbar.SelectedStack);
    }
}