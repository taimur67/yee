using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x020005DF RID: 1503
	public class OrderInsultActionProcessor : DiplomaticActionProcessor<OrderInsult, DiplomaticAbility_Insult>
	{
		// Token: 0x06001C32 RID: 7218 RVA: 0x00061470 File Offset: 0x0005F670
		public override Result Enact(OrderInsult order)
		{
			Cost cost = this.CalculateCost();
			base._currentTurn.GetDiplomaticStatus(this._player.Id, order.TargetID).SetDiplomacyPending(this.TurnProcessContext, new PendingDiplomacy_Insult(this._player.Id)
			{
				Wager = cost[ResourceTypes.Prestige]
			});
			return Result.Success;
		}
	}
}
