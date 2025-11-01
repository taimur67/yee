using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020001E5 RID: 485
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class GamePieceStandingOnTerrainFilter : GamePieceFilter
	{
		// Token: 0x06000977 RID: 2423 RVA: 0x0002C9EC File Offset: 0x0002ABEC
		protected override bool Filter(TurnContext context, GamePiece entity)
		{
			if (!entity.IsAlive())
			{
				return false;
			}
			Hex hex = context.HexBoard[entity.Location];
			return this.TerrainTypes.Contains(hex.Type);
		}

		// Token: 0x0400048F RID: 1167
		[JsonProperty]
		public List<TerrainType> TerrainTypes = new List<TerrainType>();
	}
}
