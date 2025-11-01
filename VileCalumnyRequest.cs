using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004F1 RID: 1265
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class VileCalumnyRequest : DiplomaticDecisionRequest<VileCalumnyResponse>, IGrievanceRequest
	{
		// Token: 0x17000352 RID: 850
		// (get) Token: 0x060017EF RID: 6127 RVA: 0x00056654 File Offset: 0x00054854
		// (set) Token: 0x060017F0 RID: 6128 RVA: 0x0005665C File Offset: 0x0005485C
		[JsonProperty]
		[BindableValue("scapegoat_name", BindingOption.IntPlayerId)]
		public int ScapegoatId { get; set; }

		// Token: 0x060017F1 RID: 6129 RVA: 0x00056665 File Offset: 0x00054865
		[JsonConstructor]
		protected VileCalumnyRequest()
		{
		}

		// Token: 0x060017F2 RID: 6130 RVA: 0x0005666D File Offset: 0x0005486D
		public VileCalumnyRequest(DecisionId decisionId) : base(decisionId)
		{
		}

		// Token: 0x060017F3 RID: 6131 RVA: 0x00056676 File Offset: 0x00054876
		public override TurnLogEntryType GetTurnLogEntryType()
		{
			return TurnLogEntryType.VileCalumnySentRecipient;
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x060017F4 RID: 6132 RVA: 0x0005667D File Offset: 0x0005487D
		[BindableValue(null, BindingOption.None)]
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.VileCalumny;
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x060017F5 RID: 6133 RVA: 0x00056681 File Offset: 0x00054881
		// (set) Token: 0x060017F6 RID: 6134 RVA: 0x00056689 File Offset: 0x00054889
		[JsonIgnore]
		public int InstigatorPlayerId
		{
			get
			{
				return this.ScapegoatId;
			}
			set
			{
				this.ScapegoatId = value;
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x060017F7 RID: 6135 RVA: 0x00056692 File Offset: 0x00054892
		// (set) Token: 0x060017F8 RID: 6136 RVA: 0x0005669A File Offset: 0x0005489A
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

		// Token: 0x060017F9 RID: 6137 RVA: 0x000566A4 File Offset: 0x000548A4
		public override bool RelatesToPlayers(PlayerPair pair)
		{
			PlayerPair playerPair = new PlayerPair(base.AffectedPlayerId, this.ScapegoatId);
			return playerPair.Equals(pair);
		}
	}
}
