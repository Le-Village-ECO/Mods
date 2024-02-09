// Le Village Duplication de livre T3

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

    [RequiresSkill(typeof(ResearcherSkill), 1)]
    public partial class BookTier3LV2Recipe : RecipeFamily
    {
        public BookTier3LV2Recipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Book Tier3",  //noloc
                displayName: Localizer.DoStr("Book Tier3"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(BookTier3Item), 1),  //	Duplication d'un Boook de tier 3 en 10 Book de tier 1				
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<BookTier3Item>(10)
                });
            this.Recipes = new List<Recipe> { recipe };

            // Defines how much experience is gained when crafted.
            this.ExperienceOnCraft = 100;  //A définir !!

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(1000);   //1000 calories !!

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(0.5f);  // temps?

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


}

