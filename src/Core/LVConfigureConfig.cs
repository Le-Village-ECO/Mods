using Eco.Shared.Localization;

namespace Village.Eco.Mods.Core
{
    public class LVConfigureConfig
    {
        [LocDisplayName("Oubli spécialité étoiles")]
        [LocDescription("Si vrai, récupération des étoiles en fonction du tier de la spécialité oubliée")]
        public bool SkillTierCost { get; set; } = true;

        [LocDisplayName("Boost épuisement initial")]
        [LocDescription("Si vrai, récupère le temps raté après une connexion tardive")]
        public bool ExhaustInitialBoost { get; set; } = true;

        [LocDisplayName("Cacher champs pétroliers")]
        [LocDescription("Si vrai, les champs pétroliers seront cachés")]
        public bool HiddenOilField { get; set; } = true;

        [LocDisplayName("Dévoiler champs pétroliers")]
        [LocDescription("Si vrai, les champs pétroliers seront révélés en découvrant la spécialité")]
        public bool RevealOilField { get; set; } = true;
    }
}
