using System;
using System.Linq;

namespace LoG
{
	// Token: 0x02000165 RID: 357
	public class WPEveryTitanHasOrders : WorldProperty
	{
		// Token: 0x060006EF RID: 1775 RVA: 0x00022028 File Offset: 0x00020228
		private static bool OrderMovesTitan(ActionableOrder order, Identifier titanId)
		{
			OrderMoveLegion orderMoveLegion = order as OrderMoveLegion;
			return orderMoveLegion != null && orderMoveLegion.GamePieceId == titanId;
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x00022050 File Offset: 0x00020250
		public static bool Check(GOAPPlanner planner)
		{
			foreach (GamePiece gamePiece in from gp in planner.PlayerViewOfTurnState.GetActiveGamePiecesForPlayer(planner.PlayerState.Id)
			where gp.SubCategory == GamePieceCategory.Titan
			select gp)
			{
				if (!WPEveryTitanHasOrders.Check(planner, gamePiece.Id))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x000220E0 File Offset: 0x000202E0
		public static bool Check(GOAPPlanner planner, Identifier titanId)
		{
			bool flag = planner.PlannedTurn.Orders.Any((ActionableOrder order) => WPEveryTitanHasOrders.OrderMovesTitan(order, titanId));
			bool flag2 = planner.AITransientData.LegionsWithBlockedMovement.Contains(titanId);
			return flag || flag2;
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x0002212F File Offset: 0x0002032F
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return WPEveryTitanHasOrders.Check(planner);
		}
	}
}
