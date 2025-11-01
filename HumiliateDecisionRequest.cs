using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004D4 RID: 1236
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class HumiliateDecisionRequest : DiplomaticDecisionRequest<HumiliateDecisionResponse>, IGrievanceAccessor, ISelectionAccessor, IGrievanceRequest
	{
		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06001731 RID: 5937 RVA: 0x00054A70 File Offset: 0x00052C70
		public bool PrivateGrievance
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001732 RID: 5938 RVA: 0x00054A73 File Offset: 0x00052C73
		[JsonConstructor]
		public HumiliateDecisionRequest(DecisionId decisionId) : base(decisionId)
		{
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06001733 RID: 5939 RVA: 0x00054A7C File Offset: 0x00052C7C
		// (set) Token: 0x06001734 RID: 5940 RVA: 0x00054A84 File Offset: 0x00052C84
		[JsonProperty]
		public GrievanceContext GrievanceResponse { get; set; }

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06001735 RID: 5941 RVA: 0x00054A8D File Offset: 0x00052C8D
		// (set) Token: 0x06001736 RID: 5942 RVA: 0x00054A95 File Offset: 0x00052C95
		[JsonProperty]
		public bool MustDeclareVendetta { get; set; }

		// Token: 0x06001737 RID: 5943 RVA: 0x00054A9E File Offset: 0x00052C9E
		public override TurnLogEntryType GetTurnLogEntryType()
		{
			return TurnLogEntryType.HumiliationSentRecipient;
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06001738 RID: 5944 RVA: 0x00054AA2 File Offset: 0x00052CA2
		[BindableValue(null, BindingOption.None)]
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.Humiliate;
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06001739 RID: 5945 RVA: 0x00054AA5 File Offset: 0x00052CA5
		// (set) Token: 0x0600173A RID: 5946 RVA: 0x00054AAD File Offset: 0x00052CAD
		[JsonIgnore]
		public int InstigatorPlayerId
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

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x0600173B RID: 5947 RVA: 0x00054AB6 File Offset: 0x00052CB6
		// (set) Token: 0x0600173C RID: 5948 RVA: 0x00054ABE File Offset: 0x00052CBE
		[JsonIgnore]
		public int GrievanceTargetPlayerId
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
	}
}
