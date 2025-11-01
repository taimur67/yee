using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000661 RID: 1633
	public class ConvertGamePieceItemRitualProcessor : TargetedRitualActionProcessor<ConvertGamePieceRitualOrder, ConvertGamePieceRitualData, ConvertGamePieceRitualEvent>
	{
		// Token: 0x06001E27 RID: 7719 RVA: 0x00067F20 File Offset: 0x00066120
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			GamePiece gamePiece = base._currentTurn.FetchGameItem<GamePiece>(base.request.TargetItemId);
			ConvertGamePieceRitualEvent convertGamePieceRitualEvent;
			Problem problem = base.CheckGamePieceRitualResistance(gamePiece, out convertGamePieceRitualEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			GameItemOwnershipChanged ev = new GameItemOwnershipChanged(base.request.TargetPlayerId, this._player.Id, gamePiece, gamePiece.Category);
			convertGamePieceRitualEvent.AddChildEvent<GameItemOwnershipChanged>(ev);
			GamePiece gamePiece2 = base._currentTurn.FetchGameItem<GamePiece>(base.request.TargetItemId);
			gamePiece2.ControllingPlayerId = this._player.Id;
			gamePiece2.LastConvertedTurn = base._currentTurn.TurnValue;
			gamePiece2.NextUpkeepTurn = base._currentTurn.TurnValue + 1;
			gamePiece2.UseUpkeep(true);
			convertGamePieceRitualEvent.AddChildEvent<RepatriateLegionEvent>(gamePiece2.RepatriateToStronghold(this.TurnProcessContext));
			return Result.Success;
		}
	}
}
