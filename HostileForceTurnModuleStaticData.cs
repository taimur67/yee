using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000405 RID: 1029
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class HostileForceTurnModuleStaticData : NeutralForceTurnModuleStaticData
	{
		// Token: 0x04000911 RID: 2321
		[JsonProperty]
		public int MinTurnDuration;

		// Token: 0x04000912 RID: 2322
		[JsonProperty]
		public int TurnDurationLimitStart;

		// Token: 0x04000913 RID: 2323
		[JsonProperty]
		public int TurnDurationLimitEnd;

		// Token: 0x04000914 RID: 2324
		[JsonProperty]
		public int SpawnRadius;
	}
}
