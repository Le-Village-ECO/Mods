namespace Eco.Mods.TechTree
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Eco.Core.Items;
    using Eco.Gameplay.Blocks;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Components.Auth;
    using Eco.Gameplay.DynamicValues;
    using Eco.Gameplay.Economy;
    using Eco.Gameplay.Housing;
    using Eco.Gameplay.Interactions;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Modules;
    using Eco.Gameplay.Minimap;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Occupancy;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Property;
    using Eco.Gameplay.Skills;
    using Eco.Gameplay.Systems;
    using Eco.Gameplay.Systems.TextLinks;
    using Eco.Gameplay.Pipes.LiquidComponents;
    using Eco.Gameplay.Pipes.Gases;
    using Eco.Shared;
    using Eco.Shared.Math;
    using Eco.Shared.Localization;
    using Eco.Shared.Serialization;
    using Eco.Shared.Utils;
    using Eco.Shared.View;
    using Eco.Shared.Items;
    using Eco.Shared.Networking;
    using Eco.Gameplay.Pipes;
    using Eco.World.Blocks;
    using Eco.Gameplay.Housing.PropertyValues;
    using Eco.Gameplay.Civics.Objects;
    using Eco.Gameplay.Settlements;
    using Eco.Gameplay.Systems.NewTooltip;
    using Eco.Core.Controller;
    using Eco.Core.Utils;
    using Eco.Gameplay.Components.Storage;
    using Eco.Gameplay.Items.Recipes;
    using System.Threading.Tasks;

    [Serialized]
    [LocDisplayName("Lettre")]
    [LocDescription("Un petit bout de papier aux possibilitées infinies. Clic-droit pour écrire.")]
    [Ecopedia("Crafted Objects", "Signs", createAsSubPage: true)]
    [Weight(1000)]
    public partial class LettreItem : Item
    {
        [Serialized, Notify, SyncToView(Flags = Shared.View.SyncFlags.MustRequest)]
        public string Text { get; set; }

        public override string OnUsed(Player player, ItemStack itemStack)
        {
            Task.Run(() => OnUsedAsync(player, itemStack));
            return base.OnUsed(player, itemStack);
        }

        public async Task OnUsedAsync(Player player, ItemStack itemStack)
        {
            var title = Localizer.Do($"Ecrivez votre lettre");
            var localizedText = Localizer.DoStr(Text);
            var text = await player.InputLargeString(title, localizedText);

            if (string.IsNullOrEmpty(text) is false) Text = text;
        }
    }

    [RequiresSkill(typeof(CarpenterSkill), 1)]
    [Ecopedia("Crafted Objects", "Signs", subPageName: "Lettre")]
    public partial class LettreRecipe : RecipeFamily
    {
        public LettreRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Lettres",  //noloc
                displayName: Localizer.DoStr("Lettre Item"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new(typeof(BoardItem), 2, typeof(CarpenterSkill), typeof(CarpentryFocusedSpeedTalent)),
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<LettreItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 1; // Defines how much experience is gained when crafted.

            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(120, typeof(CarpentrySkill));

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(LettreItem), start: 3, skillType: typeof(CarpentryFocusedSpeedTalent), typeof(CarpentryParallelSpeedTalent));

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Hanging Steel Framed Sign"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Lettre Item"), recipeType: typeof(LettreRecipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(CarpentryTableObject), recipe: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }
}