using System;

namespace LoG
{
	// Token: 0x02000573 RID: 1395
	[Serializable]
	public class ObjectiveCondition_DestroyLegionOnLava : ObjectiveCondition_DestroyLegions
	{
		// Token: 0x06001AAF RID: 6831 RVA: 0x0005D220 File Offset: 0x0005B420
		protected override bool Filter(TurnContext context, LegionKilledEvent @event, PlayerState owner, PlayerState target)
		{
			if (!base.Filter(context, @event, owner, target))
			{
				return false;
			}
			TurnState currentTurn = context.CurrentTurn;
			GamePiece gamePiece = currentTurn.FetchGameItem<GamePiece>(@event.GamePieceId);
			return currentTurn.HexBoard[gamePiece.Location].Type == TerrainType.Lava;
		}
	}
}
