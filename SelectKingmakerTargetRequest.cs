using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004BF RID: 1215
	[Serializable]
	public class SelectKingmakerTargetRequest : DecisionRequest<SelectKingmakerTargetResponse>
	{
		// Token: 0x060016B9 RID: 5817 RVA: 0x0005379E File Offset: 0x0005199E
		[JsonConstructor]
		public SelectKingmakerTargetRequest(DecisionId decisionId) : base(decisionId)
		{
		}

		// Token: 0x060016BA RID: 5818 RVA: 0x000537A7 File Offset: 0x000519A7
		public override TurnLogEntryType GetTurnLogEntryType()
		{
			return TurnLogEntryType.SelectKingmakerTarget;
		}

		// Token: 0x060016BB RID: 5819 RVA: 0x000537AE File Offset: 0x000519AE
		public override bool DecisionRequired(TurnContext context)
		{
			return this.MustDecide;
		}

		// Token: 0x04000B37 RID: 2871
		[JsonProperty]
		public bool MustDecide;
	}
}
