using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004EB RID: 1259
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class GrievanceDecisionRequest : DiplomaticDecisionRequest<GrievanceDecisionResponse>, IGrievanceRequest
	{
		// Token: 0x1700034C RID: 844
		// (get) Token: 0x060017C1 RID: 6081 RVA: 0x00055F39 File Offset: 0x00054139
		[JsonIgnore]
		public override OrderTypes OrderType
		{
			get
			{
				return this.TriggeringOrderType;
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x060017C2 RID: 6082 RVA: 0x00055F41 File Offset: 0x00054141
		[JsonIgnore]
		public bool MustAccept
		{
			get
			{
				return this.OrderType == OrderTypes.Insult;
			}
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x060017C3 RID: 6083 RVA: 0x00055F4C File Offset: 0x0005414C
		// (set) Token: 0x060017C4 RID: 6084 RVA: 0x00055F54 File Offset: 0x00054154
		[JsonIgnore]
		public int InstigatorPlayerId
		{
			get
			{
				return base.AffectedPlayerId;
			}
			set
			{
				base.AffectedPlayerId = value;
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x060017C5 RID: 6085 RVA: 0x00055F5D File Offset: 0x0005415D
		// (set) Token: 0x060017C6 RID: 6086 RVA: 0x00055F65 File Offset: 0x00054165
		[JsonIgnore]
		public int GrievanceTargetPlayerId
		{
			get
			{
				return base.RequestingPlayerId;
			}
			set
			{
				base.RequestingPlayerId = value;
			}
		}

		// Token: 0x060017C7 RID: 6087 RVA: 0x00055F6E File Offset: 0x0005416E
		[JsonConstructor]
		public GrievanceDecisionRequest()
		{
		}

		// Token: 0x060017C8 RID: 6088 RVA: 0x00055F76 File Offset: 0x00054176
		public GrievanceDecisionRequest(DecisionId decisionId) : base(decisionId)
		{
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x00055F80 File Offset: 0x00054180
		public override TurnLogEntryType GetTurnLogEntryType()
		{
			switch (this.TriggeringOrderType)
			{
			case OrderTypes.Insult:
				return TurnLogEntryType.InsultRejectedRecipient;
			case OrderTypes.Demand:
				return TurnLogEntryType.DemandRejectedInitiator;
			case OrderTypes.SendEmissary:
				return TurnLogEntryType.EmissaryExecutedInitiator;
			case OrderTypes.Extort:
				return TurnLogEntryType.ExtortionRejectedInitiator;
			}
			return TurnLogEntryType.VendettaSetInitiator;
		}

		// Token: 0x04000B5F RID: 2911
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public OrderTypes TriggeringOrderType;

		// Token: 0x04000B60 RID: 2912
		[JsonProperty]
		public StatModifier AdditionalPrestigeReward;
	}
}
