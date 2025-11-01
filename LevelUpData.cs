using System;
using System.Collections.Generic;
using Core.StaticData;

namespace LoG
{
	// Token: 0x0200044A RID: 1098
	[Serializable]
	public struct LevelUpData
	{
		// Token: 0x04000A59 RID: 2649
		public int TargetLevel;

		// Token: 0x04000A5A RID: 2650
		public int RequiredExperience;

		// Token: 0x04000A5B RID: 2651
		public int BonusOptionsCount;

		// Token: 0x04000A5C RID: 2652
		public List<ConfigRef<GamePieceRewardStaticData>> BonusPool;
	}
}
