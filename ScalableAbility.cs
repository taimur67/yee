using System;
using System.Collections.Generic;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000464 RID: 1124
	[Serializable]
	public class ScalableAbility : StaticDataEntity
	{
		// Token: 0x04000A98 RID: 2712
		[JsonProperty]
		public ArchfiendStat Stat;

		// Token: 0x04000A99 RID: 2713
		[JsonProperty]
		public List<LevelValue> Levels = new List<LevelValue>();
	}
}
