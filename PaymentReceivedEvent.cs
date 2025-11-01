using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000206 RID: 518
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class PaymentReceivedEvent : GameEvent
	{
		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000A19 RID: 2585 RVA: 0x0002DB85 File Offset: 0x0002BD85
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				if (!this.PublicPayment)
				{
					return GameEventVisibility.Private;
				}
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x0002DB92 File Offset: 0x0002BD92
		[JsonConstructor]
		private PaymentReceivedEvent()
		{
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x0002DB9C File Offset: 0x0002BD9C
		public bool IsEmpty()
		{
			bool flag = this.Offering == null || this.Offering.IsEmpty;
			bool flag2 = this.ItemsReceived == null || this.ItemsReceived.Count == 0;
			return flag && flag2;
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x0002DBDB File Offset: 0x0002BDDB
		public PaymentReceivedEvent(int triggeringPlayerId, int affectedPlayerId, Payment offering, List<Identifier> itemsReceived = null)
		{
			this.TriggeringPlayerID = triggeringPlayerId;
			base.AddAffectedPlayerId(affectedPlayerId);
			this.Offering = offering;
			this.ItemsReceived = itemsReceived;
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x0002DC00 File Offset: 0x0002BE00
		public override string GetDebugName(TurnContext context)
		{
			string format = "Payment received: {0} + {1} items";
			object offering = this.Offering;
			List<Identifier> itemsReceived = this.ItemsReceived;
			return string.Format(format, offering, (itemsReceived != null) ? itemsReceived.Count : 0);
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x0002DC2C File Offset: 0x0002BE2C
		public override void DeepClone(out GameEvent clone)
		{
			PaymentReceivedEvent paymentReceivedEvent = new PaymentReceivedEvent
			{
				Offering = this.Offering.DeepClone<Payment>(),
				ItemsReceived = this.ItemsReceived.DeepClone(),
				PublicPayment = this.PublicPayment
			};
			base.DeepCloneGameEventParts<PaymentReceivedEvent>(paymentReceivedEvent);
			clone = paymentReceivedEvent;
		}

		// Token: 0x040004C4 RID: 1220
		[JsonProperty]
		public bool PublicPayment;

		// Token: 0x040004C5 RID: 1221
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Payment Offering;

		// Token: 0x040004C6 RID: 1222
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public List<Identifier> ItemsReceived;
	}
}
