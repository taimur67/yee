using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004CB RID: 1227
	[BindableGameEvent]
	[Serializable]
	public class UpkeepDecisionRequest : DecisionRequest<UpkeepDecisionResponse>
	{
		// Token: 0x06001702 RID: 5890 RVA: 0x00054373 File Offset: 0x00052573
		[JsonConstructor]
		public UpkeepDecisionRequest(DecisionId decisionId) : this(decisionId, Identifier.Invalid, new Cost())
		{
		}

		// Token: 0x06001703 RID: 5891 RVA: 0x00054382 File Offset: 0x00052582
		public UpkeepDecisionRequest(DecisionId decisionId, Identifier gameItemId, Cost requiredPayment) : base(decisionId)
		{
			this.RequiredPayment = requiredPayment;
			this.GameItemId = gameItemId;
		}

		// Token: 0x06001704 RID: 5892 RVA: 0x000543AB File Offset: 0x000525AB
		public override TurnLogEntryType GetTurnLogEntryType()
		{
			return TurnLogEntryType.PayUpkeep;
		}

		// Token: 0x04000B44 RID: 2884
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		[DefaultValue(Identifier.Invalid)]
		public Identifier GameItemId = Identifier.Invalid;

		// Token: 0x04000B45 RID: 2885
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Cost RequiredPayment = new Cost();
	}
}
