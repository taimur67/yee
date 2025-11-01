using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000342 RID: 834
	[Serializable]
	public class CombatEffect_PrioritisePhase : CombatAbilityEffect
	{
		// Token: 0x06000FEE RID: 4078 RVA: 0x0003EF05 File Offset: 0x0003D105
		protected override GameEvent OnPreBattle(Ability source, CombatAbilityContext context, BattleEvent battleEvent, BattleContext battleContext)
		{
			battleContext.PhaseModifications.Add(new BattlePhaseModification(source, this.Phase, BattlePhaseModificationType.First, context));
			return null;
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x0003EF24 File Offset: 0x0003D124
		public override void DeepClone(out AbilityEffect clone)
		{
			CombatEffect_PrioritisePhase combatEffect_PrioritisePhase = new CombatEffect_PrioritisePhase
			{
				Phase = this.Phase
			};
			base.DeepCloneCombatAbilityEffectParts(combatEffect_PrioritisePhase);
			clone = combatEffect_PrioritisePhase;
		}

		// Token: 0x04000764 RID: 1892
		[JsonProperty]
		public BattlePhase Phase;
	}
}
