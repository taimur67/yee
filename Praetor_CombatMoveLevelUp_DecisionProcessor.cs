using System;

namespace LoG
{
	// Token: 0x020003C6 RID: 966
	public class Praetor_CombatMoveLevelUp_DecisionProcessor : DecisionProcessor<Praetor_CombatMoveLevelUp_DecisionRequest, Praetor_CombatMoveLevelUp_DecisionResponse>
	{
		// Token: 0x060012F5 RID: 4853 RVA: 0x000483FC File Offset: 0x000465FC
		protected override Result Process(Praetor_CombatMoveLevelUp_DecisionResponse response)
		{
			Praetor praetor;
			if (!base._currentTurn.TryFetchGameItem<Praetor>(base.request.Praetor, out praetor))
			{
				return Result.Failure;
			}
			PraetorCombatMoveInstance combatMove;
			if (!praetor.TryGetTechniqueInstance(response.CombatMove, out combatMove))
			{
				return Result.Failure;
			}
			if (!base._currentTurn.DoesPlayerControlItem(this._player.Id, praetor))
			{
				return Result.Failure;
			}
			PraetorCombatMovePowerChangedEvent gameEvent = this.TurnProcessContext.AdjustCombatMovePower(this._player, praetor, combatMove, base.request.AdditionalPower);
			base._currentTurn.AddGameEvent<PraetorCombatMovePowerChangedEvent>(gameEvent);
			return Result.Success;
		}
	}
}
