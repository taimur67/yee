using System;

namespace LoG
{
	// Token: 0x0200056D RID: 1389
	[Serializable]
	public class ObjectiveCondition_DemandWithReducedCost : ObjectiveCondition_EventFilter<DiplomaticEvent>
	{
		// Token: 0x06001AA1 RID: 6817 RVA: 0x0005CE58 File Offset: 0x0005B058
		protected override bool Filter(TurnContext context, DiplomaticEvent @event, PlayerState owner, PlayerState target)
		{
			if (!base.Filter(context, @event, owner, target))
			{
				return false;
			}
			MakeDemandEvent makeDemandEvent = @event as MakeDemandEvent;
			bool result;
			if (makeDemandEvent == null)
			{
				ExtortEvent extortEvent = @event as ExtortEvent;
				result = (extortEvent != null && extortEvent.CostReduced);
			}
			else
			{
				result = makeDemandEvent.CostReduced;
			}
			return result;
		}
	}
}
