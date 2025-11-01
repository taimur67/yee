using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000332 RID: 818
	public class CombatEffect_BlockAbilities : CombatAbilityEffect
	{
		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000FBB RID: 4027 RVA: 0x0003E58D File Offset: 0x0003C78D
		[JsonIgnore]
		public override bool CanBeCancelled
		{
			get
			{
				return !this.ApplyToSelf;
			}
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x0003E598 File Offset: 0x0003C798
		protected override GameEvent OnBlockAbilities(Ability source, CombatAbilityContext context, BattleEvent battleEvent)
		{
			(this.ApplyToSelf ? context.Actor : context.Opponent).CanUseCombatAbilities.AddModifier(new BooleanModifier(false, source));
			return new BattleAbilityEvent(this.CurrentAbilityStage, source, context, base.TypeName);
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x0003E5E4 File Offset: 0x0003C7E4
		public override void DeepClone(out AbilityEffect clone)
		{
			CombatEffect_BlockAbilities combatEffect_BlockAbilities = new CombatEffect_BlockAbilities
			{
				ApplyToSelf = this.ApplyToSelf
			};
			base.DeepCloneCombatAbilityEffectParts(combatEffect_BlockAbilities);
			clone = combatEffect_BlockAbilities;
		}

		// Token: 0x04000757 RID: 1879
		public bool ApplyToSelf;
	}
}
