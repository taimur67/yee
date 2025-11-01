using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004E0 RID: 1248
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class RequestToBeBloodLordDecisionRequest : DiplomaticDecisionRequest<RequestToBeBloodLordDecisionResponse>
	{
		// Token: 0x06001782 RID: 6018 RVA: 0x00055624 File Offset: 0x00053824
		[JsonConstructor]
		protected RequestToBeBloodLordDecisionRequest()
		{
		}

		// Token: 0x06001783 RID: 6019 RVA: 0x0005562C File Offset: 0x0005382C
		public RequestToBeBloodLordDecisionRequest(DecisionId decisionId) : base(decisionId)
		{
		}

		// Token: 0x06001784 RID: 6020 RVA: 0x00055635 File Offset: 0x00053835
		public override TurnLogEntryType GetTurnLogEntryType()
		{
			return TurnLogEntryType.OfferLordshipSentRecipient;
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06001785 RID: 6021 RVA: 0x00055639 File Offset: 0x00053839
		[BindableValue(null, BindingOption.None)]
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.RequestToBeBloodLordOfTarget;
			}
		}
	}
}
