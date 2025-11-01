using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004D7 RID: 1239
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class InsultDecisionRequest : DiplomaticDecisionRequest<InsultDecisionResponse>, IGrievanceRequest
	{
		// Token: 0x0600174A RID: 5962 RVA: 0x00054DB7 File Offset: 0x00052FB7
		[JsonConstructor]
		protected InsultDecisionRequest()
		{
		}

		// Token: 0x0600174B RID: 5963 RVA: 0x00054DBF File Offset: 0x00052FBF
		public InsultDecisionRequest(DecisionId decisionId) : base(decisionId)
		{
		}

		// Token: 0x0600174C RID: 5964 RVA: 0x00054DC8 File Offset: 0x00052FC8
		public override TurnLogEntryType GetTurnLogEntryType()
		{
			return TurnLogEntryType.InsultSentRecipient;
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x0600174D RID: 5965 RVA: 0x00054DCC File Offset: 0x00052FCC
		[BindableValue(null, BindingOption.None)]
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.Insult;
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x0600174E RID: 5966 RVA: 0x00054DCF File Offset: 0x00052FCF
		// (set) Token: 0x0600174F RID: 5967 RVA: 0x00054DD7 File Offset: 0x00052FD7
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

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06001750 RID: 5968 RVA: 0x00054DE0 File Offset: 0x00052FE0
		// (set) Token: 0x06001751 RID: 5969 RVA: 0x00054DE8 File Offset: 0x00052FE8
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
	}
}
