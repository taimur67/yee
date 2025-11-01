using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000283 RID: 643
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class RitualMaskingContext
	{
		// Token: 0x04000579 RID: 1401
		[JsonProperty]
		public RitualMaskingMode MaskingMode;

		// Token: 0x0400057A RID: 1402
		[JsonProperty]
		[DefaultValue(-2147483648)]
		public int FramedPlayerId = int.MinValue;

		// Token: 0x0400057B RID: 1403
		[JsonProperty]
		public bool MaskingSuccessful;

		// Token: 0x0400057C RID: 1404
		[JsonProperty]
		public float MaskingSuccessChance;

		// Token: 0x0400057D RID: 1405
		[JsonProperty]
		public float DetectionRoll;

		// Token: 0x0400057E RID: 1406
		[JsonProperty]
		public bool FramingReceiver;
	}
}
