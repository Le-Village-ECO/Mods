using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Core.Utils.Logging;
using Eco.Gameplay.Components;
using Eco.Gameplay.Items.Recipes;
using Eco.Mods.TechTree;
using Eco.Shared.Utils;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Equilibrage
{
    public class RecettesPlugin : IModKitPlugin, IInitializablePlugin
    {
        public NLogWriter Logger { get; set; } = NLogManager.GetLogWriter("LeVillageMods");

        public string GetCategory() => "";

        public string GetStatus() => "";

        public void Initialize(TimedTask timer)
        {
            Logger.Write($"[Equilibrage] - {RecipeManager.AllRecipes.Length} recipes in Manager");
            Logger.Write($"[Equilibrage] - {RecipeManager.AllRecipeFamilies.Length} recipe families in Manager");
            Logger.Write($"[Equilibrage] - {RecipeManager.AllRecipeFamilies.SelectMany(rf => rf.Recipes).Count()} recipes in families in Manager");
            Logger.Write($"[Equilibrage] - {CraftingComponent.AllRecipes.Count()} recipes in Component.");

            ReplaceRecipes();

            ////Différentes façon d'obtenir une ou des recettes/familles
            //var byRecipe = RecipeManager.GetRecipeByRecipeType(typeof(MortaredGraniteRecipe));
            //var byItem = RecipeManager.GetRecipeFamiliesForItem(typeof(MeatyStewItem));
            //var byFamily = RecipeManager.GetRecipeFamily<FishStewRecipe>();

            ////Pour logguer toutes les recettes d'une catégorie
            //foreach (var recipe in RecipeManager.AllRecipes) Logger.Write($"[Equilibrage] - {recipe.Name}");

            ////Pour déclencher une action après le chargement complet du serveur
            //PluginManager.Controller.RunIfOrWhenInited(delegate);
        }

        public static List<IngredientElement> GetIngredients<T>() => typeof(T) switch
        {
            Type t when typeof(Recipe).IsAssignableFrom(t) => RecipeManager.GetRecipeByRecipeType(t).Ingredients,
            Type t when typeof(RecipeFamily).IsAssignableFrom(t) => RecipeManager.GetRecipeFamily<T>().DefaultRecipe.Ingredients,
            _ => throw new InvalidCastException($"[Equilibrage] - {typeof(T)} is not Recipe nor RecipeFamily")
        };

        public static void ReplaceIngredients<T>(params IngredientElement[] newIngredients)
        {
            var oldIngredients = GetIngredients<T>();
            oldIngredients.Clear();
            oldIngredients.AddRange(newIngredients);
        }

        public static void ReplaceRecipes()
        {
            ReplaceIngredients<FishStewRecipe>(
                //new IngredientElement(typeof(CharredFishItem), 4, typeof(CampfireCookingSkill), typeof(CampfireCookingLavishResourcesTalent)),
                //new IngredientElement(typeof(FlourItem), 1, typeof(CampfireCookingSkill), typeof(CampfireCookingLavishResourcesTalent)),
                new IngredientElement("Fat", 1, typeof(CampfireCookingSkill), typeof(CampfireCookingLavishResourcesTalent))
                );
        }
    }
}