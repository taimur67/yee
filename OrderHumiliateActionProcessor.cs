using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x020005DD RID: 1501
	public class OrderHumiliateActionProcessor : DiplomaticActionProcessor<OrderHumiliate, DiplomaticAbility_Humiliate>
	{
		// Token: 0x06001C2C RID: 7212 RVA: 0x000613E0 File Offset: 0x0005F5E0
		public override Result Enact(OrderHumiliate order)
		{
			Cost cost = this.CalculateCost();
			base._currentTurn.GetDiplomaticStatus(this._player.Id, order.TargetID).SetDiplomacyPending(this.TurnProcessContext, new PendingDiplomacy_Humiliate(this._player.Id)
			{
				Wager = cost[ResourceTypes.Prestige],
				Grievance = order.GrievanceResponse
			});
			return Result.Success;
		}
	}
}
