using System;
using System.Collections.Generic;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000694 RID: 1684
	public class RevealOrdersRitualProcessor : TargetedRitualActionProcessor<RevealOrdersRitualOrder, RevealOrdersRitualData, RitualCastEvent>
	{
		// Token: 0x06001EE6 RID: 7910 RVA: 0x0006A6CC File Offset: 0x000688CC
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			RitualCastEvent ritualCastEvent;
			Problem problem = base.CheckPlayerRitualResistance(base.request.TargetPlayerId, out ritualCastEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			List<ActionableOrder> orders = base._currentTurn.FindPlayerState(base.request.TargetPlayerId, null).PlayerTurn.Orders;
			RevealOrdersEvent ev = new RevealOrdersEvent(base._currentTurn.TurnValue, orders, this._player.Id, base.request.TargetPlayerId);
			ritualCastEvent.AddChildEvent<RevealOrdersEvent>(ev);
			return Result.Success;
		}
	}
}
