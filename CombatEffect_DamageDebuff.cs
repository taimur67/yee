using System;

namespace LoG
{
	// Token: 0x02000338 RID: 824
	public class CombatEffect_DamageDebuff : CombatPhaseAbilityEffect
	{
		// Token: 0x06000FCF RID: 4047 RVA: 0x0003E96C File Offset: 0x0003CB6C
		protected override GameEvent OnWinnerDamageDealt(Ability source, CombatAbilityContext context, BattlePhaseResult phaseResult)
		{
			StatModifier statModifier = new StatModifier(-this.Debuff, source, ModifierTarget.ValueOffset);
			GameEvent result = new BattleAbilityStatModifierEvent(this.CurrentAbilityStage, source, context, statModifier, this.Phase, base.TypeName);
			context.Opponent.GetStat(this.DebuffType).AddModifier(statModifier);
			return result;
		}

		// Token: 0x06000FD0 RID: 4048 RVA: 0x0003E9C0 File Offset: 0x0003CBC0
		public override void DeepClone(out AbilityEffect clone)
		{
			CombatEffect_DamageDebuff combatEffect_DamageDebuff = new CombatEffect_DamageDebuff
			{
				Debuff = this.Debuff,
				DebuffType = this.DebuffType
			};
			base.DeepCloneCombatPhaseAbilityEffectParts(combatEffect_DamageDebuff);
			clone = combatEffect_DamageDebuff;
		}

		// Token: 0x0400075D RID: 1885
		public int Debuff;

		// Token: 0x0400075E RID: 1886
		public CombatStatType DebuffType;
	}
}
