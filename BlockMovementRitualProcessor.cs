using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000658 RID: 1624
	public class BlockMovementRitualProcessor : TargetedRitualActionProcessor<BlockMovementRitualOrder, BlockMovementRitualData, RitualCastEvent>
	{
		// Token: 0x06001E07 RID: 7687 RVA: 0x000677C0 File Offset: 0x000659C0
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			GamePiece gamePiece = base._currentTurn.FetchGameItem<GamePiece>(base.request.TargetItemId);
			RitualCastEvent ritualCastEvent;
			Problem problem = base.CheckGamePieceRitualResistance(gamePiece, out ritualCastEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			GamePiece gamePiece2 = base._currentTurn.FetchGameItem<GamePiece>(base.request.TargetItemId);
			bool targetHadAlreadyMoved = gamePiece2.LastMoveTurn == base._currentTurn.TurnValue;
			gamePiece2.LastMoveTurn = base._currentTurn.TurnValue;
			BlockMovementEvent ev = new BlockMovementEvent(this._player.Id, gamePiece, targetHadAlreadyMoved);
			ritualCastEvent.AddChildEvent<BlockMovementEvent>(ev);
			return Result.Success;
		}
	}
}
