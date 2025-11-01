using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000245 RID: 581
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class OfferVassalageResponseEvent : DiplomaticResponseEvent
	{
		// Token: 0x06000B57 RID: 2903 RVA: 0x0002FAE5 File Offset: 0x0002DCE5
		[JsonConstructor]
		private OfferVassalageResponseEvent()
		{
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x0002FAED File Offset: 0x0002DCED
		public OfferVassalageResponseEvent(int triggeringPlayerId, int respondingPlayerId, YesNo response, int armisticeDuration) : base(triggeringPlayerId, respondingPlayerId, response, armisticeDuration)
		{
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x0002FAFA File Offset: 0x0002DCFA
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("Player {0} accepted player {1} as their Vassal", this.TriggeringPlayerID, base.AffectedPlayerID);
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x0002FB1C File Offset: 0x0002DD1C
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
					return TurnLogEntryType.OfferVassalageRejectedRecipient;
				}
				return TurnLogEntryType.OfferVassalageAcceptedRecipient;
			}
			else if (forPlayerID == this.TriggeringPlayerID)
			{
				if (this.Response != YesNo.Yes)
				{
					return TurnLogEntryType.OfferVassalageRejectedInitiator;
				}
				return TurnLogEntryType.OfferVassalageAcceptedInitiator;
			}
			else
			{
				if (this.Response != YesNo.Yes)
				{
					return TurnLogEntryType.OfferVassalageRejectedWitness;
				}
				return TurnLogEntryType.OfferVassalageAcceptedWitness;
			}
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x0002FB74 File Offset: 0x0002DD74
		public override void DeepClone(out GameEvent clone)
		{
			OfferVassalageResponseEvent offerVassalageResponseEvent = new OfferVassalageResponseEvent();
			base.DeepCloneDiplomaticResponseEventParts(offerVassalageResponseEvent);
			clone = offerVassalageResponseEvent;
		}
	}
}
