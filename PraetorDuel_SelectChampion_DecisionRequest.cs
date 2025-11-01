using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003BE RID: 958
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PraetorDuel_SelectChampion_DecisionRequest : DiplomaticDecisionRequest<PraetorDuel_SelectionChampion_DecisionResponse>, IDiplomaticDecisionRequest
	{
		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x060012C7 RID: 4807 RVA: 0x00047E9E File Offset: 0x0004609E
		// (set) Token: 0x060012C8 RID: 4808 RVA: 0x00047EB1 File Offset: 0x000460B1
		[JsonIgnore]
		public PlayerPair Contestants
		{
			get
			{
				return new PlayerPair(base.RequestingPlayerId, base.AffectedPlayerId);
			}
			set
			{
				base.RequestingPlayerId = value.First;
				base.AffectedPlayerId = value.Second;
			}
		}

		// Token: 0x060012C9 RID: 4809 RVA: 0x00047ECB File Offset: 0x000460CB
		[JsonConstructor]
		protected PraetorDuel_SelectChampion_DecisionRequest()
		{
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x00047ED3 File Offset: 0x000460D3
		public PraetorDuel_SelectChampion_DecisionRequest(DecisionId decisionId, PlayerPair contestants) : base(decisionId)
		{
			this.Contestants = contestants;
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x00047EE3 File Offset: 0x000460E3
		public override TurnLogEntryType GetTurnLogEntryType()
		{
			return TurnLogEntryType.PraetorSelectChampion;
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x060012CC RID: 4812 RVA: 0x00047EEA File Offset: 0x000460EA
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.Vendetta;
			}
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x00047EED File Offset: 0x000460ED
		public override DecisionResponse GenerateResponse()
		{
			return (PraetorDuel_SelectionChampion_DecisionResponse)base.GenerateResponse();
		}
	}
}
