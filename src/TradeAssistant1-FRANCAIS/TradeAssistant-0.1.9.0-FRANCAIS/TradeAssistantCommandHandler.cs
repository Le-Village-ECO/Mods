using Eco.Core.Plugins;
using Eco.Gameplay.Items;
using Eco.Gameplay.Players;
using Eco.Gameplay.Systems;
using Eco.Gameplay.Systems.Messaging.Chat.Commands;
using Eco.Gameplay.Systems.TextLinks;
using Eco.Gameplay.Utils;
using Eco.Shared;
using Eco.Shared.Localization;
using Eco.Shared.Utils;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeAssistant
{
    [ChatCommandHandler]
    public static class TradeAssistantCommandHandler
    {
        [ChatCommand("Affiche les commandes disponibles à partir du module d'assistance commerciale.", "ta")]
        public static void TradeAssistant() { }

        [ChatSubCommand(nameof(TradeAssistant), "LIS MOI D'ABORD !", ChatAuthorizationLevel.User)]
        public static void Help(User user)
        {
            StringBuilder sb = new();
            sb.AppendLine(Localizer.DoStr("Bienvenue dans le module Trade Assistant mod !"));
            sb.AppendLine(Localizer.DoStr("Ce module vous aidera à configurer votre magasin en ajoutant des ordres d'achat et de vente, ainsi qu'en mettant à jour les prix des ordres de vente en fonction de vos configurations de profit que vous avez définies."));
            sb.Append(Localizer.DoStr("- La première étape consiste à exécuter ")).Append(Text.Name("/ta setupsell")).AppendLine(Localizer.DoStr(", cela ajoutera tous les objets que vous pouvez fabriquer à partir des établis de cette propriété aux ordres de vente du magasin. Parcourez cette liste et supprimez les objets qui vous intéressent pas à vendre. Ne vous inquiétez pas pour la mise à jour des prix, le module s'en chargera pour vous plus tard !"));
            sb.Append(Localizer.DoStr("- Ensuite, exécutez la commande ")).Append(Text.Name("/ta setupbuy")).AppendLine(" . Cela parcourra la liste des ordres de vente et ajoutera tous les articles d'entrée nécessaires pour fabriquer ceux-ci dans les ordres d'achat.");
            sb.AppendLine(Localizer.DoStr("- Maintenant, parcourez vos ordres d'achat et définissez les prix d'achat ainsi que les limites d'achat pour chaque article.")); // TODO : Mentionner comment vous pouvez supprimer les articles étiquetés.
            sb.Append(Localizer.DoStr("- Exécutez ")).Append(Text.Name("/ta config")).AppendLine(Localizer.DoStr(" pour définir votre pourcentage de profit souhaité et votre coût par calorie."));
            sb.Append(Localizer.DoStr("- Une fois que vous avez terminé, la dernière étape consiste à exécuter ")).Append(Text.Name("/ta update")).AppendLine(Localizer.DoStr(", cela mettra à jour les prix de tous vos ordres de vente en fonction de votre pourcentage de profit configuré et de vos coûts de main-d'œuvre."));

            user.MsgLocStr(sb.ToString());
        }

        [ChatSubCommand(nameof(TradeAssistant), "Ajoutez des ordres de vente pour tous les produits que vous pouvez fabriquer dans les tables de craft de cette propriété.", ChatAuthorizationLevel.User)]
        public static async Task SetupSell(User user)
        {
            var calc = await TradeAssistantCalculator.TryInitialize(user);
            if (calc == null)
            {
                user.MsgLocStr("Impossible d'initialiser le calculateur.");
                return;
            }
            
            var sb = new StringBuilder();

            var items = calc.CraftableItems.SelectMany(x => x.Value).Select(p => p.TypeID).ToHashSet();
            var soldItems = calc.Store.StoreData.SellOffers.Select(o => o.Stack.Item.TypeID).ToHashSet();
            var itemsToAdd = items.Where(i => !soldItems.Contains(i)).ToList();

            if (!itemsToAdd.Any())
            {
                sb.AppendLine(Localizer.DoStr($"Tous les objets que vous pouvez fabriquer sont déjà ajoutés au magasin."));
                user.MsgLocStr(sb.ToString());
                return;
            }

            foreach (var (table, tableItems) in calc.CraftableItems)
            {
                var addedForThisTable = tableItems.Select(i => i.TypeID).Where(itemsToAdd.Contains);
                if (!addedForThisTable.Any()) { continue; }

                calc.Store.CreateCategoryWithOffers(user.Player, addedForThisTable.ToList(), false);
                var category = calc.Store.StoreData.SellCategories.Last();
                category.Name = table;
            }
            foreach (var offer in calc.Store.StoreData.SellOffers.Where(o => itemsToAdd.Contains(o.Stack.Item.TypeID)))
                offer.Price = 999999;

            sb.AppendLine(Localizer.DoStr($"Ajouté {Text.Info(Text.Num(itemsToAdd.Count))} ordres de vente. Ouvrez le magasin et supprimez les articles qui ne vous intéressent pas à vendre dans votre boutique."));
            user.MsgLocStr(sb.ToString());
        }
        
        [ChatSubCommand(nameof(TradeAssistant), "Ajoutez des ordres d'achat pour tous les ingrédients nécessaires à la fabrication des produits que vous vendez.", ChatAuthorizationLevel.User)]
        public static async Task SetupBuy(User user)
        {
            var calc = await TradeAssistantCalculator.TryInitialize(user);
            if (calc == null)
                return;

            var sb = new StringBuilder();

            var items = new HashSet<int>();

            // Créer une table de correspondance des produits aux ingrédients nécessaires.
            var allRecipes = calc.ProductToRequiredItemsLookup();

            // Limiter les produits uniquement à ceux que nous vendons.
            var sellOfferTypeIds = calc.Store.StoreData.SellOffers.Select(o => o.Stack.Item.TypeID).ToList();

            // Parcourez la liste des objets fabriquables et trouvez le produit dont nous avons besoin pour les fabriquer.
            var todo = new Queue<int>(calc.CraftableItems.SelectMany(x => x.Value).Select(p => p.TypeID).Distinct().Where(sellOfferTypeIds.Contains));
            var done = new HashSet<int>();
            while (todo.TryDequeue(out var productId))
            {
                if (!done.Add(productId)) continue;
                var itemsToQueue = allRecipes[productId].Where(allRecipes.ContainsKey).ToList();
                var itemsToBuy = allRecipes[productId].Where(i => !itemsToQueue.Contains(i));
                items.AddRange(itemsToBuy);
                itemsToQueue.ForEach(todo.Enqueue);
            }

            items.RemoveRange(calc.Store.StoreData.BuyOffers.Select(o => o.Stack.Item.TypeID));
            if (items.Count == 0)
            {
                user.TempServerMessage(Localizer.DoStr($"Les ordres d'achat pour tous les ingrédients possibles nécessaires à la fabrication des produits de vente actuels sont déjà répertoriés dans le magasin."));
                return;
            }


            calc.Store.CreateCategoryWithOffers(user.Player, items.ToList(), true);
            foreach (var offer in calc.Store.StoreData.BuyOffers.Where(o => items.Contains(o.Stack.Item.TypeID)))
            {
                offer.Limit = 1;
                offer.Price = 0;
            }
            user.TempServerMessage(Localizer.DoStr($"{Text.Info(Text.Num(items.Count))} Les ordres d'achat ont été ajoutés, veuillez ouvrir le magasin et définir le prix et la limite pour chacun d'eux."));
        }


        [ChatSubCommand(nameof(TradeAssistant), "Mettez à jour le prix de vente de tout dans le magasin en fonction du profit configuré et du coût par calorie", ChatAuthorizationLevel.User)]
        public static async Task Update(User user)
        {
            var calc = await TradeAssistantCalculator.TryInitialize(user);
            if (calc == null) return;

            var storeSellItemIds = calc.Store.StoreData.SellOffers.Where(o => !calc.Config.FrozenSellPrices.Contains(o.Stack.Item.TypeID)).Select(o => o.Stack.Item.TypeID).Distinct().ToList();
            if (storeSellItemIds.Count == 0)
            {
                user.TempServerMessage(Localizer.Do($"{calc.Store.Parent.UILink()} n'a pas d'ordres de vente."));
                return;
            }
            var buyProductsToUpdate = calc.Store.StoreData.BuyOffers.Select(o => o.Stack.Item.TypeID).Distinct().Where(storeSellItemIds.Contains).ToList();

            var byProducts = calc.Config.ByProducts.ToHashSet();
            var craftableItems = calc.CraftableItems.SelectMany(x => x.Value).Select(x => x.TypeID).Distinct().ToHashSet();
            var output = new StringBuilder();
            var updates = new List<LocString>();
            var warnings = new List<LocString>();


            foreach (var itemID in storeSellItemIds.OrderBy(p => byProducts.Contains(p) ? 0 : 1))
            {
                if (calc.TryGetCostPrice(itemID, out var price, out var reason, out var itemWarnings))
                {
                    // PrixDeVente = Taxe + PrixDeRevient * (1 + Profit)
                    // PrixDeVente = PrixDeVente * TauxDeTaxe + PrixDeRevient * (1 + Profit)
                    // PrixDeVente - PrixDeVente * TauxDeTaxe = PrixDeRevient * (1 + Profit)
                    // PrixDeVente * (1 - TauxDeTaxe) = PrixDeRevient * (1 + Profit)
                    // PrixDeVente = PrixDeRevient * (1 + Profit) / (1 - TauxDeTaxe)
                    var newPrice = Mathf.RoundToAcceptedDigits(price * (1 + calc.Config.Profit / 100f) / (1 - calc.Store.GetTax()));
                    updates.AddRange(calc.SetSellPrice(itemID, newPrice));
                    if (itemWarnings != null)
                        warnings.AddRange(itemWarnings);
                }
                else if (!byProducts.Contains(itemID) && craftableItems.Contains(itemID))
                    output.AppendLineLoc($"Échec de l'obtention du prix de revient de {Item.Get(itemID).UILink()} ({TextLoc.FoldoutLoc($"Pourquoi ?", $"Pourquoi {Item.Get(itemID).UILink()}", reason.ToStringLoc())}).");
            }

            foreach (var itemID in buyProductsToUpdate)
                if (calc.TryGetCostPrice(itemID, out var price, out var _, out var _))
                    updates.AddRange(calc.SetBuyPrice(itemID, price));

            // TODO : Je ne suis pas satisfait de la façon dont les avertissements fonctionnent en ce moment. Je les supprime pour l'instant.
            //if (warnings.Any())
            //    output.AppendLine(warnings.Select(w => "- " + w).Distinct().FoldoutListLoc("warning", Eco.Shared.Items.TooltipOrigin.None));

            if (updates.Count == 0 && output.Length == 0)
                user.TempServerMessage(Localizer.Do($"Tous les prix sont à jour !"));
            else
            {
                output.Insert(0, Localizer.Do($"Mise à jour des prix de vente à {calc.Store.Parent.UILink()}\n"));
                updates.ForEach(u => output.AppendLine(u));
                if (updates.Count > 0)
                    output.AppendLineLoc($"Prix(s) de mise à jour de {Text.StyledNum(updates.Count)} commande(s)");
                user.TempServerMessage(output.ToStringLoc());
            }
        }
        [ChatSubCommand(nameof(TradeAssistant), "Explique comment le prix de vente est calculé pour un produit", ChatAuthorizationLevel.User)]
        public static async Task Explain(User user, string itemName, User whoToSendTo = null)
        {
            var calc = await TradeAssistantCalculator.TryInitialize(user);
            if (calc == null) return;

            var item = CommandsUtil.ClosestMatchingEntity(user, itemName, Item.AllItemsExceptHidden, x => x.GetType().Name, x => x.DisplayName);
            if (item == null) return;
            if (calc.TryGetCostPrice(item, out var price, out var reason, out var _))
            {
                var msg = new StringBuilder();
                if (whoToSendTo != null)
                    msg.AppendLineLoc($"{user.UILink()} vous a partagé cette explication de prix : ");
                else
                    whoToSendTo = user;
                var newPrice = Mathf.RoundToAcceptedDigits(price * (1 + calc.Config.Profit / 100f) / (1 - calc.Store.GetTax()));
                var costPriceLink = TextLoc.FoldoutLoc($"Prix De Revient", $"Prix de revient de {item.UILink()}", reason.ToStringLoc());
                msg.AppendLineLoc($"Prix de vente pour {item.UILink()}");
                msg.AppendLineLoc($"Prix De Vente = {costPriceLink} * (1 + Profit) / (1 - Taux De Taxe)");
                msg.AppendLineLoc($"          = {Text.StyledNum(price)} * (1 + {Text.StyledNum(calc.Config.Profit / 100f)}) / (1 - {Text.StyledNum(calc.Store.GetTax())})");
                msg.AppendLineLoc($"          = {Text.StyledNum(newPrice)}");
                whoToSendTo.TempServerMessage(msg.ToStringLoc());
            }
            else
                user.TempServerMessage(reason.ToStringLoc());
        }


        [ChatSubCommand(nameof(TradeAssistant), "Ouvre une fenêtre pour modifier vos configurations", ChatAuthorizationLevel.User)]
        public static void Config(User user)
        {
            if (!TradeAssistantData.Obj.UserConfiguration.TryGetValue(user.Id, out var config))
            {
                config = new UserConfig();
                TradeAssistantData.Obj.UserConfiguration.Add(user.Id, config);
            }

            ViewEditorUtils.PopupUserEditValue(user, typeof(UserConfigUI), Localizer.DoStr("Trade Assistant Configuration"), config.ToUI(), null, OnSubmit);
            void OnSubmit(object entry)
            {
                if (entry is UserConfigUI uiConfig)
                    config.UpdateFromUI(uiConfig);
                StorageManager.Obj.MarkDirty(TradeAssistantData.Obj);
            }
        }
    }
}