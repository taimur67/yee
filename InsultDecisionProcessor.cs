using System;

namespace LoG
{
	// Token: 0x020004D9 RID: 1241
	public class InsultDecisionProcessor : DiplomaticDecisionProcessor<InsultDecisionRequest, InsultDecisionResponse>
	{
		// Token: 0x0600175C RID: 5980 RVA: 0x00054EA0 File Offset: 0x000530A0
		protected override InsultDecisionResponse GenerateTypedFallbackResponse()
		{
			return new InsultDecisionResponse
			{
				Choice = YesNo.Yes
			};
		}

		// Token: 0x0600175D RID: 5981 RVA: 0x00054EB0 File Offset: 0x000530B0
		protected override Result Enact(InsultDecisionResponse response)
		{
			DiplomaticPairStatus diplomaticStatus = base._currentTurn.GetDiplomaticStatus(this._player.Id, base.request.RequestingPlayerId);
			diplomaticStatus.SetNeutral(this.TurnProcessContext, false);
			if (!response.IsAcceptInsult)
			{
				return this.EnactPursueGrievance(diplomaticStatus, response.GrievanceResponse);
			}
			return this.EnactAcceptInsult(diplomaticStatus, base._currentTurn.FindPlayerState(base.request.RequestingPlayerId, null));
		}

		// Token: 0x0600175E RID: 5982 RVA: 0x00054F20 File Offset: 0x00053120
		private Result EnactAcceptInsult(DiplomaticPairStatus diplomaticStatus, PlayerState opponent)
		{
			diplomaticStatus.SetInsultConceded(this.TurnProcessContext);
			Payment payment = Payment.FromPrestige(base.request.PrestigeWager);
			this._player.RemovePayment(payment);
			PaymentReceivedEvent paymentReceivedEvent = base.GiveDiplomacyPayment(opponent, payment, 2);
			if (paymentReceivedEvent == null)
			{
				return Result.Failure;
			}
			InsultResponseEvent insultResponseEvent = new InsultResponseEvent(base.request.RequestingPlayerId, this._player.Id, YesNo.Yes, base._rules.MinDiplomacyOrderCooldown);
			insultResponseEvent.Prestige = base.request.PrestigeWager;
			insultResponseEvent.AddChildEvent<PaymentReceivedEvent>(paymentReceivedEvent);
			insultResponseEvent.OrderType = OrderTypes.Insult;
			base._currentTurn.AddGameEvent<InsultResponseEvent>(insultResponseEvent);
			return Result.Success;
		}

		// Token: 0x0600175F RID: 5983 RVA: 0x00054FC4 File Offset: 0x000531C4
		private Result EnactPursueGrievance(DiplomaticPairStatus diplomaticStatus, GrievanceContext grievanceContext)
		{
			GameEvent gameEvent;
			Result result = GrievanceProcessor.BeginVendettaOrDuel(this.TurnProcessContext, this._player.Id, base.request.RequestingPlayerId, grievanceContext, out gameEvent);
			if (gameEvent != null)
			{
				base._currentTurn.AddGameEvent<GameEvent>(gameEvent);
			}
			diplomaticStatus.SetInsultRejected(this.TurnProcessContext);
			Payment payment = Payment.FromPrestige(grievanceContext.BasePrestigeWager);
			this._player.RemovePayment(payment);
			InsultResponseEvent insultResponseEvent = new InsultResponseEvent(base.request.RequestingPlayerId, this._player.Id, YesNo.No, base._rules.MinDiplomacyOrderCooldown);
			insultResponseEvent.OrderType = OrderTypes.Insult;
			insultResponseEvent.GrievanceResponse = grievanceContext;
			insultResponseEvent.Prestige = base.request.PrestigeWager;
			base._currentTurn.AddGameEvent<InsultResponseEvent>(insultResponseEvent);
			return result;
		}

		// Token: 0x06001760 RID: 5984 RVA: 0x00055080 File Offset: 0x00053280
		protected override Result Preview(InsultDecisionResponse response)
		{
			Payment payment = Payment.FromPrestige(response.IsAcceptInsult ? base.request.PrestigeWager : response.GrievanceResponse.BasePrestigeWager);
			return base._currentTurn.AcceptPayment(this._player.Id, payment);
		}
	}
}
