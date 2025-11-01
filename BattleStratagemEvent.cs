using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020001E9 RID: 489
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class BattleStratagemEvent : BattleAbilityEvent
	{
		// Token: 0x0600098B RID: 2443 RVA: 0x0002CBCA File Offset: 0x0002ADCA
		[JsonConstructor]
		private BattleStratagemEvent()
		{
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x0002CBD2 File Offset: 0x0002ADD2
		public BattleStratagemEvent(CombatAbilityStage combatAbilityStage, Ability ability, CombatAbilityContext combatAbilityContext, string effectId) : base(combatAbilityStage, ability, combatAbilityContext, effectId)
		{
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x0002CBE0 File Offset: 0x0002ADE0
		public override void DeepClone(out GameEvent clone)
		{
			BattleStratagemEvent battleStratagemEvent = new BattleStratagemEvent();
			battleStratagemEvent.BattlePhase = this.BattlePhase;
			battleStratagemEvent.PrimaryValueChange = this.PrimaryValueChange;
			battleStratagemEvent.MinValueChange = this.MinValueChange;
			battleStratagemEvent.MaxValueChange = this.MaxValueChange;
			base.DeepCloneBattleAbilityEventParts(battleStratagemEvent);
			clone = battleStratagemEvent;
		}

		// Token: 0x04000492 RID: 1170
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public BattlePhase BattlePhase;

		// Token: 0x04000493 RID: 1171
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public int PrimaryValueChange;

		// Token: 0x04000494 RID: 1172
		[BindableValue("min_value", BindingOption.None)]
		[JsonProperty]
		public int MinValueChange;

		// Token: 0x04000495 RID: 1173
		[BindableValue("max_value", BindingOption.None)]
		[JsonProperty]
		public int MaxValueChange;
	}
}
