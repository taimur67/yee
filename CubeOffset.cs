using System;

namespace LoG
{
	// Token: 0x020001D4 RID: 468
	public static class CubeOffset
	{
		// Token: 0x04000463 RID: 1123
		public static readonly CubeCoord Up = new CubeCoord(0f, 1f, -1f);

		// Token: 0x04000464 RID: 1124
		public static readonly CubeCoord Down = new CubeCoord(0f, -1f, 1f);

		// Token: 0x04000465 RID: 1125
		public static readonly CubeCoord UpRight = new CubeCoord(1f, 0f, -1f);

		// Token: 0x04000466 RID: 1126
		public static readonly CubeCoord DownLeft = new CubeCoord(-1f, 0f, 1f);

		// Token: 0x04000467 RID: 1127
		public static readonly CubeCoord UpLeft = new CubeCoord(-1f, 1f, 0f);

		// Token: 0x04000468 RID: 1128
		public static readonly CubeCoord DownRight = new CubeCoord(1f, -1f, 0f);

		// Token: 0x04000469 RID: 1129
		public static readonly CubeCoord None = new CubeCoord(0f, 0f, 0f);
	}
}
