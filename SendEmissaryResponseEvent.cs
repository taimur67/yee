using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000288 RID: 648
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class SendEmissaryResponseEvent : DiplomaticResponseEvent
	{
		// Token: 0x06000C96 RID: 3222 RVA: 0x00031CCD File Offset: 0x0002FECD
		[JsonConstructor]
		private SendEmissaryResponseEvent()
		{
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x00031CD5 File Offset: 0x0002FED5
		public SendEmissaryResponseEvent(int triggeringPlayerID, int respondingPlayerId, YesNo response, Payment payment, int armistice = 0) : base(triggeringPlayerID, respondingPlayerId, response, armistice)
		{
			this.PaymentReceived = payment;
		}

		// Token: 0x06000C98 RID: 3224 RVA: 0x00031CEC File Offset: 0x0002FEEC
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (base.Cancelled)
			{
				return TurnLogEntryType.None;
			}
			if (forPlayerID == base.AffectedPlayerID)
			{
				switch (this.Response)
				{
				case YesNo.Yes:
					return TurnLogEntryType.EmissaryAcceptedRecipient;
				case YesNo.No:
					return TurnLogEntryType.EmissaryRejectedRecipient;
				case YesNo.StrongNo:
					return TurnLogEntryType.EmissaryExecutedRecipient;
				default:
					return TurnLogEntryType.None;
				}
			}
			else if (forPlayerID == this.TriggeringPlayerID)
			{
				switch (this.Response)
				{
				case YesNo.Yes:
					return TurnLogEntryType.EmissaryAcceptedInitiator;
				case YesNo.No:
					return TurnLogEntryType.EmissaryRejectedInitiator;
				case YesNo.StrongNo:
					return TurnLogEntryType.None;
				default:
					return TurnLogEntryType.None;
				}
			}
			else
			{
				switch (this.Response)
				{
				case YesNo.Yes:
					return TurnLogEntryType.EmissaryAcceptedWitness;
				case YesNo.No:
					return TurnLogEntryType.EmissaryRejectedWitness;
				case YesNo.StrongNo:
					return TurnLogEntryType.EmissaryExecutedWitness;
				default:
					return TurnLogEntryType.None;
				}
			}
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x00031D8B File Offset: 0x0002FF8B
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0} has responded to {1}s demand", this.TriggeringPlayerID, base.AffectedPlayerID);
		}

		// Token: 0x06000C9A RID: 3226 RVA: 0x00031DB0 File Offset: 0x0002FFB0
		public override void DeepClone(out GameEvent clone)
		{
			SendEmissaryResponseEvent sendEmissaryResponseEvent = new SendEmissaryResponseEvent
			{
				PaymentReceived = this.PaymentReceived.DeepClone<Payment>()
			};
			base.DeepCloneDiplomaticResponseEventParts(sendEmissaryResponseEvent);
			clone = sendEmissaryResponseEvent;
		}

		// Token: 0x04000592 RID: 1426
		[JsonProperty]
		public Payment PaymentReceived;
	}
}
