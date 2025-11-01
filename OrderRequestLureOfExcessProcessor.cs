using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x020005E5 RID: 1509
	public class OrderRequestLureOfExcessProcessor : DiplomaticActionProcessor<OrderRequestLureOfExcess, DiplomaticAbility_LureOfExcess>
	{
		// Token: 0x06001C46 RID: 7238 RVA: 0x00061774 File Offset: 0x0005F974
		public override Result Enact(OrderRequestLureOfExcess order)
		{
			Cost cost = this.CalculateCost();
			base._currentTurn.GetDiplomaticStatus(this._player.Id, order.TargetID).SetDiplomacyPending(this.TurnProcessContext, new PendingDiplomacy_LureOfExcess(this._player.Id)
			{
				Duration = base.data.Duration,
				Wager = cost[ResourceTypes.Prestige],
				RejectionPrestigePenalty = base.data.RejectionPenalty
			});
			return Result.Success;
		}
	}
}
