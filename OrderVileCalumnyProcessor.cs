using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x020005ED RID: 1517
	public class OrderVileCalumnyProcessor : DiplomaticActionProcessor<OrderVileCalumny, DiplomaticAbility_VileCalumny>
	{
		// Token: 0x06001C78 RID: 7288 RVA: 0x00062114 File Offset: 0x00060314
		public override Result Validate()
		{
			Problem problem = base.IsAvailable() as Problem;
			if (problem != null)
			{
				return problem;
			}
			base._currentTurn.GetDiplomaticStatus(this._player.Id, base.request.TargetID);
			if (!(base._currentTurn.GetDiplomaticStatus(base.request.TargetID, base.request.ScapegoatId).DiplomaticState is NeutralState))
			{
				return new Result.ScapegoatRelationshipProblemProblem(base.request.TargetID, base.request.OrderType, base.request.ScapegoatId, DiplomaticStateValue.Neutral);
			}
			return Result.Success;
		}

		// Token: 0x06001C79 RID: 7289 RVA: 0x000621B0 File Offset: 0x000603B0
		public override Result Enact(OrderVileCalumny order)
		{
			Problem problem = this.Validate() as Problem;
			if (problem != null)
			{
				return problem;
			}
			int wager = this.CalculateCost()[ResourceTypes.Prestige];
			base._currentTurn.GetDiplomaticStatus(order.ScapegoatId, order.TargetID).SetDiplomacyPending(this.TurnProcessContext, new PendingDiplomacy_VileCalumny(order.ScapegoatId)
			{
				SourceId = this._player.Id,
				Wager = wager
			});
			return Result.Success;
		}
	}
}
