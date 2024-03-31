using Eco.Core.Items;
using Eco.Gameplay.DynamicValues;
using Eco.Gameplay.Items;
using Eco.Gameplay.Players;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;

namespace Village.Eco.Mods.Nutrition
{
    [Serialized]
    [LocDisplayName("Diet")]
    [LocDescription("Bien manger")]
    [Ecopedia("Professions", "Survivalist", createAsSubPage: true)]
    [RequiresSkill(typeof(SurvivalistSkill), 0), Tag("Survivalist Specialty"), Tier(1)]
    [Tag("Specialty")]
    [Tag("Teachable")]
    public partial class DietSkill : Skill
    {
        /*public override void OnReset(User user) { this.OnLevelChanged(user); }
        public override void OnLevelUp(User user) { this.OnLevelChanged(user); }
        private void OnLevelChanged(User user)
        {
            user.Stomach.ChangedMaxCalories();
            user.ChangedCarryWeight();
        }*/

        public static MultiplicativeStrategy MultiplicativeStrategy =
            new MultiplicativeStrategy(new float[] {
                1 + 1f,         //niveau 0
                1 + 1f,         //niveau 1 - 200%
                1 + 0.75f,
                1 + 0.5f,
                1 + 0.25f,
                1,              //niveau max - 100%
            });
        public override MultiplicativeStrategy MultiStrategy => MultiplicativeStrategy;

        public static AdditiveStrategy AdditiveStrategy =
            new AdditiveStrategy(new float[] {
                0,
                0,
                250,
                500,
                1000,
                1500,
            });
        public override AdditiveStrategy AddStrategy => AdditiveStrategy;
        public override int MaxLevel { get { return 5; } }
        public override int Tier { get { return 1; } }
    }
}
