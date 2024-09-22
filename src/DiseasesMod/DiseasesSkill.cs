using Eco.Core.Items;
using Eco.Gameplay.DynamicValues;
using Eco.Gameplay.Items;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Village.Eco.Mods.Diseases
{
    [Serialized]
    [LocDisplayName("Santé")]
    [LocDescription("Votre état de santé.")]
    [Ecopedia("Professions", "Survivalist", createAsSubPage: true)]
    [RequiresSkill(typeof(SurvivalistSkill), 0), Tag("Survivalist Specialty"), Tier(1)]
    [Tag("Specialty")]
    public partial class DiseasesSkill : Skill
    {
        public override bool CanBeRefunded => false;

        public static MultiplicativeStrategy MultiplicativeStrategy =
            new(new float[] {
                1,
                1,
                1,
            });
        public override MultiplicativeStrategy MultiStrategy => MultiplicativeStrategy;

        public static AdditiveStrategy AdditiveStrategy =
            new(new float[] {
                0,
                10,
                20,
            });
        public override AdditiveStrategy AddStrategy => AdditiveStrategy;
        public override int MaxLevel { get { return 3; } }
        public override int Tier { get { return 1; } }
    }
}
