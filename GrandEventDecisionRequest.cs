using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000500 RID: 1280
	[BindableGameEvent]
	[Serializable]
	public abstract class GrandEventDecisionRequest<T> : DecisionRequest<T>, IGrandEventDecisionRequest where T : GrandEventDecisionResponse, new()
	{
		// Token: 0x17000363 RID: 867
		// (get) Token: 0x0600183E RID: 6206 RVA: 0x00056EED File Offset: 0x000550ED
		// (set) Token: 0x0600183F RID: 6207 RVA: 0x00056EF5 File Offset: 0x000550F5
		[BindableValue(null, BindingOption.StaticDataId)]
		[JsonProperty]
		public string EventEffectId { get; set; }

		// Token: 0x06001840 RID: 6208 RVA: 0x00056EFE File Offset: 0x000550FE
		public override DecisionResponse GenerateResponse()
		{
			T t = Activator.CreateInstance<T>();
			t.DecisionId = this.DecisionId;
			return t;
		}

		// Token: 0x06001841 RID: 6209 RVA: 0x00056F1B File Offset: 0x0005511B
		protected GrandEventDecisionRequest(DecisionId decisionId, string eventEffectId) : base(decisionId)
		{
			this.EventEffectId = eventEffectId;
		}

		// Token: 0x06001842 RID: 6210 RVA: 0x00056F2B File Offset: 0x0005512B
		public override TurnLogEntryType GetTurnLogEntryType()
		{
			return TurnLogEntryType.EventCardPlayed;
		}
	}
}
