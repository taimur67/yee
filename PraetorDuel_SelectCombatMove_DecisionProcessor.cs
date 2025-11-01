using System;
using Core.StaticData;

namespace LoG
{
	// Token: 0x020003C3 RID: 963
	public class PraetorDuel_SelectCombatMove_DecisionProcessor : DecisionProcessor<PraetorDuel_SelectCombatMove_DecisionRequest, PraetorDuel_SelectCombatMove_DecisionResponse>
	{
		// Token: 0x060012E8 RID: 4840 RVA: 0x000481AC File Offset: 0x000463AC
		protected override PraetorDuel_SelectCombatMove_DecisionResponse GenerateTypedFallbackResponse()
		{
			Praetor praetor = base._currentTurn.TryFetchGameItem<Praetor>(base.request.Praetor);
			if (praetor == null)
			{
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					logger.Warn("Duel praetor could not be found - something has gone quite wrong!");
				}
				return new PraetorDuel_SelectCombatMove_DecisionResponse();
			}
			int combatMoveSlot = base._random.Next(0, praetor.CombatMoves.Count);
			return new PraetorDuel_SelectCombatMove_DecisionResponse
			{
				CombatMoveSlot = combatMoveSlot,
				Bribe = Payment.Empty
			};
		}

		// Token: 0x060012E9 RID: 4841 RVA: 0x0004821D File Offset: 0x0004641D
		protected override Result Validate(PraetorDuel_SelectCombatMove_DecisionResponse response)
		{
			if (response.CombatMoveSlot == -1)
			{
				return Result.SelectedTooFewOptions;
			}
			return Result.Success;
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x00048233 File Offset: 0x00046433
		protected override Result Preview(PraetorDuel_SelectCombatMove_DecisionResponse response)
		{
			return base._currentTurn.AcceptPayment(this._player.Id, response.Bribe);
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x00048254 File Offset: 0x00046454
		protected override Result Process(PraetorDuel_SelectCombatMove_DecisionResponse response)
		{
			PraetorDuelData praetorDuelData;
			if (!this.TurnProcessContext.TryGetPraetorDuel(base.request.Contestants, out praetorDuelData))
			{
				return Result.Failure;
			}
			PraetorDuelParticipantData praetorDuelParticipantData;
			if (!praetorDuelData.TryGetAssociated(this._player.Id, out praetorDuelParticipantData))
			{
				return Result.Failure;
			}
			if (response.CombatMoveSlot == -1 && response.CombatMove.IsEmpty())
			{
				return Result.Failure;
			}
			praetorDuelParticipantData.CombatMoveSlot = response.CombatMoveSlot;
			praetorDuelParticipantData.CombatMove = response.CombatMove;
			if (!response.Bribe.IsEmpty && base._currentTurn.AcceptPayment(this._player.Id, response.Bribe))
			{
				praetorDuelParticipantData.Bribe = response.Bribe;
			}
			return Result.Success;
		}
	}
}
