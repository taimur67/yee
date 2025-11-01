using System;

namespace LoG
{
	// Token: 0x020002EA RID: 746
	public enum GameType
	{
		// Token: 0x0400069A RID: 1690
		None,
		// Token: 0x0400069B RID: 1691
		SingleplayerSkirmish,
		// Token: 0x0400069C RID: 1692
		SingleplayerCampaign = 5,
		// Token: 0x0400069D RID: 1693
		LiveMatchmaking = 2,
		// Token: 0x0400069E RID: 1694
		AsyncMatchmaking,
		// Token: 0x0400069F RID: 1695
		LiveCustom,
		// Token: 0x040006A0 RID: 1696
		AsyncCustom = 6
	}
}
