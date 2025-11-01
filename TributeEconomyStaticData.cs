using System;
using System.Collections.Generic;
using System.Linq;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200042F RID: 1071
	[Serializable]
	public class TributeEconomyStaticData : IdentifiableStaticData
	{
		// Token: 0x17000301 RID: 769
		// (get) Token: 0x060014CD RID: 5325 RVA: 0x0004F5BE File Offset: 0x0004D7BE
		[JsonIgnore]
		public IEnumerable<ResourceAccumulation> StartingTributeCards
		{
			get
			{
				return from t in this.StartingTributeCardValues
				select t;
			}
		}

		// Token: 0x04000A16 RID: 2582
		[JsonProperty]
		public int NumResourceDrawsToStart;

		// Token: 0x04000A17 RID: 2583
		[JsonProperty]
		public List<CostStaticData> StartingTributeCardValues;

		// Token: 0x04000A18 RID: 2584
		[JsonProperty]
		public ConfigRef<CardGenerationData> DemandTributeGenerationData;

		// Token: 0x04000A19 RID: 2585
		[JsonProperty]
		public ConfigRef<CardGenerationData> OfferingTributeGenerationData;
	}
}
