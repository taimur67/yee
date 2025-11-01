using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000699 RID: 1689
	public class RitualResistanceRitualProcessor : TargetedRitualActionProcessor<RitualResistanceRitualOrder, RitualResistanceRitualData, RitualCastEvent>
	{
		// Token: 0x06001F12 RID: 7954 RVA: 0x0006B2B8 File Offset: 0x000694B8
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			RitualCastEvent ritualCastEvent;
			Problem problem = base.CheckPlayerRitualResistance(base.request.TargetPlayerId, out ritualCastEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			RitualResistanceActiveRitual ritualResistanceActiveRitual = this.TurnProcessContext.CreateActiveRitual(base.data, base.request, this.CalculateCost(), base.GameEvent.MaskingContext);
			this._player.RitualState.SlottedItems.Add(ritualResistanceActiveRitual.Id);
			ritualResistanceActiveRitual.StartRitual(this.TurnProcessContext, this._player, ritualCastEvent);
			return Result.Success;
		}
	}
}
