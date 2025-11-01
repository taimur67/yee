using System;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200043D RID: 1085
	[Serializable]
	public class ReducePowerEventStaticData : EventEffectStaticData
	{
		// Token: 0x04000A36 RID: 2614
		[JsonProperty]
		public bool AllArchfiends;

		// Token: 0x04000A37 RID: 2615
		[JsonProperty]
		public int NumberOfLevels;
	}
}
