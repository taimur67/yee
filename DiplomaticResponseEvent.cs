using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020001F8 RID: 504
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public abstract class DiplomaticResponseEvent : DiplomaticEvent, ICancellableGameEvent
	{
		// Token: 0x170001BB RID: 443
		// (get) Token: 0x060009C5 RID: 2501 RVA: 0x0002D2F2 File Offset: 0x0002B4F2
		// (set) Token: 0x060009C6 RID: 2502 RVA: 0x0002D2FA File Offset: 0x0002B4FA
		[JsonProperty]
		public bool Cancelled { get; set; }

		// Token: 0x060009C7 RID: 2503 RVA: 0x0002D303 File Offset: 0x0002B503
		protected DiplomaticResponseEvent()
		{
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x0002D30B File Offset: 0x0002B50B
		protected DiplomaticResponseEvent(int triggeringPlayerID, int targetedPlayerId, YesNo response, int armisticeDuration) : base(triggeringPlayerID, targetedPlayerId)
		{
			this.Response = response;
			this.ArmisticeDuration = armisticeDuration;
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x0002D324 File Offset: 0x0002B524
		protected void DeepCloneDiplomaticResponseEventParts(DiplomaticResponseEvent diplomaticResponseEvent)
		{
			diplomaticResponseEvent.Response = this.Response;
			diplomaticResponseEvent.ArmisticeDuration = this.ArmisticeDuration;
			diplomaticResponseEvent.Cancelled = this.Cancelled;
			base.DeepCloneDiplomaticEventParts(diplomaticResponseEvent);
		}

		// Token: 0x060009CA RID: 2506
		public abstract override void DeepClone(out GameEvent clone);

		// Token: 0x040004B1 RID: 1201
		[JsonProperty]
		public YesNo Response;

		// Token: 0x040004B2 RID: 1202
		[BindableValue("armistice", BindingOption.None)]
		[JsonProperty]
		public int ArmisticeDuration;
	}
}
