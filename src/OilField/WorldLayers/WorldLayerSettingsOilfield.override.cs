// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.
// Le Village - Rendre invisible le pétrole sur la carte

namespace Eco.Mods.WorldLayers
{
    using Eco.Shared.Localization;
    using Eco.Shared.Math;
    using Eco.Shared.Utils;
    using Eco.Simulation.WorldLayers.Layers;

    public class WorldLayerSettingsOilfield : WorldLayerSettings
    {
        public WorldLayerSettingsOilfield() : base()
        {
            this.Name = "Oilfield";
            this.MinimapName = Localizer.DoStr("Oilfield");
            this.InitMultiplier = 1f;
            this.SyncToClient = true;
			this.Visible = false;  // Ajout du paramètre de visibilité sur la carte + valeur false
            this.Range = new Range(0f, 1f);
            this.OverrideRenderRange = null;
            this.MinColor = new Color(1f, 1f, 1f);
            this.MaxColor = new Color(0f, 0f, 0f);
            this.SumRelevant = false;
            this.Unit = string.Empty;
            this.VoxelsPerEntry = 5;
            this.Category = WorldLayerCategory.World;
            this.ValueType = WorldLayerValueType.Percent;
            this.AreaDescription = string.Empty;

        }
    }
}
