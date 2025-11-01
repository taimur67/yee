using System;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000434 RID: 1076
	[Serializable]
	public class DestroyRandomLegionEventStaticData : EventEffectStaticData
	{
		// Token: 0x04000A1D RID: 2589
		[JsonProperty]
		public bool CanSelectPersonalGuard;
	}
}
