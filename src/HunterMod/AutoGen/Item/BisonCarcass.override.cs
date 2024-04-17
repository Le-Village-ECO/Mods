// Le Village
// Modification de la classe Item en FoodItem afin de rendre la carcasse périssable
// Modification du OnUsed de la carcasse pour empecher le clic droit/gauche

namespace Eco.Mods.TechTree
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Eco.Gameplay.Blocks;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.DynamicValues;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Skills;
    using Eco.Gameplay.Settlements;
    using Eco.Gameplay.Systems;
    using Eco.Gameplay.Systems.TextLinks;
    using Eco.Shared.Localization;
    using Eco.Shared.Serialization;
    using Eco.Shared.Utils;
    using Eco.Core.Items;
    using Eco.World;
    using Eco.World.Blocks;
    using Eco.Gameplay.Pipes;
    using Eco.Core.Controller;
    using Eco.Gameplay.Items.Recipes;
    using Eco.Gameplay.Systems.NewTooltip;
    using Eco.Shared.Items;

    [Serialized] // Tells the save/load system this object needs to be serialized. 
    [LocDisplayName("Bison Carcass")] // Defines the localized name of the item.
    [Weight(5500)] // Defines how heavy BisonCarcass is.
    [Ecopedia("Natural Resources", "Animal", createAsSubPage: true)]
    [Tag("Carcasse")]
    [LocDescription("A dead bison.")] //The tooltip description for the item.
    public partial class BisonCarcassItem : FoodItem //Modification de la carcasse de bison en FoodItem
    {
        public override LocString DisplayNamePlural => Localizer.DoStr("Bison Carcass");
        public override float Calories => 0;

        public override Nutrients Nutrition => new Nutrients() { Carbs = 0, Fat = 0, Protein = 0, Vitamins = 0 };

        /// <summary>Defines the default time it takes for this item to spoil. This value can be modified by the inventory this item currently resides in.</summary>
        protected override float BaseShelfLife => (float)TimeUtil.HoursToSeconds(144);

                    
        //Suppression de l'action de manger dans les lignes suivantes
        // On modifie via l'override du parent (FoodItem dans notre cas) afin que le OnUsed affiche un message vide plutôt que de faire le OnUsed classique du FoodItem
        public override string OnUsed(Player player, ItemStack itemStack) 
        {
            return string.Empty;
        }
    }
}