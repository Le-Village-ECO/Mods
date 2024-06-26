﻿// Le Village - Nouvelle spécialité pour contrôler la qualité diététique du joueur
// TODO - Rendre paramétrable via fichier de config les bonus/malus des niveaux de spécialité
// TODO - Rendre paramétrable via fichier de config le nombre d'étoiles dans la spécialité

using Eco.Core.Items;
using Eco.Gameplay.DynamicValues;
using Eco.Gameplay.Items;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;

namespace Village.Eco.Mods.Nutrition
{
    [Serialized]
    [LocDisplayName("Diététique")]
    [LocDescription("Votre niveau de diététique impacte votre <b>vitesse de fabrication</b> et il dépend de votre <b>nombre d'étoiles</b> et de votre <b>bonus de nourriture</b>.\n" +
        "Chaque gain d'étoile demandera un bonus de nourriture supérieur pour maintenir le niveau maximum de la spécialité.\n\n" +
        "Commande de chat pour plus d'information : <b>/DietInfo</b>")]
    [Ecopedia("Professions", "Survivalist", createAsSubPage: true)]
    [RequiresSkill(typeof(SurvivalistSkill), 0), Tag("Survivalist Specialty"), Tier(1)]
    [Tag("Specialty")]
    [Tag("Teachable")]
    public partial class DietSkill : Skill
    {
        public static MultiplicativeStrategy MultiplicativeStrategy =
            new(new float[] {
                1 + 19f,        //niveau 0 - 2000%
                1 + 19f,        //niveau 1 - 2000%
                1 + 9f,         //niveau 2 - 1000%
                1 + 7.5f,       //niveau 3 - 850%
                1 + 2.5f,       //niveau 4 - 350%
                1,              //niveau 5 - 100% (max)
            });
        public override MultiplicativeStrategy MultiStrategy => MultiplicativeStrategy;

        public static AdditiveStrategy AdditiveStrategy =
            new(new float[] {
                0,
                0,
                250,
                500,
                1000,
                1500,
            });
        public override AdditiveStrategy AddStrategy => AdditiveStrategy;
        public override int MaxLevel { get { return 5; } }
        public override int Tier { get { return 1; } }
    }
}
