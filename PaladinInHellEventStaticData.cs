using System;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200043B RID: 1083
	[Serializable]
	public class PaladinInHellEventStaticData : EventEffectStaticData
	{
		// Token: 0x04000A2F RID: 2607
		[JsonProperty]
		public float PrestigeLossPercent;

		// Token: 0x04000A30 RID: 2608
		[JsonProperty]
		public int TributeLossMin;

		// Token: 0x04000A31 RID: 2609
		[JsonProperty]
		public int TributeLossMax;
	}
}
