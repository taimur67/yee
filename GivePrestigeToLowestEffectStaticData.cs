using System;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000721 RID: 1825
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class GivePrestigeToLowestEffectStaticData : EdictEffectStaticData
	{
		// Token: 0x04000F3B RID: 3899
		[JsonProperty]
		public int PrestigeAmount;
	}
}
