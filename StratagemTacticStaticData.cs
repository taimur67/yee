using System;
using System.Collections.Generic;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000477 RID: 1143
	public class StratagemTacticStaticData : StaticDataEntity
	{
		// Token: 0x04000ABA RID: 2746
		[JsonProperty]
		public StratagemType StratagemType;

		// Token: 0x04000ABB RID: 2747
		[JsonProperty]
		public List<ConfigRef<StratagemTacticLevelStaticData>> TacticLevels;
	}
}
