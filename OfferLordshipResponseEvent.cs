using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000246 RID: 582
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class OfferLordshipResponseEvent : DiplomaticResponseEvent
	{
		// Token: 0x06000B5C RID: 2908 RVA: 0x0002FB91 File Offset: 0x0002DD91
		[JsonConstructor]
		private OfferLordshipResponseEvent()
		{
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x0002FB99 File Offset: 0x0002DD99
		public OfferLordshipResponseEvent(int triggeringPlayerId, int respondingPlayerId, YesNo response, int armisticeDuration) : base(triggeringPlayerId, respondingPlayerId, response, armisticeDuration)
		{
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x0002FBA6 File Offset: 0x0002DDA6
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("Player {0} accepted player {1} as their Blood Lord", this.TriggeringPlayerID, base.AffectedPlayerID);
		}

		// Token: 0x06000B5F RID: 2911 RVA: 0x0002FBC8 File Offset: 0x0002DDC8
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
					return TurnLogEntryType.OfferLordshipRejectedRecipient;
				}
				return TurnLogEntryType.OfferLordshipAcceptedRecipient;
			}
			else if (forPlayerID == this.TriggeringPlayerID)
			{
				if (this.Response != YesNo.Yes)
				{
					return TurnLogEntryType.OfferLordshipRejectedInitiator;
				}
				return TurnLogEntryType.OfferLordshipAcceptedInitiator;
			}
			else
			{
				if (this.Response != YesNo.Yes)
				{
					return TurnLogEntryType.OfferLordshipRejectedWitness;
				}
				return TurnLogEntryType.OfferLordshipAcceptedWitness;
			}
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x0002FC20 File Offset: 0x0002DE20
		public override void DeepClone(out GameEvent clone)
		{
			OfferLordshipResponseEvent offerLordshipResponseEvent = new OfferLordshipResponseEvent();
			base.DeepCloneDiplomaticResponseEventParts(offerLordshipResponseEvent);
			clone = offerLordshipResponseEvent;
		}
	}
}
