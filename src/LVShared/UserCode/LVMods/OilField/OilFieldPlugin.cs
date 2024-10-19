// Le Village - Plugin pour la gestion des champs de pétrole
// Initialement ils sont visibles (voir WorldLayerSettingsOilfield.cs)
// Au lancement du serveur, si la spécialisation n'est pas découverte, champs deviennent invisibles
// A la découverte de la spécialisation, les champs deviennent visibles mais cela demande une déco/reco du client
// TODO - Ajouter un message au joueur qui découvre la spécialité pour déco/reco

using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Core.Utils.Logging;
using Eco.Gameplay.Items;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Localization;
using Eco.Simulation.WorldLayers;
using Eco.Simulation.WorldLayers.Layers;
using Village.Eco.Mods.Core;

namespace Village.Eco.Mods.OilField
{
    [LocDisplayName(nameof(OilFieldPlugin))]
    public class OilFieldPlugin : IInitializablePlugin, IModKitPlugin
    {
        public NLogWriter log = NLogManager.GetLogWriter("LeVillageMods"); //Initialisation de la log
        public bool configHidden = LVConfigurePlugin.Config.HiddenOilField; //Activation du mod par config
        public bool configReveal = LVConfigurePlugin.Config.RevealOilField; //Révélation du pétrole avec la découverte de la spécialité

        public void Initialize(TimedTask timer)
        {
            //Statut de visibilité des champs pétroliers - par défaut True au lancement serveur
            WorldLayer oilLayer = WorldLayerManager.Obj.GetLayer("Oilfield");
            bool isOilVisible = oilLayer.Settings.Visible;
            log.Write($"Champs pétroliers sont visibles ? **{isOilVisible}**");

            //Statut découverte du forage pétrolier - par défaut False au lancement serveur
            OilDrillingSkill oilDrillingSkill = Item.Get<OilDrillingSkill>();
            bool isOilDrillingUnlocked = oilDrillingSkill.IsDiscovered();
            log.Write($"OilDrilling est découvert ? **{isOilDrillingUnlocked}**");

            log.Write($"Config Hidden : **{configHidden}**");
            log.Write($"Config Reveal : **{configReveal}**");

            //Config qui active le mod
            if (configHidden)
            {
                //Si visible (TRUE) & pas découvert (FALSE) = devient invisible (FALSE) - par défaut au lancement du serveur
                if (isOilVisible && !isOilDrillingUnlocked)
                {
                    oilLayer.Settings.Visible = false;
                    log.Write($"Changement (activation) statut affichage de **{isOilVisible}** à **{oilLayer.Settings.Visible}**");
                }
                
                //Config qui conditionne l'apparition du pétrole sur la découverte de la spécialité
                if (configReveal)
                {
                    //En cours de cycle, découverte de la spécialité qui permet d'afficher les champs
                    //Si spécialité découverte (TRUE) et invisible (FALSE) = devient visible (TRUE)
                    if (isOilDrillingUnlocked && !isOilVisible)
                    {
                        oilLayer.Settings.Visible = true;
                        log.Write($"Changement (event) statut affichage de **{isOilVisible}** à **{oilLayer.Settings.Visible}**");
                    }
                }

                //Gestion de l'évènement de découverte du forage pétrolier
                //Si découverte (EVENT) = devient visible (TRUE)
                SkillTree.DiscoveredEvent.Add(skillTree =>
                {
                    if (skillTree == oilDrillingSkill.SpecialtySkillTree) 
                    {
                        oilLayer.Settings.Visible = true;
                        log.Write($"**{skillTree}** a été découvert, les champs pétroliers sont **{oilLayer.Settings.Visible}**");
                    }
                });
            }
        }

        public string GetCategory() => "LeVillageMods";
        public override string ToString() => Localizer.DoStr("Oil Field");
        public string GetStatus() => "Active";
    }
}
