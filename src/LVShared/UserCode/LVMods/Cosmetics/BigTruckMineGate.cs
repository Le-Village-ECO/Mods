// Le Village - Grande entrée de mine pour camion 3 de large 4 de haut

using Eco.Core.Controller;
using Eco.Core.Items;
using Eco.Gameplay.Components;
using Eco.Gameplay.Components.Auth;
using Eco.Gameplay.Items;
using Eco.Gameplay.Items.Recipes;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Occupancy;
using Eco.Gameplay.Skills;
using Eco.Shared.Items;
using Eco.Shared.Localization;
using Eco.Shared.Math;
using Eco.Shared.Serialization;
using System;
using System.Collections.Generic;
namespace Eco.Mods.TechTree
{
    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(PaintableComponent))] //TODO: Fix not all the object have this component
    [Tag("Decoration")] //Mise à jour du Tag
    [Ecopedia("Decoration", "Décoration de mine", subPageName: "Grande entrée de mine pour camion")]
    public partial class BigTruckMineGateObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(BigTruckMineGateItem);
        public override LocString DisplayName => Localizer.DoStr("Grande entrée de mine pour camion");
        public override TableTextureMode TableTexture => TableTextureMode.Stone;

        static BigTruckMineGateObject()
        {
            var BlockOccupancyList = new List<BlockOccupancy>
            {
                new BlockOccupancy(new Vector3i(0, 0, 0)),
                new BlockOccupancy(new Vector3i(0, 1, 0)),
                new BlockOccupancy(new Vector3i(0, 2, 0)),
                new BlockOccupancy(new Vector3i(0, 3, 0)),
                new BlockOccupancy(new Vector3i(1, 0, 0)),
                new BlockOccupancy(new Vector3i(1, 1, 0)),
                new BlockOccupancy(new Vector3i(1, 2, 0)),
                new BlockOccupancy(new Vector3i(1, 3, 0)),
                new BlockOccupancy(new Vector3i(2, 0, 0)),
                new BlockOccupancy(new Vector3i(2, 1, 0)),
                new BlockOccupancy(new Vector3i(2, 2, 0)),
                new BlockOccupancy(new Vector3i(2, 3, 0)),
            };
            AddOccupancy<BigTruckMineGateObject>(BlockOccupancyList);
        }
        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Grande entrée de mine pour camion")]
    [LocDescription("Entrée de mine pour camion. Taille : 3 de large et 4 de haut.")] //Maj de la bonne hauteur
    [Ecopedia("Decoration", "Décoration de mine", createAsSubPage: true)]
    [Tag("Decoration")] //Maj Tag
    [Weight(2000)] // Defini le poids de cet objet
    public partial class BigTruckMineGateItem : WorldObjectItem<BigTruckMineGateObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
    }

    /// <summary>
    /// <para>Server side recipe definition for "AdornedAshlarShaleTable".</para>
    /// <para>More information about RecipeFamily objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.RecipeFamily.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>
    [RequiresSkill(typeof(CarpentrySkill), 2)]
    [Ecopedia("Decoration", "Décoration de mine", subPageName: "Grande entrée de mine pour camion")]
    public partial class BigTruckMineGateRecipe : RecipeFamily
    {
        public BigTruckMineGateRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "Grande entrée de mine pour camion",  //noloc
                displayName: Localizer.DoStr("Grande entrée de mine pour camion"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("Wood", 26, typeof(CarpenterSkill), typeof(CarpentryLavishResourcesTalent)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<BigTruckMineGateItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2.5f;

            this.LaborInCalories = CreateLaborInCaloriesValue(480, typeof(CarpentrySkill));

            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(BigTruckMineGateRecipe), start: 8, skillType: typeof(CarpentrySkill), typeof(CarpentryFocusedSpeedTalent), typeof(CarpentryParallelSpeedTalent));

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Grande entrée de mine pour camion"), recipeType: typeof(BigTruckMineGateRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(CarpentryTableObject), recipe: this);
        }
        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }
}
