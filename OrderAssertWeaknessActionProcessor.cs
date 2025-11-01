using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x020005D4 RID: 1492
	public class OrderAssertWeaknessActionProcessor : DiplomaticActionProcessor<OrderAssertWeakness, DiplomaticAbility_AssertWeakness>
	{
		// Token: 0x06001C0E RID: 7182 RVA: 0x00060F68 File Offset: 0x0005F168
		public override Result IsAvailable()
		{
			Problem problem = base.IsAvailable() as Problem;
			if (problem != null)
			{
				return problem;
			}
			int demandConcededCount = base._currentTurn.GetDiplomaticStatus(this._player.Id, base.request.TargetID).GetDemandConcededCount(base.request.TargetID);
			int concessionsForAssertionOfWeakness = base._rules.ConcessionsForAssertionOfWeakness;
			if (demandConcededCount < concessionsForAssertionOfWeakness)
			{
				return new Result.NotEnoughDemandsAcceptedProblem(base.request.TargetID, base.request.OrderType, demandConcededCount, concessionsForAssertionOfWeakness);
			}
			return Result.Success;
		}

		// Token: 0x06001C0F RID: 7183 RVA: 0x00060FEC File Offset: 0x0005F1EC
		public override Result Enact(OrderAssertWeakness order)
		{
			GameEvent gameEvent;
			Problem problem = GrievanceProcessor.BeginVendettaOrDuel(this.TurnProcessContext, this._player.Id, order.TargetID, order.GrievanceResponse, out gameEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			DiplomaticPairStatus diplomaticStatus = base._currentTurn.GetDiplomaticStatus(this._player.Id, order.TargetID);
			VendettaState vendettaState = diplomaticStatus.DiplomaticState as VendettaState;
			if (vendettaState == null)
			{
				return Result.Failure;
			}
			diplomaticStatus.SetAssertWeaknessCooldown(this.TurnProcessContext);
			Vendetta vendetta = vendettaState.Vendetta;
			AssertWeaknessEvent gameEvent2 = new AssertWeaknessEvent(this._player.Id, order.TargetID, vendetta.Objective, vendetta.PrestigeWager, vendetta.TurnRemaining);
			base._currentTurn.AddGameEvent<AssertWeaknessEvent>(gameEvent2);
			return Result.Success;
		}
	}
}
