using System;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000748 RID: 1864
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class TaxTributeEffectStaticData : EdictEffectStaticData
	{
		// Token: 0x04000F47 RID: 3911
		[JsonProperty]
		public int PrestigeTaxInterval;
	}
}
