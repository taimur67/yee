using System;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000724 RID: 1828
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class GiveTributeToLowestEffectStaticData : EdictEffectStaticData
	{
		// Token: 0x04000F3C RID: 3900
		[JsonProperty]
		public int TributeCount;
	}
}
