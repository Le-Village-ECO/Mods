// Le Village
// Abstract class pour gérer tous les consommable de type boost qui ajoute de l'énergie
// Sans épuisement, il s'agit d'un FoodItem classique
// TODO - gérer le délai entre 2 consommations par un paramètre de l'item avec la remise à 0 basée sur l'heure de remise à 0 de l'épuisement au niveau serveur

using Eco.Core;
using Eco.Core.Utils;
using Eco.Core.Utils.Logging;
using Eco.Gameplay.Items;
using Eco.Gameplay.Players;
using Eco.Gameplay.Systems.TextLinks;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using Eco.Shared.Time;
using Eco.Shared.Utils;
using Eco.Simulation.Time;
using System.ComponentModel;
using Village.Eco.Mods.Core;

namespace Village.Eco.Mods.ExhaustionMod
{
    [Serialized]
    [LocDisplayName("Exhaustion Boost")]
    [LocDescription("Permet de redonner de l'énergie si épuisé")]
    [Category("Hidden")]

    public abstract class ExhaustionBoost : FoodItem
    {
        //Paramètres par défaut FoodItem
        public override float Calories => 100;
        public override Nutrients Nutrition => new Nutrients() { Carbs = 1, Fat = 1, Protein = 1, Vitamins = 1 };
        protected override float BaseShelfLife => (float)TimeUtil.HoursToSeconds(72);

        //Paramètre par défaut pour la gestion du boost
        public virtual float BoostTime => 1.0f; //Valeur par défaut pour la durée du boost
        public virtual bool CheckDate => false; //True si on doit vérifier l'utilisation 1 fois par jour
        //public const double DailyBoostCooldown = 1; //Delai entre 2 utilisations de boost journalier

        //Fonction Exhaustion activée ?
        public bool configExhaust = DifficultySettingsConfig.Vals.ExhaustionEnabled;

        //Gestion event
        public static ThreadSafeAction<User> ConsumeEnergyBoost = new();

        //Gestion log & messages
        public NLogWriter log = NLogManager.GetLogWriter("LeVillageMods");

        public override string OnUsed(Player player, ItemStack itemStack)
        {
            bool isExhausted = player.User.ExhaustionMonitor.IsExhausted;

            if (configExhaust)
            {
                if (!isExhausted)
                {
                    player.ErrorLocStr(Localizer.Do($"Vous n'êtes pas encore épuisé... Alors n'abusez pas des boissons énergisantes !"));
                    return string.Empty;
                }

                if (CheckDate && !ValidateDate(player))
                {
                    player.ErrorLocStr(Localizer.Do($"Vous devez attendre {Text.Num(1)} jour avant de reprendre {itemStack.Item.UILink()}."));
                    return string.Empty;
                }

                // Mise à jour de l'énergie si tout est validé
                player.User.ExhaustionMonitor.Energize(BoostTime);
            }
            else  //Épuisement non activé = uniquement FoodItem
            {
                //On bloque si le joueur à déjà pris un café
                if (CheckDate && !ValidateDate(player))
                {
                    player.ErrorLocStr(Localizer.Do($"Vous avez déjà pris un {itemStack.Item.UILink()} aujourd'hui. Il ne faut pas en abuser !"));
                    return string.Empty;
                }
            }

            //Event
            ConsumeEnergyBoost?.Invoke(player.User);

            //Log
            log.Write($"Le joueur **{player.DisplayName}** a utilisé **{itemStack.Item}**.");

            return base.OnUsed(player, itemStack);
        }
 
        public bool ValidateDate(Player player) 
        {
            //Recuperation des donnees du joueur
            var plugin = PluginManager.GetPlugin<PlayersDataPlugin>();
            var playerData = plugin.GetPlayerDataOrDefault(player);

            int playerDayInteger = (int)playerData.LastDailyBoost;
            int worldDayInteger = (int)WorldTime.Day;

            if (playerData.LastDailyBoost > 0 && playerDayInteger == worldDayInteger)
            {
                return false;
            }
            else
            {
                //Mise a jour des donnees du joueur
                playerData.LastDailyBoost = WorldTime.Day;
                plugin.AddOrSetPlayerData(player, playerData);

                return true;
            }
        }

        //Pour les FoodItem, il faut gérer aussi le consume
        public string Consume(Player player) => OnUsed(player, player.User.Inventory.Toolbar.SelectedStack);
    }
}
