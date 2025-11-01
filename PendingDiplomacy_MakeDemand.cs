using System;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000519 RID: 1305
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PendingDiplomacy_MakeDemand : PendingDiplomacyState
	{
		// Token: 0x17000398 RID: 920
		// (get) Token: 0x0600194A RID: 6474 RVA: 0x00059471 File Offset: 0x00057671
		public override DiplomaticPendingValue DiplomaticPendingValue
		{
			get
			{
				return DiplomaticPendingValue.MakeDemand;
			}
		}

		// Token: 0x0600194B RID: 6475 RVA: 0x00059474 File Offset: 0x00057674
		[JsonConstructor]
		public PendingDiplomacy_MakeDemand()
		{
		}

		// Token: 0x0600194C RID: 6476 RVA: 0x0005947C File Offset: 0x0005767C
		public PendingDiplomacy_MakeDemand(int playerId) : base(playerId)
		{
		}

		// Token: 0x0600194D RID: 6477 RVA: 0x00059488 File Offset: 0x00057688
		protected override void Update(TurnProcessContext context, PlayerState actor, PlayerState target)
		{
			TurnState currentTurn = context.CurrentTurn;
			bool costReduced = IEnumerableExtensions.Any<StatModifierBase>(actor.Get(ArchfiendStat.SelfDemandCostReduction).ActiveModifiers);
			GameEvent gameEvent = currentTurn.AddGameEvent<MakeDemandEvent>(new MakeDemandEvent(actor.Id, target.Id)
			{
				OrderType = OrderTypes.Demand,
				CostReduced = costReduced
			});
			MakeDemandDecisionRequest decisionRequest = new MakeDemandDecisionRequest(currentTurn)
			{
				RequestingPlayerId = actor.Id,
				AffectedPlayerId = target.Id,
				PrestigeWager = this.Wager,
				DemandOption = this.Demand,
				NumCards = this.Demand.ToNumCards()
			};
			gameEvent.AddChildEvent<DecisionAddedEvent>(currentTurn.AddDecisionToAskPlayer(target.Id, decisionRequest));
		}

		// Token: 0x0600194E RID: 6478 RVA: 0x00059534 File Offset: 0x00057734
		public override void DeepClone(out DiplomaticState clone)
		{
			PendingDiplomacy_MakeDemand pendingDiplomacy_MakeDemand = new PendingDiplomacy_MakeDemand
			{
				Demand = this.Demand
			};
			base.DeepClonePendingDiplomacyParts(pendingDiplomacy_MakeDemand);
			clone = pendingDiplomacy_MakeDemand;
		}

		// Token: 0x04000B9F RID: 2975
		[JsonProperty]
		public DemandOptions Demand;
	}
}
