using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020001EA RID: 490
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class BattleAbilityStatModifierEvent : BattleAbilityEvent
	{
		// Token: 0x0600098E RID: 2446 RVA: 0x0002CC2D File Offset: 0x0002AE2D
		[JsonConstructor]
		private BattleAbilityStatModifierEvent()
		{
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x0002CC35 File Offset: 0x0002AE35
		public BattleAbilityStatModifierEvent(CombatAbilityStage combatAbilityStage, Ability ability, CombatAbilityContext combatAbilityContext, StatModifier statModifier, BattlePhase phase, string effectId) : base(combatAbilityStage, ability, combatAbilityContext, effectId)
		{
			this.Phase = phase;
			this.StatModifier = statModifier;
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x0002CC52 File Offset: 0x0002AE52
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0} changed stat with value {1} and method {2}", this.AbilityContext.SourceId, this.StatModifier.Value, this.StatModifier.TargetType);
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x0002CC8C File Offset: 0x0002AE8C
		public override void DeepClone(out GameEvent clone)
		{
			BattleAbilityStatModifierEvent battleAbilityStatModifierEvent = new BattleAbilityStatModifierEvent
			{
				StatModifier = this.StatModifier.DeepClone<StatModifier>(),
				Phase = this.Phase
			};
			base.DeepCloneBattleAbilityEventParts(battleAbilityStatModifierEvent);
			clone = battleAbilityStatModifierEvent;
		}

		// Token: 0x04000496 RID: 1174
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public StatModifier StatModifier;

		// Token: 0x04000497 RID: 1175
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public BattlePhase Phase;
	}
}
