using System;
using System.ComponentModel;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000715 RID: 1813
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class DrawEventCardEffectStaticData : EdictEffectStaticData
	{
		// Token: 0x04000F35 RID: 3893
		[JsonProperty]
		[DefaultValue(1)]
		public int DrawCount = 1;
	}
}
