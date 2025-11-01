using System;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200043F RID: 1087
	[Serializable]
	public class RemoveResourceOfTypeEventStaticData : EventEffectStaticData
	{
		// Token: 0x04000A3A RID: 2618
		[JsonProperty]
		public ResourceTypes ResourceType;

		// Token: 0x04000A3B RID: 2619
		[JsonProperty]
		public float PercentToRemove;
	}
}
