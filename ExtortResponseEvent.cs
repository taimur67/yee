using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020001F9 RID: 505
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ExtortResponseEvent : DiplomaticResponseEvent
	{
		// Token: 0x060009CB RID: 2507 RVA: 0x0002D351 File Offset: 0x0002B551
		[JsonConstructor]
		private ExtortResponseEvent()
		{
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x0002D359 File Offset: 0x0002B559
		public ExtortResponseEvent(int triggeringPlayerID, int respondingPlayerId, YesNo response, int armisticeDuration) : base(triggeringPlayerID, respondingPlayerId, response, armisticeDuration)
		{
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x0002D368 File Offset: 0x0002B568
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (base.Cancelled)
			{
				return TurnLogEntryType.None;
			}
			if (forPlayerID == base.AffectedPlayerID)
			{
				if (this.Response != YesNo.Yes)
				{
					return TurnLogEntryType.ExtortionRejectedRecipient;
				}
				return TurnLogEntryType.ExtortionAcceptedRecipient;
			}
			else if (forPlayerID == this.TriggeringPlayerID)
			{
				if (this.Response != YesNo.Yes)
				{
					return TurnLogEntryType.None;
				}
				return TurnLogEntryType.ExtortionAcceptedInitiator;
			}
			else
			{
				if (this.Response != YesNo.Yes)
				{
					return TurnLogEntryType.ExtortionRejectedWitness;
				}
				return TurnLogEntryType.ExtortionAcceptedWitness;
			}
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x0002D3BC File Offset: 0x0002B5BC
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0} has responded to {1}s extortion", this.TriggeringPlayerID, base.AffectedPlayerID);
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x0002D3E0 File Offset: 0x0002B5E0
		public override void DeepClone(out GameEvent clone)
		{
			ExtortResponseEvent extortResponseEvent = new ExtortResponseEvent();
			extortResponseEvent.ExtortedItem = this.ExtortedItem;
			base.DeepCloneDiplomaticResponseEventParts(extortResponseEvent);
			clone = extortResponseEvent;
		}

		// Token: 0x040004B4 RID: 1204
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Identifier ExtortedItem;
	}
}
