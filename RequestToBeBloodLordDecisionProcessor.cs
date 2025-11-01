using System;

namespace LoG
{
	// Token: 0x020004E2 RID: 1250
	public class RequestToBeBloodLordDecisionProcessor : DiplomaticDecisionProcessor<RequestToBeBloodLordDecisionRequest, RequestToBeBloodLordDecisionResponse>
	{
		// Token: 0x0600178B RID: 6027 RVA: 0x000556C4 File Offset: 0x000538C4
		protected override Result Enact(RequestToBeBloodLordDecisionResponse response)
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
			diplomaticStatus.SetVassalageConceded(this.TurnProcessContext, base.request.RequestingPlayerId);
			OfferLordshipResponseEvent offerLordshipResponseEvent = new OfferLordshipResponseEvent(base.request.RequestingPlayerId, this._player.Id, YesNo.Yes, base._rules.MinDiplomacyOrderCooldown);
			offerLordshipResponseEvent.OrderType = OrderTypes.RequestToBeBloodLordOfTarget;
			PlayerState toPlayer = base._currentTurn.FindPlayerState(base.request.RequestingPlayerId, null);
			PaymentReceivedEvent ev = this.TurnProcessContext.GivePrestige(this._player, toPlayer, this._player.SpendablePrestige);
			offerLordshipResponseEvent.AddChildEvent<PaymentReceivedEvent>(ev);
			base._currentTurn.AddGameEvent<OfferLordshipResponseEvent>(offerLordshipResponseEvent);
			return Result.Success;
		}

		// Token: 0x0600178C RID: 6028 RVA: 0x000557F8 File Offset: 0x000539F8
		private void HasRejected(DiplomaticPairStatus diplomaticPairStatus)
		{
			diplomaticPairStatus.SetVassalageRejected(this.TurnProcessContext);
			OfferLordshipResponseEvent offerLordshipResponseEvent = new OfferLordshipResponseEvent(base.request.RequestingPlayerId, this._player.Id, YesNo.No, base._rules.MinDiplomacyOrderCooldown);
			offerLordshipResponseEvent.OrderType = OrderTypes.RequestToBeBloodLordOfTarget;
			base._currentTurn.AddGameEvent<OfferLordshipResponseEvent>(offerLordshipResponseEvent);
		}

		// Token: 0x0600178D RID: 6029 RVA: 0x0005584E File Offset: 0x00053A4E
		protected override Result Preview(RequestToBeBloodLordDecisionResponse response)
		{
			return base._currentTurn.AcceptPayment(this._player.Id, response.Payment);
		}
	}
}
