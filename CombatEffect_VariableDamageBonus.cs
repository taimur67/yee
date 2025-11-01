using System;

namespace LoG
{
	// Token: 0x02000349 RID: 841
	public class CombatEffect_VariableDamageBonus : CombatPhaseAbilityEffect
	{
		// Token: 0x06001006 RID: 4102 RVA: 0x0003F4D8 File Offset: 0x0003D6D8
		protected override GameEvent OnWinnerDamageModifier(Ability source, CombatAbilityContext context, BattlePhaseResult phaseResult)
		{
			StatModifier statModifier = new StatModifier(context.Turn.Random.Next(this.MinBonus, this.MaxBonus), source, ModifierTarget.ValueOffset);
			GameEvent result = new BattleAbilityStatModifierEvent(this.CurrentAbilityStage, source, context, statModifier, this.Phase, base.TypeName);
			phaseResult.HPDamage.AddModifier(statModifier);
			return result;
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x0003F534 File Offset: 0x0003D734
		public override void DeepClone(out AbilityEffect clone)
		{
			CombatEffect_VariableDamageBonus combatEffect_VariableDamageBonus = new CombatEffect_VariableDamageBonus
			{
				MinBonus = this.MinBonus,
				MaxBonus = this.MaxBonus
			};
			base.DeepCloneCombatPhaseAbilityEffectParts(combatEffect_VariableDamageBonus);
			clone = combatEffect_VariableDamageBonus;
		}

		// Token: 0x0400076D RID: 1901
		public int MinBonus;

		// Token: 0x0400076E RID: 1902
		public int MaxBonus;
	}
}
