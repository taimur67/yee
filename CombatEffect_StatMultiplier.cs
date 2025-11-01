using System;

namespace LoG
{
	// Token: 0x02000347 RID: 839
	public class CombatEffect_StatMultiplier : CombatAbilityEffect
	{
		// Token: 0x06001000 RID: 4096 RVA: 0x0003F33C File Offset: 0x0003D53C
		protected override GameEvent OnPreBattle(Ability source, CombatAbilityContext context, BattleEvent battleEvent, BattleContext battleContext)
		{
			StatModifier statModifier = new StatModifier(this.PercentagePointMultiplier, source, ModifierTarget.ValuePercentagePointScalar);
			GameEvent result = new BattleAbilityStatModifierEvent(this.CurrentAbilityStage, source, context, statModifier, BattlePhase.Undefined, base.TypeName);
			context.Actor.CombatStats.GetStat(this.Stat).AddModifier(statModifier);
			return result;
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x0003F390 File Offset: 0x0003D590
		public override void DeepClone(out AbilityEffect clone)
		{
			CombatEffect_StatMultiplier combatEffect_StatMultiplier = new CombatEffect_StatMultiplier
			{
				Stat = this.Stat,
				PercentagePointMultiplier = this.PercentagePointMultiplier
			};
			base.DeepCloneCombatAbilityEffectParts(combatEffect_StatMultiplier);
			clone = combatEffect_StatMultiplier;
		}

		// Token: 0x0400076B RID: 1899
		public CombatStatType Stat;

		// Token: 0x0400076C RID: 1900
		public int PercentagePointMultiplier = 100;
	}
}
