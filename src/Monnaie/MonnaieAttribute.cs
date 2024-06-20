using Eco.Gameplay.Items;

namespace Village.Eco.Mods.Monnaie
{
    public class MonnaieAttribute : ItemAttribute
    {
        public int Monnaie { get; private set; }

        public MonnaieAttribute(int Monnaie)
        {
            this.Monnaie = Monnaie;
        }
    }
}
