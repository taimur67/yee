using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004E3 RID: 1251
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class RequestToBeVassalizedDecisionRequest : DiplomaticDecisionRequest<RequestToBeVassalizedDecisionResponse>
	{
		// Token: 0x0600178F RID: 6031 RVA: 0x00055874 File Offset: 0x00053A74
		[JsonConstructor]
		protected RequestToBeVassalizedDecisionRequest()
		{
		}

		// Token: 0x06001790 RID: 6032 RVA: 0x0005587C File Offset: 0x00053A7C
		public RequestToBeVassalizedDecisionRequest(DecisionId decisionId) : base(decisionId)
		{
		}

		// Token: 0x06001791 RID: 6033 RVA: 0x00055885 File Offset: 0x00053A85
		public override TurnLogEntryType GetTurnLogEntryType()
		{
			return TurnLogEntryType.OfferVassalageSentRecipient;
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06001792 RID: 6034 RVA: 0x00055889 File Offset: 0x00053A89
		[BindableValue(null, BindingOption.None)]
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.RequestToBeVassalizedByTarget;
			}
		}
	}
}
