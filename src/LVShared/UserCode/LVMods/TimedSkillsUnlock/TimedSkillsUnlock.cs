// Le Village Mod - Permet d'imposer un planning d'apprentissage des spécialités
// Le joueur ne peut pas apprendre la spécialité (comprendre lire le parchemin) avant le jour X du serveur
// Pour chaque spécialité, il faut changer la valeur en jour dans le dictionnaire

using Eco.Gameplay.Items;
using Eco.Gameplay.Players;
using Eco.Shared.Utils;
using Eco.Simulation.Time;
using System;
using System.Collections.Generic;

namespace Eco.Mods.TechTree
{
    public static class TSUSettings
    {
        public static readonly Dictionary<Type, int> Planning = new Dictionary<Type, int>
        {
            { typeof(CarpentrySkillScroll),             0 },
            { typeof(MasonrySkillScroll),               0 },
            { typeof(FarmingSkillScroll),               0 },
            { typeof(ButcherySkillScroll),              0 },
            { typeof(TailoringSkillScroll),             0 },
            { typeof(BasicEngineeringSkillScroll),      0 },
            { typeof(PaperMillingSkillScroll),          0 },
            { typeof(ShipwrightSkillScroll),            0 },
            { typeof(SmeltingSkillScroll),              0 },
            { typeof(MillingSkillScroll),               0 },
            { typeof(FertilizersSkillScroll),           0 },
            { typeof(CookingSkillScroll),               0 },
            { typeof(BakingSkillScroll),                0 },
            { typeof(BlacksmithSkillScroll),            0 },
            { typeof(PaintingSkillScroll),              0 },
            { typeof(PotterySkillScroll),               0 },
            { typeof(MechanicsSkillScroll),             0 },
            { typeof(GlassworkingSkillScroll),          0 },
            { typeof(AdvancedSmeltingSkillScroll),      0 },
            { typeof(AdvancedCookingSkillScroll),       0 },
            { typeof(AdvancedBakingSkillScroll),        0 },
            { typeof(AdvancedMasonrySkillScroll),       0 },
            { typeof(OilDrillingSkillScroll),           0 },
            { typeof(ElectronicsSkillScroll),           0 },
            { typeof(CompositesSkillScroll),            0 },
            { typeof(IndustrySkillScroll),              0 },
            { typeof(CuttingEdgeCookingSkillScroll),    0 },
        };
    }

    public partial class CarpentrySkillScroll
    {
        public override string OnUsed(Player player, ItemStack itemStack) 
        {
            if (SkillUtils.CheckDay<CarpentrySkillScroll>(player)) return base.OnUsed(player, itemStack);
            else return string.Empty;
        }
    }

    public partial class MasonrySkillScroll
    {
        public override string OnUsed(Player player, ItemStack itemStack)
        {
            if (SkillUtils.CheckDay<MasonrySkillScroll>(player)) return base.OnUsed(player, itemStack);
            else return string.Empty;
        }
    }

    public partial class FarmingSkillScroll
    {
        public override string OnUsed(Player player, ItemStack itemStack)
        {
            if (SkillUtils.CheckDay<FarmingSkillScroll>(player)) return base.OnUsed(player, itemStack);
            else return string.Empty;
        }
    }

    public partial class ButcherySkillScroll
    {
        public override string OnUsed(Player player, ItemStack itemStack)
        {
            if (SkillUtils.CheckDay<ButcherySkillScroll>(player)) return base.OnUsed(player, itemStack);
            else return string.Empty;
        }
    }

    public partial class TailoringSkillScroll
    {
        public override string OnUsed(Player player, ItemStack itemStack)
        {
            if (SkillUtils.CheckDay<TailoringSkillScroll>(player)) return base.OnUsed(player, itemStack);
            else return string.Empty;
        }
    }

    public partial class BasicEngineeringSkillScroll
    {
        public override string OnUsed(Player player, ItemStack itemStack)
        {
            if (SkillUtils.CheckDay<BasicEngineeringSkillScroll>(player)) return base.OnUsed(player, itemStack);
            else return string.Empty;
        }
    }

    public partial class PaperMillingSkillScroll
    {
        public override string OnUsed(Player player, ItemStack itemStack)
        {
            if (SkillUtils.CheckDay<PaperMillingSkillScroll>(player)) return base.OnUsed(player, itemStack);
            else return string.Empty;
        }
    }

    public partial class ShipwrightSkillScroll
    {
        public override string OnUsed(Player player, ItemStack itemStack)
        {
            if (SkillUtils.CheckDay<ShipwrightSkillScroll>(player)) return base.OnUsed(player, itemStack);
            else return string.Empty;
        }
    }

    public partial class SmeltingSkillScroll
    {
        public override string OnUsed(Player player, ItemStack itemStack)
        {
            if (SkillUtils.CheckDay<SmeltingSkillScroll>(player)) return base.OnUsed(player, itemStack);
            else return string.Empty;
        }
    }

    public partial class MillingSkillScroll
    {
        public override string OnUsed(Player player, ItemStack itemStack)
        {
            if (SkillUtils.CheckDay<MillingSkillScroll>(player)) return base.OnUsed(player, itemStack);
            else return string.Empty;
        }
    }

