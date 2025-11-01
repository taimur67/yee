using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000212 RID: 530
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public abstract class VoteRevealedEvent : VoteEvent
	{
		// Token: 0x06000A53 RID: 2643 RVA: 0x0002E0FA File Offset: 0x0002C2FA
		protected VoteRevealedEvent()
		{
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x0002E102 File Offset: 0x0002C302
		public VoteRevealedEvent(string edictId) : base(edictId)
		{
		}

		// Token: 0x06000A55 RID: 2645
		public abstract bool DidPlayerVote(int playerId);

		// Token: 0x06000A56 RID: 2646
		public abstract IEnumerable<int> GetPlayersWhoVotedForWinningOption();
	}
}
