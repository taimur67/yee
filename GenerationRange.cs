using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000452 RID: 1106
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public struct GenerationRange
	{
		// Token: 0x04000A6F RID: 2671
		[JsonProperty]
		public int Min;

		// Token: 0x04000A70 RID: 2672
		[JsonProperty]
		public int Max;
	}
}
