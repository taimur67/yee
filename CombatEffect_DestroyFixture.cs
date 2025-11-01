using System;

namespace LoG
{
	// Token: 0x0200033C RID: 828
	public class CombatEffect_DestroyFixture : CombatAbilityEffect
	{
		// Token: 0x06000FDC RID: 4060 RVA: 0x0003EB48 File Offset: 0x0003CD48
		protected override GameEvent OnPostBattle(Ability source, CombatAbilityContext context, BattleEvent battleEvent)
		{
			GamePiece opponent = context.Opponent;
			if (!battleEvent.BattleResult.DidWin(context.Actor.Id))
			{
				return null;
			}
			if (!opponent.IsFixture())
			{
				return null;
			}
			BattleAbilityEvent battleAbilityEvent = new BattleAbilityEvent(this.CurrentAbilityStage, source, context, base.TypeName);
			battleAbilityEvent.AddChildEvent<LegionKilledEvent>(context.TurnContext.KillGamePiece(context.Opponent, context.Actor.ControllingPlayerId));
			return battleAbilityEvent;
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x0003EBB8 File Offset: 0x0003CDB8
		public override void DeepClone(out AbilityEffect clone)
		{
			CombatEffect_DestroyFixture combatEffect_DestroyFixture = new CombatEffect_DestroyFixture();
			base.DeepCloneCombatAbilityEffectParts(combatEffect_DestroyFixture);
			clone = combatEffect_DestroyFixture;
		}
	}
}
