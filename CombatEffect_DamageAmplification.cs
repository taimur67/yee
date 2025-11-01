using System;

namespace LoG
{
	// Token: 0x02000337 RID: 823
	public class CombatEffect_DamageAmplification : CombatPhaseAbilityEffect
	{
		// Token: 0x06000FCC RID: 4044 RVA: 0x0003E8BC File Offset: 0x0003CABC
		protected override GameEvent OnWinnerDamageModifier(Ability source, CombatAbilityContext context, BattlePhaseResult phaseResult)
		{
			float num = (float)context.Actor.CombatStats.GetStat(this.AddedStat).Value * this.PercentageAdded;
			if (num > 0f)
			{
				StatModifier statModifier = new StatModifier((int)num, source, ModifierTarget.ValueOffset);
				GameEvent result = new BattleAbilityStatModifierEvent(this.CurrentAbilityStage, source, context, statModifier, this.Phase, base.TypeName);
				phaseResult.HPDamage.AddModifier(statModifier);
				return result;
			}
			return null;
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x0003E92C File Offset: 0x0003CB2C
		public override void DeepClone(out AbilityEffect clone)
		{
			CombatEffect_DamageAmplification combatEffect_DamageAmplification = new CombatEffect_DamageAmplification
			{
				AddedStat = this.AddedStat,
				PercentageAdded = this.PercentageAdded
			};
			base.DeepCloneCombatPhaseAbilityEffectParts(combatEffect_DamageAmplification);
			clone = combatEffect_DamageAmplification;
		}

		// Token: 0x0400075B RID: 1883
		public DamageType AddedStat;

		// Token: 0x0400075C RID: 1884
		public float PercentageAdded;
	}
}
