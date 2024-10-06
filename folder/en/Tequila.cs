namespace Eco.Mods.TechTree
{
    using System;
    using System.Collections.Generic;
    using Eco.Core.Items;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Items.Recipes;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Skills;
    using Eco.Gameplay.Systems.Messaging.Notifications;
    using Eco.Shared.Localization;
    using Eco.Shared.Math;
    using Eco.Shared.Networking;
    using Eco.Shared.Serialization;
    using Eco.Shared.Time;
    using Eco.Shared.Utils;
    using Eco.World;
    using World = Eco.World.World;


    [Serialized] // Tells the save/load system this object needs to be serialized. 
    [LocDisplayName("Tequila")] // Defines the localized name of the item.
    [Weight(500)] // Defines how heavy the BakedCorn is.
    [Tag("BakedFood")]
    [Ecopedia("Food", "Alcool", createAsSubPage: true)]
    [LocDescription("Bouteille d'alcool, Produit à base de Figue de Barbarie.")] //The tooltip description for the food item.
    public partial class TequilaItem : FoodItem
    {

        public override LocString DisplayNamePlural => Localizer.DoStr("Tequila");

        public override float Calories => 1200;
        public override Nutrients Nutrition => new Nutrients() { Carbs = 5, Fat = 4, Protein = 5, Vitamins = 25 };

        protected override float BaseShelfLife => (float)TimeUtil.HoursToSeconds(72);

        [RPC]
        public string Consume(Player player, bool reactOnly = false)
        {
            // React only allows to only get audio/visual response after client sent consume event from animation
            if (reactOnly)
            {
                player?.User.Stomach.PlayFoodReaction(this);
                return string.Empty;
            }

            // By default item will be consumed as usual on event
            return this.ConsumeInternal(player.User.Inventory.Toolbar.SelectedStack, player.User, true);
        }


        string ConsumeInternal(ItemStack itemStack, User user, bool reaction = false, bool overrideSlot = false)
        {
            bool can_eat = user.Stomach.CanEat(this);
            var message = string.Empty;
            var isItemValid = itemStack?.Item != null && itemStack.Item.TypeID == this.TypeID;  // Checks if tem is not null and of needed type

            if (isItemValid)
            {
                // After food item has been consumed - force clear override slot, cause food can only be eat from there
                itemStack.TryModifyStack(user, -1, () => user.Stomach.CanEat(this), () =>
                {
                    user.OverrideToolbarSlot(overrideSlot ? itemStack.Item : null);      // Update food in override slot, allows for client to start doing eating animation
                    user.Stomach.Eat(this, out message);                            // Fill stomach instantly
                    if (reaction) user.Stomach.PlayFoodReaction(this);
                });
            }

            NotificationManager.ServerMessageToPlayer(Localizer.Do($"Toolbar."), user);

            if (can_eat == true)
            {
                Random rnd = new Random();

                int chance_tp;

                chance_tp = rnd.Next(1, 100);

                NotificationManager.ServerMessageToPlayer(Localizer.Do($"VOMIR"), user);


                if (chance_tp < 100)
                {
                    //NotificationManager.ServerMessageToPlayer(Localizer.Do($"VOMIR"), user);
                    //user.Stomach.ClearCalories(this);
                    user.Stomach.IsAnyCalories();
                    
                }
                else
                {
                    // tp_normal
                }
            }

            return message;
        }
    }


    [RequiresSkill(typeof(MillingSkill), 1)]
    [Ecopedia("Food", "Milling", subPageName: "Tequila Item")]
    public partial class TequilaRecipe : RecipeFamily
    {
        public TequilaRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Tequila",  //noloc
                displayName: Localizer.DoStr("Tequila"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(PricklyPearFruitItem), 15, typeof(BakingSkill), typeof(BakingLavishResourcesTalent)),
                    new IngredientElement(typeof(YeastItem), 3)
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<TequilaItem>(1)
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 1; // Defines how much experience is gained when crafted.

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(25, typeof(MillingSkill));

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(TequilaRecipe), start: 2, skillType: typeof(MillingSkill), typeof(MillingFocusedSpeedTalent), typeof(MillingParallelSpeedTalent));

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Baked Corn"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Tequila"), recipeType: typeof(TequilaRecipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(MillObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }
}