using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000520 RID: 1312
	public class PendingDiplomacy_LureOfExcess : PendingDiplomacyState
	{
		// Token: 0x1700039F RID: 927
		// (get) Token: 0x0600196D RID: 6509 RVA: 0x000599F9 File Offset: 0x00057BF9
		public override DiplomaticPendingValue DiplomaticPendingValue
		{
			get
			{
				return DiplomaticPendingValue.LureOfExcess;
			}
		}

		// Token: 0x0600196E RID: 6510 RVA: 0x00059A00 File Offset: 0x00057C00
		[JsonConstructor]
		public PendingDiplomacy_LureOfExcess()
		{
		}

		// Token: 0x0600196F RID: 6511 RVA: 0x00059A08 File Offset: 0x00057C08
		public PendingDiplomacy_LureOfExcess(int playerId) : base(playerId)
		{
		}

		// Token: 0x06001970 RID: 6512 RVA: 0x00059A14 File Offset: 0x00057C14
		protected override void Update(TurnProcessContext context, PlayerState actor, PlayerState target)
		{
			TurnState currentTurn = context.CurrentTurn;
			GameEvent gameEvent = currentTurn.AddGameEvent<RequestLureOfExcessEvent>(new RequestLureOfExcessEvent(actor.Id, target.Id)
			{
				OrderType = OrderTypes.LureOfExcess
			});
			EnterLureOfExcessDecisionRequest decisionRequest = new EnterLureOfExcessDecisionRequest(currentTurn)
			{
				RequestingPlayerId = actor.Id,
				AffectedPlayerId = target.Id,
				Duration = this.Duration,
				RejectionPrestigeCost = this.RejectionPrestigePenalty,
				PrestigeWager = this.Wager
			};
			gameEvent.AddChildEvent<DecisionAddedEvent>(currentTurn.AddDecisionToAskPlayer(target.Id, decisionRequest));
		}

		// Token: 0x06001971 RID: 6513 RVA: 0x00059AA4 File Offset: 0x00057CA4
		public override void DeepClone(out DiplomaticState clone)
		{
			PendingDiplomacy_LureOfExcess pendingDiplomacy_LureOfExcess = new PendingDiplomacy_LureOfExcess
			{
				Duration = this.Duration,
				RejectionPrestigePenalty = this.RejectionPrestigePenalty
			};
			base.DeepClonePendingDiplomacyParts(pendingDiplomacy_LureOfExcess);
			clone = pendingDiplomacy_LureOfExcess;
		}

		// Token: 0x04000BA6 RID: 2982
		[JsonProperty]
		public int Duration;

		// Token: 0x04000BA7 RID: 2983
		[JsonProperty]
		public int RejectionPrestigePenalty;
	}
}
