using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x0200065D RID: 1629
	public class BuffGamePieceHitPointsRitualOrderProcessor : TargetedRitualActionProcessor<BuffGamePieceHitPointsRitualOrder, BuffGamePieceHitPointsRitualData, RitualCastEvent>
	{
		// Token: 0x06001E19 RID: 7705 RVA: 0x00067C48 File Offset: 0x00065E48
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			RitualCastEvent ritualCastEvent;
			Problem problem = base.CheckPlayerRitualResistance(base.request.TargetPlayerId, out ritualCastEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			BuffGamePieceHitPointsActiveRitual buffGamePieceHitPointsActiveRitual = this.TurnProcessContext.CreateActiveRitual(base.data, base.request, this.CalculateCost(), base.GameEvent.MaskingContext);
			this._player.RitualState.SlottedItems.Add(buffGamePieceHitPointsActiveRitual.Id);
			buffGamePieceHitPointsActiveRitual.StartRitual(this.TurnProcessContext, this._player, ritualCastEvent);
			return Result.Success;
		}
	}
}
