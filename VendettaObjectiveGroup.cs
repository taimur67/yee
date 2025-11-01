using System;
using System.Collections.Generic;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200052F RID: 1327
	[Serializable]
	public class VendettaObjectiveGroup : IdentifiableStaticData
	{
		// Token: 0x04000BC3 RID: 3011
		[JsonProperty]
		public List<ConfigRef<VendettaObjectiveGenerator>> ObjectiveGenerators;

		// Token: 0x04000BC4 RID: 3012
		[JsonProperty]
		public int MinTurnLimit;

		// Token: 0x04000BC5 RID: 3013
		[JsonProperty]
		public int MaxTurnLimit;
	}
}
