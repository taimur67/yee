using System;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x0200060C RID: 1548
	public class PromoteLegionEventProcessor : GrandEventActionProcessor<PromoteGamePieceEventOrder, PromoteLegionEventStaticData>
	{
		// Token: 0x06001CC7 RID: 7367 RVA: 0x000634CC File Offset: 0x000616CC
		protected override Result ProcessInternal(PlayGrandEventOrder order)
		{
			GamePiece gamePiece = this.TurnProcessContext.CurrentTurn.FetchGameItem<GamePiece>(base.request.TargetGamePiece);
			if (gamePiece == null)
			{
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					logger.Error(string.Format("GrandEvent {0} is trying to promote null legion {1}", order.EventCardId, base.request.TargetGamePiece));
				}
				return new NoValidTargetsProblem();
			}
			if (gamePiece.Status != GameItemStatus.InPlay)
			{
				return new NoValidTargetsProblem();
			}
			LegionLevelTable levelTable = base._database.Fetch(base.data.LevelTable);
			gamePiece.ProcessLevelUp(this.TurnProcessContext, levelTable, base.data.LevelUps);
			base.GameEvent.AddAffectedPlayerId(gamePiece.ControllingPlayerId);
			return Result.Success;
		}
	}
}
