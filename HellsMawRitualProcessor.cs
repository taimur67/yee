using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x0200067A RID: 1658
	public class HellsMawRitualProcessor : TargetedRitualActionProcessor<HellsMawRitualOrder, HellsMawRitualData, RitualCastEvent>
	{
		// Token: 0x06001E86 RID: 7814 RVA: 0x00069378 File Offset: 0x00067578
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			RitualCastEvent ritualCastEvent;
			Problem problem = base.CheckPlayerRitualResistance(base.request.TargetPlayerId, out ritualCastEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			HellsMawActiveRitual hellsMawActiveRitual = this.TurnProcessContext.CreateActiveRitual(base.data, base.request, this.CalculateCost(), base.GameEvent.MaskingContext);
			this._player.RitualState.SlottedItems.Add(hellsMawActiveRitual.Id);
			hellsMawActiveRitual.StartRitual(this.TurnProcessContext, this._player, ritualCastEvent);
			return Result.Success;
		}
	}
}
