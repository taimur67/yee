using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x020005E1 RID: 1505
	public class OrderMakeDemandActionProcessor : DiplomaticActionProcessor<OrderMakeDemand, DiplomaticAbility_MakeDemand>
	{
		// Token: 0x06001C3B RID: 7227 RVA: 0x000615FC File Offset: 0x0005F7FC
		public override Result Enact(OrderMakeDemand order)
		{
			Cost cost = this.CalculateCost();
			base._currentTurn.GetDiplomaticStatus(this._player.Id, order.TargetID).SetDiplomacyPending(this.TurnProcessContext, new PendingDiplomacy_MakeDemand(this._player.Id)
			{
				Wager = cost[ResourceTypes.Prestige],
				Demand = order.DemandOption
			});
			return Result.Success;
		}

		// Token: 0x06001C3C RID: 7228 RVA: 0x00061668 File Offset: 0x0005F868
		public override Cost CalculateCost()
		{
			Cost cost = base.CalculateCost();
			int num = cost[ResourceTypes.Prestige];
			PlayerState playerState = base._currentTurn.FindPlayerState(base.request.TargetID, null);
			num = Math.Max(0, num + playerState.OtherDemandCostIncrease - this._player.SelfDemandCostReduction);
			cost[ResourceTypes.Prestige] = num;
			return cost;
		}
	}
}
