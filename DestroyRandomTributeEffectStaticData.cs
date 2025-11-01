using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000709 RID: 1801
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class DestroyRandomTributeEffectStaticData : EdictEffectStaticData
	{
		// Token: 0x04000F31 RID: 3889
		[JsonProperty]
		public int TokenCount;

		// Token: 0x04000F32 RID: 3890
		[JsonProperty]
		public List<Rank> TargetRanks;
	}
}
