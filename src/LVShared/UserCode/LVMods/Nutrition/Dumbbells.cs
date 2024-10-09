// Le Village - ajout d'un objet qui permet de vider son estomac
// Cela permet de débloquer certains joueurs dans des cas particuliers avec le mod diététique

using Eco.Core.Items;
using Eco.Gameplay.Components;
using Eco.Gameplay.Items;
using Eco.Gameplay.Items.Recipes;
using Eco.Gameplay.Players;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using System.Collections.Generic;

namespace Village.Eco.Mods.Nutrition
{
    [Serialized]
    [Ecopedia("Items", "Products", createAsSubPage: true)]
    [LocDisplayName("Haltéres")]
    [LocDescription("Des haltéres pour garder la forme : utilisez les pour dépenser vos calories.")]
    public class DumbbellsItem : Item
    {
        public override string OnUsed(Player player, ItemStack itemStack)
        {
            var StomachCal = player.User.Stomach.Calories;  //Contenu calorique estomac

            if (StomachCal == 0) return base.OnUsed(player, itemStack);

            float cal = StomachCal > 1000 ? 1000 : StomachCal; //Enlève 1000 calories
            player.User.Stomach.BurnCalories(cal, false);

            player.User.InfoBoxLocStr($"Bel effort, vous avez consommé {(int)cal} calories !");
            return base.OnUsed(player, itemStack);
        }
    }

    [RequiresSkill(typeof(DietSkill), 1)]
    public partial class DumbbellsRecipe : RecipeFamily
    {
        public DumbbellsRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Dumbbells",  //noloc
                displayName: Localizer.DoStr("Dumbbells"),

                ingredients: new List<IngredientElement> 
                {
                    new IngredientElement("Wood", 1,typeof(Skill)), //noloc
                },

                items: new List<CraftingElement> 
                {
                    new CraftingElement<DumbbellsItem>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.LaborInCalories = CreateLaborInCaloriesValue(10);
            this.CraftMinutes = CreateCraftTimeValue(1);

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Dumbbells"), recipeType: typeof(DumbbellsRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ToolBenchObject), recipe: this);
        }
        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }
}
