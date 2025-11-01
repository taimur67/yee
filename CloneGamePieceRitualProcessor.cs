using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x0200065F RID: 1631
	public class CloneGamePieceRitualProcessor : TargetedRitualActionProcessor<CloneGamePieceRitualOrder, CloneGamePieceRitualData, RitualCastEvent>
	{
		// Token: 0x06001E1F RID: 7711 RVA: 0x00067DA8 File Offset: 0x00065FA8
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			RitualCastEvent ritualCastEvent;
			Problem problem = base.CheckPlayerRitualResistance(base.request.TargetPlayerId, out ritualCastEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			CloneGamePieceActiveRitual cloneGamePieceActiveRitual = this.TurnProcessContext.CreateActiveRitual(base.data, base.request, this.CalculateCost(), base.GameEvent.MaskingContext);
			this._player.RitualState.SlottedItems.Add(cloneGamePieceActiveRitual.Id);
			cloneGamePieceActiveRitual.CloneHexes = base.request.CloneHexes;
			cloneGamePieceActiveRitual.TargetGamePiece = base.request.TargetItemId;
			cloneGamePieceActiveRitual.StartRitual(this.TurnProcessContext, this._player, ritualCastEvent);
			return Result.Success;
		}
	}
}
