// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System;
using Eco.Gameplay.Components;
using Eco.Gameplay.Components.Auth;
using Eco.Core.Controller;
using Eco.Core.Items;
using Eco.Core.Systems;
using Eco.Core.Utils;
using Eco.Core.PropertyHandling;
using Eco.Gameplay.Auth;
using Eco.Gameplay.Economy;
using Eco.Gameplay.GameActions;
using Eco.Gameplay.Items;
using Eco.Gameplay.Items.PersistentData;
using Eco.Gameplay.Items.SearchAndSelect;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Players;
using Eco.Gameplay.Systems.Chat;
using Eco.Gameplay.Systems.NewTooltip;
using Eco.Gameplay.Systems.Messaging.Notifications;
using Eco.Gameplay.Systems.TextLinks;
using Eco.Gameplay.Utils;
using Eco.Gameplay.Economy;
using Eco.Shared.Items;
using Eco.Shared.Localization;
using Eco.Shared.Networking;
using Eco.Shared.Serialization;
using Eco.Shared.Services;
using Eco.Shared.Utils;
using PropertyChanged;
using Eco.Shared.SharedTypes;
using Eco.Gameplay.Economy.Transfer;
using Eco.Gameplay.Economy.Transfer.Internal; //Todo: refactor to not need this, by not using account change set.
using Eco.Gameplay.Components.Storage;
using Eco.Core.Properties;
using Eco.Shared.Localization.ConstLocs;

namespace Village.Eco.Mods.Monnaie
{

    [Serialized, Priority(0), LocDescription("Create and mint new coins for currencies.")]
    [RequireComponent(typeof(LinkComponent))]
    [RequireComponent(typeof(InOutLinkedInventoriesComponent))] // Mint only has output, so we should add a new component later that only has input
    [RequireComponent(typeof(NameDataTrackerComponent))]
    [RequireComponent(typeof(AuthDataTrackerComponent))]
    [HasIcon]
    [Tag("Economy")]
    [Ecopedia]
    public class MintComponent2 : WorldObjectComponent, IPersistentData, INotifyPropertyChanged
    {
        [DoNotNotify] public override WorldObjectComponentClientAvailability Availability => WorldObjectComponentClientAvailability.UI;

        private const float DefaultNumCoinsPerItem = 10;

        public static List<MintComponent2> AllMints = new List<MintComponent2>();

        public static ThreadSafeAction<User, Currency> CurrencyCreatedEvent = new();
        [Serialized, SyncToView, DoNotNotify] public SearchAndSelectItem SelectedItem { get; private set; } = new SearchAndSelectItem(BlockTags.Currency, Localizer.DoStr("Choose mint item"), true);
        [DoNotNotify]                         public ItemStack           Stack        => this.SelectedItem.Stack;

        [SyncToView, DoNotNotify] public string      CurrencyName     => this.MintData.Currency?.Name ?? string.Empty;
        [SyncToView, DoNotNotify] public float       TotalCirculation => this.MintData.Currency?.Circulation ?? 0;
        
        [SyncToView, DoNotNotify] public float       CoinsPerItem     => this.MintData.Currency?.CoinsPerItem ?? DefaultNumCoinsPerItem;
        [SyncToView, Eco]         public BankAccount TargetAccount    { get; set; }

        [SyncToView, Serialized, NewTooltipChildren(CacheAs.Instance)] public MintItemData MintData { get; set; }
        [DoNotNotify] public object PersistentData { get => this.MintData; set => this.MintData = value as MintItemData; }

        private LinkComponent link;

        public override void OnCreate()
        {
            if (this.Parent.Creator != null)
                this.TargetAccount = BankAccountManager.Obj.GetPersonalBankAccount(this.Parent.NameOfCreator);
        }

        public override void Initialize()
        {
            this.MintData ??= new MintItemData();
            AllMints.Add(this);
            this.link = this.Parent.GetComponent<LinkComponent>();

            if (this.TargetAccount == null) this.TargetAccount = this.GetDefaultAccount();
            this.SetupBackingItem();

            this.WatchPropOnPropAndCall(this.MintData, nameof(Currency), nameof(Currency.Circulation), () => this.Changed(nameof(this.TotalCirculation)));
        }

