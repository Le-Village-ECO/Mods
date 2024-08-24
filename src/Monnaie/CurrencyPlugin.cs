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
        public void Initialize(TimedTask timer) 
        {
            UserManager.NewUserJoinedEvent.Add(NewUserJoinedEvent);
        }
        public static void NewUserJoinedEvent(User user)
        {
            //Récupération de la configuration
            bool isVanille = LVConfigurePlugin.Config.MonnaieVanille;
            float valeur = LVConfigurePlugin.Config.MonnaiePerso;

            // Récupération de la devise personnelle du joueur
            var currency = CurrencyManager.GetPlayerCurrency(user);
            // Récupération du compte personnel du joueur
            var account = user.BankAccount;

            if (!isVanille)
            {
                // Remise à 0 de ce compte pour cette devise
                account.CurrencyHoldings.Remove(currency);
                
                if (valeur != 0)
                {
                    // Ajout d'un montant de la devise personnelle dans le compte personel
                    account.CurrencyHoldings.Add(currency, new CurrencyHolding() { Currency = currency, Val = valeur });
                }
            }
        }

        public string GetCategory() => "LeVillageMods";
        public override string ToString() => Localizer.DoStr("Currency Plugin");
        public string GetStatus() => "Active";
    }
}
