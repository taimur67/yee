using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004DD RID: 1245
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class MakeDemandDecisionRequest : DiplomaticDecisionRequest<MakeDemandDecisionResponse>, IDemandRequestAccessor
	{
		// Token: 0x1700033E RID: 830
		// (get) Token: 0x0600176C RID: 5996 RVA: 0x000552A2 File Offset: 0x000534A2
		// (set) Token: 0x0600176D RID: 5997 RVA: 0x000552AA File Offset: 0x000534AA
		[JsonProperty]
		public DemandOptions DemandOption { get; set; }

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x0600176E RID: 5998 RVA: 0x000552B3 File Offset: 0x000534B3
		// (set) Token: 0x0600176F RID: 5999 RVA: 0x000552C0 File Offset: 0x000534C0
		[JsonIgnore]
		public int NumCards
		{
			get
			{
				return this.Cost.RequiredTokenCount;
			}
			set
			{
				this.Cost.RequiredTokenCount = value;
			}
		}

		// Token: 0x06001770 RID: 6000 RVA: 0x000552CE File Offset: 0x000534CE
		[JsonConstructor]
		protected MakeDemandDecisionRequest()
		{
		}

		// Token: 0x06001771 RID: 6001 RVA: 0x000552E2 File Offset: 0x000534E2
		public MakeDemandDecisionRequest(DecisionId decisionId) : base(decisionId)
		{
		}

		// Token: 0x06001772 RID: 6002 RVA: 0x000552F7 File Offset: 0x000534F7
		public override TurnLogEntryType GetTurnLogEntryType()
		{
			return TurnLogEntryType.DemandSentRecipient;
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06001773 RID: 6003 RVA: 0x000552FB File Offset: 0x000534FB
		[BindableValue(null, BindingOption.None)]
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.Demand;
			}
		}

		// Token: 0x04000B54 RID: 2900
		[JsonProperty]
		public Cost Cost = new Cost(2);
	}
}
