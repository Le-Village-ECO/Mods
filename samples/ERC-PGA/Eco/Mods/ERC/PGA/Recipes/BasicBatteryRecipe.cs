using System;
using System.Collections.Generic;
using Eco.Gameplay.Components;
using Eco.Gameplay.Items.Recipes;
using Eco.Gameplay.Skills;
using Eco.Mods.ERC.PGA.Items;
using Eco.Mods.TechTree;
using Eco.Shared.Localization;

namespace Eco.Mods.ERC.PGA.Recipes
{
	// Token: 0x02000007 RID: 7
	[RequiresSkill(typeof(ElectronicsSkill), 1)]
	internal class BasicBatteryRecipe : RecipeFamily
	{
		// Token: 0x0600000B RID: 11 RVA: 0x00002114 File Offset: 0x00000314
		public BasicBatteryRecipe()
		{
			Recipe recipe = new Recipe();
			recipe.Init("BasicBattery", Localizer.DoStr("Basic Battery"), new List<IngredientElement>
			{
				new IngredientElement(typeof(IronPlateItem), 2f, true),
				new IngredientElement(typeof(CopperWiringItem), 5f, true),
				new IngredientElement(typeof(PlasticItem), 1f, true),
				new IngredientElement(typeof(EpoxyItem), 1f, true)
			}, new List<CraftingElement>
			{
				new CraftingElement<BasicBatteryItem>(2f)
			});
			base.Recipes = new List<Recipe>
			{
				recipe
			};
			this.ExperienceOnCraft = 4f;
			base.LaborInCalories = RecipeFamily.CreateLaborInCaloriesValue(80f, typeof(ElectronicsSkill));
			base.CraftMinutes = RecipeFamily.CreateCraftTimeValue(0.2f);
			base.Initialize(Localizer.DoStr("Basic Battery"), typeof(BasicBatteryRecipe));
			CraftingComponent.AddRecipe(typeof(ElectricMachinistTableObject), this);
		}
	}
}
