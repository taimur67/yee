using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200073C RID: 1852
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class RemoveTributeForActiveGamePiecesEffectStaticData : EdictEffectStaticData
	{
		// Token: 0x04000F40 RID: 3904
		[JsonProperty]
		public bool IncludeStrongholds;

		// Token: 0x04000F41 RID: 3905
		[JsonProperty]
		public bool IncludePersonalGuards;

		// Token: 0x04000F42 RID: 3906
		[JsonProperty]
		public List<GamePieceCategory> Categories;

		// Token: 0x04000F43 RID: 3907
		[JsonProperty]
		public int TributePerGamePiece;
	}
}
