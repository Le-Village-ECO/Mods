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
    [LocDisplayName("Pastis")] // Defines the localized name of the item.
    [Weight(500)] // Defines how heavy the BakedCorn is.
    [Tag("BakedFood")]
    [Ecopedia("Food", "Alcool", createAsSubPage: true)]
    [LocDescription("Bouteille d'alcool concentré, je sens que j'ai une chance sur cinq de finir dans les vapes")] //The tooltip description for the food item.
    public partial class PastisItem : FoodItem
    {
        public override LocString DisplayNamePlural => Localizer.DoStr("Pastis");

        public override float Calories => 700;
        public override Nutrients Nutrition => new Nutrients() { Carbs = 4, Fat = 25, Protein = 5, Vitamins = 5 };

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

            NotificationManager.ServerMessageToPlayer(Localizer.Do($"Je crois que j'ai trop bu ... Ou suis-je ???"), user);

            if (can_eat == true)
            {
                Random rnd = new Random();

                int chance_tp;
                int x;
                int y;
                int y_min;
                int z;

                chance_tp = rnd.Next(1, 100);

                if (chance_tp <= 20)
                {
                    // tp aleatoire
                    x = rnd.Next(0, 1000);
                    y = (int)user.Position.Y + 1;
                    z = rnd.Next(0, 1000);

                    var pos2d = new Vector2i(x, z);

                    y_min = World.GetTopBlockY(pos2d) + 1;

                    if (y < y_min)
                    {
                        y = y_min;
                    }

                    // update player position
                    user.Player.SetPosition(pos2d.X_Z(y));
                }
                else
                {
                    // tp_normal
                }
            }

            return message;
        }

        public override string OnUsed(Player player, ItemStack itemStack)
        {
            var mounted = player.Mount?.IsMounted ?? false;  // No need to try play food animation while mounted (ik problems, jiggles, etc)
            return this.ConsumeInternal(itemStack, player.User, false, !mounted);
        }

        public override void OnLeftClicked(Player player, ItemStack itemStack)
        {
            var mounted = player.Mount?.IsMounted ?? false;  // No need to try play food animation while mounted (ik problems, jiggles, etc)
            this.ConsumeInternal(itemStack, player.User, false, !mounted);
        }
    }

    [RequiresSkill(typeof(MillingSkill), 1)]
    [Ecopedia("Food", "Milling", subPageName: "Pastis Item")]
    public partial class PastisRecipe : RecipeFamily
    {
        public PastisRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Pastis",  //noloc
                displayName: Localizer.DoStr("Pastis"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(CamasBulbItem), 15, typeof(BakingSkill), typeof(BakingLavishResourcesTalent)),
                    new IngredientElement(typeof(YeastItem), 3)
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<PastisItem>(1)
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 1; // Defines how much experience is gained when crafted.

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(25, typeof(MillingSkill));

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(PastisRecipe), start: 2, skillType: typeof(MillingSkill), typeof(MillingFocusedSpeedTalent), typeof(MillingParallelSpeedTalent));

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Baked Corn"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Pastis"), recipeType: typeof(PastisRecipe));
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