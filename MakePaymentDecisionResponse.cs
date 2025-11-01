using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004B0 RID: 1200
	[Serializable]
	public class MakePaymentDecisionResponse : DecisionResponse
	{
		// Token: 0x0600167E RID: 5758 RVA: 0x00052DE8 File Offset: 0x00050FE8
		public override string GetDebugString()
		{
			return string.Concat(new string[]
			{
				base.GetDebugString(),
				": ",
				this.Cost.ToString(),
				" | ",
				this.Payment.ToString()
			});
		}

		// Token: 0x0600167F RID: 5759 RVA: 0x00052E38 File Offset: 0x00051038
		public override void DeepClone(out DecisionResponse clone)
		{
			MakePaymentDecisionResponse makePaymentDecisionResponse = new MakePaymentDecisionResponse
			{
				Cost = this.Cost.DeepClone<Cost>(),
				Payment = this.Payment.DeepClone<Payment>()
			};
			base.DeepCloneDecisionResponseParts(makePaymentDecisionResponse);
			clone = makePaymentDecisionResponse;
		}

		// Token: 0x04000B27 RID: 2855
		[JsonProperty]
		public Cost Cost = new Cost();

		// Token: 0x04000B28 RID: 2856
		[JsonProperty]
		public Payment Payment;
	}
}
