using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x02000166 RID: 358
	public class WPForcesFlanking : WorldProperty
	{
		// Token: 0x060006F4 RID: 1780 RVA: 0x0002213F File Offset: 0x0002033F
		public static WPForcesFlanking RequiredTargetToBeFlanked(Identifier flankedGamePieceID)
		{
			return new WPForcesFlanking(flankedGamePieceID, Identifier.Invalid, Identifier.Invalid, 1);
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x0002214A File Offset: 0x0002034A
		public static WPForcesFlanking RequiresFlankersToAssist(Identifier flankedGamePieceID, Identifier supportedAttackerID, int requiredNumFlankers = 1)
		{
			return new WPForcesFlanking(flankedGamePieceID, Identifier.Invalid, supportedAttackerID, requiredNumFlankers);
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x00022155 File Offset: 0x00020355
		public static WPForcesFlanking ProvidesFlanking(Identifier flankedGamePieceID, Identifier flankingGamePiece)
		{
			return new WPForcesFlanking(flankedGamePieceID, flankingGamePiece, Identifier.Invalid, 1);
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x00022160 File Offset: 0x00020360
		private WPForcesFlanking(Identifier flankedGamePieceID, Identifier flankingGamePieceID = Identifier.Invalid, Identifier supportedAttackerID = Identifier.Invalid, int requiredNumFlankers = 1)
		{
			this.FlankingGamePieceID = flankingGamePieceID;
			this.FlankedGamePieceID = flankedGamePieceID;
			this.RequiredNumFlankers = requiredNumFlankers;
			this.SupportedAttackerID = supportedAttackerID;
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x00022188 File Offset: 0x00020388
		public override WPProvidesEffect ProvidesEffect(WorldProperty precondition)
		{
			WPForcesFlanking wpforcesFlanking = precondition as WPForcesFlanking;
			if (wpforcesFlanking == null)
			{
				return WPProvidesEffect.No;
			}
			if (wpforcesFlanking.FlankedGamePieceID != this.FlankedGamePieceID)
			{
				return WPProvidesEffect.No;
			}
			if (wpforcesFlanking.SupportedAttackerID == this.FlankingGamePieceID)
			{
				return WPProvidesEffect.No;
			}
			return WPProvidesEffect.Yes;
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x000221C4 File Offset: 0x000203C4
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			int num = this.RequiredNumFlankers;
			GamePiece gamePiece = viewContext.CurrentTurn.FetchGameItem<GamePiece>(this.FlankedGamePieceID);
			if (gamePiece != null && gamePiece.Location != HexCoord.Invalid && gamePiece.Location != HexCoord.Invalid)
			{
				IEnumerable<HexCoord> neighbours = viewContext.HexBoard.GetNeighbours(gamePiece.Location, false);
				foreach (GamePiece gamePiece2 in viewContext.CurrentTurn.GetAllActiveLegionsForPlayer(playerState.Id))
				{
					if (IEnumerableExtensions.Contains<HexCoord>(neighbours, gamePiece2.Location) && --num <= 0)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x04000332 RID: 818
		public Identifier FlankingGamePieceID;

		// Token: 0x04000333 RID: 819
		public Identifier FlankedGamePieceID;

		// Token: 0x04000334 RID: 820
		public Identifier SupportedAttackerID;

		// Token: 0x04000335 RID: 821
		public int RequiredNumFlankers;
	}
}
