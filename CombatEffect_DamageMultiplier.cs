using System;

namespace LoG
{
	// Token: 0x02000339 RID: 825
	public class CombatEffect_DamageMultiplier : CombatPhaseAbilityEffect
	{
		// Token: 0x06000FD2 RID: 4050 RVA: 0x0003EA00 File Offset: 0x0003CC00
		protected override GameEvent OnWinnerDamageModifier(Ability source, CombatAbilityContext context, BattlePhaseResult phaseResult)
		{
			StatModifier statModifier = new StatModifier(this.PercentagePointMultiplier, source, ModifierTarget.ValuePercentagePointScalar);
			GameEvent result = new BattleAbilityStatModifierEvent(this.CurrentAbilityStage, source, context, statModifier, this.Phase, "DamageAmplification");
			phaseResult.HPDamage.AddModifier(statModifier);
			return result;
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x0003EA48 File Offset: 0x0003CC48
		public override void DeepClone(out AbilityEffect clone)
		{
			CombatEffect_DamageMultiplier combatEffect_DamageMultiplier = new CombatEffect_DamageMultiplier
			{
				PercentagePointMultiplier = this.PercentagePointMultiplier
			};
			base.DeepCloneCombatPhaseAbilityEffectParts(combatEffect_DamageMultiplier);
			clone = combatEffect_DamageMultiplier;
		}

		// Token: 0x0400075F RID: 1887
		public int PercentagePointMultiplier;
	}
}
