using System;

namespace LoG
{
	// Token: 0x02000341 RID: 833
	[Serializable]
	public class CombatEffect_PreventCapture : CombatPhaseAbilityEffect
	{
		// Token: 0x06000FEB RID: 4075 RVA: 0x0003EEB9 File Offset: 0x0003D0B9
		protected override GameEvent OnLoserPreCapture(Ability source, CombatAbilityContext context, BattlePhaseResult phaseResult)
		{
			context.Actor.CanBeCaptured.AddModifier(new BooleanModifier(false, new AbilityContext(source.SourceId)));
			return null;
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x0003EEE0 File Offset: 0x0003D0E0
		public override void DeepClone(out AbilityEffect clone)
		{
			CombatEffect_PreventCapture combatEffect_PreventCapture = new CombatEffect_PreventCapture();
			base.DeepCloneCombatPhaseAbilityEffectParts(combatEffect_PreventCapture);
			clone = combatEffect_PreventCapture;
		}
	}
}
