using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200051F RID: 1311
	public class PendingDiplomacy_ChainsOfAvarice : PendingDiplomacyState
	{
		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06001968 RID: 6504 RVA: 0x0005993F File Offset: 0x00057B3F
		public override DiplomaticPendingValue DiplomaticPendingValue
		{
			get
			{
				return DiplomaticPendingValue.ChainsOfAvarice;
			}
		}

		// Token: 0x06001969 RID: 6505 RVA: 0x00059946 File Offset: 0x00057B46
		[JsonConstructor]
		public PendingDiplomacy_ChainsOfAvarice()
		{
		}

		// Token: 0x0600196A RID: 6506 RVA: 0x0005994E File Offset: 0x00057B4E
		public PendingDiplomacy_ChainsOfAvarice(int playerId) : base(playerId)
		{
		}

		// Token: 0x0600196B RID: 6507 RVA: 0x00059958 File Offset: 0x00057B58
		protected override void Update(TurnProcessContext context, PlayerState actor, PlayerState target)
		{
			TurnState currentTurn = context.CurrentTurn;
			GameEvent gameEvent = currentTurn.AddGameEvent<RequestChainsOfAvariceEvent>(new RequestChainsOfAvariceEvent(actor.Id, target.Id)
			{
				OrderType = OrderTypes.ChainsOfAvarice
			});
			EnterChainsOfAvariceDecisionRequest decisionRequest = new EnterChainsOfAvariceDecisionRequest(currentTurn)
			{
				RequestingPlayerId = actor.Id,
				AffectedPlayerId = target.Id,
				Duration = this.Duration
			};
			gameEvent.AddChildEvent<DecisionAddedEvent>(currentTurn.AddDecisionToAskPlayer(target.Id, decisionRequest));
		}

		// Token: 0x0600196C RID: 6508 RVA: 0x000599D0 File Offset: 0x00057BD0
		public override void DeepClone(out DiplomaticState clone)
		{
			PendingDiplomacy_ChainsOfAvarice pendingDiplomacy_ChainsOfAvarice = new PendingDiplomacy_ChainsOfAvarice
			{
				Duration = this.Duration
			};
			base.DeepClonePendingDiplomacyParts(pendingDiplomacy_ChainsOfAvarice);
			clone = pendingDiplomacy_ChainsOfAvarice;
		}

		// Token: 0x04000BA5 RID: 2981
		[JsonProperty]
		public int Duration;
	}
}
