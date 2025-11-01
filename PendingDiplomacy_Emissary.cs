using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200051B RID: 1307
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PendingDiplomacy_Emissary : PendingDiplomacyState
	{
		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06001954 RID: 6484 RVA: 0x00059626 File Offset: 0x00057826
		public override DiplomaticPendingValue DiplomaticPendingValue
		{
			get
			{
				return DiplomaticPendingValue.Emissary;
			}
		}

		// Token: 0x06001955 RID: 6485 RVA: 0x0005962A File Offset: 0x0005782A
		[JsonConstructor]
		public PendingDiplomacy_Emissary()
		{
		}

		// Token: 0x06001956 RID: 6486 RVA: 0x00059632 File Offset: 0x00057832
		public PendingDiplomacy_Emissary(int playerId) : base(playerId)
		{
		}

		// Token: 0x06001957 RID: 6487 RVA: 0x0005963C File Offset: 0x0005783C
		protected override void Update(TurnProcessContext context, PlayerState actor, PlayerState target)
		{
			TurnState currentTurn = context.CurrentTurn;
			GameEvent gameEvent = currentTurn.AddGameEvent<SendEmissaryEvent>(new SendEmissaryEvent(actor.Id, target.Id)
			{
				OrderType = OrderTypes.SendEmissary
			});
			SendEmissaryDecisionRequest decisionRequest = new SendEmissaryDecisionRequest(currentTurn)
			{
				RequestingPlayerId = actor.Id,
				AffectedPlayerId = target.Id,
				OfferPayment = this.Offer,
				PrestigeWager = this.Wager,
				ArmisticeLength = this.ArmisticeLength
			};
			gameEvent.AddChildEvent<DecisionAddedEvent>(currentTurn.AddDecisionToAskPlayer(target.Id, decisionRequest));
		}

		// Token: 0x06001958 RID: 6488 RVA: 0x000596CC File Offset: 0x000578CC
		public override void DeepClone(out DiplomaticState clone)
		{
			PendingDiplomacy_Emissary pendingDiplomacy_Emissary = new PendingDiplomacy_Emissary
			{
				ArmisticeLength = this.ArmisticeLength,
				Offer = this.Offer.DeepClone<Payment>()
			};
			base.DeepClonePendingDiplomacyParts(pendingDiplomacy_Emissary);
			clone = pendingDiplomacy_Emissary;
		}

		// Token: 0x04000BA1 RID: 2977
		[JsonProperty]
		public int ArmisticeLength;

		// Token: 0x04000BA2 RID: 2978
		[JsonProperty]
		public Payment Offer;
	}
}
