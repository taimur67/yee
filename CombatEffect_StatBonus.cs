using System;

namespace LoG
{
	// Token: 0x02000346 RID: 838
	public class CombatEffect_StatBonus : CombatAbilityEffect
	{
		// Token: 0x06000FFC RID: 4092 RVA: 0x0003F290 File Offset: 0x0003D490
		public CombatEffect_StatBonus()
		{
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x0003F298 File Offset: 0x0003D498
		public CombatEffect_StatBonus(CombatStatType stat, int bonus)
		{
			this.Stat = stat;
			this.Bonus = bonus;
		}

		// Token: 0x06000FFE RID: 4094 RVA: 0x0003F2B0 File Offset: 0x0003D4B0
		protected override GameEvent OnPreBattle(Ability source, CombatAbilityContext context, BattleEvent battleEvent, BattleContext battleContext)
		{
			StatModifier statModifier = new StatModifier(this.Bonus, source, ModifierTarget.ValueOffset);
			GameEvent result = new BattleAbilityStatModifierEvent(this.CurrentAbilityStage, source, context, statModifier, BattlePhase.Undefined, base.TypeName);
			context.Actor.CombatStats.GetStat(this.Stat).AddModifier(statModifier);
			return result;
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x0003F304 File Offset: 0x0003D504
		public override void DeepClone(out AbilityEffect clone)
		{
			CombatEffect_StatBonus combatEffect_StatBonus = new CombatEffect_StatBonus
			{
				Stat = this.Stat,
				Bonus = this.Bonus
			};
			base.DeepCloneCombatAbilityEffectParts(combatEffect_StatBonus);
			clone = combatEffect_StatBonus;
		}

		// Token: 0x04000769 RID: 1897
		public CombatStatType Stat;

		// Token: 0x0400076A RID: 1898
		public int Bonus;
	}
}
