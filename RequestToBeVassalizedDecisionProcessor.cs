using System;

namespace LoG
{
	// Token: 0x020004E5 RID: 1253
	public class RequestToBeVassalizedDecisionProcessor : DiplomaticDecisionProcessor<RequestToBeVassalizedDecisionRequest, RequestToBeVassalizedDecisionResponse>
	{
		// Token: 0x06001798 RID: 6040 RVA: 0x00055914 File Offset: 0x00053B14
		protected override RequestToBeVassalizedDecisionResponse GenerateTypedFallbackResponse()
		{
			return new RequestToBeVassalizedDecisionResponse
			{
				Choice = YesNo.No
			};
		}

		// Token: 0x06001799 RID: 6041 RVA: 0x00055924 File Offset: 0x00053B24
		protected override Result Enact(RequestToBeVassalizedDecisionResponse response)
		{
			DiplomaticPairStatus diplomaticStatus = base._currentTurn.GetDiplomaticStatus(this._player.Id, base.request.RequestingPlayerId);
			if (response.Choice == YesNo.No)
			{
				this.HasRejected(diplomaticStatus);
				return Result.Success;
			}
			if (base.request.PrestigeWager > response.Payment.Prestige)
			{
				this.HasRejected(diplomaticStatus);
				return Result.Success;
			}
			if (!base._currentTurn.AcceptPayment(this._player.Id, response.Payment).successful)
			{
				this.HasRejected(diplomaticStatus);
				return Result.Success;
			}
			diplomaticStatus.SetVassalageConceded(this.TurnProcessContext, this._player.Id);
			OfferVassalageResponseEvent offerVassalageResponseEvent = new OfferVassalageResponseEvent(base.request.RequestingPlayerId, this._player.Id, YesNo.Yes, base._rules.MinDiplomacyOrderCooldown);
			offerVassalageResponseEvent.OrderType = OrderTypes.RequestToBeVassalizedByTarget;
			PlayerState playerState = base._currentTurn.FindPlayerState(base.request.RequestingPlayerId, null);
			PaymentReceivedEvent ev = this.TurnProcessContext.GivePrestige(playerState, this._player, playerState.SpendablePrestige);
			offerVassalageResponseEvent.AddChildEvent<PaymentReceivedEvent>(ev);
			base._currentTurn.AddGameEvent<OfferVassalageResponseEvent>(offerVassalageResponseEvent);
			return Result.Success;
		}

		// Token: 0x0600179A RID: 6042 RVA: 0x00055A50 File Offset: 0x00053C50
		private void HasRejected(DiplomaticPairStatus diplomaticPairStatus)
		{
			diplomaticPairStatus.SetVassalageRejected(this.TurnProcessContext);
			OfferVassalageResponseEvent offerVassalageResponseEvent = new OfferVassalageResponseEvent(base.request.RequestingPlayerId, this._player.Id, YesNo.No, base._rules.MinDiplomacyOrderCooldown);
			offerVassalageResponseEvent.OrderType = OrderTypes.RequestToBeVassalizedByTarget;
			base._currentTurn.AddGameEvent<OfferVassalageResponseEvent>(offerVassalageResponseEvent);
		}

		// Token: 0x0600179B RID: 6043 RVA: 0x00055AA6 File Offset: 0x00053CA6
		protected override Result Preview(RequestToBeVassalizedDecisionResponse response)
		{
			return base._currentTurn.AcceptPayment(this._player.Id, response.Payment);
		}
	}
}
