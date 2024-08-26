
namespace Eco.Mods.TechTree
{
    using System.ComponentModel;
    using Eco.Core.Items;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Players;
    using Eco.Shared.Localization;
    using Eco.Shared.Serialization;
    using Eco.Shared.Utils;
    using Eco.Shared.Time;
    using Eco.Core.Controller;
    using Eco.Gameplay.Items.Recipes;

    [Serialized] // Tells the save/load system this object needs to be serialized. 
    [LocDisplayName("Pop Corn")] // Defines the localized name of the item.
    [Category("Hidden")] // Hides this object from the player. It is not recommended you remove this tag.
    [Weight(500)] // Defines how heavy the CampfireStew is.
    [LocDescription("A thick stew chock-full of meat, camas, and corn. A surprisingly good combination.")] //The tooltip description for the food item.
    public partial class PopCornItem : FoodItem
    {


        /// <summary>The amount of calories awarded for eating the food item.</summary>
        public override float Calories => 1200;
        /// <summary>The nutritional value of the food item.</summary>
        public override Nutrients Nutrition => new Nutrients() { Carbs = 5, Fat = 9, Protein = 10, Vitamins = 4 };

        /// <summary>Defines the default time it takes for this item to spoil. This value can be modified by the inventory this item currently resides in.</summary>
        protected override float BaseShelfLife => (float)TimeUtil.HoursToSeconds(72);
    }

}