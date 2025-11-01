using System;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000455 RID: 1109
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class TurnTimer_Data : IdentifiableStaticData
	{
		// Token: 0x04000A78 RID: 2680
		[JsonProperty]
		public int TimeValue;

		// Token: 0x04000A79 RID: 2681
		[JsonProperty]
		public TimeSpanType TimeSpanType;
	}
}
