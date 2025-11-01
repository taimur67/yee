using System;

namespace LoG
{
	// Token: 0x020001AB RID: 427
	[Flags]
	public enum GamePieceAvoidance
	{
		// Token: 0x0400039F RID: 927
		None = 0,
		// Token: 0x040003A0 RID: 928
		FriendlyLegion = 1,
		// Token: 0x040003A1 RID: 929
		EnemyLegion = 4,
		// Token: 0x040003A2 RID: 930
		FriendlyFixture = 8,
		// Token: 0x040003A3 RID: 931
		EnemyFixture = 16,
		// Token: 0x040003A4 RID: 932
		All = -1
	}
}
