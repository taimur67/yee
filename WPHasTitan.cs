using System;
using System.Linq;

namespace LoG
{
	// Token: 0x02000171 RID: 369
	public class WPHasTitan : WorldProperty
	{
		// Token: 0x06000713 RID: 1811 RVA: 0x00022656 File Offset: 0x00020856
		public static bool Check(TurnState turn, PlayerState player)
		{
			return turn.GetAllActiveLegionsForPlayer(player.Id).Any((GamePiece t) => t.SubCategory == GamePieceCategory.Titan);
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x00022688 File Offset: 0x00020888
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return WPHasTitan.Check(viewContext.CurrentTurn, playerState);
		}
	}
}
