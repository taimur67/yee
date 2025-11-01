using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000501 RID: 1281
	[Serializable]
	public class GrandEventDecisionResponse : DecisionResponse
	{
		// Token: 0x06001843 RID: 6211 RVA: 0x00056F2F File Offset: 0x0005512F
		protected void DeepCloneGrandEventDecisionResponseParts(GrandEventDecisionResponse grandEventDecisionResponse)
		{
			grandEventDecisionResponse.Choice = this.Choice;
			base.DeepCloneDecisionResponseParts(grandEventDecisionResponse);
		}

		// Token: 0x06001844 RID: 6212 RVA: 0x00056F44 File Offset: 0x00055144
		public override void DeepClone(out DecisionResponse clone)
		{
			GrandEventDecisionResponse grandEventDecisionResponse = new GrandEventDecisionResponse();
			this.DeepCloneGrandEventDecisionResponseParts(grandEventDecisionResponse);
			clone = grandEventDecisionResponse;
		}

		// Token: 0x04000B7D RID: 2941
		[JsonProperty]
		public YesNo Choice;
	}
}
