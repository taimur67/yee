using System;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200045E RID: 1118
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class RelicsEconomyData : StaticDataEntity
	{
		// Token: 0x04000A8F RID: 2703
		[JsonProperty]
		public int MaxRelicsValue;
	}
}
