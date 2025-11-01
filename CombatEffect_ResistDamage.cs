using System;

namespace LoG
{
	// Token: 0x02000344 RID: 836
	public class CombatEffect_ResistDamage : CombatPhaseAbilityEffect
	{
		// Token: 0x06000FF4 RID: 4084 RVA: 0x0003F058 File Offset: 0x0003D258
		protected override GameEvent OnLoserDamageModifier(Ability source, CombatAbilityContext context, BattlePhaseResult phaseResult)
		{
			StatModifier statModifier = new StatModifier(-this.ResistAmount, source, ModifierTarget.ValueOffset);
			GameEvent result = new BattleAbilityStatModifierEvent(this.CurrentAbilityStage, source, context, statModifier, this.Phase, base.TypeName);
			phaseResult.HPDamage.AddModifier(statModifier);
			return result;
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x0003F0A0 File Offset: 0x0003D2A0
		public override void DeepClone(out AbilityEffect clone)
		{
			CombatEffect_ResistDamage combatEffect_ResistDamage = new CombatEffect_ResistDamage
			{
				ResistAmount = this.ResistAmount
			};
			base.DeepCloneCombatPhaseAbilityEffectParts(combatEffect_ResistDamage);
			clone = combatEffect_ResistDamage;
		}

		// Token: 0x04000766 RID: 1894
		public int ResistAmount;
	}
}
