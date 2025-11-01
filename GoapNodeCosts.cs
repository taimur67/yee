using System;

namespace LoG
{
	// Token: 0x0200012D RID: 301
	public static class GoapNodeCosts
	{
		// Token: 0x040002BB RID: 699
		public static float Base = 100f;

		// Token: 0x040002BC RID: 700
		public static float Unattractive = 1000f;

		// Token: 0x040002BD RID: 701
		public static float Discount_Scalar_FullyFufilled = 0.5f;

		// Token: 0x040002BE RID: 702
		public static float Discount_Scalar_PartialFufilled = 0.75f;

		// Token: 0x040002BF RID: 703
		public static float Penalty_Scalar_NotFulfilled = 1.25f;

		// Token: 0x040002C0 RID: 704
		public static float BaseGoal = GoapNodeCosts.Unattractive;

		// Token: 0x040002C1 RID: 705
		public static float UnattractiveMultiplier = GoapNodeCosts.Unattractive / GoapNodeCosts.Base;
	}
}
