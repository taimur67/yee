using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004C2 RID: 1218
	[Serializable]
	public class SelectPowerDecisionRequest : DecisionRequest<SelectPowerDecisionResponse>
	{
		// Token: 0x060016C8 RID: 5832 RVA: 0x00053965 File Offset: 0x00051B65
		public SelectPowerDecisionRequest(DecisionId decisionId) : base(decisionId)
		{
		}

		// Token: 0x060016C9 RID: 5833 RVA: 0x0005396E File Offset: 0x00051B6E
		[JsonConstructor]
		protected SelectPowerDecisionRequest()
		{
		}

		// Token: 0x060016CA RID: 5834 RVA: 0x00053976 File Offset: 0x00051B76
		public override TurnLogEntryType GetTurnLogEntryType()
		{
			return TurnLogEntryType.SelectPower;
		}

		// Token: 0x04000B39 RID: 2873
		[JsonProperty]
		public PowerType PowerType;

		// Token: 0x04000B3A RID: 2874
		[JsonProperty]
		public int Level;
	}
}
