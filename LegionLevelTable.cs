using System;
using System.Collections.Generic;
using Core.StaticData;
using LoG.Simulation.Extensions;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200044B RID: 1099
	[Serializable]
	public class LegionLevelTable : StaticDataEntity
	{
		// Token: 0x060014F3 RID: 5363 RVA: 0x0004F99A File Offset: 0x0004DB9A
		public LevelUpData GetDataForLevel(int targetLevel)
		{
			return this.LevelConfigs.SelectClosestUnderOrEqual((LevelUpData x) => x.TargetLevel, targetLevel);
		}

		// Token: 0x04000A5D RID: 2653
		[JsonProperty]
		public List<LevelUpData> LevelConfigs;
	}
}
