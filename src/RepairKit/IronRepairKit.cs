//Mod le Village le kit de réparation remplace le système de réparation vanille du jeu pour le rendre plus polyvalent
//Kit de réparation en fer - Forgeron - Pour les outils en fer

using Eco.Core.Items;
using Eco.Gameplay.Components;
using Eco.Gameplay.Items;
using Eco.Gameplay.Items.Recipes;
using Eco.Gameplay.Skills;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using System.Collections.Generic;
using System.ComponentModel;

namespace Eco.Mods.TechTree
{
    [RequiresSkill(typeof(SmeltingSkill), 1)]
    public partial class IronRepairKitRecipe : RecipeFamily
    {
        public IronRepairKitRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Kit de réparation en fer",  //noloc
                displayName: Localizer.DoStr("Kit de réparation en fer"),

                ingredients: new List<IngredientElement>
                {
                new IngredientElement(typeof(IronBarItem), 4, typeof(SmeltingSkill), typeof(SmeltingLavishResourcesTalent)),
                },

                items: new List<CraftingElement>
                {
                new CraftingElement<IronRepairKitItem>(1),
                });
            this.Recipes = new List<Recipe> { recipe };

            this.ExperienceOnCraft = 1;
            this.LaborInCalories = CreateLaborInCaloriesValue(120, typeof(SmeltingSkill));
            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(IronRepairKitRecipe), start: 2, skillType: typeof(SmeltingSkill), typeof(SmeltingFocusedSpeedTalent), typeof(SmeltingParallelSpeedTalent));

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Kit de réparation en fer"), recipeType: typeof(IronRepairKitRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(AnvilObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Kit de réparation en fer")]
    [Weight(1000)]  //Défini le poids.
    [Category("Tool")]  
    [Tag("RepairKit")]
    [Ecopedia("Items", "Tools", createAsSubPage: true)]  //Page ECOpedia
    [LocDescription("Un kit de réparation pour tous les outils en fer. Popeye en prend au petit déj !")]  //Description détaillée.
    public partial class IronRepairKitItem : Item
    {

    }
}