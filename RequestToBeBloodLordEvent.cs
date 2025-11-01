using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000244 RID: 580
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class RequestToBeBloodLordEvent : DiplomaticEvent
	{
		// Token: 0x06000B52 RID: 2898 RVA: 0x0002FA79 File Offset: 0x0002DC79
		[JsonConstructor]
		public RequestToBeBloodLordEvent()
		{
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x0002FA81 File Offset: 0x0002DC81
		public RequestToBeBloodLordEvent(int triggeringPlayerId, int targetPlayerId) : base(triggeringPlayerId, targetPlayerId)
		{
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x0002FA8B File Offset: 0x0002DC8B
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("Player {0} requested to be the Blood Lord of {1}", this.TriggeringPlayerID, base.AffectedPlayerID);
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x0002FAAD File Offset: 0x0002DCAD
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (forPlayerID == this.TriggeringPlayerID)
			{
				return TurnLogEntryType.OfferLordshipSentInitiator;
			}
			if (forPlayerID != base.AffectedPlayerID)
			{
				return TurnLogEntryType.OfferLordshipSentWitness;
			}
			return TurnLogEntryType.None;
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x0002FAC8 File Offset: 0x0002DCC8
		public override void DeepClone(out GameEvent clone)
		{
			RequestToBeBloodLordEvent requestToBeBloodLordEvent = new RequestToBeBloodLordEvent();
			base.DeepCloneDiplomaticEventParts(requestToBeBloodLordEvent);
			clone = requestToBeBloodLordEvent;
		}
	}
}
