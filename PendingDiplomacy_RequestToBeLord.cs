using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200051C RID: 1308
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PendingDiplomacy_RequestToBeLord : PendingDiplomacyState
	{
		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06001959 RID: 6489 RVA: 0x00059706 File Offset: 0x00057906
		public override DiplomaticPendingValue DiplomaticPendingValue
		{
			get
			{
				return DiplomaticPendingValue.RequestToBeLord;
			}
		}

		// Token: 0x0600195A RID: 6490 RVA: 0x0005970D File Offset: 0x0005790D
		[JsonConstructor]
		public PendingDiplomacy_RequestToBeLord()
		{
		}

		// Token: 0x0600195B RID: 6491 RVA: 0x00059715 File Offset: 0x00057915
		public PendingDiplomacy_RequestToBeLord(int playerId) : base(playerId)
		{
		}

		// Token: 0x0600195C RID: 6492 RVA: 0x00059720 File Offset: 0x00057920
		protected override void Update(TurnProcessContext context, PlayerState actor, PlayerState target)
		{
			TurnState currentTurn = context.CurrentTurn;
			GameEvent gameEvent = currentTurn.AddGameEvent<RequestToBeBloodLordEvent>(new RequestToBeBloodLordEvent(actor.Id, target.Id)
			{
				OrderType = OrderTypes.RequestToBeBloodLordOfTarget
			});
			RequestToBeBloodLordDecisionRequest decisionRequest = new RequestToBeBloodLordDecisionRequest(currentTurn)
			{
				RequestingPlayerId = actor.Id,
				AffectedPlayerId = target.Id,
				PrestigeWager = this.Wager
			};
			gameEvent.AddChildEvent<DecisionAddedEvent>(currentTurn.AddDecisionToAskPlayer(target.Id, decisionRequest));
		}

		// Token: 0x0600195D RID: 6493 RVA: 0x00059798 File Offset: 0x00057998
		public override void DeepClone(out DiplomaticState clone)
		{
			PendingDiplomacy_RequestToBeLord pendingDiplomacy_RequestToBeLord = new PendingDiplomacy_RequestToBeLord();
			base.DeepClonePendingDiplomacyParts(pendingDiplomacy_RequestToBeLord);
			clone = pendingDiplomacy_RequestToBeLord;
		}
	}
}
