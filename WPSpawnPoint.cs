using System;
using System.Linq;

namespace LoG
{
	// Token: 0x02000190 RID: 400
	public class WPSpawnPoint : WorldProperty
	{
		// Token: 0x06000766 RID: 1894 RVA: 0x0002306C File Offset: 0x0002126C
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return viewContext.CurrentTurn.HexBoard.Hexes.Any((Hex hex) => viewContext.IsValidSpawnPoint(playerState, hex.HexCoord, null));
		}
	}
}
