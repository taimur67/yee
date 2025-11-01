using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200044F RID: 1103
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class RiverData : FeatureData
	{
		// Token: 0x04000A64 RID: 2660
		[JsonProperty]
		public int NumberOfPoints;

		// Token: 0x04000A65 RID: 2661
		[JsonProperty]
		public int Windiness;
	}
}
