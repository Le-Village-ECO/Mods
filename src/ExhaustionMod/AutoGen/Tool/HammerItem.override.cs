// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.
// Le village : retirer le coût calorique si épuisé

namespace Eco.Mods.TechTree
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Eco.Core.Controller;
    using Eco.Core.Items;
    using Eco.Core.Utils;
    using Eco.Gameplay.Blocks;
    using Eco.Gameplay.DynamicValues;
    using Eco.Gameplay.GameActions;
    using Eco.Gameplay.Interactions;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Utils;
    using Eco.Mods.TechTree;
    using Eco.Shared.Items;
    using Eco.Shared.Localization;
    using Eco.Shared.Math;
    using Eco.Shared.Networking;
    using Eco.Shared.Serialization;
    using Eco.Shared.Utils;
    using Eco.World;
    using Eco.World.Blocks;

    [Serialized]
    [LocDisplayName("Hammer")]
    [LocDescription("Used to construct buildings and pickup manmade objects.")]
    [Category("Hidden")]
    [CanMakeBlockForm, Tag("Construction")]
    public abstract class HammerItem : ToolItem
    {
        static IDynamicValue tier = new ConstantValue(0);
        static IDynamicValue caloriesBurn = new ConstantValue(1);
        private static IDynamicValue skilledRepairCost = new ConstantValue(1);

        public override ItemCategory ItemCategory { get { return ItemCategory.Hammer; } }

        public override IDynamicValue SkilledRepairCost             { get { return skilledRepairCost; } }
        [SyncToView] public override IDynamicValue Tier             => base.Tier; // tool tier, overriden to have SyncToView only for hammer
        public override IDynamicValue CaloriesBurn                  { get { return caloriesBurn; } }

        public override bool IsValidForInteraction(Item item)
        {
            var blockItem = item as BlockItem;
            return !(item is LogItem) && blockItem != null && Block.Is<Constructed>(blockItem.OriginType);
        }

        public override LocString GetNoSuitablePickupTargetFailureMessage(Inventory inventory)
        {
            if (!inventory.IsEmpty) return Localizer.DoStr("Object storage must be empty to pick up.");
            return base.GetNoSuitablePickupTargetFailureMessage(inventory);
        }

        /// <summary> Attempts to pick up the block at given position. </summary>
        [RPC] public bool PickupBlock(Player player, Vector3i blockPos)
        {
            var block = World.GetBlock(blockPos);
            if (block.Is<Empty>()) { Result.FailLocStr("This block has already been picked up.").Notify(player.User); return false; } //This could happen due to lag, if the block is already picked up.

            //If the block is a ramp don't create a new ConstructOrDeconstruct GameAction, because that GameAction will be created inside RampItem.RampPickupOverride (special case because the item have multiple blocks) and we don't want a duplicate.
            //TO DO : After refactoring ramps, the ramp-specific check will need to be removed so that when removing ramps using a hammer item, it will just use the same logic as other blocks.
            Result result;
            if (block.Is<Constructed>()) result = AtomicActions.DeleteBlockNow(this.CreateMultiblockContext(player, true, blockPos, null, DeconstructGameAction), addTo: player?.User.Inventory);
            else if (block.Is<Ramp>())   result = AtomicActions.DeleteBlockNow(this.CreateMultiblockContext(player, true, blockPos),                              addTo: player?.User.Inventory);
            else                         result = Result.FailLocStr($"Block type {block.GetType()} can not be picked up with this tool").Notify(player.User);
            
            return result;

            ConstructOrDeconstruct DeconstructGameAction() => new() { ConstructedOrDeconstructed = ConstructedOrDeconstructed.Deconstructed };
        }

        /// <summary> Attempts to pick up a World Object. </summary>
        [RPC] public async Task<bool> PickupWorldObject(Player player, WorldObject worldObj)
        {
            //Make necessary pre-action checks, like inventory availability, and any confirmations needed from the player (e.g.: When contracts would be affected after obj is picked up).
            if (worldObj == null)       { Result.FailLocStr("This WorldObject has already been picked up.").Notify(player.User); return false; } //This could happen due to lag, if the WorldObject is already picked up.
            if (!await worldObj.CheckForPickUpAsync(player)) return false;

            var actionPack = worldObj.TryPickUp(new GameActionPack(), player, player.User.Inventory, this.NeededCalories(player), false);
            actionPack.UseTool(this.CreateMultiblockContext(player, false));

            //.. and if all go well, then try to perform. Notify the user if any errors occur (e.g.: low calories, no Auth, etc).
            var result = actionPack.TryPerform(player.User);
            return result;
        }

        /// <summary> Attempts to place a layout of blocks at given position, with given rotation. </summary>
        [RPC] public bool Place(Player player, BlockLayout layout, int rotation)
        {
            if (player.User.Carrying.Item == null) { Result.FailLocStr("No carried item found.").Notify(player.User); return false; } //This could happen due to lag, if the block item is already placed.

            var form      = player.User.Inventory.Carried.SelectedForm?.FormType.Name;
            var blockType = BlockFormManager.GetBlockTypeToCreate(player, player.User.Avatar.HeldItem, (BlockItem) player.User.Carrying.Item, form, rotation);

            // Create a game action pack, fill it with a multiblock context, and try to perform the action.
            using (var pack = new GameActionPack()) 
            {
                pack.PlaceBlock(
                    context: this.CreateMultiblockContext(player, true, layout.Blocks.Select(x => x.Key), layout.Blocks.Select(x => BlockManager.FromId(x.Value))),
                    blockType: blockType,
                    createBlockAction: true,
                    removeFromInv: player.User.Inventory,
                    itemToRemove: player.User.Carrying.Item.GetType());
                var result = pack.TryPerform(player.User);
                return result;
            }
        }
    }
}
