using System;

namespace LoG
{
	// Token: 0x0200064B RID: 1611
	public class TeleportLegionActionActionProcessor : ActionProcessor<OrderTeleportLegion>
	{
		// Token: 0x06001DB6 RID: 7606 RVA: 0x00066708 File Offset: 0x00064908
		public override Result Validate()
		{
			GamePiece gamePiece = base._currentTurn.FetchGameItem<GamePiece>(base.request.GamePieceId);
			if (!gamePiece.CanTeleport)
			{
				return new DebugProblem(string.Format("{0} cannot teleport.", gamePiece));
			}
			return LegionMovementProcessor.CanEnterCanton(this.TurnProcessContext, gamePiece, base.request.DestinationHex, PathMode.Teleport, null);
		}

		// Token: 0x06001DB7 RID: 7607 RVA: 0x00066764 File Offset: 0x00064964
		public override Result Process(ActionProcessContext context)
		{
			LegionMoveEvent legionMoveEvent = LegionMovementProcessor.TeleportMove(this.TurnProcessContext, base.request.GamePieceId, base.request.DestinationHex, false, base.request.AttackOutcomeIntent);
			base._currentTurn.AddGameEvent<LegionMoveEvent>(legionMoveEvent);
			return legionMoveEvent.Result;
		}
	}
}
