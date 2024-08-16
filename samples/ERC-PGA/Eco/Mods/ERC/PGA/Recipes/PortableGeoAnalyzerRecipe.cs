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
	// Token: 0x02000008 RID: 8
	[RequiresSkill(typeof(ElectronicsSkill), 1)]
	internal class PortableGeoAnalyzerRecipe : RecipeFamily
	{
		// Token: 0x0600000C RID: 12 RVA: 0x0000223C File Offset: 0x0000043C
		public PortableGeoAnalyzerRecipe()
		{
			Recipe recipe = new Recipe();
			recipe.Init("PortableGeoAnalyzer", Localizer.DoStr("Portable Geological Analyzer"), new List<IngredientElement>
			{
				new IngredientElement(typeof(BasicCircuitItem), 2f, true),
				new IngredientElement(typeof(PlasticItem), 1f, true),
				new IngredientElement(typeof(AdvancedCircuitItem), 1f, true),
				new IngredientElement(typeof(BasicBatteryItem), 4f, true)
			}, new List<CraftingElement>
			{
				new CraftingElement<PortableGeoAnalyzerItem>(1f)
			});
			base.Recipes = new List<Recipe>
			{
				recipe
			};
			this.ExperienceOnCraft = 25f;
			base.LaborInCalories = RecipeFamily.CreateLaborInCaloriesValue(350f, typeof(ElectronicsSkill));
			base.CraftMinutes = RecipeFamily.CreateCraftTimeValue(3f);
			base.Initialize(Localizer.DoStr("Portable Geological Analyzer"), typeof(PortableGeoAnalyzerRecipe));
			CraftingComponent.AddRecipe(typeof(ElectricMachinistTableObject), this);
		}
	}
}
