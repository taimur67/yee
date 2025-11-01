using System;

namespace LoG
{
	// Token: 0x020001D5 RID: 469
	public static class HexOffset
	{
		// Token: 0x0400046A RID: 1130
		public static readonly HexCoord Up = new HexCoord(-1, 0);

		// Token: 0x0400046B RID: 1131
		public static readonly HexCoord Down = new HexCoord(1, 0);

		// Token: 0x0400046C RID: 1132
		public static readonly HexCoord UpRightEven = new HexCoord(-1, 1);

		// Token: 0x0400046D RID: 1133
		public static readonly HexCoord DownLeftEven = new HexCoord(0, -1);

		// Token: 0x0400046E RID: 1134
		public static readonly HexCoord UpLeftEven = new HexCoord(-1, -1);

		// Token: 0x0400046F RID: 1135
		public static readonly HexCoord DownRightEven = new HexCoord(0, 1);

		// Token: 0x04000470 RID: 1136
		public static readonly HexCoord UpRightOdd = new HexCoord(0, 1);

		// Token: 0x04000471 RID: 1137
		public static readonly HexCoord DownLeftOdd = new HexCoord(1, -1);

		// Token: 0x04000472 RID: 1138
		public static readonly HexCoord UpLeftOdd = new HexCoord(0, -1);

		// Token: 0x04000473 RID: 1139
		public static readonly HexCoord DownRightOdd = new HexCoord(1, 1);

		// Token: 0x04000474 RID: 1140
		public static readonly HexCoord None = new HexCoord(0, 0);
	}
}
