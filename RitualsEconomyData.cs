using System;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200045F RID: 1119
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class RitualsEconomyData : StaticDataEntity
	{
		// Token: 0x04000A90 RID: 2704
		[JsonProperty]
		public CostStaticData MaskingCost;

		// Token: 0x04000A91 RID: 2705
		[JsonProperty]
		public CostStaticData FramingCost;
	}
}
