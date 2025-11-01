using System;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000516 RID: 1302
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PendingDiplomacy_Extortion : PendingDiplomacyState
	{
		// Token: 0x17000395 RID: 917
		// (get) Token: 0x0600193B RID: 6459 RVA: 0x00059218 File Offset: 0x00057418
		public override DiplomaticPendingValue DiplomaticPendingValue
		{
			get
			{
				return DiplomaticPendingValue.Extort;
			}
		}

		// Token: 0x0600193C RID: 6460 RVA: 0x0005921B File Offset: 0x0005741B
		[JsonConstructor]
		public PendingDiplomacy_Extortion()
		{
		}

		// Token: 0x0600193D RID: 6461 RVA: 0x00059223 File Offset: 0x00057423
		public PendingDiplomacy_Extortion(int playerId) : base(playerId)
		{
		}

		// Token: 0x0600193E RID: 6462 RVA: 0x0005922C File Offset: 0x0005742C
		protected override void Update(TurnProcessContext context, PlayerState actor, PlayerState target)
		{
			TurnState currentTurn = context.CurrentTurn;
			bool costReduced = IEnumerableExtensions.Any<StatModifierBase>(actor.Get(ArchfiendStat.SelfDemandCostReduction).ActiveModifiers);
			GameEvent gameEvent = currentTurn.AddGameEvent<ExtortEvent>(new ExtortEvent(actor.Id, target.Id)
			{
				OrderType = OrderTypes.Extort,
				CostReduced = costReduced
			});
			ExtortDecisionRequest decisionRequest = new ExtortDecisionRequest(currentTurn)
			{
				RequestingPlayerId = actor.Id,
				AffectedPlayerId = target.Id,
				PrestigeWager = this.Wager,
				DemandOption = this.Demand
			};
			gameEvent.AddChildEvent<DecisionAddedEvent>(currentTurn.AddDecisionToAskPlayer(target.Id, decisionRequest));
		}

		// Token: 0x0600193F RID: 6463 RVA: 0x000592C8 File Offset: 0x000574C8
		public override void DeepClone(out DiplomaticState clone)
		{
			PendingDiplomacy_Extortion pendingDiplomacy_Extortion = new PendingDiplomacy_Extortion
			{
				Demand = this.Demand
			};
			base.DeepClonePendingDiplomacyParts(pendingDiplomacy_Extortion);
			clone = pendingDiplomacy_Extortion;
		}

		// Token: 0x04000B9C RID: 2972
		[JsonProperty]
		public DemandOptions Demand;
	}
}
