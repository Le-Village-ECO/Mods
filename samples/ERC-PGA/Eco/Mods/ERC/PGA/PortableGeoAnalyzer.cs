using System;
using System.Runtime.CompilerServices;
using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Shared.Utils;
using Eco.Simulation.WorldLayers;
using Eco.Simulation.WorldLayers.Layers;
using Eco.World;
using Eco.WorldGenerator;

namespace Eco.Mods.ERC.PGA
{
	// Token: 0x02000006 RID: 6
	internal class PortableGeoAnalyzer : IModKitPlugin, IServerPlugin, IInitializablePlugin
	{
		// Token: 0x06000006 RID: 6 RVA: 0x0000209D File Offset: 0x0000029D
		[NullableContext(1)]
		public void Initialize(TimedTask timer)
		{
			if (World.ChunksCount == 0)
			{
				WorldGeneratorPlugin.OnCompleted.Add(new Action(this.HideLayer));
				return;
			}
			this.HideLayer();
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000020C4 File Offset: 0x000002C4
		private void HideLayer()
		{
			WorldLayer layer = Singleton<WorldLayerManager>.Obj.GetLayer("Oilfield");
			if (layer != null)
			{
				layer.Settings.Visible = false;
				return;
			}
			Log.WriteErrorLineLocStr("Unable to hide Oilfield World Layer, layer not exist", false);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000020FC File Offset: 0x000002FC
		[NullableContext(1)]
		public string GetCategory()
		{
			return string.Empty;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002103 File Offset: 0x00000303
		[NullableContext(1)]
		public string GetStatus()
		{
			return string.Empty;
		}
	}
}
