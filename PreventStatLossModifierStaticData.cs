using System;
using Core.StaticData.Attributes;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200045B RID: 1115
	[StaticDataValueType]
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PreventStatLossModifierStaticData : ModifierStaticData
	{
		// Token: 0x04000A85 RID: 2693
		[JsonProperty]
		public bool PreventWrathLoss;

		// Token: 0x04000A86 RID: 2694
		[JsonProperty]
		public bool PreventDeceitLoss;

		// Token: 0x04000A87 RID: 2695
		[JsonProperty]
		public bool PreventProphecyLoss;

		// Token: 0x04000A88 RID: 2696
		[JsonProperty]
		public bool PreventDestructionLoss;

		// Token: 0x04000A89 RID: 2697
		[JsonProperty]
		public bool PreventCharismaLoss;
	}
}
