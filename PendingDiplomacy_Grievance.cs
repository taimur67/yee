using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000517 RID: 1303
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PendingDiplomacy_Grievance : PendingDiplomacyState
	{
		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06001940 RID: 6464 RVA: 0x000592F1 File Offset: 0x000574F1
		public override DiplomaticPendingValue DiplomaticPendingValue
		{
			get
			{
				return DiplomaticPendingValue.Vendetta;
			}
		}

		// Token: 0x06001941 RID: 6465 RVA: 0x000592F8 File Offset: 0x000574F8
		[JsonConstructor]
		public PendingDiplomacy_Grievance()
		{
		}

		// Token: 0x06001942 RID: 6466 RVA: 0x00059307 File Offset: 0x00057507
		public PendingDiplomacy_Grievance(int playerId, OrderTypes cause = OrderTypes.Demand) : base(playerId)
		{
			this.Cause = cause;
		}

		// Token: 0x06001943 RID: 6467 RVA: 0x00059320 File Offset: 0x00057520
		protected override void Update(TurnProcessContext context, PlayerState actor, PlayerState target)
		{
			GrievanceDecisionRequest decisionRequest = new GrievanceDecisionRequest(context.CurrentTurn)
			{
				TriggeringOrderType = this.Cause,
				RequestingPlayerId = actor.Id,
				AffectedPlayerId = target.Id,
				AdditionalPrestigeReward = this.AdditionalPrestigeReward
			};
			context.CurrentTurn.AddDecisionToAskPlayer(target.Id, decisionRequest);
		}

		// Token: 0x06001944 RID: 6468 RVA: 0x00059384 File Offset: 0x00057584
		public override void DeepClone(out DiplomaticState clone)
		{
			PendingDiplomacy_Grievance pendingDiplomacy_Grievance = new PendingDiplomacy_Grievance
			{
				Cause = this.Cause,
				AdditionalPrestigeReward = this.AdditionalPrestigeReward.DeepClone<StatModifier>()
			};
			base.DeepClonePendingDiplomacyParts(pendingDiplomacy_Grievance);
			clone = pendingDiplomacy_Grievance;
		}

		// Token: 0x04000B9D RID: 2973
		[JsonProperty]
		[DefaultValue(OrderTypes.Demand)]
		public OrderTypes Cause = OrderTypes.Demand;

		// Token: 0x04000B9E RID: 2974
		[JsonProperty]
		[DefaultValue(null)]
		public StatModifier AdditionalPrestigeReward;
	}
}
