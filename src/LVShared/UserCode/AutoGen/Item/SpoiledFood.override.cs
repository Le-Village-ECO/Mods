// Le Village
// Ajout d'un Tag pour le stockage dans les nouveaux frigos

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

            
    /// <summary>
    /// <para>Server side item definition for the "SpoiledFood" item.</para>
    /// <para>More information about Item objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.Item.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>
    [Serialized] // Tells the save/load system this object needs to be serialized. 
    [LocDisplayName("Spoiled Food")] // Defines the localized name of the item.
    [Compostable] // Defines if the object is compostable
    [Weight(20)] // Defines how heavy SpoiledFood is.
    [Ecopedia("Items", "Products", createAsSubPage: true)]
    [Tag("Spoiled Food")]
    [LocDescription("Food that has spoiled beyond the point of being suitable for human consumption. It can still be used for compost.")] //The tooltip description for the item.
    public partial class SpoiledFoodItem : Item
    {


    }
}