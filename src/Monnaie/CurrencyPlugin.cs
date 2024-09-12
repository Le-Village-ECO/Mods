// Le Village - Gestion des crédits personnels du joueur à son entrée dans le monde
// TODO - Possible de supprimer la device complétement dans le Registrar ? (voir CurrencyManager.cs / AddCurrency)

using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Gameplay.Economy;
using Eco.Gameplay.Players;
using Eco.Shared.Localization;

using Village.Eco.Mods.Core;

namespace Village.Eco.Mods.Monnaie
{
    public class CurrencyPlugin : IInitializablePlugin, IModKitPlugin
    {
        public string GetCategory() => "LeVillageMods";
        public override string ToString() => Localizer.DoStr("Currency Plugin");
        public string GetStatus() => "Active";

        public void Initialize(TimedTask timer)
        {
            UserManager.NewUserJoinedEvent.Add(NewUserJoinedEvent);

            ///Appels des codes de corrections des bankAccounts incohérents
            //UserManager.OnUserLoggedIn.Add(OnUserLoggedIn);
            //PluginManager.Controller.RunIfOrWhenInited(RepairAllUser);
        }

        public static void NewUserJoinedEvent(User user)
        {
            //On ne fait rien si la monnaie est laissé en Vanille
            if (LVConfigurePlugin.Config.MonnaieVanille) return;

            // Limite de monnaie perso
            float defaultValue = LVConfigurePlugin.Config.MonnaiePerso;
            // Récupération de la monnaie personnelle du joueur
            var currency = CurrencyManager.GetPlayerCurrency(user);
            // Récupération du compte personnel du joueur
            var account = user.BankAccount;

            var holding = account.CurrencyHoldings.GetOrDefault(currency);
            holding.SetVal(defaultValue);

            ///Code qui provoque un bug en rendant les BankAccount incohérent (la Currency est là sans être là...)
            ///
            ////Si il y a une valeur de monnaie perso défini, elle remplacera la limite infini
            //if (defaultValue != 0)
            //{
            //    var holding = account.CurrencyHoldings.GetOrDefault(currency);
            //    holding.SetVal(defaultValue);
            //}
            //else // Sinon on supprime la monnaie perso
            //{
            //    account.CurrencyHoldings.Remove(currency);
            //    CurrencyManager.Registrar.Remove(currency);
            //    CurrencyManager.UsernameToCurrency.Remove(user.Name);
            //}
        }

        //public static void OnUserLoggedIn(User user)
        //{
        //    //On ne fait rien si la monnaie est laissé en Vanille
        //    if (LVConfigurePlugin.Config.MonnaieVanille) return;

        //    //Si le joueur n'a pas déjà une monnaie
        //    if (CurrencyManager.GetPlayerCurrency(user) is null)
        //    {
        //        //recréé la monnaie perso
        //        CurrencyManager.TryCreatePersonalCurrency(user);
        //    }

        //    //Si le joueur n'a pas d'account
        //    if (user.BankAccount is null)
        //    {
        //        BankAccountManager.Obj.TryCreateUserAccount(user);
        //    }

        //    var currency = CurrencyManager.GetPlayerCurrency(user);
        //    //Si l'account du joueur n'a pas sa monnaie perso, la rajoute
        //    if (user.BankAccount.CurrencyHoldings.Keys.Any(c =>c.Name == currency.Name) is false)
        //    {
        //        user.BankAccount.CurrencyHoldings.Add(currency,
        //            new CurrencyHolding() { Currency = currency, Val = LVConfigurePlugin.Config.MonnaiePerso });
        //    }
        //}

        //public static void RepairAllUser()
        //{
        //    StorageManager.Obj.LoadAsync<User>("Users", user =>
        //    {
        //        //On ne fait rien si la monnaie est laissé en Vanille
        //        if (LVConfigurePlugin.Config.MonnaieVanille) return;

        //        //Si le joueur n'a pas déjà une monnaie
        //        if (CurrencyManager.GetPlayerCurrency(user) is null)
        //        {
        //            //recréé la monnaie perso
        //            CurrencyManager.TryCreatePersonalCurrency(user);
        //        }

        //        //Si le joueur n'a pas d'account
        //        if (user.BankAccount is null)
        //        {
        //            BankAccountManager.Obj.TryCreateUserAccount(user);
        //        }

        //        var currency = CurrencyManager.GetPlayerCurrency(user);
        //        //Si l'account du joueur n'a pas sa monnaie perso, la rajoute
        //        if (user.BankAccount.CurrencyHoldings.Keys.Any(c => c.Name == currency.Name) is false)
        //        {
        //            user.BankAccount.CurrencyHoldings.Add(currency,
        //                new CurrencyHolding() { Currency = currency, Val = LVConfigurePlugin.Config.MonnaiePerso });
        //        }
        //    });
        //}
    }
}