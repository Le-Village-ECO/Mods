// Le Village

using Eco.Mods.TechTree;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;

namespace Village.Eco.Mods.UnSkillScroll
{
    [Serialized]
    [LocDisplayName("Parchemin d'oubli : Chasse")]
    public partial class HuntingUnSkillScroll : UnSkillScroll<HuntingSkill> { }
}