using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020001F6 RID: 502
	[BindableGameEvent]
	[Serializable]
	public abstract class DiplomaticEvent : GameEvent
	{
		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060009BB RID: 2491 RVA: 0x0002D27D File Offset: 0x0002B47D
		// (set) Token: 0x060009BC RID: 2492 RVA: 0x0002D285 File Offset: 0x0002B485
		[BindableValue(null, BindingOption.OrderTypeAsNoun)]
		[JsonProperty]
		public OrderTypes OrderType { get; set; }

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x060009BD RID: 2493 RVA: 0x0002D28E File Offset: 0x0002B48E
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x0002D291 File Offset: 0x0002B491
		protected DiplomaticEvent(int triggeringPlayerId, int targetPlayerId) : base(triggeringPlayerId)
		{
			base.AddAffectedPlayerId(targetPlayerId);
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x0002D2A1 File Offset: 0x0002B4A1
		protected DiplomaticEvent()
		{
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x0002D2A9 File Offset: 0x0002B4A9
		protected void DeepCloneDiplomaticEventParts(DiplomaticEvent diplomaticEvent)
		{
			diplomaticEvent.OrderType = this.OrderType;
			base.DeepCloneGameEventParts<DiplomaticEvent>(diplomaticEvent);
		}

		// Token: 0x060009C1 RID: 2497
		public abstract override void DeepClone(out GameEvent clone);
	}
}
