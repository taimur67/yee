using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200020A RID: 522
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class TributeOfferingEvent : GameEvent
	{
		// Token: 0x06000A2F RID: 2607 RVA: 0x0002DE20 File Offset: 0x0002C020
		[JsonConstructor]
		private TributeOfferingEvent()
		{
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x0002DE28 File Offset: 0x0002C028
		public TributeOfferingEvent(int playerId, PaymentReceivedEvent paymentEvent) : base(playerId)
		{
			base.AddChildEvent<PaymentReceivedEvent>(paymentEvent);
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x0002DE39 File Offset: 0x0002C039
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			return TurnLogEntryType.TributeOffering;
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x0002DE40 File Offset: 0x0002C040
		public override void DeepClone(out GameEvent clone)
		{
			TributeOfferingEvent tributeOfferingEvent = new TributeOfferingEvent
			{
				TurnsUntilNextOffering = this.TurnsUntilNextOffering
			};
			base.DeepCloneGameEventParts<TributeOfferingEvent>(tributeOfferingEvent);
			clone = tributeOfferingEvent;
		}

		// Token: 0x040004CB RID: 1227
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public int TurnsUntilNextOffering;
	}
}
