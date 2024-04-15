// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Gameplay.Items
{
    using System;
    using Eco.Core.Controller;
    using Eco.Gameplay.Systems;
    using Eco.Shared.Items;
    using Eco.Shared.Localization;
    using Eco.Shared.Serialization;
    using Eco.Shared.Utils;

    /// All food items that are new start with a durability of 100%, when creating the item an ImmutableCountdown is created, what this does is take the time from when the item was created until it becomes spoiled.
    /// It should be clarified that every time the item changes state, a new counter is generated with the new parameters, that is, if the item changes storage or the item's durability changes for some reason, the total time is calculated and the remaining time as well. (taking into account the time that has already passed), and another ImmutableCountdown is created.
    /// There may be the occasion that there is a storage that preserves the food indefinitely, such as the square pot at the time of writing this comment.
    /// In this case, the immutable countdown can generate buggs with infinite time, therefore a paused counter is created, and the decomposition would not occur.
    /// If the Shelf life changes (gets in a storage that preserves food), the times change relative to each other, meaning that the total shelf life is increased by a multiplier and also the time remaining for the same multiplier.
    /// therefore the quality will not be improved, it will simply take longer to get spoiled.

    /// <summary> Food Items are items that spoil over time, See DurabilityItems.md </summary>
    public abstract partial class CarcassItem : DurabilityItem, IStackableMergable
    {
        //Time when food gets spoiled in world time reference, this has the modifier backed in.
        ImmutableCountdown spoilageTime;
        [Serialized, SyncToView(Flags = Shared.View.SyncFlags.MustRequest)]
        public ImmutableCountdown SpoilageTime
        {
            get => this.spoilageTime;
            set => this.spoilageTime = value;
        }

        protected abstract float BaseShelfLife { get; } //The amount of time in seconds before a fresh food becomes spoiled.

        public override LocString TooltipMaximumDurability() { return LocString.Empty; }
        public override float MinDefaultDurability => 50;
        public override ItemCategory ItemCategory => CarcassItem;  
        public override bool CanBeHeld => false;

        float AdjustedShelfLife => this.BaseShelfLife * this.shelfLifeMultiplier * (BalanceConfig.Obj?.ShelfLifeMultiplier ?? 1); // Returns the base time of the food item adjusted with the storage shelf life multiplier and the server balance multiplier.
        float shelfLifeMultiplier = 1;                                                                                            // Multiplier that the item has in extra shelf life, for being in the current storage.

        bool HasInfiniteShelfLife => float.IsInfinity(this.AdjustedShelfLife);

        //Defining 3 different states of durability so they will not be merged together when items are rearranged or when adding new ones.
        public int StackableQualityGroup() => (int)(this.GetDurability() / 34); // Divide it by 34 since we have to take into account the decimals, if we used 33, 4 categories would be generated and we only want 3, bad, average, and good quality.

        /// <summary> Update Durability value before merging to items to apply correct durability value. </summary>
        public Item Merge(Item another, int first, int second)
        {
            // If another is null, or we are resting from current (second < 0), the durability won't be changed, since there are nothing to merge
            if (second <= 0 || another == null) return this;
            if (another.Type != this.Type) throw new Exception("Trying to merge different kinds of food, this shouldn't be possible");

            //We won't edit the same item to prevent situations with having 2 stacks with same same item instance
            var CarcassItem = (CarcassItem)this.Clone;
            var firstDurability = this.GetDurability() * first;                               // Calculate first item durability modifier.
            var secondDurability = ((CarcassItem)another).GetDurability() * second;              // Calculate second item durability modifier.
            var averageDurability = (firstDurability + secondDurability) / (first + second);  // Calculate average druability between the two items based on the count of each side being merged.
            CarcassItem.SpoilageTime = this.GetSpoilageTimeBasedOnDurability(averageDurability); // Calculate Spoilage time based on average durability.
            return CarcassItem;
        }

        public override Item Clone
        {
            get
            {
                var copy = (CarcassItem)base.Clone;
                copy.spoilageTime = this.spoilageTime;
                copy.shelfLifeMultiplier = this.shelfLifeMultiplier;
                return copy;
            }
        }

        public void SetSpoilageTimeBasedOnDurability(float durability) => this.SpoilageTime = this.GetSpoilageTimeBasedOnDurability(durability);

        /// <summary> Sets the spoilage time based on durability or updates the durability with spoilage progress if the storage is still the same. </summary>
        public void UpdateSpoilageTime(float shelfLifeMultiplier = 1)
        {
            var cachedDurability = this.SpoilageTime.Duration() > 0 ? this.GetDurabilityBasedOnSpoilageTime(this.SpoilageTime) : DurabilityMax; // Initialize cached value with current durability value.(100 for new items).
            this.shelfLifeMultiplier = shelfLifeMultiplier;                                                                                     // Set multiplier to use later.
            this.SpoilageTime = this.GetSpoilageTimeBasedOnDurability(cachedDurability);                                                        // Update Spoilage time with the new set durability, applying the storage shelf life multiplier.
        }

        public override float GetDurability()
        {
            if (this.SpoilageTime.Duration() == 0) return DurabilityMax;                                                                        // If Spoilage time isn't inited, just return max durability since it isn't inited. 
            return this.GetDurabilityBasedOnSpoilageTime(this.SpoilageTime);
        }

        /// <summary> Create a new immutable countdown based on the given durability value and the current storage modifier. </summary>
        public ImmutableCountdown GetSpoilageTimeBasedOnDurability(float durability, bool paused = false)
        {
            // Get current adjusted shelf life based of the current shelf life multiplier, this is the duration for the countdown. If the adjusted shelf life is infinite then make the duration the base shelf life,
            // since immutable countdown can generate errors with infinite duration (it doesn't matter tho since the time remaining is not gonna be seen by the player when its infinite).
            var duration = this.AdjustedShelfLife;
            return float.IsInfinity(duration) ? ImmutableCountdown.CreatePaused(float.MaxValue) : ImmutableCountdown.Create(duration, duration * (durability / 100), paused); // Create immutableCountdown with duration and time left and depending if shelf life is infinite create it running or paused.
        }

        float GetDurabilityBasedOnSpoilageTime(ImmutableCountdown spoilageTime) => spoilageTime.PercentLeft() * 100f;   // Get Durability based on world time and spoilage time delta.
    }
}
