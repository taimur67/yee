using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020001EC RID: 492
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class BattleAbilityHealEvent : BattleAbilityEvent
	{
		// Token: 0x06000997 RID: 2455 RVA: 0x0002CDD9 File Offset: 0x0002AFD9
		[JsonConstructor]
		public BattleAbilityHealEvent()
		{
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x0002CDE1 File Offset: 0x0002AFE1
		public BattleAbilityHealEvent(CombatAbilityStage combatAbilityStage, Ability ability, CombatAbilityContext combatAbilityContext, int healAmt, string effectId) : base(combatAbilityStage, ability, combatAbilityContext, effectId)
		{
			this.HealAmt = healAmt;
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x0002CDF8 File Offset: 0x0002AFF8
		public override void DeepClone(out GameEvent clone)
		{
			BattleAbilityHealEvent battleAbilityHealEvent = new BattleAbilityHealEvent
			{
				HealAmt = this.HealAmt
			};
			base.DeepCloneBattleAbilityEventParts(battleAbilityHealEvent);
			clone = battleAbilityHealEvent;
		}

		// Token: 0x0400049A RID: 1178
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public int HealAmt;
	}
}
