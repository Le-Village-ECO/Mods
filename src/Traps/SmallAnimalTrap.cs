// Le Village - Piège à petits mamifères (agoutis et lièvres)

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
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [Tag("Usable")]
    [Ecopedia("Crafted Objects", "Specialty", subPageName: "Small Animal Trap Item")]
    public partial class SmallAnimalTrapObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(SmallAnimalTrapItem);
        public override LocString DisplayName => Localizer.DoStr("Small Animal Trap");
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
    [LocDisplayName("Small Animal Trap")]
    [LocDescription("Un piege a renard")]
    [IconGroup("World Object Minimap")]
    [Ecopedia("Crafted Objects", "Specialty", createAsSubPage: true)]
    [Weight(500)]
    public partial class SmallAnimalTrapItem : WorldObjectItem<SmallAnimalTrapObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));

    }

    [RequiresSkill(typeof(HuntingSkill), 3)]
    [Ecopedia("Crafted Objects", "Specialty", subPageName: "Small Animal Trap Item")]
    public partial class SmallAnimalTrapRecipe : RecipeFamily
    {
        public SmallAnimalTrapRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "SmallAnimalTrap",  //noloc
                displayName: Localizer.DoStr("Small Animal Trap"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("HewnLog", 30, typeof(HuntingSkill)), //noloc
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<SmallAnimalTrapItem>()
                });
            this.Recipes = new List<Recipe> { recipe };

            this.ExperienceOnCraft = 3;
            this.LaborInCalories = CreateLaborInCaloriesValue(120, typeof(HuntingSkill));
            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(SmallAnimalTrapRecipe), start: 4, skillType: typeof(HuntingSkill));

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Small Animal Trap"), recipeType: typeof(SmallAnimalTrapRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(WorkbenchObject), recipe: this);
        }
        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }
}
