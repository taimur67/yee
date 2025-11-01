using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004A7 RID: 1191
	[BindableGameEvent]
	[Serializable]
	public class MaintainRitualDecisionRequest : DecisionRequest<MaintainRitualDecisionResponse>
	{
		// Token: 0x06001657 RID: 5719 RVA: 0x00052A2E File Offset: 0x00050C2E
		[JsonConstructor]
		public MaintainRitualDecisionRequest(DecisionId decisionId) : this(decisionId, Identifier.Invalid, new Cost())
		{
		}

		// Token: 0x06001658 RID: 5720 RVA: 0x00052A3D File Offset: 0x00050C3D
		public MaintainRitualDecisionRequest(DecisionId decisionId, Identifier activeRitualId, Cost requiredPayment) : base(decisionId)
		{
			this.RequiredPayment = requiredPayment;
			this.ActiveRitualId = activeRitualId;
		}

		// Token: 0x06001659 RID: 5721 RVA: 0x00052A54 File Offset: 0x00050C54
		public override TurnLogEntryType GetTurnLogEntryType()
		{
			if (!this.RequiredPayment.IsZero)
			{
				return TurnLogEntryType.PayUpkeepRitual;
			}
			return TurnLogEntryType.None;
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x00052A6C File Offset: 0x00050C6C
		public override DecisionResponse GenerateResponse()
		{
			MaintainRitualDecisionResponse maintainRitualDecisionResponse = (MaintainRitualDecisionResponse)base.GenerateResponse();
			if (this.RequiredPayment.IsZero)
			{
				maintainRitualDecisionResponse.Choice = YesNo.Yes;
			}
			return maintainRitualDecisionResponse;
		}

		// Token: 0x0600165B RID: 5723 RVA: 0x00052A9A File Offset: 0x00050C9A
		public override bool DecisionRequired(TurnContext context)
		{
			return !this.RequiredPayment.IsZero;
		}

		// Token: 0x04000B19 RID: 2841
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		[DefaultValue(Identifier.Invalid)]
		public Identifier ActiveRitualId;

		// Token: 0x04000B1A RID: 2842
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Cost RequiredPayment;
	}
}
