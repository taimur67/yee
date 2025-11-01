using System;

namespace LoG
{
	// Token: 0x020004B5 RID: 1205
	[Serializable]
	public class TributeDecisionResponse : DecisionResponse
	{
		// Token: 0x06001692 RID: 5778 RVA: 0x00052FD0 File Offset: 0x000511D0
		public override void DeepClone(out DecisionResponse clone)
		{
			TributeDecisionResponse tributeDecisionResponse = new TributeDecisionResponse();
			base.DeepCloneDecisionResponseParts(tributeDecisionResponse);
			clone = tributeDecisionResponse;
		}
	}
}
