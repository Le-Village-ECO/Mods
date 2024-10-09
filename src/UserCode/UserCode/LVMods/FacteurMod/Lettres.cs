// Le Village - Lettre pour le mod facteur
// TODO - Passer de RepairableItem à DurabilityItem pour simplifier

using Eco.Core.Controller;
using Eco.Core.Items;
using Eco.Gameplay.Components;
using Eco.Gameplay.DynamicValues;
using Eco.Gameplay.Items;
using Eco.Gameplay.Items.Recipes;
using Eco.Gameplay.Players;
using Eco.Gameplay.Skills;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eco.Mods.TechTree
{
    [Serialized]
    [LocDisplayName("Lettre")]
    [LocDescription("Un petit bout de papier aux possibilitées infinies. Clic-droit pour écrire.")]
    [Ecopedia("Crafted Objects", "Signs", createAsSubPage: true)]
    [Weight(1000)]
    public partial class LettreItem : RepairableItem
    {
        // Implémentation du RepairableItem
        public override IDynamicValue SkilledRepairCost => skilledRepairCost;
        private static IDynamicValue skilledRepairCost = new ConstantValue(1);
        public override LocString BrokenDescription => Localizer.Do($"Trop d'utilisation l'a rendu fragile et inutilisable.");

        [Serialized, Notify, SyncToView(Flags = Shared.View.SyncFlags.MustRequest)]
        public string Text { get; set; }

        public override string OnUsed(Player player, ItemStack itemStack)
        {
            //Vérification de la durabilité
            var item = itemStack.Item as RepairableItem;
            if (item.Durability == 0) DisplayLetter(player, itemStack); //Affichage de la lettre
            else Task.Run(() => OnUsedAsync(player, itemStack));  //Modification de la lettre

            return base.OnUsed(player, itemStack);
        }

        //Pour modifier la lettre
        public async Task OnUsedAsync(Player player, ItemStack itemStack)
        {
            var title = Localizer.Do($"Ecrivez votre lettre");
            var localizedText = Localizer.DoStr(Text);
            var text = await player.InputLargeString(title, localizedText);

            if (string.IsNullOrEmpty(text) is false) Text = text;

            //Mise à jour de la durabilité
            var item = itemStack.Item as RepairableItem;
            item.Durability -= 10f;  //Réduction en %
            if (item.DurabilityPercent > 0f && item.DurabilityPercent <= 0.5f)
                player.InfoBoxLocStr($"La {itemStack.Item.DisplayName} commence à s'abimée ({item.DurabilityPercent * 100}%).");
        }

        //Pour uniquement afficher le contenu de la lettre
        public void DisplayLetter(Player player, ItemStack itemStack) 
        {
            var title = Localizer.Do($"Affichage du contenu (lettre trop abimée pour être modifiée)");
            var text = Localizer.DoStr(Text);
            player.LargeInfoBox(title,text);
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
                    new(typeof(WoodPulpItem), 2, typeof(CarpenterSkill), typeof(CarpentryFocusedSpeedTalent)),
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