using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Core.Utils.Logging;
using Eco.Gameplay.Items.Recipes;
using Eco.Mods.TechTree;

using System;
using System.Collections.Generic;
using System.Linq;

using Village.Eco.Mods.Cooking;

namespace Equilibrage
{
    public partial class RecettesPlugin : IModKitPlugin, IInitializablePlugin
    {
        public NLogWriter Logger { get; } = NLogManager.GetLogWriter("LeVillageMods");

        public string GetCategory() => "";

        public string GetStatus() => ""; // ajouter nombre de recette modifié

        public void Initialize(TimedTask timer)
        {
            //Logger.Write($"[Equilibrage] - {RecipeManager.AllRecipes.Length} recipes in Manager");
            //Logger.Write($"[Equilibrage] - {RecipeManager.AllRecipeFamilies.Length} recipe families in Manager");
            //Logger.Write($"[Equilibrage] - {RecipeManager.AllRecipeFamilies.SelectMany(rf => rf.Recipes).Count()} recipes in families in Manager");
            //Logger.Write($"[Equilibrage] - {CraftingComponent.AllRecipes.Count()} recipes in Component.");

            ReplaceRecipes();
            ReplaceRecipes();
            //ReplaceRecipes();
            ReplaceRecipes();
            //ReplaceRecipes();

            ////Différentes façon d'obtenir une ou des recettes/familles
            //var byRecipe = RecipeManager.GetRecipeByRecipeType(typeof(MortaredGraniteRecipe));
            //var byItem = RecipeManager.GetRecipeFamiliesForItem(typeof(MeatyStewItem));
            //var byFamily = RecipeManager.GetRecipeFamily<FishStewRecipe>();

            ////Pour logguer toutes les recettes d'une catégorie
            //foreach (var recipe in RecipeManager.AllRecipes) Logger.Write($"[Equilibrage] - {recipe.Name}");

            //Pour déclencher une action après le chargement complet du serveur
            //PluginManager.Controller.RunIfOrWhenInited(delegate);
        }

        public static List<IngredientElement> GetIngredients(Type type)
        {
            if (typeof(RecipeFamily).IsAssignableFrom(type)) return RecipeManager.GetRecipeFamily(type).DefaultRecipe.Ingredients;
            else if (typeof(Recipe).IsAssignableFrom(type)) return RecipeManager.GetRecipeByRecipeType(type).Ingredients;
            else throw new InvalidCastException($"[Equilibrage] - {type.Name} is not Recipe nor RecipeFamily");
        }

        public static void AddIngredients<T>(params IngredientElement[] newIngredients)
        {
            var ingredients = GetIngredients(typeof(T));
            ingredients.AddRange(newIngredients);
        }
        public static void RemoveIngredients<T>(params Type[] oldItems)
        {
            var ingredients = GetIngredients(typeof(T));
            ingredients.RemoveAll(ingre => oldItems.Any(item => ingre.Item.Type == item));
        }
        public static void RemoveIngredients<T>(params string[] oldTags)
        {
            var ingredients = GetIngredients(typeof(T));
            ingredients.RemoveAll(ingre => oldTags.Any(tag => ingre.Tag.Name == tag));
        }
        public static void ReplaceAllIngredients<T>(params IngredientElement[] newIngredients)
        {
            var ingredients = GetIngredients(typeof(T));
            ingredients.Clear();
            ingredients.AddRange(newIngredients);
        }

        public static void AddIngredient(IngredientElement ingredient, params Type[] recipes)
        {
            foreach (var recipe in recipes)
            {
                var ingredients = GetIngredients(recipe);
                ingredients.Add(ingredient);
            }
        }
        public static void RemoveIngredient<T>(params Type[] recipes)
        {
            foreach (var recipe in recipes)
            {
                var ingredients = GetIngredients(recipe);
                ingredients.RemoveAll(ingre => ingre.Item.Type == recipe);
            }
        }
        
        public static void ReplicateIngredients<T>(params Type[] recipes)
        {
            var oldIngredients = GetIngredients(typeof(T));
            foreach (var recipe in recipes)
            {
                var ingredients = GetIngredients(recipe);
                ingredients.Clear();
                ingredients.AddRange(oldIngredients);
            }
        }

        public static void ReplaceRecipes()
        {
            // On remplace la farine par un bol
            RemoveIngredients<FishStewRecipe>(typeof(FlourItem));
            AddIngredients<FishStewRecipe>(new IngredientElement(typeof(WoodenBowlItem), 1, typeof(CampfireCookingSkill), typeof(CampfireCookingLavishResourcesTalent)));

            ReplaceAllIngredients<AcornPowderRecipe>(
                new IngredientElement(typeof(CharredFishItem), 4, typeof(CampfireCookingSkill), typeof(CampfireCookingLavishResourcesTalent)),
                new IngredientElement(typeof(FlourItem), 1, typeof(CampfireCookingSkill), typeof(CampfireCookingLavishResourcesTalent)),
                new IngredientElement("Fat", 1, typeof(CampfireCookingSkill), typeof(CampfireCookingLavishResourcesTalent)));

            RemoveIngredient<FlourItem>(typeof(MeatyStewRecipe), typeof(WildStewRecipe));
            AddIngredient(new IngredientElement(typeof(WoodenBowlItem), 1, typeof(CampfireCookingSkill), typeof(CampfireCookingLavishResourcesTalent)),
                typeof(MeatyStewRecipe), typeof(WildStewRecipe));

            ReplicateIngredients<FishStewRecipe>(typeof(RootCampfireStewRecipe), typeof(JungleCampfireStewRecipe));
        }
    }
}