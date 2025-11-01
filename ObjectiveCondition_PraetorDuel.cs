using System;

namespace LoG
{
	// Token: 0x0200059B RID: 1435
	[Serializable]
	public class ObjectiveCondition_PraetorDuel : ObjectiveCondition_EventFilter<PraetorDuelOutcomeEvent>
	{
		// Token: 0x06001B1E RID: 6942 RVA: 0x0005E7E8 File Offset: 0x0005C9E8
		protected override bool Filter(TurnContext context, PraetorDuelOutcomeEvent @event, PlayerState owner, PlayerState target)
		{
			return @event.IsAssociatedWith(owner.Id) && (target == null || @event.IsAssociatedWith(target.Id));
		}
	}
}
