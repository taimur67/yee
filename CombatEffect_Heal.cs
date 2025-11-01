using System;

namespace LoG
{
	// Token: 0x0200033F RID: 831
	public class CombatEffect_Heal : CombatPhaseAbilityEffect
	{
		// Token: 0x06000FE5 RID: 4069 RVA: 0x0003EDC4 File Offset: 0x0003CFC4
		protected override GameEvent OnWinnerDamageDealt(Ability source, CombatAbilityContext context, BattlePhaseResult phaseResult)
		{
			int num = (int)((float)phaseResult.HPDamage * this.Effectiveness);
			if (num > 0)
			{
				BattleAbilityHealEvent battleAbilityHealEvent = new BattleAbilityHealEvent(this.CurrentAbilityStage, source, context, num, base.TypeName);
				battleAbilityHealEvent.AddChildEvent<HealGamePieceEvent>(context.Actor.Heal(num));
				return battleAbilityHealEvent;
			}
			return null;
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x0003EE14 File Offset: 0x0003D014
		public override void DeepClone(out AbilityEffect clone)
		{
			CombatEffect_Heal combatEffect_Heal = new CombatEffect_Heal
			{
				Effectiveness = this.Effectiveness
			};
			base.DeepCloneCombatPhaseAbilityEffectParts(combatEffect_Heal);
			clone = combatEffect_Heal;
		}

		// Token: 0x04000763 RID: 1891
		public float Effectiveness = 1f;
	}
}
