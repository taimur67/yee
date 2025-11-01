using System;
using System.Linq;

namespace LoG
{
	// Token: 0x020001D7 RID: 471
	public static class HexBoardExtensions
	{
		// Token: 0x06000927 RID: 2343 RVA: 0x0002BAE8 File Offset: 0x00029CE8
		public static bool IsTerritoryEdge(this HexBoard board, HexCoord coord)
		{
			int owner = board.GetOwnership(coord);
			return owner != -1 && board.GetNeighbours(coord, false).Any((HexCoord x) => board.GetOwnership(x) != owner);
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x0002BB40 File Offset: 0x00029D40
		public static bool IsBorderCanton(this HexBoard board, HexCoord coord)
		{
			int ownerA = board.GetOwnership(coord);
			return ownerA != -1 && board.GetNeighbours(coord, false).Any((HexCoord x) => board.GetOwnership(x) != ownerA && !board[x].IsUnclaimed());
		}
	}
}
