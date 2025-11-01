using System;
using Game.Simulation.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000439 RID: 1081
	[Serializable]
	public class ModifyGamePieceEventStaticData : EventEffectStaticData
	{
		// Token: 0x04000A2B RID: 2603
		[JsonProperty]
		public EventTargets Targets;

		// Token: 0x04000A2C RID: 2604
		[JsonProperty]
		public GamePieceModifierStaticData Modifiers;

		// Token: 0x04000A2D RID: 2605
		[JsonProperty]
		public int MinDuration;

		// Token: 0x04000A2E RID: 2606
		[JsonProperty]
		public int MaxDuration;
	}
}
