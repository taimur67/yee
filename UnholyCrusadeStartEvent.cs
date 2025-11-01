using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200020E RID: 526
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class UnholyCrusadeStartEvent : GameEvent
	{
		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000A44 RID: 2628 RVA: 0x0002DF6A File Offset: 0x0002C16A
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x0002DF6D File Offset: 0x0002C16D
		[JsonConstructor]
		private UnholyCrusadeStartEvent()
		{
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x0002DF75 File Offset: 0x0002C175
		public UnholyCrusadeStartEvent(int triggeringPlayerID) : base(triggeringPlayerID)
		{
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x0002DF7E File Offset: 0x0002C17E
		public override string GetDebugName(TurnContext context)
		{
			return "The Unholy Crusade begins!";
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x0002DF85 File Offset: 0x0002C185
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			return TurnLogEntryType.UnholyCrusadeSent;
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x0002DF8C File Offset: 0x0002C18C
		public override void DeepClone(out GameEvent clone)
		{
			UnholyCrusadeStartEvent unholyCrusadeStartEvent = new UnholyCrusadeStartEvent();
			base.DeepCloneGameEventParts<UnholyCrusadeStartEvent>(unholyCrusadeStartEvent);
			clone = unholyCrusadeStartEvent;
		}
	}
}
