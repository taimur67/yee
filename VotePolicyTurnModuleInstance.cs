using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200074C RID: 1868
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class VotePolicyTurnModuleInstance : VoteTurnModuleInstance
	{
		// Token: 0x0600230A RID: 8970 RVA: 0x00079A12 File Offset: 0x00077C12
		public void Vote(int playerId, string policyId)
		{
			if (!this.Votes.ContainsKey(playerId))
			{
				this.Votes.Add(playerId, policyId);
			}
			this.Votes[playerId] = policyId;
		}

		// Token: 0x0600230B RID: 8971 RVA: 0x00079A3C File Offset: 0x00077C3C
		public override void DeepClone(out TurnModuleInstance clone)
		{
			VotePolicyTurnModuleInstance votePolicyTurnModuleInstance = new VotePolicyTurnModuleInstance
			{
				Votes = this.Votes.DeepClone()
			};
			base.DeepCloneVoteTurnModuleInstanceParts(votePolicyTurnModuleInstance);
			clone = votePolicyTurnModuleInstance;
		}

		// Token: 0x04000F49 RID: 3913
		[JsonProperty]
		public Dictionary<int, string> Votes = new Dictionary<int, string>();
	}
}
