using System;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000727 RID: 1831
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class LaurelOfTriumphEffectStaticData : EdictEffectStaticData
	{
		// Token: 0x04000F3D RID: 3901
		[JsonProperty]
		public int PrestigeBoost;
	}
}
