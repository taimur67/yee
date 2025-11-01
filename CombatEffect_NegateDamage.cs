using System;

namespace LoG
{
	// Token: 0x02000340 RID: 832
	public class CombatEffect_NegateDamage : CombatPhaseAbilityEffect
	{
		// Token: 0x06000FE8 RID: 4072 RVA: 0x0003EE50 File Offset: 0x0003D050
		protected override GameEvent OnLoserDamageModifier(Ability source, CombatAbilityContext context, BattlePhaseResult phaseResult)
		{
			StatModifier statModifier = new StatModifier(0, source, ModifierTarget.ValueScalar);
			GameEvent result = new BattleAbilityStatModifierEvent(this.CurrentAbilityStage, source, context, statModifier, this.Phase, base.TypeName);
			phaseResult.HPDamage.AddModifier(statModifier);
			return result;
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x0003EE94 File Offset: 0x0003D094
		public override void DeepClone(out AbilityEffect clone)
		{
			CombatEffect_NegateDamage combatEffect_NegateDamage = new CombatEffect_NegateDamage();
			base.DeepCloneCombatPhaseAbilityEffectParts(combatEffect_NegateDamage);
			clone = combatEffect_NegateDamage;
		}
	}
}
