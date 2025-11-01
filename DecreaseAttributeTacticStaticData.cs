using System;
using System.ComponentModel;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000426 RID: 1062
	[Serializable]
	public class DecreaseAttributeTacticStaticData : ModifierStaticData
	{
		// Token: 0x040009E8 RID: 2536
		[JsonProperty]
		public CombatStatType Stat;

		// Token: 0x040009E9 RID: 2537
		[JsonProperty]
		[DefaultValue(1)]
		public int MinRoll = 1;

		// Token: 0x040009EA RID: 2538
		[JsonProperty]
		[DefaultValue(6)]
		public int MaxRoll = 6;
	}
}
