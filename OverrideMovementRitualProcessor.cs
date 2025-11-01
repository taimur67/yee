using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x0200068B RID: 1675
	public class OverrideMovementRitualProcessor : TargetedRitualActionProcessor<OverrideMovementRitualOrder, OverrideMovementRitualData, RitualCastEvent>
	{
		// Token: 0x06001EC2 RID: 7874 RVA: 0x00069E2C File Offset: 0x0006802C
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			GamePiece gamePiece = base._currentTurn.FetchGameItem<GamePiece>(base.request.TargetItemId);
			RitualCastEvent ritualCastEvent;
			Problem problem = base.CheckGamePieceRitualResistance(gamePiece, out ritualCastEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			bool flag = base._currentTurn.FetchGameItem<GamePiece>(base.request.TargetItemId).LastMoveTurn == base._currentTurn.TurnValue;
			OverrideMovementEvent overrideMovementEvent = ritualCastEvent.AddChildEvent<OverrideMovementEvent>(new OverrideMovementEvent(this._player.Id, gamePiece, flag));
			if (!flag)
			{
				LegionMoveEvent legionMoveEvent = LegionMovementProcessor.GroundMove(this.TurnProcessContext, base.request.TargetItemId, base.request.MovePath, AttackOutcomeIntent.Default);
				legionMoveEvent.TriggeringPlayerID = this._player.Id;
				legionMoveEvent.AddAffectedPlayerId(gamePiece.ControllingPlayerId);
				overrideMovementEvent.AddChildEvent<LegionMoveEvent>(legionMoveEvent);
			}
			return Result.Success;
		}
	}
}
