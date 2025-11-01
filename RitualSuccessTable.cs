using System;
using System.Collections.Generic;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000461 RID: 1121
	[Serializable]
	public class RitualSuccessTable : StaticDataEntity
	{
		// Token: 0x04000A93 RID: 2707
		[JsonProperty]
		public List<RitualSuccessChance> SuccessChances;
	}
}
