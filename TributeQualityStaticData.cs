using System;
using System.Collections.Generic;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200042E RID: 1070
	[Serializable]
	public class TributeQualityStaticData : IdentifiableStaticData
	{
		// Token: 0x04000A14 RID: 2580
		[JsonProperty]
		public int TributeQuality;

		// Token: 0x04000A15 RID: 2581
		[JsonProperty]
		public List<WeightedValue<float>> ValueWeights;
	}
}
