// Le Village - Component pour relier le magasin au mod Trade Assistant

using Eco.Core.Controller;
using Eco.Core.Items;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Players;
using Eco.Shared.Localization;
using Eco.Shared.Networking;
using Eco.Shared.Serialization;
using System.ComponentModel;

namespace LVShared.UserCode.LVMods.Utils
{
    [Tag("Economy"), Category("Hidden"), NoIcon, Serialized, AutogenClass, LocDisplayName("Paramétrage magasin")]
    public class StoreExtComponent : WorldObjectComponent
    {
        [RPC, Autogen]
        public void LoadProducts(Player player)
        {
            player.MsgLocStr("Test produits");
        }

        [RPC, Autogen]
        public void LoadComponents(Player player)
        {
            player.MsgLocStr("Test composants");
        }
        [SyncToView, Autogen, AutoRPC, Serialized] public int MarginPercentage { get; set; }

        [RPC, Autogen]
        public void CalculatePrice(Player player)
        {
            player.MsgLocStr($"Test calcul prix {MarginPercentage}");
        }

        public StoreExtComponent()
        {
            this.MarginPercentage = 10;  //Default value
        }

        public override void Initialize()
        {
            base.Initialize();
        }

    }
}
