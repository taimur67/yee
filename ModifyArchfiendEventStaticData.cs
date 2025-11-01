using System;
using Game.Simulation.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000438 RID: 1080
	[Serializable]
	public class ModifyArchfiendEventStaticData : EventEffectStaticData
	{
		// Token: 0x04000A27 RID: 2599
		[JsonProperty]
		public EventTargets Targets;

		// Token: 0x04000A28 RID: 2600
		[JsonProperty]
		public ArchfiendModifierStaticData Modifiers;

		// Token: 0x04000A29 RID: 2601
		[JsonProperty]
		public int MinDuration;

		// Token: 0x04000A2A RID: 2602
		[JsonProperty]
		public int MaxDuration;
	}
}
