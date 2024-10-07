// Le Village - Gestion des crédits personnels du joueur à son entrée dans le monde
// Note : IMPOSSIBLE de supprimer la device complétement dans le Registrar car cela casse des fonctionnalités du jeu qui ne fonctionne pas en troc (transferts par exemple)

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

            // Modification de la valeur de monnaie perso du joueur
            var holding = account.CurrencyHoldings.GetOrDefault(currency);
            holding.SetVal(defaultValue);
        }
    }
}