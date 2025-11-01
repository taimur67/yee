using System;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000436 RID: 1078
	[Serializable]
	public class IncreaseRitualCostsEventStaticData : EventEffectStaticData
	{
		// Token: 0x04000A1E RID: 2590
		[JsonProperty]
		public int MinIncrease;

		// Token: 0x04000A1F RID: 2591
		[JsonProperty]
		public int MaxIncrease;

		// Token: 0x04000A20 RID: 2592
		[JsonProperty]
		public int MinDuration;

		// Token: 0x04000A21 RID: 2593
		[JsonProperty]
		public int MaxDuration;
	}
}
