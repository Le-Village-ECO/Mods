//Le Village - Détecteur de pétrole
//TODO - Revoir et comprendre GetOilAmount

using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Shared.Utils;
using Eco.Simulation.WorldLayers.Layers;
using Eco.Simulation.WorldLayers;
using Eco.WorldGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eco.Gameplay.Interactions.Interactors;
using Eco.Gameplay.Items;
using Eco.Gameplay.DynamicValues;
using Eco.Shared.SharedTypes;
using Eco.Core.Items;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using System.ComponentModel;
using Eco.Gameplay.Plants;
using Eco.Gameplay.Players;
using Eco.Mods.Organisms;
using Eco.Shared.Math;
using Eco.Simulation;
using Eco.Shared.Voxel;
using Eco.Mods.TechTree;
using Eco.Gameplay.Components;
using Eco.Gameplay.Items.Recipes;
using System.Runtime.CompilerServices;
using Eco.Gameplay.Skills;
using Eco.Gameplay.GameActions;
using static Eco.Shared.Utils.LimitMapper;
using Eco.Mods.FiniteOil;

namespace Village.Eco.Mods.OilField
{

    // Definition de l'outil
    [Serialized]
    [LocDisplayName("Oil Prospector")]
    [LocDescription("Analyse le sol pour savoir combien de pétrole peut y être extrait")]
    [Category("Tools")]
    [Tag("Tool")]
    [Ecopedia("Items", "Tools", createAsSubPage: true)]
    [Weight(1000)]
    public class AdminProspectorItem : ToolItem, IInteractor
    {
        public override float DurabilityRate { get { return 0; } }

        private static IDynamicValue skilledRepairCost = new ConstantValue(4);
        public override IDynamicValue SkilledRepairCost { get { return skilledRepairCost; } }

        [Interaction(InteractionTrigger.LeftClick, "Prospecter le sol")]
        public bool Prospect(Player player, InteractionTriggerInfo triggerInfo, InteractionTarget target)
        {
            if (target.IsBlock)
            {
                var title = new LocStringBuilder();
                var text = new LocStringBuilder();
                var button = new LocStringBuilder();

                //Affichage du titre de la popup
                title.Append($"{this.DisplayName} {target.BlockPosition}");

                //Affichage du contenu de la popup - quantité de pétrole
                string text1 = "Dans un rayon de {0} cases, il y a une quantite de {1}";
                object[] array1 = new object[2];
                array1[0] = PumpJackObject.Radius;
                array1[1] = Text.Num(AdminProspectorItem.GetOilAmount(target.BlockPosition.Value.XZ));
                text.AppendLine(TextLoc.HeaderLocStr("Pétrole en sous-sol")).AppendLine(1);
                text.Append(FormattableStringFactory.Create(text1, array1)).AppendLine(2);

                //Affichage du bouton de la popup
                button.Append($"Intéressant !");

                //Affichage de la popup
                player.LargeInfoBox(title.ToLocString(), text.ToLocString(), button.ToLocString());

                return true;
            }
            return false;
        }

        [Interaction(InteractionTrigger.LeftClick, "Ajouter au Layer", InteractionModifier.Ctrl)]
        public bool AddAmount(Player player, InteractionTriggerInfo triggerInfo, InteractionTarget target)
        {
            if (target.IsBlock)
            {
                var pos = target.BlockPosition.Value.X_Z();
                player.InfoBoxLoc($"Ctrl click = {pos}");
                FiniteOilPlugin.AddAmount(pos, 0.1f);
                return true;
            }
            return false;
        }

        [Interaction(InteractionTrigger.LeftClick, "Enlever au Layer", InteractionModifier.Shift)]
        public bool RemoveAmount(Player player, InteractionTriggerInfo triggerInfo, InteractionTarget target)
        {
            if (target.IsBlock)
            {
                player.InfoBoxLoc($"Shift click = {target.BlockPosition.Value.X_Z()}");

                return true;
            }
            return false;
        }
        private static float GetOilAmount(Vector2i pos)
        {
            WorldLayer layer = Singleton<WorldLayerManager>.Obj.GetLayer("Oilfield");
            return layer[layer.WorldPosToLayerPos(pos)];
        }
    }
}
