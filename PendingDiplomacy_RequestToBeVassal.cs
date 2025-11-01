using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200051D RID: 1309
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PendingDiplomacy_RequestToBeVassal : PendingDiplomacyState
	{
		// Token: 0x1700039C RID: 924
		// (get) Token: 0x0600195E RID: 6494 RVA: 0x000597B5 File Offset: 0x000579B5
		public override DiplomaticPendingValue DiplomaticPendingValue
		{
			get
			{
				return DiplomaticPendingValue.RequestToBeVassal;
			}
		}

		// Token: 0x0600195F RID: 6495 RVA: 0x000597B9 File Offset: 0x000579B9
		[JsonConstructor]
		public PendingDiplomacy_RequestToBeVassal()
		{
		}

		// Token: 0x06001960 RID: 6496 RVA: 0x000597C1 File Offset: 0x000579C1
		public PendingDiplomacy_RequestToBeVassal(int playerId) : base(playerId)
		{
		}

		// Token: 0x06001961 RID: 6497 RVA: 0x000597CC File Offset: 0x000579CC
		protected override void Update(TurnProcessContext context, PlayerState actor, PlayerState target)
		{
			TurnState currentTurn = context.CurrentTurn;
			GameEvent gameEvent = currentTurn.AddGameEvent<RequestToBeVassalizedEvent>(new RequestToBeVassalizedEvent(actor.Id, target.Id)
			{
				OrderType = OrderTypes.RequestToBeVassalizedByTarget
			});
			RequestToBeVassalizedDecisionRequest decisionRequest = new RequestToBeVassalizedDecisionRequest(currentTurn)
			{
				RequestingPlayerId = actor.Id,
				AffectedPlayerId = target.Id,
				PrestigeWager = this.Wager
			};
			gameEvent.AddChildEvent<DecisionAddedEvent>(currentTurn.AddDecisionToAskPlayer(target.Id, decisionRequest));
		}

		// Token: 0x06001962 RID: 6498 RVA: 0x00059844 File Offset: 0x00057A44
		public override void DeepClone(out DiplomaticState clone)
		{
			PendingDiplomacy_RequestToBeVassal pendingDiplomacy_RequestToBeVassal = new PendingDiplomacy_RequestToBeVassal();
			base.DeepClonePendingDiplomacyParts(pendingDiplomacy_RequestToBeVassal);
			clone = pendingDiplomacy_RequestToBeVassal;
		}
	}
}