        void SetupBackingItem()
        {
            if (this.MintData.Currency != null && this.SelectedItem.Item == null)
            {
                this.SelectedItem.ReplaceStack(null, (int)(this.MintData.Currency.BackingItem?.TypeID ?? 0), 1);
                this.Changed("SelectedItem");
            }
        }

        public void InitializeCurrency(Currency currencyHandle)
        {
            this.MintData.SetCurrency(currencyHandle);
            if (this.MintData.Currency != null && this.MintData.Currency.BackingItem != null)
            {
                this.SelectedItem.ReplaceStack(null, (int)this.MintData.Currency.BackingItem.TypeID, 1);
                this.Changed("SelectedItem");
            }
        }

        public override void Destroy() => AllMints.Remove(this);

        private BankAccount GetDefaultAccount() => BankAccountManager.Obj.GetPersonalBankAccount(this.Parent.NameOfCreator);

        [RPC] public void CurrencyReport(Player player) => CurrencyManager.CurrencyReport(player, (Currency)this.MintData.Currency);
        [RPC] public bool SetCurrencyName(Player player, string currencyName) => this.SetCurrencyName(player, currencyName, false);
        [RPC] public bool CreateCurrency(Player player, string currencyName) => this.SetCurrencyName(player, currencyName, true);

        private Result SetCurrencyName(Player player, string currencyName, bool allowCreation)
        {
            if (!allowCreation && this.MintData.Currency == null) return Result.FailedNoMessage;          // Return with no message
            if (!this.Parent.Enabled) { player.ErrorLocStr("This mint is currently disabled, check status for details."); return Result.FailedNoMessage; }

            return this.SetCurrencyNameNoCheck(player.User, new LocString(currencyName.StripTags())); // Assume the passed in name is localized already.
        }

        public Result SetCurrencyNameNoCheck(User user, LocString currencyName)
        {
            var result = Result.Succeeded;
            currencyName = currencyName.Trim();
            if (string.IsNullOrEmpty(currencyName) || currencyName.Length < RegistrarConstants.MinNameLength || currencyName.Length > RegistrarConstants.MaxNameLength || currencyName.ToString().ToLower() == Localizer.DoStr("none"))
            {
                user.Player?.ErrorLocStr("Invalid currency name.");
                result = Result.FailedNoMessage;
            }
            else if (CurrencyManager.Currencies.Any(x => x.Name.EqualsCaseInsensitive(currencyName) && x.Name != this.CurrencyName))
            {
                user.Player?.ErrorLocStr("That currency name already exists at another mint.");
                result = Result.FailedNoMessage;
            }
            else
            {
                var createNew = this.MintData.Currency == null;
                if (createNew)
                {
                    var performResult = new CreateCurrency
                    {
                        ActionLocation = this.Parent.Position3i,
                        ItemUsed = this.SelectedItem?.Item,
                        Citizen = user
                    }.TryPerform(user);
                    if (!performResult) return performResult;

                    this.MintData.SetCurrency(CurrencyManager.AddCurrency(user, currencyName, CurrencyType.Backed));
                }
                var oldName = this.MintData.Currency.Name;
                CurrencyManager.Registrar.Rename(this.MintData.Currency, currencyName, true);
                EconomyManager.Obj.MarkDirty();
                CurrencyCreatedEvent?.Invoke(user, this.MintData.Currency);
                if (createNew) NotificationManager.ServerMessageToAll(Localizer.Do($"{user.UILink()} created the currency {this.MintData.Currency.UILink()}."), NotificationCategory.Trades);
                else           NotificationManager.ServerMessageToAll(Localizer.Do($"{user.UILink()} renamed currency {Text.Currency(oldName)} to {this.MintData.Currency.UILink()}."), NotificationCategory.Trades);
            }

            // Update the currency name even if it failed, so the UI in the mint will update back.
            this.Changed(nameof(this.CurrencyName));
            return result;
        }

