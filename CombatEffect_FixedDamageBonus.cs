using System;

namespace LoG
{
	// Token: 0x0200033E RID: 830
	public class CombatEffect_FixedDamageBonus : CombatPhaseAbilityEffect
	{
		// Token: 0x06000FE2 RID: 4066 RVA: 0x0003ED48 File Offset: 0x0003CF48
		protected override GameEvent OnWinnerDamageModifier(Ability source, CombatAbilityContext context, BattlePhaseResult phaseResult)
		{
			StatModifier statModifier = new StatModifier(this.Bonus, source, ModifierTarget.ValueOffset);
			GameEvent result = new BattleAbilityStatModifierEvent(this.CurrentAbilityStage, source, context, statModifier, this.Phase, base.TypeName);
			phaseResult.HPDamage.AddModifier(statModifier);
			return result;
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x0003ED90 File Offset: 0x0003CF90
		public override void DeepClone(out AbilityEffect clone)
		{
			CombatEffect_FixedDamageBonus combatEffect_FixedDamageBonus = new CombatEffect_FixedDamageBonus
			{
				Bonus = this.Bonus
			};
			base.DeepCloneCombatPhaseAbilityEffectParts(combatEffect_FixedDamageBonus);
			clone = combatEffect_FixedDamageBonus;
		}

		// Token: 0x04000762 RID: 1890
		public int Bonus;
	}
}
