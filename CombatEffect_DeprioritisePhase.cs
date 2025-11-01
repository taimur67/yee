using System;

namespace LoG
{
	// Token: 0x0200033B RID: 827
	public class CombatEffect_DeprioritisePhase : CombatAbilityEffect
	{
		// Token: 0x06000FD9 RID: 4057 RVA: 0x0003EAF5 File Offset: 0x0003CCF5
		protected override GameEvent OnPreBattle(Ability source, CombatAbilityContext context, BattleEvent battleEvent, BattleContext battleContext)
		{
			battleContext.PhaseModifications.Add(new BattlePhaseModification(source, this.Phase, BattlePhaseModificationType.Last, context));
			return null;
		}

		// Token: 0x06000FDA RID: 4058 RVA: 0x0003EB14 File Offset: 0x0003CD14
		public override void DeepClone(out AbilityEffect clone)
		{
			CombatEffect_DeprioritisePhase combatEffect_DeprioritisePhase = new CombatEffect_DeprioritisePhase
			{
				Phase = this.Phase
			};
			base.DeepCloneCombatAbilityEffectParts(combatEffect_DeprioritisePhase);
			clone = combatEffect_DeprioritisePhase;
		}

		// Token: 0x04000761 RID: 1889
		public BattlePhase Phase;
	}
}
