using System;

namespace LoG
{
	// Token: 0x020005AB RID: 1451
	public abstract class ObjectiveCondition_SuccessfullyCastRitual<T> : ObjectiveCondition_EventFilter<T> where T : RitualCastEvent
	{
		// Token: 0x06001B3F RID: 6975 RVA: 0x0005EB8B File Offset: 0x0005CD8B
		protected override bool Filter(TurnContext context, T @event, PlayerState owner, PlayerState target)
		{
			return @event.Succeeded && base.Filter(context, @event, owner, target);
		}
	}
}
