// Le Village - Village Hebdo est un journal d'information absolument pas objectif

using Eco.Core.Items;
using Eco.Gameplay.Items;
using Eco.Gameplay.Players;
using Eco.Gameplay.Systems.NewTooltip;
using Eco.Gameplay.Systems.NewTooltip.TooltipLibraryFiles;
using Eco.Gameplay.Systems.TextLinks;
using Eco.Shared.Items;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using LVShared.UserCode.LVMods.Plugins;

namespace LVShared.UserCode.LVMods.VillageHebdo
{
    [Serialized]
    [LocDisplayName("Le Village Hebdo")]
    [LocDescription("Les nouvelles fraîches du Village.")]
    [Ecopedia("Items", "Products", createAsSubPage: true)]
    [Weight(50)]
    public partial class VillageHebdoItem : Item
    {
        public override string OnUsed(Player player, ItemStack itemStack) 
        {
            WelcomePlugin.LastNews(player.User);
            return base.OnUsed(player, itemStack);
        }
    }

    [TooltipLibrary]
    public static class VillageHebdoExt 
    {
        //[NewTooltip(CacheAs.Disabled, overrideType: typeof(VillageHebdoItem))]
        [NewTooltip(CacheAs.Disabled)]
        public static LocString VillageHebdoTooltip(this VillageHebdoItem type) => TextLoc.ControlsLoc($"Right-click to read {type.UILink()}.", "[", "]");
    }
}
