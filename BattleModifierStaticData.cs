using System;
using Core.StaticData.Attributes;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000424 RID: 1060
	[StaticDataValueType]
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class BattleModifierStaticData : ModifierStaticData
	{
		// Token: 0x040009E3 RID: 2531
		[JsonProperty]
		public bool SkipPhase;

		// Token: 0x040009E4 RID: 2532
		[JsonProperty]
		public BattlePhase PhaseToSkip;

		// Token: 0x040009E5 RID: 2533
		[JsonProperty]
		public bool PrioritisePhase;

		// Token: 0x040009E6 RID: 2534
		[JsonProperty]
		public BattlePhase PhaseToPrioritise;
	}
}
