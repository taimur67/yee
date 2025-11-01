using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x020005DB RID: 1499
	public class OrderExtortActionProcessor : DiplomaticActionProcessor<OrderExtort, DiplomaticAbility_Extort>
	{
		// Token: 0x06001C23 RID: 7203 RVA: 0x000612D4 File Offset: 0x0005F4D4
		public override Result Enact(OrderExtort order)
		{
			Cost cost = this.CalculateCost();
			base._currentTurn.GetDiplomaticStatus(this._player.Id, order.TargetID).SetDiplomacyPending(this.TurnProcessContext, new PendingDiplomacy_Extortion(this._player.Id)
			{
				Wager = cost[ResourceTypes.Prestige],
				Demand = order.DemandOption
			});
			return Result.Success;
		}

		// Token: 0x06001C24 RID: 7204 RVA: 0x00061340 File Offset: 0x0005F540
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
