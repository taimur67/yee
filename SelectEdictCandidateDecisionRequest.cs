using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004B6 RID: 1206
	[BindableGameEvent]
	[Serializable]
	public class SelectEdictCandidateDecisionRequest : DecisionRequest<SelectEdictCandidateDecisionResponse>
	{
		// Token: 0x06001694 RID: 5780 RVA: 0x00052FF5 File Offset: 0x000511F5
		[JsonConstructor]
		public SelectEdictCandidateDecisionRequest(DecisionId decisionId) : base(decisionId)
		{
		}

		// Token: 0x06001695 RID: 5781 RVA: 0x00053009 File Offset: 0x00051209
		public override TurnLogEntryType GetTurnLogEntryType()
		{
			if (!this.Unannounced)
			{
				return TurnLogEntryType.VotingStarted;
			}
			return TurnLogEntryType.EmergencyVotingStarted;
		}

		// Token: 0x04000B2A RID: 2858
		[JsonProperty]
		public TurnModuleInstanceId TurnModuleInstanceId;

		// Token: 0x04000B2B RID: 2859
		[BindableValue("edict", BindingOption.StaticDataId)]
		[JsonProperty]
		public string EdictId;

		// Token: 0x04000B2C RID: 2860
		[JsonProperty]
		public List<int> Candidates = new List<int>();

		// Token: 0x04000B2D RID: 2861
		[JsonProperty]
		public bool Unannounced;
	}
}
