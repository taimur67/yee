using System;

namespace LoG
{
	// Token: 0x02000335 RID: 821
	public class CombatEffect_CounterAttack : CombatPhaseAbilityEffect
	{
		// Token: 0x06000FC6 RID: 4038 RVA: 0x0003E720 File Offset: 0x0003C920
		protected override GameEvent OnLoserDamageTaken(Ability source, CombatAbilityContext context, BattlePhaseResult phaseResult)
		{
			StatModifier statModifier = new StatModifier(context.Actor.GetStat(this.DamageType).Value, source, ModifierTarget.ValueOffset);
			GameEvent result = new BattleAbilityStatModifierEvent(this.CurrentAbilityStage, source, context, statModifier, this.Phase, base.TypeName);
			phaseResult.CounterDamage.AddModifier(statModifier);
			return result;
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x0003E778 File Offset: 0x0003C978
		public override void DeepClone(out AbilityEffect clone)
		{
			CombatEffect_CounterAttack combatEffect_CounterAttack = new CombatEffect_CounterAttack
			{
				DamageType = this.DamageType
			};
			base.DeepCloneCombatPhaseAbilityEffectParts(combatEffect_CounterAttack);
			clone = combatEffect_CounterAttack;
		}

		// Token: 0x04000759 RID: 1881
		public CombatStatType DamageType;
	}
}
