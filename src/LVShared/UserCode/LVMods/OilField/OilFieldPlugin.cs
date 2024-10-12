// Le Village - Plugin pour la gestion des champs de pétrole
// Initialement ils sont visibles (voir WorldLayerSettingsOilfield.cs)
// Au lancement du serveur, si la spécialisation n'est pas découverte, champs deviennent invisibles
// A la découverte de la spécialisation, les champs deviennent visibles mais cela demande une déco/reco du client
// TODO - Utiliser le fichier de configuration
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

namespace Village.Eco.Mods.OilField
{
    [LocDisplayName(nameof(OilFieldPlugin))]
    public class OilFieldPlugin : IInitializablePlugin, IModKitPlugin
    {
        public void Initialize(TimedTask timer)
        {
            var log = NLogManager.GetLogWriter("LeVillageMods");

            //Statut de visibilité des champs pétroliers - par défaut true au lancement serveur
            WorldLayer oilLayer = WorldLayerManager.Obj.GetLayer("Oilfield");
            bool isOilVisible = oilLayer.Settings.Visible;
            //Log
            log.Write($"Champs sont visibles ? **{isOilVisible}**");

            //Statut découverte du forage pétrolier - par défaut false au lancement serveur
            OilDrillingSkill oilDrillingSkill = Item.Get<OilDrillingSkill>();
            bool isOilDrillingUnlocked = oilDrillingSkill.IsDiscovered();
            //Log
            log.Write($"OilDrilling est découvert ? **{isOilDrillingUnlocked}**");

            //Si visible et pas découvert = devient invisible - par défaut au lancement du serveur
            if (isOilVisible && !isOilDrillingUnlocked)
            { 
                oilLayer.Settings.Visible = false;
                //Log
                log.Write($"Changement1 de **{isOilVisible}** à **{oilLayer.Settings.Visible}**");
            }

            //Si découvert et invisible = devient visible
            //En cours de cycle, découverte de la spécialité qui permet d'afficher les champs
            if (isOilDrillingUnlocked && !isOilVisible)
            { 
                oilLayer.Settings.Visible = true;
                //Log
                log.Write($"Changement2 de **{isOilVisible}** à **{oilLayer.Settings.Visible}**");
            }

            //Gestion de l'évènement de découverte du forage pétrolier
            //Si découverte = devient visible
            SkillTree.DiscoveredEvent.Add(skillTree =>
            {
                if (skillTree == oilDrillingSkill.SpecialtySkillTree) 
                {
                    oilLayer.Settings.Visible = true;
                    //Log
                    var log = NLogManager.GetLogWriter("LeVillageMods");
                    log.Write($"**{skillTree}** a été découvert, champs sont **{oilLayer.Settings.Visible}**");
                }
            });
        }
        public string GetCategory() => "LeVillageMods";
        public override string ToString() => Localizer.DoStr("Oil Field");
        public string GetStatus() => "Active";
    }
}
