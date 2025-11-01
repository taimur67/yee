using System;
using System.Collections.Generic;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000460 RID: 1120
	[Serializable]
	public class RitualMaskingSuccessTable : StaticDataEntity
	{
		// Token: 0x04000A92 RID: 2706
		[JsonProperty]
		public List<RitualSuccessChance> SuccessChances;
	}
}
