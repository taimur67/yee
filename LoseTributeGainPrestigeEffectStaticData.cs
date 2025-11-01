using System;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000730 RID: 1840
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class LoseTributeGainPrestigeEffectStaticData : EdictEffectStaticData
	{
		// Token: 0x04000F3E RID: 3902
		[JsonProperty]
		public int TributeLossCount;

		// Token: 0x04000F3F RID: 3903
		[JsonProperty]
		public int PrestigeGain;
	}
}
