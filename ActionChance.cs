using System;

namespace LoG
{
	// Token: 0x02000412 RID: 1042
	[Serializable]
	public enum ActionChance
	{
		// Token: 0x040009A1 RID: 2465
		Normal,
		// Token: 0x040009A2 RID: 2466
		Likely = -50,
		// Token: 0x040009A3 RID: 2467
		VeryLikely = -100,
		// Token: 0x040009A4 RID: 2468
		Unlikely = 50,
		// Token: 0x040009A5 RID: 2469
		VeryUnlikely = 100,
		// Token: 0x040009A6 RID: 2470
		Forbidden = -2147483648
	}
}
