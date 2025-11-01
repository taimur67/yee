using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000209 RID: 521
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class PaymentRemovedEvent : GameEvent
	{
		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000A29 RID: 2601 RVA: 0x0002DD32 File Offset: 0x0002BF32
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return this.Visibility;
			}
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x0002DD3C File Offset: 0x0002BF3C
		public bool IsEmpty()
		{
			bool flag = this.Payment == null || this.Payment.IsEmpty;
			bool flag2 = this.RemovedItems == null || this.RemovedItems.Count == 0;
			return flag && flag2;
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x0002DD7B File Offset: 0x0002BF7B
		[JsonConstructor]
		private PaymentRemovedEvent()
		{
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x0002DD83 File Offset: 0x0002BF83
		public PaymentRemovedEvent(int fromPlayerId, int forPlayerId, Payment payment, List<Identifier> removedItems = null)
		{
			this.TriggeringPlayerID = fromPlayerId;
			base.AddAffectedPlayerId(forPlayerId);
			this.Payment = payment;
			this.RemovedItems = removedItems;
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x0002DDA8 File Offset: 0x0002BFA8
		public override string GetDebugName(TurnContext context)
		{
			string format = "Payment removed: {0} + {1} items";
			object payment = this.Payment;
			List<Identifier> removedItems = this.RemovedItems;
			return string.Format(format, payment, (removedItems != null) ? removedItems.Count : 0);
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x0002DDD4 File Offset: 0x0002BFD4
		public override void DeepClone(out GameEvent clone)
		{
			PaymentRemovedEvent paymentRemovedEvent = new PaymentRemovedEvent
			{
				Visibility = this.Visibility,
				Payment = this.Payment.DeepClone<Payment>(),
				RemovedItems = this.RemovedItems.DeepClone()
			};
			base.DeepCloneGameEventParts<PaymentRemovedEvent>(paymentRemovedEvent);
			clone = paymentRemovedEvent;
		}

		// Token: 0x040004C8 RID: 1224
		[JsonProperty]
		public GameEventVisibility Visibility;

		// Token: 0x040004C9 RID: 1225
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Payment Payment;

		// Token: 0x040004CA RID: 1226
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public List<Identifier> RemovedItems;
	}
}
