using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x020005E9 RID: 1513
	public class OrderRequestToBeVassalizedByTargetProcessor : DiplomaticActionProcessor<OrderRequestToBeVassalizedByTarget, DiplomaticAbility_RequestToBeBloodVassal>
	{
		// Token: 0x06001C59 RID: 7257 RVA: 0x00061BE4 File Offset: 0x0005FDE4
		public override Result IsAvailable()
		{
			Problem problem = base.IsAvailable() as Problem;
			if (problem != null)
			{
				return problem;
			}
			Problem problem2 = this.ValidateInternal() as Problem;
			if (problem2 != null)
			{
				return problem2;
			}
			Problem problem3 = this.ValidatePrestige() as Problem;
			if (problem3 != null)
			{
				return problem3;
			}
			return Result.Success;
		}

		// Token: 0x06001C5A RID: 7258 RVA: 0x00061C2C File Offset: 0x0005FE2C
		private Result ValidatePrestige()
		{
			PlayerState conclaveFavourite = base._currentTurn.GetConclaveFavourite();
			DiplomaticStateValue type = base._currentTurn.GetDiplomaticStatus(this._player.Id, base.request.TargetID).DiplomaticState.Type;
			int spendablePrestige = this._player.SpendablePrestige;
			int num = (int)MathF.Ceiling((float)conclaveFavourite.SpendablePrestige / 2f);
			if (spendablePrestige > num)
			{
				return new Result.TooMuchPrestigeProblem(base.request.TargetID, base.request.OrderType, type, spendablePrestige, num, base.request.TargetID, this._player.Id, this._player.Id);
			}
			return Result.Success;
		}

		// Token: 0x06001C5B RID: 7259 RVA: 0x00061CD8 File Offset: 0x0005FED8
		private Result ValidateInternal()
		{
			DiplomaticStateValue type = base._currentTurn.GetDiplomaticStatus(this._player.Id, base.request.TargetID).DiplomaticState.Type;
			if (base._currentTurn.TurnPhase != TurnPhase.None)
			{
				return new Result.InvalidGameStateForDiplomacyProblem(base.request.TargetID, base.request.OrderType, base._currentTurn.TurnPhase);
			}
			int bloodLordPlayerId;
			if (this.TurnProcessContext.Diplomacy.IsVassalOfAny(this._player.Id, out bloodLordPlayerId))
			{
				return new Result.CannotTargetVassalProblem(base.request.TargetID, base.request.OrderType, bloodLordPlayerId, this._player.Id, this._player.Id);
			}
			int bloodVassalPlayerId;
			if (this.TurnProcessContext.Diplomacy.IsBloodLordOfAny(base.request.TargetID, out bloodVassalPlayerId))
			{
				return new Result.CannotTargetLiegeProblem(base.request.TargetID, base.request.OrderType, type, base.request.TargetID, bloodVassalPlayerId, this._player.Id);
			}
			if (IEnumerableExtensions.Any<PendingDiplomacy_RequestToBeVassal>(this.TurnProcessContext.Diplomacy.DiplomaticStatesOfType<PendingDiplomacy_RequestToBeVassal>(this._player.Id)))
			{
				return new Result.DiplomacyProblem(base.request.TargetID, base.request.OrderType);
			}
			return Result.Success;
		}

		// Token: 0x06001C5C RID: 7260 RVA: 0x00061E30 File Offset: 0x00060030
		public override Result Validate()
		{
			Problem problem = base.IsAvailable() as Problem;
			if (problem != null)
			{
				return problem;
			}
			Problem problem2 = this.ValidateInternal() as Problem;
			if (problem2 != null)
			{
				return problem2;
			}
			return Result.Success;
		}

		// Token: 0x06001C5D RID: 7261 RVA: 0x00061E64 File Offset: 0x00060064
		public override Result Enact(OrderRequestToBeVassalizedByTarget order)
		{
			Cost cost = this.CalculateCost();
			Problem problem = this.ValidatePrestige() as Problem;
			if (problem != null)
			{
				return problem;
			}
			base._currentTurn.GetDiplomaticStatus(this._player.Id, order.TargetID).SetDiplomacyPending(this.TurnProcessContext, new PendingDiplomacy_RequestToBeVassal(this._player.Id)
			{
				Wager = cost[ResourceTypes.Prestige]
			});
			return Result.Success;
		}
	}
}
