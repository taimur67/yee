using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200051A RID: 1306
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PendingDiplomacy_Humiliate : PendingDiplomacyState
	{
		// Token: 0x17000399 RID: 921
		// (get) Token: 0x0600194F RID: 6479 RVA: 0x0005955D File Offset: 0x0005775D
		public override DiplomaticPendingValue DiplomaticPendingValue
		{
			get
			{
				return DiplomaticPendingValue.Humiliate;
			}
		}

		// Token: 0x06001950 RID: 6480 RVA: 0x00059560 File Offset: 0x00057760
		[JsonConstructor]
		public PendingDiplomacy_Humiliate()
		{
		}

		// Token: 0x06001951 RID: 6481 RVA: 0x00059568 File Offset: 0x00057768
		public PendingDiplomacy_Humiliate(int playerId) : base(playerId)
		{
		}

		// Token: 0x06001952 RID: 6482 RVA: 0x00059574 File Offset: 0x00057774
		protected override void Update(TurnProcessContext context, PlayerState actor, PlayerState target)
		{
			TurnState currentTurn = context.CurrentTurn;
			GameEvent gameEvent = currentTurn.AddGameEvent<HumiliateEvent>(new HumiliateEvent(actor.Id, target.Id)
			{
				OrderType = OrderTypes.Humiliate
			});
			HumiliateDecisionRequest decisionRequest = new HumiliateDecisionRequest(currentTurn)
			{
				RequestingPlayerId = actor.Id,
				AffectedPlayerId = target.Id,
				PrestigeWager = this.Wager,
				GrievanceResponse = this.Grievance
			};
			gameEvent.AddChildEvent<DecisionAddedEvent>(currentTurn.AddDecisionToAskPlayer(target.Id, decisionRequest));
		}

		// Token: 0x06001953 RID: 6483 RVA: 0x000595F8 File Offset: 0x000577F8
		public override void DeepClone(out DiplomaticState clone)
		{
			PendingDiplomacy_Humiliate pendingDiplomacy_Humiliate = new PendingDiplomacy_Humiliate
			{
				Grievance = this.Grievance.DeepClone<GrievanceContext>()
			};
			base.DeepClonePendingDiplomacyParts(pendingDiplomacy_Humiliate);
			clone = pendingDiplomacy_Humiliate;
		}

		// Token: 0x04000BA0 RID: 2976
		[JsonProperty]
		public GrievanceContext Grievance;
	}
}