        public Result CraftCoinsResult(Player player, int itemQuantity, float coinsPerItem, BankAccount account)
        {
            if (this.MintData.Currency == null || string.IsNullOrEmpty(this.CurrencyName)) { player.ErrorLocStr("Need to give your currency a name before you can craft it."); return Result.FailedNoMessage; }
            if (account == null)                                                           { player.Error(Localizer.DoStr("No account selected")); return Result.FailedNoMessage; }
            if (this.TotalCirculation <= 0)
            {
                if (this.SelectedItem.Item == null || this.SelectedItem.Item == Item.Get(typeof(SearchAndSelectItem)))  { player.ErrorLoc($"Need to give your currency a backing item before you can craft it."); return Result.FailedNoMessage; }
                if (coinsPerItem <= 0)                                                                                  { player.ErrorLocStr("Can't make a currency with coins per item less or equal to zero."); return Result.FailedNoMessage; }

                // Le Village
                if (coinsPerItem > 5) { player.ErrorLocStr($"Can't make more than 5 coins per item - {this.SelectedItem.Item}"); }

                this.MintData.Currency.BackingItem = this.SelectedItem.Item;
                this.MintData.Currency.CoinsPerItem = coinsPerItem;
                this.TargetAccount = account;

                this.MintData.Currency.Changed(nameof(this.MintData.Currency.BackingItem));
                this.MintData.Currency.Changed(nameof(this.MintData.Currency.CoinsPerItem));
                this.Changed(nameof(this.CoinsPerItem));
                this.Changed(nameof(this.TargetAccount));
            }

            // check account access for emission
            if (account != this.TargetAccount && !account.CanAccess(player.User, sendNoticeOnFail: true)) return Result.FailedNoMessage;

            if (itemQuantity <= 0) { player.ErrorLoc($"You must craft more than zero coins."); return Result.FailedNoMessage; }

            // remove ingredients
            var totalCurrency = this.MintData.Currency?.TotalCurrency ?? 0;
            var amountCreated = itemQuantity * this.CoinsPerItem;
            var pack = new GameActionPack(new MintCurrency
            {
                CurrencyAmount              = itemQuantity,
                Currency                    = this.MintData.Currency,
                NewCurrencyCreated          = amountCreated,
                TotalCurrencyBeforeCreation = totalCurrency,
                TotalCurrencyAfterCreation  = totalCurrency + amountCreated,
                BankAccount                 = account,
                ActionLocation              = this.Parent.Position3i,
                ItemUsed                    = this.SelectedItem?.Item,
                Citizen                     = player.User,
                WorldObject                 = this.Parent,
            });

            var inventories        = this.link.GetSortedLinkedInventoriesAndUser(player);
            var inventoryChangeSet = pack.GetOrCreateInventoryChangeSet(inventories, player.User);
            inventoryChangeSet.RemoveItems(this.Stack.Item?.Type, itemQuantity);

            var accountChangeResult = pack.GetAccountChangeSet().AddChange(null, account, this.MintData.Currency, amountCreated, Localizer.Do($"{player.User.MarkedUpName} minted new currency"));
            pack.EarlyResult.AppendLine(accountChangeResult.Message);

            pack.AddPostEffect(() => player.MsgLoc($"You successfully created {this.MintData.Currency.UILink(amountCreated)}, you now have {account.DisplayAmount(this.MintData.Currency)} in account {account.UILink()}."));
            pack.AddPostEffect(() => NotificationManager.ServerMessageToAll(Localizer.Do($"{player.User.UILink()} created {this.MintData.Currency.UILink(amountCreated)}."), NotificationCategory.Trades, NotificationStyle.Info, new[] { player.User }));

            var result = pack.TryPerform(player.User);
            if (!result) return result;

            this.Changed(nameof(this.TotalCirculation));

            return Result.Succeeded;
        }
        public void UpdateCirculation(Currency changedCurrency)
        {
            if (this.MintData.Currency == changedCurrency) this.Changed(nameof(this.TotalCirculation)); //When changing total circulation of a currency also change circulation values in corresponding mints
        }

        [RPC]
        public bool CraftCoins(Player player, int itemQuantity, float coinsPerItem, BankAccount account)
        {
            if (!this.Parent.Enabled) { player.ErrorLocStr("This mint is currently not functional, check status for details."); return Result.FailedNoMessage; }
            return this.CraftCoinsResult(player, itemQuantity, coinsPerItem, account);
        }
    }
}
