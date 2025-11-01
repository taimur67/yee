using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003B3 RID: 947
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PraetorDuelDamageEvent : GameEvent
	{
		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06001284 RID: 4740 RVA: 0x00046A26 File Offset: 0x00044C26
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x00046A29 File Offset: 0x00044C29
		[JsonConstructor]
		protected PraetorDuelDamageEvent()
		{
		}

		// Token: 0x06001286 RID: 4742 RVA: 0x00046A3F File Offset: 0x00044C3F
		public PraetorDuelDamageEvent(Identifier praetor, Identifier target, int damage)
		{
			this.Praetor = praetor;
			this.Target = target;
			this.Damage = damage;
		}

		// Token: 0x06001287 RID: 4743 RVA: 0x00046A6A File Offset: 0x00044C6A
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0} did {1} damage to {2}", context.Debug_GetItemName(this.Praetor), this.Damage, context.Debug_GetItemName(this.Target));
		}

		// Token: 0x06001288 RID: 4744 RVA: 0x00046A9C File Offset: 0x00044C9C
		public override void DeepClone(out GameEvent clone)
		{
			PraetorDuelDamageEvent praetorDuelDamageEvent = new PraetorDuelDamageEvent
			{
				Praetor = this.Praetor,
				Target = this.Target,
				Damage = this.Damage
			};
			base.DeepCloneGameEventParts<PraetorDuelDamageEvent>(praetorDuelDamageEvent);
			clone = praetorDuelDamageEvent;
		}

		// Token: 0x0400089C RID: 2204
		[JsonProperty]
		[DefaultValue(Identifier.Invalid)]
		public Identifier Praetor = Identifier.Invalid;

		// Token: 0x0400089D RID: 2205
		[JsonProperty]
		[DefaultValue(Identifier.Invalid)]
		public Identifier Target = Identifier.Invalid;

		// Token: 0x0400089E RID: 2206
		[JsonProperty]
		public int Damage;
	}
}
