using System;

namespace LoG
{
	// Token: 0x020005B1 RID: 1457
	[Serializable]
	public class ObjectiveCondition_VoteOnWinningEdict : ObjectiveCondition_EventFilter<VoteRevealedEvent>
	{
		// Token: 0x06001B4C RID: 6988 RVA: 0x0005ECFD File Offset: 0x0005CEFD
		protected override bool Filter(TurnContext context, VoteRevealedEvent @event, PlayerState owner, PlayerState target)
		{
			return IEnumerableExtensions.Contains<int>(@event.GetPlayersWhoVotedForWinningOption(), owner.Id);
		}
	}
}
