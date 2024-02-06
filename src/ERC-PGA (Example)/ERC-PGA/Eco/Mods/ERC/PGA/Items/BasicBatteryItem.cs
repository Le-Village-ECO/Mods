using System;
using Eco.Gameplay.Items;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;

namespace Eco.Mods.ERC.PGA.Items
{
	// Token: 0x02000009 RID: 9
	[Serialized]
	[LocDisplayName("Basic Battery")]
	[LocDescription("Used for powering handheld electric tools, sadly not for long time")]
	[MaxStackSize(25)]
	[Weight(25)]
	internal class BasicBatteryItem : Item
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000D RID: 13 RVA: 0x00002361 File Offset: 0x00000561
		public override LocString DisplayNamePlural
		{
			get
			{
				return Localizer.DoStr("Basic Batteries");
			}
		}
	}
}
