using System;

namespace LoG
{
	// Token: 0x02000533 RID: 1331
	public abstract class DiplomaticDecisionProcessor<Request, Response> : DecisionProcessor<Request, Response>, IDiplomaticDecisionProcessor where Request : DecisionRequest, IDiplomaticDecisionRequest where Response : DiplomaticDecisionResponse
	{
		// Token: 0x060019D6 RID: 6614 RVA: 0x0005A728 File Offset: 0x00058928
		protected sealed override Result Process(Response response)
		{
			this.EnactImmediate(response);
			int actor = base.request.RequestingPlayerId;
			VileCalumnyRequest vileCalumnyRequest = base.request as VileCalumnyRequest;
			if (vileCalumnyRequest != null)
			{
				actor = vileCalumnyRequest.ScapegoatId;
			}
			this.TurnProcessContext.DiplomaticContext.AddDiplomaticAction(actor, base.request.AffectedPlayerId, new PlayerDiplomaticDecision
			{
				Player = this._player,
				Request = base.request,
				Response = response,
				Processor = this,
				OrderSlotIndex = 0
			});
			return Result.Success;
		}

		// Token: 0x060019D7 RID: 6615 RVA: 0x0005A7D0 File Offset: 0x000589D0
		protected virtual Result EnactImmediate(DiplomaticDecisionResponse response)
		{
			return Result.Success;
		}

		// Token: 0x060019D8 RID: 6616 RVA: 0x0005A7D7 File Offset: 0x000589D7
		public Result Enact(DiplomaticDecisionResponse response)
		{
			return this.Enact((Response)((object)response));
		}

		// Token: 0x060019D9 RID: 6617
		protected abstract Result Enact(Response response);

		// Token: 0x060019DA RID: 6618 RVA: 0x0005A7E5 File Offset: 0x000589E5
		protected override Result Validate(Response response)
		{
			if (response.Choice != YesNo.Undefined)
			{
				return Result.Success;
			}
			return Result.Failure;
		}

		// Token: 0x060019DB RID: 6619 RVA: 0x0005A7FF File Offset: 0x000589FF
		protected PaymentReceivedEvent GiveDiplomacyPayment(PlayerState receiver, Payment payment, int prestigeMultiplier = 1)
		{
			payment.Prestige *= prestigeMultiplier;
			payment.Prestige += receiver.DiplomacyPrestigeBonus;
			return this.TurnProcessContext.GivePayment(receiver, payment, null);
		}
	}
}
