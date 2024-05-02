// Le Village - Paramètres des différents mods

using Eco.Core.Controller;
using Eco.Shared.Localization;
using System.ComponentModel;

namespace Village.Eco.Mods.Core
{
    public class LVConfigureConfig
    {
        [Category("UnSkill")]
        [LocDisplayName("Oubli spécialité étoiles")]
        [LocDescription("Si vrai, récupération des étoiles en fonction du tier de la spécialité oubliée")]
        [SyncToView] public bool SkillTierCost { get; set; } = true;

        [Category("Exhaustion")]
        [LocDisplayName("Boost épuisement initial")]
        [LocDescription("Si vrai, récupère le temps raté après une connexion tardive")]
        [SyncToView] public bool ExhaustInitialBoost { get; set; } = true;

        [Category("Oil Field")]
        [LocDisplayName("Cacher champs pétroliers")]
        [LocDescription("Si vrai, les champs pétroliers seront cachés")]
        [SyncToView] public bool HiddenOilField { get; set; } = true;
        
        [Category("Oil Field")]
        [LocDisplayName("Dévoiler champs pétroliers")]
        [LocDescription("Si vrai, les champs pétroliers seront révélés en découvrant la spécialité")]
        [SyncToView] public bool RevealOilField { get; set; } = true;

        #region Nutrition
        [Category("Nutrition")]
        [LocDisplayName("Paliers de bonus de nourriture")]
        [LocDescription("Palier pour chaque étoile, au delà, pour les autres étoiles la valeur du palier ne change plus")]
        [SyncToView] public int[] DietTiers { get; set; } = new int[] { 0, 20, 32, 44 };
        
        [Category("Nutrition")]
        [LocDisplayName("Écart avec le palier")]
        [LocDescription("% entre chaque niveau de la spécialité")]
        [SyncToView] public float DietTiersGap { get; set; } = 10f;

        [Category("Nutrition")]
        [LocDisplayName("Active le DEBUG")]
        [LocDescription("Si vrai alors des logs sont ajoutées dans le fichier de log des mods Le village")]
        [SyncToView] public bool DietDebug { get; set; } = false;
        #endregion
    }
}
