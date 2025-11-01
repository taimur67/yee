using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x020005EB RID: 1515
	public class OrderSendEmissaryActionProcessor : DiplomaticActionProcessor<OrderSendEmissary, DiplomaticAbility_SendEmissary>
	{
		// Token: 0x06001C6B RID: 7275 RVA: 0x00061F88 File Offset: 0x00060188
		public override Result Enact(OrderSendEmissary order)
		{
			Cost cost = this.CalculateCost();
			int armisticeLength = this.CalculateArmisticeLength(order.OfferPayment.Resources.Count);
			base._currentTurn.GetDiplomaticStatus(this._player.Id, order.TargetID).SetDiplomacyPending(this.TurnProcessContext, new PendingDiplomacy_Emissary(this._player.Id)
			{
				Wager = cost[ResourceTypes.Prestige],
				Offer = order.OfferPayment,
				ArmisticeLength = armisticeLength
			});
			return Result.Success;
		}

		// Token: 0x06001C6C RID: 7276 RVA: 0x0006200F File Offset: 0x0006020F
		private int CalculateArmisticeLength(int tributeCount)
		{
			return (int)(tributeCount + this._player.Rank);
		}
	}
}
