using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004AF RID: 1199
	[Serializable]
	public class YesNoDecisionResponse : DecisionResponse
	{
		// Token: 0x0600167A RID: 5754 RVA: 0x00052D86 File Offset: 0x00050F86
		public virtual void SubmitDecision(YesNo response)
		{
			this.Response = response;
		}

		// Token: 0x0600167B RID: 5755 RVA: 0x00052D8F File Offset: 0x00050F8F
		public override string GetDebugString()
		{
			return base.GetDebugString() + ": " + this.Response.ToString();
		}

		// Token: 0x0600167C RID: 5756 RVA: 0x00052DB4 File Offset: 0x00050FB4
		public override void DeepClone(out DecisionResponse clone)
		{
			YesNoDecisionResponse yesNoDecisionResponse = new YesNoDecisionResponse
			{
				Response = this.Response
			};
			base.DeepCloneDecisionResponseParts(yesNoDecisionResponse);
			clone = yesNoDecisionResponse;
		}

		// Token: 0x04000B26 RID: 2854
		[JsonProperty]
		public YesNo Response;
	}
}
