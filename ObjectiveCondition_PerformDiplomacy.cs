using System;

namespace LoG
{
	// Token: 0x02000599 RID: 1433
	public abstract class ObjectiveCondition_PerformDiplomacy<T> : ObjectiveCondition_EventFilter<T> where T : DiplomaticEvent
	{
		// Token: 0x06001B1B RID: 6939 RVA: 0x0005E7BC File Offset: 0x0005C9BC
		protected override bool Filter(TurnContext context, T @event, PlayerState owner, PlayerState target)
		{
			return !(@event is DiplomaticResponseEvent) && base.Filter(context, @event, owner, target);
		}
	}
}
