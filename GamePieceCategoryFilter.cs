using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020001E3 RID: 483
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class GamePieceCategoryFilter : GamePieceFilter
	{
		// Token: 0x06000973 RID: 2419 RVA: 0x0002C96B File Offset: 0x0002AB6B
		protected override bool Filter(TurnContext context, GamePiece entity)
		{
			return this.Categories.Contains(entity.SubCategory);
		}

		// Token: 0x0400048E RID: 1166
		[JsonProperty]
		public List<GamePieceCategory> Categories = new List<GamePieceCategory>();
	}
}
