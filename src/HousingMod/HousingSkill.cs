using Eco.Core.Items;
using Eco.Gameplay.Items;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;

namespace Village.Eco.Mods.HousingMod
{
    [Serialized]
    [LocDisplayName("Habitation")]
    [LocDescription("...")]
    [Ecopedia("Professions", "Survivalist", createAsSubPage: true)]
    [RequiresSkill(typeof(SurvivalistSkill), 0), Tag("Survivalist Specialty"), Tier(1)]
    [Tag("Specialty")]
    [Tag("Teachable")]
    public partial class HousingSkill : Skill
    {
        public override int MaxLevel { get { return 6; } } // 1 niveau par tier d'habitation en commençant par Tier0 = niveau 1 jusque Tier5 = niveau 6
        public override int Tier { get { return 1; } }
    }
}
