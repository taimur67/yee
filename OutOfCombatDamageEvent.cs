using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200027A RID: 634
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class OutOfCombatDamageEvent : GameEvent
	{
		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000C68 RID: 3176 RVA: 0x00031512 File Offset: 0x0002F712
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x00031518 File Offset: 0x0002F718
		public override void DeepClone(out GameEvent clone)
		{
			OutOfCombatDamageEvent outOfCombatDamageEvent = new OutOfCombatDamageEvent
			{
				DamageType = this.DamageType
			};
			base.DeepCloneGameEventParts<OutOfCombatDamageEvent>(outOfCombatDamageEvent);
			clone = outOfCombatDamageEvent;
		}

		// Token: 0x04000564 RID: 1380
		[JsonProperty]
		public OutOfCombatDamageType DamageType;
	}
}
