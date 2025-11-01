using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x0200067D RID: 1661
	public class IncreasePassivePrestigeItemRitualProcessor : TargetedRitualActionProcessor<IncreasePassivePrestigeRitualOrder, IncreasePassivePrestigeRitualData, RitualCastEvent>
	{
		// Token: 0x06001E90 RID: 7824 RVA: 0x000694D0 File Offset: 0x000676D0
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			RitualCastEvent ritualCastEvent;
			Problem problem = base.CheckPlayerRitualResistance(base.request.TargetPlayerId, out ritualCastEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			IncreasePassivePrestigeActiveRitual increasePassivePrestigeActiveRitual = this.TurnProcessContext.CreateActiveRitual(base.data, base.request, this.CalculateCost(), base.GameEvent.MaskingContext);
			this._player.RitualState.SlottedItems.Add(increasePassivePrestigeActiveRitual.Id);
			increasePassivePrestigeActiveRitual.StartRitual(this.TurnProcessContext, this._player, ritualCastEvent);
			return Result.Success;
		}
	}
}
