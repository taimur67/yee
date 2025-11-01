using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x0200028B RID: 651
	public class GameGenerationContext
	{
		// Token: 0x06000CA3 RID: 3235 RVA: 0x00031EE8 File Offset: 0x000300E8
		public bool Contains(HexCoord coord)
		{
			HexCoord item = this.Board.ToRelativeHex(coord);
			return this.AvailableHexes.Contains(item);
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x00031F10 File Offset: 0x00030110
		public GameGenerationContext(HexBoard board, int seed)
		{
			this.Board = board;
			this.Rand = new SimulationRandom(seed);
			this.AvailableHexes = new List<HexCoord>(from x in board.Hexes
			select x.HexCoord);
		}

		// Token: 0x0400059A RID: 1434
		public readonly HexBoard Board;

		// Token: 0x0400059B RID: 1435
		public readonly SimulationRandom Rand;

		// Token: 0x0400059C RID: 1436
		public readonly List<HexCoord> AvailableHexes;

		// Token: 0x0400059D RID: 1437
		public readonly List<HexIsland> Features = new List<HexIsland>();

		// Token: 0x0400059E RID: 1438
		public readonly List<HexCoord> Structures = new List<HexCoord>();

		// Token: 0x0400059F RID: 1439
		public readonly List<HexCoord> PathsFromPlayersToStructures = new List<HexCoord>();
	}
}
