using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x020005D6 RID: 1494
	public class OrderDeclareBloodFeudActionProcessor : DiplomaticActionProcessor<OrderDeclareBloodFeud, DiplomaticAbility_StartBloodFeud>
	{
		// Token: 0x06001C15 RID: 7189 RVA: 0x000610D4 File Offset: 0x0005F2D4
		public override Result IsAvailable()
		{
			Problem problem = base.IsAvailable() as Problem;
			if (problem != null)
			{
				return problem;
			}
			if (base._currentTurn.IsPlayerDisgraced(base.request.TargetID))
			{
				return Result.Success;
			}
			int currentVictories;
			int requiredVictories;
			if (!this.TurnProcessContext.HasEnoughVendettasForBloodFeud(this._player, base.request.TargetID, out currentVictories, out requiredVictories))
			{
				return new Result.NotEnoughVendettasWonProblem(base.request.TargetID, base.request.OrderType, currentVictories, requiredVictories);
			}
			return Result.Success;
		}

		// Token: 0x06001C16 RID: 7190 RVA: 0x00061158 File Offset: 0x0005F358
		public override Result Enact(OrderDeclareBloodFeud order)
		{
			base._currentTurn.GetDiplomaticStatus(this._player.Id, order.TargetID).SetBloodFeud(this.TurnProcessContext, this._player.Id);
			DeclareBloodFeudEvent declareBloodFeudEvent = new DeclareBloodFeudEvent(this._player.Id, order.TargetID);
			declareBloodFeudEvent.OrderType = OrderTypes.DeclareBloodFeud;
			base._currentTurn.AddGameEvent<DeclareBloodFeudEvent>(declareBloodFeudEvent);
			return Result.Success;
		}
	}
}
