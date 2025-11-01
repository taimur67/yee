using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x020005E7 RID: 1511
	public class OrderRequestToBeBloodLordOfTargetProcessor : DiplomaticActionProcessor<OrderRequestToBeBloodLordOfTarget, DiplomaticAbility_RequestToBeBloodLord>
	{
		// Token: 0x06001C4F RID: 7247 RVA: 0x0006187C File Offset: 0x0005FA7C
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

		// Token: 0x06001C50 RID: 7248 RVA: 0x000618C4 File Offset: 0x0005FAC4
		private Result ValidatePrestige()
		{
			PlayerState conclaveFavourite = base._currentTurn.GetConclaveFavourite();
			PlayerState playerState = base._currentTurn.FindPlayerState(base.request.TargetID, null);
			DiplomaticStateValue type = base._currentTurn.GetDiplomaticStatus(this._player.Id, base.request.TargetID).DiplomaticState.Type;
			int spendablePrestige = playerState.SpendablePrestige;
			int num = (int)MathF.Ceiling((float)conclaveFavourite.SpendablePrestige / 2f);
			if (spendablePrestige > num)
			{
				return new Result.TooMuchPrestigeProblem(base.request.TargetID, base.request.OrderType, type, spendablePrestige, num, this._player.Id, base.request.TargetID, this._player.Id);
			}
			return Result.Success;
		}

		// Token: 0x06001C51 RID: 7249 RVA: 0x00061984 File Offset: 0x0005FB84
		private Result ValidateInternal()
		{
			DiplomaticStateValue type = base._currentTurn.GetDiplomaticStatus(this._player.Id, base.request.TargetID).DiplomaticState.Type;
			if (base._currentTurn.TurnPhase != TurnPhase.None)
			{
				return new Result.InvalidGameStateForDiplomacyProblem(base.request.TargetID, base.request.OrderType, base._currentTurn.TurnPhase);
			}
			if (IEnumerableExtensions.Any<PendingDiplomacy_RequestToBeLord>(this.TurnProcessContext.Diplomacy.DiplomaticStatesOfType<PendingDiplomacy_RequestToBeLord>(this._player.Id)))
			{
				return new Result.QueuedDiplomacyActionOfSameTypeProblem(this._player.Id, base.request.OrderType, base.request.TargetID);
			}
			if (IEnumerableExtensions.Any<PendingDiplomacy_RequestToBeLord>(this.TurnProcessContext.Diplomacy.DiplomaticStatesOfType<PendingDiplomacy_RequestToBeLord>(base.request.TargetID)))
			{
				return new Result.DiplomacyProblem(base.request.TargetID, base.request.OrderType);
			}
			int bloodLordPlayerId;
			if (this.TurnProcessContext.Diplomacy.IsVassalOfAny(base.request.TargetID, out bloodLordPlayerId))
			{
				return new Result.CannotTargetVassalProblem(base.request.TargetID, base.request.OrderType, bloodLordPlayerId, base.request.TargetID, this._player.Id);
			}
			int bloodVassalPlayerId;
			if (this.TurnProcessContext.Diplomacy.IsBloodLordOfAny(this._player.Id, out bloodVassalPlayerId))
			{
				return new Result.CannotWhileLiegeProblem(base.request.TargetID, base.request.OrderType, type, this._player.Id, bloodVassalPlayerId);
			}
			return Result.Success;
		}

		// Token: 0x06001C52 RID: 7250 RVA: 0x00061B18 File Offset: 0x0005FD18
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

		// Token: 0x06001C53 RID: 7251 RVA: 0x00061B4C File Offset: 0x0005FD4C
		public override Result Enact(OrderRequestToBeBloodLordOfTarget order)
		{
			Cost cost = this.CalculateCost();
			Problem problem = this.ValidatePrestige() as Problem;
			if (problem != null)
			{
				return problem;
			}
			base._currentTurn.GetDiplomaticStatus(this._player.Id, order.TargetID).SetDiplomacyPending(this.TurnProcessContext, new PendingDiplomacy_RequestToBeLord(this._player.Id)
			{
				Wager = cost[ResourceTypes.Prestige]
			});
			return Result.Success;
		}
	}
}
