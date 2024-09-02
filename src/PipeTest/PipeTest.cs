namespace Eco.Mods.TechTree
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Eco.Gameplay.Blocks;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.DynamicValues;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Skills;
    using Eco.Gameplay.Systems;
    using Eco.Gameplay.Systems.TextLinks;
    using Eco.Core.Items;
    using Eco.Shared.Localization;
    using Eco.Shared.Serialization;
    using Eco.Shared.Utils;
    using Eco.Shared.SharedTypes;
    using Eco.World;
    using Eco.World.Blocks;
    using Eco.World.Water;
    using Eco.Gameplay.Pipes;
    using Eco.Gameplay.Pipes.LiquidComponents;
    using Eco.Core.Controller;
    using Eco.Gameplay.Items.Recipes;
    using Eco.Shared.Graphics;
    using Eco.World.Color;



    [RequiresSkill(typeof(SmeltingSkill), 1)]
    [Ecopedia("Blocks", "Pipes", subPageName: "Iron Pipe Item")]
    public partial class BlockPipeRecipe : RecipeFamily
    {
        public BlockPipeRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "BlockPipe",  //noloc
                displayName: Localizer.DoStr("Iron Pipe"),

                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronBarItem), 1, typeof(SmeltingSkill), typeof(SmeltingLavishResourcesTalent)),
                },

                items: new List<CraftingElement>
                {
                    new CraftingElement<BlockPipeItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 0.5f; 


            this.LaborInCalories = CreateLaborInCaloriesValue(15, typeof(SmeltingSkill));

            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(BlockPipeRecipe), start: 0.8f, skillType: typeof(SmeltingSkill), typeof(SmeltingFocusedSpeedTalent), typeof(SmeltingParallelSpeedTalent));


            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Iron Pipe"), recipeType: typeof(BlockPipeRecipe));
            this.ModsPostInitialize();


            CraftingComponent.AddRecipe(tableType: typeof(AnvilObject), recipe: this);
        }


        partial void ModsPreInitialize();


        partial void ModsPostInitialize();
    }

    [Serialized]
    [Solid, Constructed]
    [BlockTier(3)]
    [DoesntEncase]
    [RequiresSkill(typeof(SmeltingSkill), 1)]
    public partial class BlockPipeBlock :
        PipeBlock
        , IRepresentsItem
    {
        public virtual Type RepresentedItemType { get { return typeof(BlockPipeItem); } }
    }

    [Serialized]
    [LocDisplayName("Iron Pipe")]
    [LocDescription("A pipe for transporting liquids.")]
    [MaxStackSize(10)]
    [Weight(2000)]
    [Ecopedia("Blocks", "Pipes", createAsSubPage: true)]
    [Tier(3)]
    public partial class BlockPipeItem :

    PipeItem<BlockPipeBlock>
    {

    }

}
