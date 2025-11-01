using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x020005E3 RID: 1507
	public class OrderRequestChainsOfAvariceProcessor : DiplomaticActionProcessor<OrderRequestChainsOfAvarice, DiplomaticAbility_ChainsOfAvarice>
	{
		// Token: 0x06001C41 RID: 7233 RVA: 0x000616E8 File Offset: 0x0005F8E8
		public override Result Enact(OrderRequestChainsOfAvarice order)
		{
			Cost cost = this.CalculateCost();
			base._currentTurn.GetDiplomaticStatus(this._player.Id, order.TargetID).SetDiplomacyPending(this.TurnProcessContext, new PendingDiplomacy_ChainsOfAvarice(this._player.Id)
			{
				Duration = base.data.Duration,
				Wager = cost[ResourceTypes.Prestige]
			});
			return Result.Success;
		}
	}
}
