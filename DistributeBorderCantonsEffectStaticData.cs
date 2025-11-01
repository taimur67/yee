using System;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000712 RID: 1810
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class DistributeBorderCantonsEffectStaticData : EdictEffectStaticData
	{
		// Token: 0x04000F34 RID: 3892
		[JsonProperty]
		public int CantonCount;
	}
}
