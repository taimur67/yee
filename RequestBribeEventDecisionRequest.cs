using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004F8 RID: 1272
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class RequestBribeEventDecisionRequest : GrandEventDecisionRequest<RequestBribeEventDecisionResponse>
	{
		// Token: 0x06001824 RID: 6180 RVA: 0x00056BA4 File Offset: 0x00054DA4
		public RequestBribeEventDecisionRequest(DecisionId decisionId, string effectId) : base(decisionId, effectId)
		{
		}

		// Token: 0x04000B76 RID: 2934
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Cost Cost;

		// Token: 0x04000B77 RID: 2935
		[BindableValue("archfiend_name", BindingOption.IntPlayerId)]
		[JsonProperty]
		public int PlayerId;

		// Token: 0x04000B78 RID: 2936
		[JsonProperty]
		public float PrestigeLossPercent;
	}
}
