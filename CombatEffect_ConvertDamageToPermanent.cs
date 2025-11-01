using System;

namespace LoG
{
	// Token: 0x02000334 RID: 820
	public class CombatEffect_ConvertDamageToPermanent : CombatPhaseAbilityEffect
	{
		// Token: 0x06000FC3 RID: 4035 RVA: 0x0003E6D1 File Offset: 0x0003C8D1
		protected override GameEvent OnWinnerDamageModifier(Ability source, CombatAbilityContext context, BattlePhaseResult phaseResult)
		{
			GameEvent result = new BattleAbilityPermanentDamageEvent(this.CurrentAbilityStage, source, context, phaseResult.HPDamage.Value, base.TypeName);
			phaseResult.PermanentDamage = true;
			return result;
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x0003E6F8 File Offset: 0x0003C8F8
		public override void DeepClone(out AbilityEffect clone)
		{
			CombatEffect_ConvertDamageToPermanent combatEffect_ConvertDamageToPermanent = new CombatEffect_ConvertDamageToPermanent();
			base.DeepCloneCombatPhaseAbilityEffectParts(combatEffect_ConvertDamageToPermanent);
			clone = combatEffect_ConvertDamageToPermanent;
		}
	}
}
