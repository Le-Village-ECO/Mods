using Eco.Core.Controller;
using Eco.Core.Items;
using Eco.Gameplay.Components;
using Eco.Gameplay.Items;
using Eco.Gameplay.Items.Recipes;
using Eco.Mods.TechTree;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using System.Collections.Generic;
using Village.Eco.Mods.FacteurMod;

namespace LVShared.UserCode.LVMods.FacteurMod
{

    [Serialized] // Tells the save/load system this object needs to be serialized. 
    [LocDisplayName("Hardwood Board")] // Defines the localized name of the item.
    [Weight(500)] // Defines how heavy HardwoodBoard is.
    [Fuel(2000)]
    [Tag("Fuel")] // Marks HardwoodBoard as fuel item.
    [Tag("Currency")]
    [Currency] // Defines if this item can be used to back a currency
    [Ecopedia("Items", "Products", createAsSubPage: true)]
    [Tag("WoodBoard")]
    [Tag("Burnable Fuel")]
    [LocDescription("A higher quality hardwood board used for long lasting furniture.")] //The tooltip description for the item.
    public partial class HardwoodBoardItem : Item
    {

    }

    [ForceCreateView]
    public partial class BoiteAuxLettresTier3Type2Recipe : Recipe
    {
        public BoiteAuxLettresTier3Type2Recipe()
        {
            this.Init(
                name: "BoiteAuxLettresTier3Type2",  //noloc
                displayName: Localizer.DoStr("Boite aux Lettres Tier3 Type2"),

                ingredients: new List<IngredientElement>
                {
                    new("WoodBoard", 10, typeof(CarpentrySkill), typeof(CarpentryLavishResourcesTalent)), //noloc
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<BoiteAuxLettresTier3Item>(1),
                });
            // Perform post initialization steps for user mods and initialize our recipe instance as a tag product with the crafting system
            this.ModsPostInitialize();
            CraftingComponent.AddTagProduct(typeof(WorkbenchObject), typeof(BoardsRecipe), this);
        }


        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }
}
