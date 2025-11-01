using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x020001D8 RID: 472
	public struct HexBounds
	{
		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000929 RID: 2345 RVA: 0x0002BB95 File Offset: 0x00029D95
		public HexCoord End
		{
			get
			{
				return new HexCoord(this.Start.row + this.Height, this.Start.column + this.Width);
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x0600092A RID: 2346 RVA: 0x0002BBC0 File Offset: 0x00029DC0
		public IEnumerable<HexCoord> Range
		{
			get
			{
				return HexUtils.EnumerateRange(this.Start, this.Width, this.Height);
			}
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x0002BBD9 File Offset: 0x00029DD9
		public HexBounds(HexCoord start, int width, int height)
		{
			this.Start = start;
			this.Width = width;
			this.Height = height;
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x0002BBF0 File Offset: 0x00029DF0
		public static HexBounds FromCenter(HexCoord center, int width, int height)
		{
			int column = (int)Math.Ceiling((double)((float)width / 2f));
			int row = (int)Math.Ceiling((double)((float)height / 2f));
			HexCoord hexCoord = new HexCoord(row, column);
			return new HexBounds(center - hexCoord, width, height);
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x0002BC38 File Offset: 0x00029E38
		public static HexBounds Between(HexCoord start, HexCoord end)
		{
			return new HexBounds(new HexCoord(Math.Min(start.row, end.row), Math.Min(start.column, end.column)), Math.Abs(start.column - end.column), Math.Abs(start.row - end.row));
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x0002BC98 File Offset: 0x00029E98
		public bool InRange(HexCoord coord)
		{
			return coord.row >= this.Start.row && coord.column >= this.Start.column && coord.row < this.End.row && coord.column < this.End.column;
		}

		// Token: 0x04000479 RID: 1145
		public HexCoord Start;

		// Token: 0x0400047A RID: 1146
		public int Width;

		// Token: 0x0400047B RID: 1147
		public int Height;
	}
}
