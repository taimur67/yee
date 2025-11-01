using System;

namespace LoG
{
	// Token: 0x020005B0 RID: 1456
	public class ObjectiveCondition_VoteOnEdict : ObjectiveCondition_EventFilter<VoteRevealedEvent>
	{
		// Token: 0x06001B4A RID: 6986 RVA: 0x0005ECE2 File Offset: 0x0005CEE2
		protected override bool Filter(TurnContext context, VoteRevealedEvent @event, PlayerState owner, PlayerState target)
		{
			return @event.DidPlayerVote(owner.Id);
		}
	}
}
