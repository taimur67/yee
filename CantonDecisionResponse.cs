using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x020004B4 RID: 1204
	[Serializable]
	public class CantonDecisionResponse : DecisionResponse
	{
		// Token: 0x0600168F RID: 5775 RVA: 0x00052FA5 File Offset: 0x000511A5
		public virtual void SubmitDecision(List<int> selectedCantons)
		{
		}

		// Token: 0x06001690 RID: 5776 RVA: 0x00052FA8 File Offset: 0x000511A8
		public override void DeepClone(out DecisionResponse clone)
		{
			CantonDecisionResponse cantonDecisionResponse = new CantonDecisionResponse();
			base.DeepCloneDecisionResponseParts(cantonDecisionResponse);
			clone = cantonDecisionResponse;
		}
	}
}
