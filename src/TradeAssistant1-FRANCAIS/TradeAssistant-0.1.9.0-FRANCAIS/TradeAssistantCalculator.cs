using Eco.Gameplay.Components;
using Eco.Gameplay.Components.Store;
using Eco.Gameplay.DynamicValues;
using Eco.Gameplay.Items;
using Eco.Gameplay.Items.Recipes;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Players;
using Eco.Gameplay.Property;
using Eco.Gameplay.Systems.NewTooltip;
using Eco.Gameplay.Systems.TextLinks;
using Eco.Shared;
using Eco.Shared.Items;
using Eco.Shared.Localization;
using Eco.Shared.Math;
using Eco.Shared.Utils;
using Eco.Shared.Voxel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeAssistant
{
    public class TradeAssistantCalculator
    {
        private record CachedPrice(float Price, StringBuilder Reason, Recipe Recipe = null, List<LocString> Warnings = null);
        private record IngredientPrice(float Price, Item Item, LocString Reason, List<LocString> Warnings);
        private record ProductPrice(CraftingElement Product, float Price, LocString Reason);
        private List<CraftingComponent> CraftingTables { get; }
        private List<User> Users { get; }
        private Dictionary<int, float> StoreBuyPrices { get; }

        private Dictionary<int, float> StoreSellPrices { get; }
        private Dictionary<int, CachedPrice> CachedPrices { get; } = new();

        public UserConfig Config { get; }
        public StoreComponent Store { get; }
        public Dictionary<LocString, List<Item>> CraftableItems { get; }

        public const int WORLD_OBJECT_CAP_NUMBER = 1;
        public const int TOOL_CAP_NUMBER = 5;

        private TradeAssistantCalculator(StoreComponent store, List<CraftingComponent> craftingTables, Dictionary<LocString, List<Item>> craftableItems, User user, UserConfig config)
        {
            Store = store;
            CraftingTables = craftingTables;
            CraftableItems = craftableItems;
            Users = config.PartnerPlayers.Append(user.Id).ToHashSet().Select(id => UserManager.FindUserByID(id)).ToList();
            Config = config;

            StoreBuyPrices = store.StoreData.BuyOffers
                .GroupBy(o => o.Stack.Item.TypeID)
                .ToDictionary(x => x.Key, x => x.Max(o => o.Price));

            StoreSellPrices = store.StoreData.SellOffers
                .GroupBy(o => o.Stack.Item.TypeID)
                .ToDictionary(x => x.Key, x => x.Min(o => o.Price));
        }

        public static async Task<TradeAssistantCalculator> TryInitialize(User user)
        {
            var sb = new StringBuilder();
            var deed = GetDeed(sb, user);
            if (deed == null)
            {
                user.MsgLocStr(sb.ToString());
                return null;
            }

            var store = await GetStore(sb, user, deed);
            if (store == null)
            {
                user.MsgLocStr(sb.ToString());
                return null;
            }

            if (TryGetStoreAndCraftingTables(sb, user, deed, out var craftingTables, out var craftableItems))
            {
                // Si le pourcentage de bénéfice est fixé à -100 %, les calculs du mod ne fonctionneront pas.
                if (user.Config().Profit == -100f)
                {
                    user.TempServerMessage(Localizer.Do($"La calcul des prix ne fonctionnera pas, votre bénéfice est fixé à -100 %."));
                    return null;
                }

                return new TradeAssistantCalculator(store!, craftingTables!, craftableItems!, user, user.Config());
            }

            user.MsgLocStr(sb.ToString());
            return null;
        }

        public Dictionary<int, List<int>> ProductToRequiredItemsLookup()
        {
            return CraftingTables
                .SelectMany(w => w.Recipes.Where(recipe => Users.Any(user => recipe.RequiredSkills.All(s => s.IsMet(user)))))
                .SelectMany(r => r.Recipes.Skip(r.CraftableDefault ? 0 : 1))
                .SelectMany(r => r.Products.Select(i => new { product = i.Item, r.Ingredients }))
                .SelectMany(x => x.Ingredients.Select(i => new { x.product, ingredient = i }))
                .SelectMany(x => x.ingredient.IsSpecificItem ? new[] { new { x.product, item = x.ingredient.Item } } : x.ingredient.Tag.TaggedItems().Select(t => new { x.product, item = t }))
                .Where(x => !x.item.Hidden)
                .DistinctBy(x => $"{x.product.TypeID}:{x.item.TypeID}")
                .GroupBy(x => x.product.TypeID)
                .ToDictionary(x => x.Key, x => x.Select(y => y.item.TypeID).ToList());
        }

        public bool TryGetCostPrice(int item, out float outPrice, out StringBuilder reason, out List<LocString> warnings) => TryGetCostPrice(Item.Get(item), out outPrice, out reason, out warnings);
        public bool TryGetCostPrice(Item item, out float outPrice, out StringBuilder reason, out List<LocString> warnings)
        {
            warnings = null;

            // Vérifiez si l'utilisateur achète déjà l'article dans le magasin.
            var hasBuyOrder = StoreBuyPrices.TryGetValue(item.TypeID, out var buyPrice);
            var buyReason = Localizer.DoStr($"{Store.Parent.UILink()} a une commande d'achat pour {item.UILink()} à un prix de {Text.StyledNum(buyPrice)}");

            if (hasBuyOrder && !StoreSellPrices.ContainsKey(item.TypeID))
            {
                reason = new StringBuilder(buyReason);
                outPrice = buyPrice;
                return true;
            }

            // Vérifiez si nous n'avons pas déjà calculé le prix de l'article.

            if (CachedPrices.TryGetValue(item.TypeID, out var cachedPrice))
            {
                outPrice = cachedPrice.Price;
                reason = cachedPrice.Reason;
                warnings = cachedPrice.Warnings;
                return !float.IsPositiveInfinity(cachedPrice.Price);
            }
            CachedPrices.Add(item.TypeID, new CachedPrice(float.PositiveInfinity, new StringBuilder(Localizer.Do($"Recette récursive !"))));

            if (Config.FrozenSellPrices.Any(typeID => typeID == item.TypeID))
            {
                var productIsSold = StoreSellPrices.TryGetValue(item.TypeID, out outPrice);
                if (productIsSold)
                {

                    // Prix de vente = Prix de revient * (1 + Bénéfice) / (1 - Taux d'imposition)
                    // Prix de revient = Prix de vente / (1 + Bénéfice) * (1 - Taux d'imposition)
                    var costPrice = outPrice / (1 + Config.Profit / 100f) * (1 - Store.GetTax());
                    reason = new StringBuilder(Localizer.Do($"{item.UILink()} a un prix de vente figé et est vendu à un prix de {outPrice.ToStyledNum()}"));
                    reason.AppendLineLoc($"Prix de revient = Prix de vente / (1 + Bénéfice) * (1 - Taux d'imposition)");
                    reason.AppendLineLoc($"Prix de revient = {outPrice.ToStyledNum()} / (1 + {(Config.Profit / 100f).ToStyledNum()}) * (1 - {Store.GetTax().ToStyledNum()}) = {costPrice.ToStyledNum()}");
                    outPrice = costPrice;
                }
                else
                {
                    outPrice = float.PositiveInfinity;
                    reason = new StringBuilder(Localizer.Do($"Pas de commande de vente pour l'article avec un prix de vente figé : {item.UILink()}"));
                }

                CachedPrices[item.TypeID] = new CachedPrice(outPrice, reason);
                return productIsSold;
            }


            var recipes = CraftingTables.SelectMany(ct => ct.Recipes
                .SelectMany(rf => rf.Recipes.Skip(rf.CraftableDefault ? 0 : 1).Select(r => (ct, r)))
                .Where(x => x.r.Products.Any(i => i.Item.TypeID == item.TypeID))
            ).ToList();

            if (!recipes.Any())
            {
                outPrice = hasBuyOrder ? buyPrice : float.PositiveInfinity;
                reason = new StringBuilder(Localizer.DoStr($"Il n'y a aucune recette sur l'une des tables d'artisanat qui peut fabriquer {item.UILink()}"));
                if (hasBuyOrder) reason.Append("\n\n").AppendLine(buyReason);
                CachedPrices[item.TypeID] = new CachedPrice(outPrice, reason);
                return hasBuyOrder;
            }

            CachedPrice bestPrice = null;
            var rejectedPrices = new List<CachedPrice>();
            var failedRecipes = new List<LocString>();
            foreach (var (craftingTable, recipe) in recipes)
                foreach (var user in Users)
                {
                    var explanation = new StringBuilder();
                    LocString userText = Users.Count == 1 ? LocString.Empty : Localizer.Do($" par {user.UILink()}");

                    // Ignorez les sous-produits s'il y a plus d'un produit, et assurez-vous que le seul produit est l'élément spécifié.
                    var products = recipe.Products;
                    var resourceEfficiencyContext = new ModuleContext(user, craftingTable.Parent.Position, craftingTable.ResourceEfficiencyModule);
                    if (recipe.Products.Count > 1)
                    {
                        products = recipe.Products.Where(p => !Config.ByProducts.Any(byProductId => byProductId == p.Item.TypeID)).ToList();
                        if (products.Count > 1)
                            failedRecipes.AddLoc($"{recipe.UILink()} a plusieurs produits en sortie, spécifiez lesquels sont des sous-produits : {string.Join(", ", products.Select(p => p.Item.UILink()))}");
                        else if (products.Count == 0)
                            failedRecipes.AddLoc($"Tous les produits ({string.Join(", ", recipe.Products.Select(p => p.Item.UILink()))}) du {recipe.UILink()} a été marqué comme déchet, l'autre ne devrait pas l'être. Exécutez {Text.Name("/ta config")} pour définir les sous-produits.");
                        if (products.Count != 1) continue;
                    }
                    var product = products[0];
                    if (product.Item.TypeID != item.TypeID)
                    {
                        failedRecipes.AddLoc($"{recipe.UILink()} a {item.UILink()} comme sous-produit. Le produit principal de cette recette semble être {product.Item.UILink()}.");
                        continue;
                    }

                    // Vérifiez le prix des sous-produits
                    var byProducts = recipe.Products.Where(p => p != product).Select(p => ParseByProduct(p, resourceEfficiencyContext, craftingTable)).ToList();
                    var unsetByProduct = byProducts.Where(p => float.IsPositiveInfinity(p.Price)).FirstOrDefault();
                    if (unsetByProduct != null)
                    {
                        failedRecipes.AddLoc($"Aucun prix n'est défini pour le sous-produit. {unsetByProduct.Product.Item.UILink()}, incapable de traiter la recette. {recipe.UILink()}");
                        continue;
                    }
                    var byProductsPrice = byProducts.Sum(p => p.Price);
                    var byProductText = LocString.Empty;
                    if (byProducts.Any())
                    {
                        var byProductFoldout = TextLoc.Foldout(TextLoc.StyledNum(byProductsPrice), Localizer.Do($"Sous-produits pour {recipe.UILink()}"), byProducts.Select(bp => bp.Reason).FoldoutListLoc("Sous-produit", Eco.Shared.Items.TooltipOrigin.None));
                        explanation.AppendLineLoc($"Sous-produit : {byProductFoldout}");
                    }

                    // Coût de la main-d'œuvre
                    var labourCost = recipe.Family.LaborInCalories.GetCurrentValue(user) * Config.CostPer1000Calories / 1000f;
                    explanation.AppendLineLoc($"Main-d'œuvre{userText}: UserModifiedCalories ({Text.StyledNum(recipe.Family.LaborInCalories.GetCurrentValue(user))}) * Coût Par 1000 Calories ({Text.StyledNum(Config.CostPer1000Calories)}) / 1000 = {Text.StyledNum(labourCost)}");

                    // Coût des ingrédients
                    IngredientPrice getIngredientPrice(Item ingredient, IngredientElement element)
                    {
                        if (!TryGetCostPrice(ingredient, out var tempPrice, out var reason, out var innerWarnings))
                            return new IngredientPrice(float.PositiveInfinity, ingredient, reason.ToStringLoc(), innerWarnings);

                        var ingredientCount = element.Quantity.GetCurrentValue(resourceEfficiencyContext, craftingTable);
                        var count = ingredientCount;
                        var countText = TextLoc.StyledNum(count);

                        if (item is WorldObjectItem)
                        {
                            count = element.Quantity.GetCurrentValueInt(resourceEfficiencyContext, craftingTable, WORLD_OBJECT_CAP_NUMBER) * 1f / WORLD_OBJECT_CAP_NUMBER;
                            countText = TextLoc.Foldout(TextLoc.StyledNum(count), Localizer.Do($"Raison du arrondi du compte"), Localizer.Do($"Les objets fabriquables placables sont limités à la fabrication. {Text.Info(WORLD_OBJECT_CAP_NUMBER)} à la fois, donc le nombre a été arrondi à partir de {Text.StyledNum(ingredientCount)} à {Text.StyledNum(count)}"));
                        }
                        else if (item.IsTool)
                        {
                            count = element.Quantity.GetCurrentValueInt(resourceEfficiencyContext, craftingTable, TOOL_CAP_NUMBER) * 1f / TOOL_CAP_NUMBER;
                            countText = TextLoc.Foldout(TextLoc.StyledNum(count), Localizer.Do($"Raison de l'arrondi du compte"), Localizer.Do($"Les outils sont limités à la fabrication. {Text.Info(TOOL_CAP_NUMBER)} outil(s) à la fois, donc le nombre a été arrondi à partir de {Text.StyledNum(ingredientCount)} à {Text.StyledNum(count)}"));

                        }
                        var costPriceLink = TextLoc.FoldoutLoc($"Prix De Revient", $"Prix de revient de {ingredient.UILink()}", reason.ToStringLoc());
                        return new IngredientPrice(tempPrice * count, ingredient, Localizer.Do($"Ingrédient {ingredient.UILink()}: Nombre ({countText}) * {costPriceLink} ({Text.StyledNum(tempPrice)}) = {Text.StyledNum(tempPrice * count)}"), innerWarnings);
                    }

                    var costs = recipe.Ingredients.Select(i => i.IsSpecificItem
                        ? getIngredientPrice(i.Item, i)
                        : i.Tag.TaggedItems().Select(ti => getIngredientPrice(ti, i)).OrderBy(x => x.Price).First());
                    if (costs.Any(c => float.IsPositiveInfinity(c.Price)))
                    {
                        failedRecipes.AddLoc($"Pour la fabrication {recipe.UILink()} nous n'avons pas pu déterminer le prix de {string.Join(", ", costs.Where(c => float.IsPositiveInfinity(c.Price)).Select(c => Localizer.Do($"{c.Item.UILink()} ({WhyFoldout(c)})")))}. Veuillez exécuter /ta setupbuy pour ajouter les ingrédients manquants.");
                        continue;
                    };
                    var ingredientsTotal = costs.Select(c => c.Price).Sum();
                    costs.ForEach(c => explanation.AppendLine(c.Reason));
                    var ingredientWarnings = costs.Where(c => c.Warnings != null).SelectMany(c => c.Warnings!).ToList();
                    if (ingredientWarnings.Any())
                    {
                        warnings ??= new List<LocString>();
                        warnings.AddRange(ingredientWarnings);
                    }
                    explanation.AppendLineLoc($"Total des ingrédients : {Text.StyledNum(ingredientsTotal)}");

                    var productCount = product.Quantity.GetCurrentValue(resourceEfficiencyContext, craftingTable);

                    var totalCost = (ingredientsTotal - byProductsPrice + labourCost) / productCount;
                    explanation.AppendLineLoc($"Coût total : (Ingrédients ({Text.StyledNum(ingredientsTotal)}) - Sous-produit ({Text.StyledNum(byProductsPrice)}) + Coût de la main-d'œuvre ({Text.StyledNum(labourCost)})) / Nombre De Produits ({Text.StyledNum(productCount)}) = {Text.StyledNum(totalCost)}");
                    if (bestPrice == null || totalCost < bestPrice.Price)
                    {
                        if (bestPrice != null) { rejectedPrices.Add(bestPrice); }
                        bestPrice = new CachedPrice(totalCost, explanation, recipe);
                    }
                    else
                        rejectedPrices.Add(new CachedPrice(totalCost, explanation, recipe));
                }


            if (bestPrice == null)
            {
                reason = new StringBuilder();
                reason.AppendLineLoc($"Impossible de calculer le coût de l'article à partir de l'une des recettes fournies :");
                foreach (var recipeError in failedRecipes)
                    reason.AppendLine($"- {recipeError}");
                if (hasBuyOrder) reason.AppendLine().AppendLine(buyReason);

                outPrice = hasBuyOrder ? buyPrice : float.PositiveInfinity;
                CachedPrices[item.TypeID] = new CachedPrice(outPrice, reason);
                return hasBuyOrder;
            }
            else
            {
                if (failedRecipes.Count > 0 || rejectedPrices.Count > 0)
                {
                    var rejectedWarnings = new List<LocString>();
                    rejectedPrices
                        .Select(price => new
                        {
                            Price = price,
                            Foldout = TextLoc.FoldoutLoc($"{(price.Price > bestPrice.Price ? Localizer.Do($"supérieur à") : Localizer.Do($"égal à"))}", $"{item.UILink()} prix de revient alternatif", price.Reason.ToStringLoc()),
                            // MeilleurPrix * (1 + Pourcentage) = AutrePrix
                            // 1 + Pourcentage = AutrePrix / MeilleurPrix
                            // Pourcentage = AutrePrix / MeilleurPrix - 1
                            IncreasePercentage = bestPrice.Price <= 0 ? float.PositiveInfinity : (price.Price / bestPrice.Price - 1f),
                        })
                        .ForEach(x =>
                        {
                            failedRecipes.AddLoc($"{x.Price.Recipe.UILink()} le prix est {(x.IncreasePercentage > 0 ? (" " + Text.Negative(Text.Percent(x.IncreasePercentage))) : LocString.Empty)} {x.Foldout} le prix de la recette sélectionnée.");
                            if (x.IncreasePercentage > Config.Profit / 100f)
                            {
                                rejectedWarnings.AddLoc($"Fabrication {x.Price.Recipe.UILink()} vous fera perdre de l'argent. Vous devriez diminuer le prix d'achat de ses ingrédients.");
                            }
                        });

                    if (rejectedWarnings.Count > 0)
                    {
                        warnings ??= new List<LocString>();
                        warnings.AddRange(rejectedWarnings);
                    }

                    var failedFoldout = TextLoc.FoldoutLoc($"ces", $"Recettes ignorées", new StringBuilder(string.Join("\n", failedRecipes.Select(m => $"- {m}"))).ToStringLoc());
                    bestPrice.Reason.Insert(0, Localizer.Do($"Ignoré {failedFoldout} recettes.\n"));
                }
                if (warnings != null)
                    bestPrice = new CachedPrice(bestPrice.Price, bestPrice.Reason, bestPrice.Recipe, new List<LocString>(warnings));
                CachedPrices[item.TypeID] = bestPrice;
                outPrice = bestPrice.Price;
                reason = bestPrice.Reason;
                return true;
            }
        }

        private static LocString WhyFoldout(IngredientPrice price) => TextLoc.FoldoutLoc($"Pourquoi ?", $"Pourquoi {price.Item.UILink()}", price.Reason);

        private ProductPrice ParseByProduct(CraftingElement product, ModuleContext resourceEfficiency, CraftingComponent craftingTable)
        {
            if (!StoreSellPrices.TryGetValue(product.Item.TypeID, out var storePrice))
                return new ProductPrice(product, float.PositiveInfinity, Localizer.Do($"Aucun prix de vente n'est défini pour le sous-produit. {product.Item.UILink()}."));
            // Prix de vente = Prix de revient * (1 + Bénéfice) / (1 - Taux d'imposition)
            // Prix de revient = Prix de vente / (1 + Bénéfice) * (1 - Taux d'imposition)
            var costPrice = storePrice / (1 + Config.Profit / 100f) * (1 - Store.GetTax());
            var quantity = product.Quantity.GetCurrentValue(resourceEfficiency, craftingTable);
            var totalCostPrice = costPrice * quantity;

            var costReasonContent = new LocStringBuilder();
            costReasonContent.AppendLine(Localizer.Do($"Prix de revient = Prix de vente / (1 + Bénéfice) * (1 - Taux d'imposition)"));
            costReasonContent.AppendLine(Localizer.Do($"          = {Text.StyledNum(storePrice)} / (1 + {Text.StyledNum(Config.Profit / 100f)}) * (1 - {Text.StyledNum(Store.GetTax())})"));
            costReasonContent.AppendLine(Localizer.Do($"          = {Text.StyledNum(costPrice)}"));
            var costLink = TextLoc.Foldout(Localizer.Do($"Prix de revient ({Text.StyledNum(costPrice)})"), Localizer.Do($"Sous-produit {product.Item.UILink()} Le prix de revient"), costReasonContent.ToLocString());

            var reason = Localizer.Do($"{Text.StyledNum(quantity)} {product.Item.UILink()} * {costLink} = {Text.StyledNum(totalCostPrice)}");
            return new ProductPrice(product, totalCostPrice, reason);
        }

        private static Deed GetDeed(StringBuilder sb, User user)
        {
            // Obtenir la parcelle où l'utilisateur se trouve actuellement
            var playerPlot = user.Position.XZi().ToPlotPos();

            // Vérifier si l'utilisateur se trouve dans une propriété
            var accessedDeeds = PropertyManager.Obj.Deeds.Where(d => d.IsAuthorized(user, AccessType.FullAccess));
            var deedStandingIn = accessedDeeds.FirstOrDefault(d => d.Plots.Any(p => p.PlotPos == playerPlot));
            if (deedStandingIn == null)
            {
                sb.AppendLine(Localizer.Do($"Vous devez vous trouver dans une propriété à laquelle vous avez accès lorsque vous exécutez cette commande."));
                return null;
            }
            return deedStandingIn;
        }
        private static async Task<StoreComponent> GetStore(StringBuilder sb, User user, Deed deed)
        {
            var stores = WorldObjectUtil.AllObjsWithComponent<StoreComponent>()
                .Where(store => store.IsRPCAuthorized(user.Player, AccessType.FullAccess, Array.Empty<object>())
                    && deed.Plots.Any(p => p.PlotPos == store.Parent.PlotPos()))
                .OrderBy(store => World.WrappedDistance(user.Player.WorldPos(), store.Parent.WorldPos()))
                .Take(2)
                .ToList();
            if (stores.Count == 0)
            {
                sb.AppendLine(Localizer.Do($"Vous n'avez pas de magasin auquel vous avez un accès propriétaire dans la parcelle où vous vous trouvez."));
                return null;
            }
            var store = stores.First();

            if (stores.Count == 1 || await user.ConfirmBoxLoc($"Vous avez plusieurs magasins sur votre parcelle, veuillez confirmer pour appliquer la commande au magasin le plus proche : {store.Parent.MarkedUpName}"))
                return store;

            sb.AppendLine(Localizer.Do($"Vous avez plus d'un magasin sur cette propriété et avez refusé d'utiliser le magasin le plus proche."));
            return null;
        }
        private static bool TryGetStoreAndCraftingTables(StringBuilder sb, User user, Deed deed, out List<CraftingComponent> craftingTables, out Dictionary<LocString, List<Item>> craftableItems)
        {
            craftingTables = null;
            craftableItems = null;


            // Obtenir toutes les tables de craft dans la propriété
            craftingTables = WorldObjectUtil.AllObjsWithComponent<CraftingComponent>()
                .Where(workbench => workbench.IsRPCAuthorized(user.Player, AccessType.FullAccess, Array.Empty<object>()) && deed.Plots.Any(p => p.PlotPos == workbench.Parent.PlotPos()))
                .DistinctBy(craftingTable => $"{craftingTable.Parent.Name}:{(craftingTable.ResourceEfficiencyModule == null ? "null" : craftingTable.ResourceEfficiencyModule.Name)}")
                .ToList();
            if (!craftingTables.Any())
            {
                sb.AppendLine(Localizer.Do($"Aucune table de craft n'a pu être trouvée dans. {deed.UILink()}"));
                return false;
            }

            // Vérifier que l'utilisateur peut au moins créer une recette à partir des tables de craft.
            craftableItems = craftingTables
                .SelectMany(ct => ct.Recipes
                    .Where(recipe => recipe.RequiredSkills.All(s => s.IsMet(user)))
                    .SelectMany(rf => rf.CraftableDefault ? rf.Recipes : rf.Recipes.Skip(1))
                    .SelectMany(r => r.Products)
                    .Select(p => new { CraftingTable = ct.Parent.DisplayName, p.Item })
                )
                .DistinctBy(x => $"{x.CraftingTable}:{x.Item.Name}")
                .GroupBy(x => x.CraftingTable)
                .ToDictionary(x => x.Key, x => x.Select(x => x.Item).ToList());
            if (!craftableItems.Any())
            {
                sb.AppendLine(Localizer.Do($"Vous n'avez pas les compétences/niveaux requis pour créer l'une des recettes dans ces tables de craft : {string.Join(", ", craftingTables.Select(w => w.Parent.UILink()))}"));
                return false;
            }

            return true;
        }

        public List<LocString> SetSellPrice(int item, float newPrice) => SetSellPrice(Item.Get(item), newPrice);
        public List<LocString> SetSellPrice(Item item, float newPrice)
        {
            newPrice = Mathf.RoundToAcceptedDigits(newPrice);
            StoreSellPrices[item.TypeID] = newPrice;

            var msgs = new List<LocString>();
            Store.StoreData.SellOffers.Where(o => o.Stack.Item.TypeID == item.TypeID && o.Price != newPrice).ForEach(o =>
            {
                msgs.AddLoc($"Mise à jour du prix de vente de {item.UILink()} de {Text.StyledNum(o.Price)} à {Text.StyledNum(newPrice)}");
                o.Price = newPrice;
            });
            return msgs;
        }

        public List<LocString> SetBuyPrice(int item, float newPrice) => SetBuyPrice(Item.Get(item), newPrice);
        public List<LocString> SetBuyPrice(Item item, float newPrice)
        {
            newPrice = Mathf.RoundToAcceptedDigits(newPrice);
            StoreBuyPrices[item.TypeID] = newPrice;

            var msgs = new List<LocString>();
            Store.StoreData.BuyOffers.Where(o => o.Stack.Item.TypeID == item.TypeID && o.Price != newPrice).ForEach(o =>
            {
                msgs.AddLoc($"Mise à jour du prix d'achat de {item.UILink()} de {Text.StyledNum(o.Price)} à {Text.StyledNum(newPrice)}");
                o.Price = newPrice;
            });
            return msgs;
        }
    }
}