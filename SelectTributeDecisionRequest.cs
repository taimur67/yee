using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004C8 RID: 1224
	[Serializable]
	public class SelectTributeDecisionRequest : DecisionRequest<SelectTributeDecisionResponse>
	{
		// Token: 0x060016EB RID: 5867 RVA: 0x00053E7E File Offset: 0x0005207E
		[JsonConstructor]
		public SelectTributeDecisionRequest(DecisionId decisionId) : base(decisionId)
		{
		}

		// Token: 0x060016EC RID: 5868 RVA: 0x00053E92 File Offset: 0x00052092
		public SelectTributeDecisionRequest(DecisionId decisionId, DemandPayload payload, int selectionMax, bool isOffering = false) : base(decisionId)
		{
			this.Candidates = payload;
			this.SelectionMax = selectionMax;
		}

		// Token: 0x060016ED RID: 5869 RVA: 0x00053EB4 File Offset: 0x000520B4
		public override TurnLogEntryType GetTurnLogEntryType()
		{
			if (!IEnumerableExtensions.Any<Manuscript>(this.Candidates.Manuscripts))
			{
				return TurnLogEntryType.DemandTribute;
			}
			return TurnLogEntryType.SeekManuscripts;
		}

		// Token: 0x04000B41 RID: 2881
		[JsonProperty]
		public DemandPayload Candidates = new DemandPayload();

		// Token: 0x04000B42 RID: 2882
		[JsonProperty]
		public int SelectionMax;
	}
}
