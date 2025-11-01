using System;

namespace LoG
{
	// Token: 0x02000330 RID: 816
	public class CombatEffect_AddPhase : CombatAbilityEffect
	{
		// Token: 0x06000FB3 RID: 4019 RVA: 0x0003E329 File Offset: 0x0003C529
		public CombatEffect_AddPhase()
		{
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x0003E331 File Offset: 0x0003C531
		public CombatEffect_AddPhase(BattlePhase newPhase)
		{
			this.Phase = newPhase;
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x0003E340 File Offset: 0x0003C540
		protected override GameEvent OnPreBattle(Ability source, CombatAbilityContext context, BattleEvent battleEvent, BattleContext battleContext)
		{
			battleContext.PhaseModifications.Add(new BattlePhaseModification(source, this.Phase, BattlePhaseModificationType.Twice, context));
			return null;
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x0003E360 File Offset: 0x0003C560
		public override void DeepClone(out AbilityEffect clone)
		{
			CombatEffect_AddPhase combatEffect_AddPhase = new CombatEffect_AddPhase
			{
				Phase = this.Phase
			};
			base.DeepCloneCombatAbilityEffectParts(combatEffect_AddPhase);
			clone = combatEffect_AddPhase;
		}

		// Token: 0x04000755 RID: 1877
		public BattlePhase Phase;
	}
}
