using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020001ED RID: 493
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class BattleAbilityPermanentDamageEvent : BattleAbilityEvent
	{
		// Token: 0x0600099A RID: 2458 RVA: 0x0002CE21 File Offset: 0x0002B021
		[JsonConstructor]
		public BattleAbilityPermanentDamageEvent()
		{
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x0002CE29 File Offset: 0x0002B029
		public BattleAbilityPermanentDamageEvent(CombatAbilityStage combatAbilityStage, Ability ability, CombatAbilityContext combatAbilityContext, int permanentDamageAmt, string effectId) : base(combatAbilityStage, ability, combatAbilityContext, effectId)
		{
			this.PermanentDamageAmt = permanentDamageAmt;
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x0002CE40 File Offset: 0x0002B040
		public override void DeepClone(out GameEvent clone)
		{
			BattleAbilityPermanentDamageEvent battleAbilityPermanentDamageEvent = new BattleAbilityPermanentDamageEvent
			{
				PermanentDamageAmt = this.PermanentDamageAmt
			};
			base.DeepCloneBattleAbilityEventParts(battleAbilityPermanentDamageEvent);
			clone = battleAbilityPermanentDamageEvent;
		}

		// Token: 0x0400049B RID: 1179
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public int PermanentDamageAmt;
	}
}
