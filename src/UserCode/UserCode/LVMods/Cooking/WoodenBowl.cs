// Le Village - Ajout d'un bol en bois (même recette que les chevilles en bois) à utiliser dans des recettes de cuisine

using Eco.Core.Items;
using Eco.Gameplay.Components;
using Eco.Gameplay.Items;
using Eco.Gameplay.Items.Recipes;
using Eco.Gameplay.Players;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Village.Eco.Mods.Cooking
{
    [RequiresSkill(typeof(LoggingSkill), 1)]
    [Ecopedia("Items", "Products", subPageName: "Wooden Bowl Item")]
    public partial class WoodenBowlRecipe : RecipeFamily
    {
        public WoodenBowlRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Wooden Bowl",  //noloc
                displayName: Localizer.DoStr("Wodden Bowl"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("Wood", 2,typeof(LoggingSkill)), //noloc
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<WoodenBowlItem>(16)
                });
            this.Recipes = new List<Recipe> { recipe };

            this.ExperienceOnCraft = 0.5f;
            this.LaborInCalories = CreateLaborInCaloriesValue(40, typeof(LoggingSkill));
            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(WoodenBowlRecipe), start: 0.4f, skillType: typeof(LoggingSkill));

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Wooden Bowl"), recipeType: typeof(WoodenBowlRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(CarpentryTableObject), recipe: this);
        }
        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Bol en bois")]
    [LocDescription("Un joli bol en bois.")]
    [Ecopedia("Items", "Products", createAsSubPage: true)]
    [Weight(50)]
    public partial class WoodenBowlItem : Item
    {
        public static void GiveTo(User user)
        {
            // 75% chance de rendre un bol
            if (new Random().Next(100) > 75) return;

            var result = user.Inventory.TryAddItemNonUnique(typeof(WoodenBowlItem));
            if (result.Success) user.InfoBoxLocStr("Chanceux ! Tu as récupéré un bol.");
        }
    }
}
