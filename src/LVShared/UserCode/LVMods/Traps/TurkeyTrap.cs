// Le Village - Piège à dindons

using System;
using System.Collections.Generic;
using Eco.Core.Items;
using Eco.Gameplay.Components;
using Eco.Gameplay.Components.Auth;
using Eco.Gameplay.Items;
using Eco.Gameplay.Items.Recipes;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Occupancy;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Items;
using Eco.Shared.Localization;
using Eco.Shared.Math;
using Eco.Shared.Serialization;

namespace Village.Eco.Mods.Traps
{
    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(MinimapComponent))]
    //[RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [Tag("Usable")]
    [Ecopedia("Crafted Objects", "Specialty", subPageName: "Turkey Trap Item")]
    public partial class TurkeyTrapObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(TurkeyTrapItem);
        public override LocString DisplayName => Localizer.DoStr("Piège à dindon");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<MinimapComponent>().SetCategory(Localizer.DoStr("Misc"));
            this.ModsPostInitialize();
        }

        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Piège à dindon")]
    [LocDescription("Un piege à dindon")]
    [IconGroup("World Object Minimap")]
    [Ecopedia("Crafted Objects", "Specialty", createAsSubPage: true)]
    [Weight(500)]
    public partial class TurkeyTrapItem : WorldObjectItem<TurkeyTrapObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));

    }

    [RequiresSkill(typeof(HuntingSkill), 2)]
    [Ecopedia("Crafted Objects", "Specialty", subPageName: "Turkey Trap Item")]
    public partial class TurkeyTrapRecipe : RecipeFamily
    {
        public TurkeyTrapRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "TurkeyTrap",  //noloc
                displayName: Localizer.DoStr("Piège à dindon"),

                ingredients: new List<IngredientElement>
                {
                    new("Rock", 5, true),
                    new("Wood", 3, true),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<TurkeyTrapItem>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.ExperienceOnCraft = 3;
            this.LaborInCalories = CreateLaborInCaloriesValue(120, typeof(HuntingSkill));
            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(TurkeyTrapRecipe), start: 4, skillType: typeof(HuntingSkill));

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Piège à dindon"), recipeType: typeof(TurkeyTrapRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(WorkbenchObject), recipe: this);
        }
        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }
}
