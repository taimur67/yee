using System;

namespace LoG
{
	// Token: 0x0200063D RID: 1597
	public class MoveLegionActionActionProcessor : ActionProcessor<OrderMarchLegion>
	{
		// Token: 0x06001D84 RID: 7556 RVA: 0x00065F20 File Offset: 0x00064120
		public override Result Process(ActionProcessContext context)
		{
			LegionMoveEvent legionMoveEvent = LegionMovementProcessor.GroundMove(this.TurnProcessContext, base.request.GamePieceId, base.request.MovePath, base.request.AttackOutcomeIntent);
			base._currentTurn.AddGameEvent<LegionMoveEvent>(legionMoveEvent);
			return legionMoveEvent.Result;
		}

		// Token: 0x06001D85 RID: 7557 RVA: 0x00065F6D File Offset: 0x0006416D
		public override Result AIPreview(ActionProcessContext context)
		{
			return this.Process(context);
		}

		// Token: 0x06001D86 RID: 7558 RVA: 0x00065F78 File Offset: 0x00064178
		public override Result Validate()
		{
			GamePiece gamePiece = base._currentTurn.FetchGameItem<GamePiece>(base.request.GamePieceId);
			if (gamePiece == null)
			{
				return Result.Failure;
			}
			if (this._player.Id != gamePiece.ControllingPlayerId)
			{
				return Result.ConvertedBeforeMoving(gamePiece, base.request.MovePath[0]);
			}
			return LegionMovementProcessor.CheckForProblemInMoveSequence(this.TurnProcessContext, gamePiece, base.request.MovePath);
		}
	}
}