    public partial class FertilizersSkillScroll
    {
        public override string OnUsed(Player player, ItemStack itemStack)
        {
            if (SkillUtils.CheckDay<FertilizersSkillScroll>(player)) return base.OnUsed(player, itemStack);
            else return string.Empty;
        }
    }

    public partial class CookingSkillScroll
    {
        public override string OnUsed(Player player, ItemStack itemStack)
        {
            if (SkillUtils.CheckDay<CookingSkillScroll>(player)) return base.OnUsed(player, itemStack);
            else return string.Empty;
        }
    }

    public partial class BakingSkillScroll
    {
        public override string OnUsed(Player player, ItemStack itemStack)
        {
            if (SkillUtils.CheckDay<BakingSkillScroll>(player)) return base.OnUsed(player, itemStack);
            else return string.Empty;
        }
    }

    public partial class BlacksmithSkillScroll
    {
        public override string OnUsed(Player player, ItemStack itemStack)
        {
            if (SkillUtils.CheckDay<BlacksmithSkillScroll>(player)) return base.OnUsed(player, itemStack);
            else return string.Empty;
        }
    }

    public partial class PaintingSkillScroll
    {
        public override string OnUsed(Player player, ItemStack itemStack)
        {
            if (SkillUtils.CheckDay<PaintingSkillScroll>(player)) return base.OnUsed(player, itemStack);
            else return string.Empty;
        }
    }

    public partial class PotterySkillScroll
    {
        public override string OnUsed(Player player, ItemStack itemStack)
        {
            if (SkillUtils.CheckDay<PotterySkillScroll>(player)) return base.OnUsed(player, itemStack);
            else return string.Empty;
        }
    }

    public partial class MechanicsSkillScroll
    {
        public override string OnUsed(Player player, ItemStack itemStack)
        {
            if (SkillUtils.CheckDay<MechanicsSkillScroll>(player)) return base.OnUsed(player, itemStack);
            else return string.Empty;
        }
    }

    public partial class GlassworkingSkillScroll
    {
        public override string OnUsed(Player player, ItemStack itemStack)
        {
            if (SkillUtils.CheckDay<GlassworkingSkillScroll>(player)) return base.OnUsed(player, itemStack);
            else return string.Empty;
        }
    }

    public partial class AdvancedSmeltingSkillScroll
    {
        public override string OnUsed(Player player, ItemStack itemStack)
        {
            if (SkillUtils.CheckDay<AdvancedSmeltingSkillScroll>(player)) return base.OnUsed(player, itemStack);
            else return string.Empty;
        }
    }

    public partial class AdvancedCookingSkillScroll
    {
        public override string OnUsed(Player player, ItemStack itemStack)
        {
            if (SkillUtils.CheckDay<AdvancedCookingSkillScroll>(player)) return base.OnUsed(player, itemStack);
            else return string.Empty;
        }
    }

    public partial class AdvancedBakingSkillScroll
    {
        public override string OnUsed(Player player, ItemStack itemStack)
        {
            if (SkillUtils.CheckDay<AdvancedBakingSkillScroll>(player)) return base.OnUsed(player, itemStack);
            else return string.Empty;
        }
    }

    public partial class AdvancedMasonrySkillScroll
    {
        public override string OnUsed(Player player, ItemStack itemStack)
        {
            if (SkillUtils.CheckDay<AdvancedMasonrySkillScroll>(player)) return base.OnUsed(player, itemStack);
            else return string.Empty;
        }
    }

    public partial class OilDrillingSkillScroll
    {
        public override string OnUsed(Player player, ItemStack itemStack)
        {
            if (SkillUtils.CheckDay<OilDrillingSkillScroll>(player)) return base.OnUsed(player, itemStack);
            else return string.Empty;
        }
    }

    public partial class ElectronicsSkillScroll
    {
        public override string OnUsed(Player player, ItemStack itemStack)
        {
            if (SkillUtils.CheckDay<ElectronicsSkillScroll>(player)) return base.OnUsed(player, itemStack);
            else return string.Empty;
        }
    }

    public partial class CompositesSkillScroll
    {
        public override string OnUsed(Player player, ItemStack itemStack)
        {
            if (SkillUtils.CheckDay<CompositesSkillScroll>(player)) return base.OnUsed(player, itemStack);
            else return string.Empty;
        }
    }

    public partial class IndustrySkillScroll
    {
        public override string OnUsed(Player player, ItemStack itemStack)
        {
            if (SkillUtils.CheckDay<IndustrySkillScroll>(player)) return base.OnUsed(player, itemStack);
            else return string.Empty;
        }
    }

    public partial class CuttingEdgeCookingSkillScroll
    {
        public override string OnUsed(Player player, ItemStack itemStack)
        {
            if (SkillUtils.CheckDay<CuttingEdgeCookingSkillScroll>(player)) return base.OnUsed(player, itemStack);
            else return string.Empty;
        }
    }

    public static class SkillUtils 
    {
        public static bool CheckDay<TSkill>(Player player)
        {
            var configDay = TSUSettings.Planning[typeof(TSkill)];
            if (WorldTime.Day < configDay)
            {
                player.User.ErrorLocStr($"Nous sommes le jour {Text.Num(WorldTime.Day)} et il faut attendre le jour {configDay} pour apprendre cette spécialité.");
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
