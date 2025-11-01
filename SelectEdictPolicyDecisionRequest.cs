using System;
using System.Collections.Generic;
using Core.StaticData;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004B9 RID: 1209
	[BindableGameEvent]
	[Serializable]
	public class SelectEdictPolicyDecisionRequest : DecisionRequest<SelectEdictPolicyDecisionResponse>
	{
		// Token: 0x060016A0 RID: 5792 RVA: 0x000531AB File Offset: 0x000513AB
		[JsonConstructor]
		public SelectEdictPolicyDecisionRequest(DecisionId decisionId) : base(decisionId)
		{
		}

		// Token: 0x060016A1 RID: 5793 RVA: 0x000531B4 File Offset: 0x000513B4
		public override TurnLogEntryType GetTurnLogEntryType()
		{
			if (!this.Unannounced)
			{
				return TurnLogEntryType.VotingStarted;
			}
			return TurnLogEntryType.EmergencyVotingStarted;
		}

		// Token: 0x04000B2F RID: 2863
		[JsonProperty]
		public TurnModuleInstanceId TurnModuleInstanceId;

		// Token: 0x04000B30 RID: 2864
		[BindableValue("edict", BindingOption.StaticDataId)]
		[JsonProperty]
		public string EdictId;

		// Token: 0x04000B31 RID: 2865
		[JsonProperty]
		public List<ConfigRef<EdictEffectStaticData>> Candidates;

		// Token: 0x04000B32 RID: 2866
		[JsonProperty]
		public bool Unannounced;
	}
}
