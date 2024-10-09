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
    using Eco.Shared.Time;



    /// <summary>
    /// <para>Server side item definition for the "ElkCarcass" item.</para>
    /// <para>More information about Item objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.Item.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>
    [Serialized] // Tells the save/load system this object needs to be serialized. 
    [LocDisplayName("Elk Carcass")] // Defines the localized name of the item.
    [Weight(3000)] // Defines how heavy ElkCarcass is.
    [Ecopedia("Natural Resources", "Animal", createAsSubPage: true)]
    [Tag("MediumCarcass")]
    [Tag("MediumLeatherCarcass")]
    [Tag("Carcasse")]
    [LocDescription("A dead elk.")] //The tooltip description for the item.
    public partial class ElkCarcassItem : FoodItem
    {
        public override LocString DisplayNamePlural => Localizer.DoStr("Elk Carcass");
        public override float Calories => 0;

        public override Nutrients Nutrition => new Nutrients() { Carbs = 0, Fat = 0, Protein = 0, Vitamins = 0 };

        /// <summary>Defines the default time it takes for this item to spoil. This value can be modified by the inventory this item currently resides in.</summary>
        protected override float BaseShelfLife => (float)TimeUtil.HoursToSeconds(144);

        //Suppression de l'action de manger dans les lignes suivantes
        // On modifie via l'override du parent (FoodItem dans notre cas) afin que le OnUsed affiche un message vide plutôt que de faire le OnUsed classique du FoodItem
        public override string OnUsed(Player player, ItemStack itemStack)
        {
            return $"Durée de conservation restante {SpoilageTime.TimeLeft().SecToDays():F1} jour(s).";
        }
    }
}