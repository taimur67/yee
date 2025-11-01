using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004FC RID: 1276
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class UnholyCrusadeEventDecisionRequest : GrandEventDecisionRequest<UnholyCrusadeEventDecisionResponse>
	{
		// Token: 0x06001830 RID: 6192 RVA: 0x00056D45 File Offset: 0x00054F45
		public UnholyCrusadeEventDecisionRequest(DecisionId decisionId, string eventEffectId) : base(decisionId, eventEffectId)
		{
		}

		// Token: 0x04000B7A RID: 2938
		[JsonProperty]
		public TurnModuleInstanceId TurnModuleInstanceId;
	}
}
