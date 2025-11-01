using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004DB RID: 1243
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EnterLureOfExcessDecisionRequest : DiplomaticDecisionRequest<EnterLureOfExcessDecisionResponse>
	{
		// Token: 0x06001766 RID: 5990 RVA: 0x00055128 File Offset: 0x00053328
		public override TurnLogEntryType GetTurnLogEntryType()
		{
			return TurnLogEntryType.LureOfExcessSentRecipient;
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06001767 RID: 5991 RVA: 0x0005512F File Offset: 0x0005332F
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.LureOfExcess;
			}
		}

		// Token: 0x06001768 RID: 5992 RVA: 0x00055133 File Offset: 0x00053333
		[JsonConstructor]
		protected EnterLureOfExcessDecisionRequest()
		{
		}

		// Token: 0x06001769 RID: 5993 RVA: 0x0005513B File Offset: 0x0005333B
		public EnterLureOfExcessDecisionRequest(DecisionId decisionId) : base(decisionId)
		{
		}

		// Token: 0x04000B51 RID: 2897
		[JsonProperty]
		public int Duration;

		// Token: 0x04000B52 RID: 2898
		[JsonProperty]
		public int RejectionPrestigeCost;
	}
}
