// Le Village

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

    [RequiresSkill(typeof(ResearcherSkill), 1)]
    public partial class BookTier3Recipe : RecipeFamily
    {
        public BookTier3Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Book Tier3",  //noloc
                displayName: Localizer.DoStr("Book Tier3"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(MillingResearchPaperBasicItem), 20),  //Mouture
                    new IngredientElement(typeof(AgricultureResearchPaperModernItem), 20),  //Engrais
					new IngredientElement(typeof(EngineeringResearchPaperModernItem), 20),  //Mécanique
//					new IngredientElement(typeof(...), 10),  // Cuisine n'existe pas
					new IngredientElement(typeof(MetallurgyResearchPaperModernItem), 20),  //Fonte avancée
					new IngredientElement(typeof(CulinaryResearchPaperAdvancedItem), 20),  //Boulangerie
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<BookTier3Item>()
                });
            this.Recipes = new List<Recipe> { recipe };
			
            // Defines how much experience is gained when crafted.
			this.ExperienceOnCraft = 100;  //A définir !!
            
            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(3000);  //3000 calories 
            
            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(5760);  //4 jours !!

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Paper"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Book Tier3"), recipeType: typeof(BookTier3Recipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(LaboratoryObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }
    
    /// <summary>
    /// <para>Server side item definition for the "Paper" item.</para>
    /// <para>More information about Item objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.Item.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>
    [Serialized] // Tells the save/load system this object needs to be serialized. 
    [LocDisplayName("Book Tier3")] // Defines the localized name of the item.
    [Ecopedia("Items", "Products", createAsSubPage: true)]
    [Tag("Book Tier")]
    public partial class BookTier3Item : Item
    {
        /// <summary>The tooltip description for the item.</summary>
        public override LocString DisplayDescription { get { return Localizer.DoStr("C'est le livre du Tier 3"); } }
    }
}