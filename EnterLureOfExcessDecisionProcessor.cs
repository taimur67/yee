using System;

namespace LoG
{
	// Token: 0x020004DC RID: 1244
	public class EnterLureOfExcessDecisionProcessor : DiplomaticDecisionProcessor<EnterLureOfExcessDecisionRequest, EnterLureOfExcessDecisionResponse>
	{
		// Token: 0x0600176A RID: 5994 RVA: 0x00055144 File Offset: 0x00053344
		protected override Result Enact(EnterLureOfExcessDecisionResponse response)
		{
			YesNo choice = response.Choice;
			DiplomaticPairStatus diplomaticStatus = base._currentDiplomacy.GetDiplomaticStatus(this._player.Id, base.request.RequestingPlayerId);
			int duration = base.request.Duration;
			int armisticeDuration = Math.Max(this.TurnProcessContext.Rules.MinDiplomacyOrderCooldown, duration);
			LureOfExcessResponseEvent lureOfExcessResponseEvent = base._currentTurn.AddGameEvent<LureOfExcessResponseEvent>(new LureOfExcessResponseEvent(base.request.RequestingPlayerId, this._player.Id, choice, armisticeDuration, base.request.PrestigeWager, duration));
			lureOfExcessResponseEvent.OrderType = base.request.OrderType;
			if (choice == YesNo.Yes)
			{
				PlayerState toPlayer = base._currentTurn.FindPlayerState(base.request.RequestingPlayerId, null);
				diplomaticStatus.SetLureOfExcess(this.TurnProcessContext, base.request.RequestingPlayerId, base.request.Duration, false);
				lureOfExcessResponseEvent.AddChildEvent<PaymentReceivedEvent>(this.TurnProcessContext.GivePayment(toPlayer, new Payment
				{
					Prestige = base.request.PrestigeWager
				}, null));
			}
			else
			{
				Payment payment = new Payment
				{
					Prestige = base.request.RejectionPrestigeCost
				};
				lureOfExcessResponseEvent.AddChildEvent<PaymentRemovedEvent>(this.TurnProcessContext.RemovePayment(this._player, payment, null));
				diplomaticStatus.SetNeutral(this.TurnProcessContext, false);
			}
			return Result.Success;
		}
	}
}
