using System;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000440 RID: 1088
	[Serializable]
	public class RemoveResourcesEventStaticData : EventEffectStaticData
	{
		// Token: 0x04000A3C RID: 2620
		[JsonProperty]
		public float PercentToRemove;
	}
}
