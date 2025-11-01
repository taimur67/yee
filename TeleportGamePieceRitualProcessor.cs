using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x020006BF RID: 1727
	public class TeleportGamePieceRitualProcessor : TargetedRitualActionProcessor<TeleportGamePieceRitualOrder, TeleportGamePieceRitualData, RitualCastEvent>
	{
		// Token: 0x06001F99 RID: 8089 RVA: 0x0006C800 File Offset: 0x0006AA00
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			GamePiece gamePiece = base._currentTurn.FetchGameItem<GamePiece>(base.request.TargetItemId);
			RitualCastEvent ritualCastEvent;
			Problem problem = base.CheckGameItemRitualResistance(gamePiece, gamePiece.ControllingPlayerId, out ritualCastEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			LegionMoveEvent legionMoveEvent = LegionMovementProcessor.TeleportMove(this.TurnProcessContext, gamePiece, base.request.TargetHex, true, AttackOutcomeIntent.Default);
			ritualCastEvent.AddChildEvent<LegionMoveEvent>(legionMoveEvent);
			Problem problem2 = legionMoveEvent.Result as Problem;
			if (problem2 != null)
			{
				return problem2;
			}
			return Result.Success;
		}
	}
}
