using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000689 RID: 1673
	public class OngoingRitualStateModifierRitualProcessor : TargetedRitualActionProcessor<ModifyRitualStateRitualOrder, ModifyRitualStateRitualData, RitualCastEvent>
	{
		// Token: 0x06001EBD RID: 7869 RVA: 0x00069D64 File Offset: 0x00067F64
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			RitualCastEvent ritualCastEvent;
			Problem problem = base.CheckPlayerRitualResistance(base.request.TargetPlayerId, out ritualCastEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			RitualStateModifierActiveRitual ritualStateModifierActiveRitual = this.TurnProcessContext.CreateActiveRitual(base.data, base.request, this.CalculateCost(), base.GameEvent.MaskingContext);
			this._player.RitualState.SlottedItems.Add(ritualStateModifierActiveRitual.Id);
			ritualStateModifierActiveRitual.StartRitual(this.TurnProcessContext, this._player, ritualCastEvent);
			return Result.Success;
		}
	}
}
