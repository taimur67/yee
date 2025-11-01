using System;
using System.Collections.Generic;
using System.Linq;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000430 RID: 1072
	public class CardGenerationData : IdentifiableStaticData
	{
		// Token: 0x060014CF RID: 5327 RVA: 0x0004F5F4 File Offset: 0x0004D7F4
		public IEnumerable<WeightedValue<ResourceTypes>> GetResourceDistributionsFor(ResourceTypes resource)
		{
			return from t in this.ResourceDistribution
			where t.Value == resource
			select t;
		}

		// Token: 0x04000A1A RID: 2586
		[JsonProperty]
		public List<WeightedValue<int>> CardCombinations;

		// Token: 0x04000A1B RID: 2587
		[JsonProperty]
		public List<WeightedValue<ResourceTypes>> ResourceDistribution = new List<WeightedValue<ResourceTypes>>();

		// Token: 0x04000A1C RID: 2588
		[JsonProperty]
		public List<ConfigRef<TributeQualityStaticData>> TributeDemandQualitySelections = new List<ConfigRef<TributeQualityStaticData>>();
	}
}
