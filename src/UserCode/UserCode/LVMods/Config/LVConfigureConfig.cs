// Le Village - Paramètres des différents mods

using Eco.Core.Controller;
using Eco.Gameplay.Systems.Messaging.Chat.Commands;
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

        [Category("Admin")]
        [LocDisplayName("Permettre aux joueurs de voler (Fly)")]
        [LocDescription("Si vrai, les joueurs ont acces a une commande pour voler")]
        [SyncToView] public bool AllowFlyAll { get; set; } = false;

        #region Monnaie
        [Category("Monnaie")]
        [LocDisplayName("Monnaie illimitée")]
        [LocDescription("Si true, alors paramétrage vanille avec une monnaie personelle illimitée, sinon valeur du paramètre suivant")]
        [SyncToView] public bool MonnaieVanille { get; set; } = true;

        [Category("Monnaie")]
        [LocDisplayName("Montant monnaie perso")]
        [LocDescription("Montant de la monnaie personnelle dans le compte personnel du joueur")]
        [SyncToView] public float MonnaiePerso { get; set; } = 1000f;
        #endregion

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
