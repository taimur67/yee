using System;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000474 RID: 1140
	public class StratagemTacticLevelStaticData : StaticDataEntity
	{
		// Token: 0x04000AA8 RID: 2728
		[JsonProperty]
		public int Level;

		// Token: 0x04000AA9 RID: 2729
		[JsonProperty]
		public PowerType PowerType;

		// Token: 0x04000AAA RID: 2730
		[JsonProperty]
		public CostStaticData Cost;
	}
}
