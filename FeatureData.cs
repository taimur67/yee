using System;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200044E RID: 1102
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class FeatureData : IdentifiableStaticData
	{
		// Token: 0x04000A63 RID: 2659
		[JsonProperty]
		public float Frequency;
	}
}
