using System;
using System.Collections.Generic;
using Core.StaticData;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200071E RID: 1822
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class GiftTributeEffectStaticData : EdictEffectStaticData
	{
		// Token: 0x04000F39 RID: 3897
		[JsonProperty]
		public int GiftAmount;

		// Token: 0x04000F3A RID: 3898
		[JsonProperty]
		public List<ConfigRef<ArchfiendRankStaticData>> ReceivingRanks;
	}
}
