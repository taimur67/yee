using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x020001A2 RID: 418
	public class PlayerTerritory
	{
		// Token: 0x04000385 RID: 901
		public HashSet<HexCoord> OwnedTerritory = new HashSet<HexCoord>();

		// Token: 0x04000386 RID: 902
		public HashSet<HexCoord> InternalBorder = new HashSet<HexCoord>();

		// Token: 0x04000387 RID: 903
		public HashSet<HexCoord> ExternalBorder = new HashSet<HexCoord>();

		// Token: 0x04000388 RID: 904
		public HashSet<HexCoord> UnclaimedExternalBorder = new HashSet<HexCoord>();

		// Token: 0x04000389 RID: 905
		public List<HashSet<HexCoord>> Enclaves = new List<HashSet<HexCoord>>();
	}
}
