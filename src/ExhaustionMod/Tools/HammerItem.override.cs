// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.
// Le Village - Suppression des contrôles de calorie/exhaustion

namespace Eco.Mods.TechTree
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Eco.Core.Controller;
    using Eco.Core.Items;
    using Eco.Core.Utils;
    using Eco.Core.Utils.Logging;
    using Eco.Gameplay.Blocks;
    using Eco.Gameplay.DynamicValues;
    using Eco.Gameplay.GameActions;
    using Eco.Gameplay.Interactions;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Rooms;
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
    using static Eco.Gameplay.Civics.IfThenBlock;
    using static Eco.Gameplay.Objects.WorldObjectUtil;

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
            //if (block.Is<Constructed>()) result = AtomicActions.DeleteBlockNow(this.CreateMultiblockContext(player, true, blockPos, null, DeconstructGameAction), addTo: player?.User.Inventory); //Le Village
            //else if (block.Is<Ramp>()) result = AtomicActions.DeleteBlockNow(this.CreateMultiblockContext(player, true, blockPos), addTo: player?.User.Inventory); //Le Village
            if (block.Is<Constructed>()) result = AtomicActions.DeleteBlockNow(CreateMultiblockContext(this, player, true, blockPos.SingleItemAsEnumerable(), null, DeconstructGameAction), addTo: player?.User.Inventory);
            else if (block.Is<Ramp>())   result = AtomicActions.DeleteBlockNow(CreateMultiblockContext(this, player, true, blockPos.SingleItemAsEnumerable()),                              addTo: player?.User.Inventory);
            else                         result = Result.FailLocStr($"Block type {block.GetType()} can not be picked up with this tool").Notify(player.User);
            
            return result;

            ConstructOrDeconstruct DeconstructGameAction() => new() { ConstructedOrDeconstructed = ConstructedOrDeconstructed.Deconstructed };
        }

        /// <summary> Attempts to pick up a World Object. </summary>
        [RPC] public async Task<bool> PickupWorldObject(Player player, WorldObject worldObj)
        {
            //Make necessary pre-action checks, like inventory availability, and any confirmations needed from the player (e.g.: When contracts would be affected after obj is picked up).
            if (worldObj == null) { Result.FailLocStr("This WorldObject has already been picked up.").Notify(player.User); return false; } //This could happen due to lag, if the WorldObject is already picked up.
            if (!await worldObj.CheckForPickUpAsync(player)) return false;

            //var actionPack = worldObj.TryPickUp(new GameActionPack(), player, player.User.Inventory, this.NeededCalories(player), false);  //Le Village
            var actionPack = TryPickUp(worldObj, new GameActionPack(), player, player.User.Inventory, this.NeededCalories(player), false);  //Le village
            //actionPack.UseTool(this.CreateMultiblockContext(player, false));  //Le village
            actionPack.UseTool(CreateMultiblockContext(this, player, false));

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
                    //context: this.CreateMultiblockContext(player, true, layout.Blocks.Select(x => x.Key), layout.Blocks.Select(x => BlockManager.FromId(x.Value))),  //Le Village
                    context: CreateMultiblockContext(this, player, true, layout.Blocks.Select(x => x.Key), layout.Blocks.Select(x => BlockManager.FromId(x.Value))),
                    blockType: blockType,
                    createBlockAction: true,
                    removeFromInv: player.User.Inventory,
                    itemToRemove: player.User.Carrying.Item.GetType());
                var result = pack.TryPerform(player.User);
                return result;
            }
        }

        public static GameActionPack TryPickUp(WorldObject obj, GameActionPack actionPack, Player player, Inventory targetInventory, float caloriesNeeded, bool force, AccessType accessType = AccessType.FullAccess)
        {
            var worldObjectItem = obj.CreationItem ?? WorldObjectItem.MakeItemFromWorldObjectType(obj.GetType());
            DebugUtils.Assert(worldObjectItem != null, $"Failed to get the appropriate item for {obj.Name} ({obj.DisplayName}).");
            if (worldObjectItem == null) return actionPack; //This should normally not happen.

            actionPack.AddGameAction(new PlaceOrPickUpObject
            {
                ActionLocation = obj.Position3i,
                ItemUsed = obj.CreatingItem,
                Citizen = player?.User,
                WorldObject = obj,
                PlacedOrPickedUp = PlacedOrPickedUp.PickingUpObject,
                PlotPosList = null,   //For picking up, we only check the position of the world object.
                AccessNeeded = accessType
            });

            //Attempt to burn the appropriate calories amount. If this makes the action fail, do not proceed.
            //actionPack.BurnCaloriesAndCheckExhaustion(player?.User, false, caloriesNeeded);  //Le Village
            //if (!actionPack.EarlyResult) { actionPack.Dispose(); return actionPack; }  //Le Village

            //Make a changeset and fill it with all the results of the pickup calls on the components, also check for action fail.
            //This must be done before the pickup below, which would otherwise destroy the objects.
            var pickupResult = obj.TryPickupComponents(PickupType.PickupAllComponents, targetInventory, player?.User, actionPack, force);

            if (!actionPack.EarlyResult) { actionPack.Dispose(); return actionPack; }        // world object pick failed, so end action
            else if (!pickupResult.PartialMove)                                                         // can only pickup world object when also picked up all component
            {
                var gameActionPackProperties = typeof(GameActionPack).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
                
                //var log= NLogManager.GetLogWriter("levillage");
                //log.Write("Test");
                //log.Write(string.Join(' ', gameActionPackProperties.Select(p => p.Name).ToList()));

                var changeSetsPropertiesInfo = gameActionPackProperties.First(p => p.Name == "ChangeSets");
                var changeSetsValue = (Dictionary<Type, IGameActionPackChangeSet>)changeSetsPropertiesInfo.GetValue(actionPack)!;

                (changeSetsValue.First().Value as InventoryChangeSet)!.AddItem(worldObjectItem, 1, targetInventory);

                //Clean up the WorldObject if the actions succeeds.
                actionPack.AddPostEffect(() =>
                {
                    foreach (var comp in obj.Components) comp.OnPickup(player);
                    (worldObjectItem as IPersistentData)?.CollectWorldObjectPersistentData(obj);
                    worldObjectItem.OnPickup(obj);
                    obj.Destroy();
                    RoomData.QueuePositionsTest(obj.WorldOccupancy);
                });
            }

            return actionPack;
        }

        public static MultiblockActionContext CreateMultiblockContext(ToolItem tool, Player player, bool applyXPSkill, IEnumerable<Vector3i>? area = null, IEnumerable<Type>? blockTypesInArea = null, Func<GameAction>? gameActionConstructor = null) =>
            new MultiblockActionContext()
            {
                Player = player,
                ActionDescription = tool.DescribeBlockAction,
                Area = area,
                BlockTypesInArea = blockTypesInArea,
                ExperienceSkill = applyXPSkill ? tool.ExperienceSkill : null, //When set to NULL, no AddExperience post effect will be added to the action
                ExperiencePerAction = tool.ExperienceRate.GetCurrentValue(player?.User),
                //CaloriesPerAction = tool.NeededCalories(player),  //Le Village
                ToolUsed = tool,
                RepairableItem = tool,
                GameActionConstructor = gameActionConstructor
            };

    }
}
