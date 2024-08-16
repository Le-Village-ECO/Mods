using Eco.Core.Controller;
using Eco.Gameplay.Components;
using Eco.Gameplay.Items;
using Eco.Gameplay.Items.Recipes;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Occupancy;
using Eco.Gameplay.Skills;
using Eco.Gameplay.Systems.NewTooltip;
using Eco.Mods.TechTree;
using Eco.Shared.Items;
using Eco.Shared.Localization;
using Eco.Shared.Math;
using Eco.Shared.Serialization;
using System;
using System.Collections.Generic;

namespace Village.Eco.Mods.Templates
{
    [Serialized]
    [RequireComponent(typeof(OnOffComponent))]
    public partial class TemplateRecipeObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(TemplateRecipeItem);
        public override LocString DisplayName => Localizer.DoStr("Template Object");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;
        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.ModsPostInitialize();
        }
        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Template Object")]
    public partial class TemplateRecipeItem : WorldObjectItem<TemplateRecipeObject>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }

    [RequiresSkill(typeof(SurvivalistSkill), 1)]
    public partial class TemplateRecipeRecipe : RecipeFamily
    {
        public TemplateRecipeRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "TemplateRecipe",  //noloc
                displayName: Localizer.DoStr("Template Recipe"),

                ingredients: new List<IngredientElement>
                {
                    new (typeof(PaperItem), 1, true),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<TemplateRecipeItem>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.LaborInCalories = CreateLaborInCaloriesValue(10);
            this.CraftMinutes = CreateCraftTimeValue(5);

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Template Recipe"), recipeType: typeof(TemplateRecipeRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(WorkbenchObject), recipe: this);
        }
        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }
}