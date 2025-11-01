using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x020005F5 RID: 1525
	public class DestroyRandomLegionEventProcessor : GrandEventActionProcessor<DestroyRandomLegionEventOrder, DestroyRandomLegionEventStaticData>
	{
		// Token: 0x06001C8B RID: 7307 RVA: 0x000626F8 File Offset: 0x000608F8
		protected override Result ProcessInternal(PlayGrandEventOrder order)
		{
			List<GamePiece> list = IEnumerableExtensions.ToList<GamePiece>(from gamePiece in base._currentTurn.GetAllActiveLegions()
			where gamePiece.ControllingPlayerId != this._player.Id
			where gamePiece.ControllingPlayerId != -1
			where base.data.CanSelectPersonalGuard || !gamePiece.IsPersonalGuard(base._currentTurn)
			select gamePiece);
			if (!IEnumerableExtensions.Any<GamePiece>(list))
			{
				return new NoValidTargetsProblem();
			}
			GamePiece gamePiece2 = list.WeightedRandom((GamePiece gamePiece) => (float)gamePiece.Level, base.Random, false);
			LegionKilledEvent ev = this.TurnProcessContext.KillGamePiece(gamePiece2, this._player.Id);
			base.GameEvent.AddChildEvent<LegionKilledEvent>(ev);
			return Result.Success;
		}
	}
}
