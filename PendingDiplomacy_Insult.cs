using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000518 RID: 1304
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PendingDiplomacy_Insult : PendingDiplomacyState
	{
		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06001945 RID: 6469 RVA: 0x000593BE File Offset: 0x000575BE
		public override DiplomaticPendingValue DiplomaticPendingValue
		{
			get
			{
				return DiplomaticPendingValue.Insult;
			}
		}

		// Token: 0x06001946 RID: 6470 RVA: 0x000593C1 File Offset: 0x000575C1
		[JsonConstructor]
		public PendingDiplomacy_Insult()
		{
		}

		// Token: 0x06001947 RID: 6471 RVA: 0x000593C9 File Offset: 0x000575C9
		public PendingDiplomacy_Insult(int playerId) : base(playerId)
		{
		}

		// Token: 0x06001948 RID: 6472 RVA: 0x000593D4 File Offset: 0x000575D4
		protected override void Update(TurnProcessContext context, PlayerState actor, PlayerState target)
		{
			GameEvent gameEvent = context.CurrentTurn.AddGameEvent<InsultHurledEvent>(new InsultHurledEvent(actor.Id, target.Id)
			{
				OrderType = OrderTypes.Insult
			});
			InsultDecisionRequest decisionRequest = new InsultDecisionRequest(context.CurrentTurn)
			{
				RequestingPlayerId = actor.Id,
				AffectedPlayerId = target.Id,
				PrestigeWager = this.Wager
			};
			gameEvent.AddChildEvent<DecisionAddedEvent>(context.CurrentTurn.AddDecisionToAskPlayer(target.Id, decisionRequest));
		}

		// Token: 0x06001949 RID: 6473 RVA: 0x00059454 File Offset: 0x00057654
		public override void DeepClone(out DiplomaticState clone)
		{
			PendingDiplomacy_Insult pendingDiplomacy_Insult = new PendingDiplomacy_Insult();
			base.DeepClonePendingDiplomacyParts(pendingDiplomacy_Insult);
			clone = pendingDiplomacy_Insult;
		}
	}
}
