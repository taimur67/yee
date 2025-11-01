using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200074A RID: 1866
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class VoteCandidateTurnModuleInstance : VoteTurnModuleInstance
	{
		// Token: 0x06002304 RID: 8964 RVA: 0x00079788 File Offset: 0x00077988
		public void Vote(int votingPlayerId, int targetPlayerId)
		{
			this.Votes.TryAdd(votingPlayerId, targetPlayerId);
			this.Votes[votingPlayerId] = targetPlayerId;
		}

		// Token: 0x06002305 RID: 8965 RVA: 0x000797A8 File Offset: 0x000779A8
		public override void DeepClone(out TurnModuleInstance clone)
		{
			VoteCandidateTurnModuleInstance voteCandidateTurnModuleInstance = new VoteCandidateTurnModuleInstance
			{
				Votes = this.Votes.DeepClone()
			};
			base.DeepCloneVoteTurnModuleInstanceParts(voteCandidateTurnModuleInstance);
			clone = voteCandidateTurnModuleInstance;
		}

		// Token: 0x04000F48 RID: 3912
		[JsonProperty]
		public Dictionary<int, int> Votes = new Dictionary<int, int>();
	}
}
