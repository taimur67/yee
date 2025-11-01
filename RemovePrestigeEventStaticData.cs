using System;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200043E RID: 1086
	[Serializable]
	public class RemovePrestigeEventStaticData : EventEffectStaticData
	{
		// Token: 0x04000A38 RID: 2616
		[JsonProperty]
		public int PrestigeMin;

		// Token: 0x04000A39 RID: 2617
		[JsonProperty]
		public int PrestigeMax;
	}
}
