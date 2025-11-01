using System;

namespace LoG
{
	// Token: 0x020004FA RID: 1274
	public class RequestBribeEventDecisionProcessor : DecisionProcessor<RequestBribeEventDecisionRequest, RequestBribeEventDecisionResponse>
	{
		// Token: 0x0600182B RID: 6187 RVA: 0x00056C2C File Offset: 0x00054E2C
		protected override Result Process(RequestBribeEventDecisionResponse response)
		{
			if (response.Choice == YesNo.No)
			{
				this.DeductPrestige();
				return Result.Success;
			}
			if (!base._currentTurn.ValidatePayment(this._player.Id, base.request.Cost, response.Payment))
			{
				this.DeductPrestige();
				return Result.Failure;
			}
			if (!base._currentTurn.AcceptPayment(this._player.Id, response.Payment))
			{
				this.DeductPrestige();
				return Result.Failure;
			}
			return Result.Success;
		}

		// Token: 0x0600182C RID: 6188 RVA: 0x00056CBC File Offset: 0x00054EBC
		private void DeductPrestige()
		{
			int prestige = (int)Math.Round((double)((float)this._player.SpendablePrestige * base.request.PrestigeLossPercent), MidpointRounding.AwayFromZero);
			PaymentRemovedEvent gameEvent = this.TurnProcessContext.RemovePayment(this._player, new Payment
			{
				Prestige = prestige
			}, null);
			base._currentTurn.AddGameEvent<PaymentRemovedEvent>(gameEvent);
		}
	}
}
