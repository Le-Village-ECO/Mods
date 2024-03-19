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
    public partial class Diet : Skill
    {
        public override void OnReset(User user) { this.OnLevelChanged(user); }
        public override void OnLevelUp(User user) { this.OnLevelChanged(user); }
        private void OnLevelChanged(User user)
        {
            user.Stomach.ChangedMaxCalories();
            user.ChangedCarryWeight();
        }

        public static MultiplicativeStrategy MultiplicativeStrategy =
            new MultiplicativeStrategy(new float[] {
                1,
                1 - 0.05f,
                1 - 0.1f,
                1 - 0.15f,
                1 - 0.2f,
                1 - 0.25f,
                1 - 0.25f,
                1 - 0.25f,
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
                2000,
                2500,
            });
        public override AdditiveStrategy AddStrategy => AdditiveStrategy;
        public override int MaxLevel { get { return 7; } }
        public override int Tier { get { return 1; } }
    }
}
