using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Core.Utils.Logging;
using Eco.Gameplay.Items.Recipes;
using Eco.Mods.TechTree;
using Eco.Shared.Utils;

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
            //ReplaceFlour();  //Suite cycle 12, retour arrière sur le retrait de la farine - Recettes étaient trop peu chère par rapport au début de la cuisine
            AddBowls();

            ////Pour logguer toutes les recettes d'une catégorie
            //foreach (var recipe in RecipeManager.AllRecipes) Logger.Write($"[Equilibrage] - {recipe.Name}");

            ////Pour déclencher une action après le chargement complet du serveur
            //PluginManager.Controller.RunIfOrWhenInited(delegate);
        }

        public static List<IngredientElement> GetIngredients(Type type)
        {
            if (typeof(RecipeFamily).IsAssignableFrom(type)) return RecipeManager.GetRecipeFamily(type).DefaultRecipe.Ingredients;
            //else if (typeof(Recipe).IsAssignableFrom(type)) return RecipeManager.GetRecipeByRecipeType(type).Ingredients;
            else throw new InvalidCastException($"[Equilibrage] - {type.Name} is not Recipe nor RecipeFamily");
        }

        public static void AddIngredients<T>(params IngredientElement[] newIngredients)
        {
            var ingredients = GetIngredients(typeof(T));
            ingredients.AddRange(newIngredients);
        }
        public static void RemoveIngredients<T>(params string[] ingredientNames)
        {
            var ingredients = GetIngredients(typeof(T));
            ingredients.RemoveAll(i => ingredientNames.Any(n => i.InnerName == n));
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
        public static void RemoveIngredient(string ingredientName, params Type[] recipes)
        {
            foreach (var recipe in recipes)
            {
                var ingredients = GetIngredients(recipe);
                ingredients.RemoveAll(ingre => ingre.InnerName == ingredientName);
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

        public static void ReplaceFlour()
        {
            // List de nouveaux ingrédients (voir mod correspondant)
            var bowlIngredient = new IngredientElement(typeof(WoodenBowlItem), 1, true);

            // On remplace tous les ingredients d'une recette
            ReplaceAllIngredients<FishStewRecipe>(
                new IngredientElement(typeof(CharredFishItem), 4, typeof(CampfireCookingSkill), typeof(CampfireCookingLavishResourcesTalent)),
                //new IngredientElement(typeof(FlourItem), 1, typeof(CampfireCookingSkill), typeof(CampfireCookingLavishResourcesTalent)),
                bowlIngredient,
                new IngredientElement("Fat", 1, typeof(CampfireCookingSkill), typeof(CampfireCookingLavishResourcesTalent)));

            // On remplace la farine par un bol sur une recette
            // On peut aussi le faire sur plusieurs ingredients à la fois
            RemoveIngredients<WildStewRecipe>(typeof(FlourItem).Name);
            AddIngredients<WildStewRecipe>(bowlIngredient);

            // On remplace la farine par un bol sur plusieurs recettes à la fois
            RemoveIngredient(typeof(FlourItem).Name,
                typeof(MeatyStewRecipe), typeof(FieldCampfireStewRecipe), typeof(RootCampfireStewRecipe), typeof(JungleCampfireStewRecipe));
            AddIngredient(bowlIngredient,
                typeof(MeatyStewRecipe), typeof(FieldCampfireStewRecipe), typeof(RootCampfireStewRecipe), typeof(JungleCampfireStewRecipe));

            // On peut aussi appliquer les ingredients d'une recette vers plusieurs autres recettes
            //ReplicateIngredients<xxxRecipe>(typeof(yyyRecipe), typeof(zzzRecipe));
        }

        public static void AddBowls() 
        {
            // List de nouveaux ingrédients (voir mod correspondant)
            var bowlIngredient = new IngredientElement(typeof(WoodenBowlItem), 1, true);

            //Ajout d'un bol sur plusieurs recettes de ragout à la fois
            AddIngredient(bowlIngredient,
                typeof(MeatyStewRecipe), typeof(FieldCampfireStewRecipe), typeof(RootCampfireStewRecipe), typeof(JungleCampfireStewRecipe),
                typeof(WildStewRecipe), typeof(FishStewRecipe));
        }
    }
}