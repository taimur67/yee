using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000241 RID: 577
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class RequestToBeVassalizedEvent : DiplomaticEvent
	{
		// Token: 0x06000B45 RID: 2885 RVA: 0x0002F95D File Offset: 0x0002DB5D
		[JsonConstructor]
		private RequestToBeVassalizedEvent()
		{
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x0002F965 File Offset: 0x0002DB65
		public RequestToBeVassalizedEvent(int triggeringPlayerId, int targetPlayerId) : base(triggeringPlayerId, targetPlayerId)
		{
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x0002F96F File Offset: 0x0002DB6F
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("Player {0} requested to be a Vassal of {1}", this.TriggeringPlayerID, base.AffectedPlayerID);
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x0002F991 File Offset: 0x0002DB91
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (forPlayerID == this.TriggeringPlayerID)
			{
				return TurnLogEntryType.OfferVassalageSentInitiator;
			}
			if (forPlayerID != base.AffectedPlayerID)
			{
				return TurnLogEntryType.OfferVassalageSentWitness;
			}
			return TurnLogEntryType.None;
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x0002F9AC File Offset: 0x0002DBAC
		public override void DeepClone(out GameEvent clone)
		{
			RequestToBeVassalizedEvent requestToBeVassalizedEvent = new RequestToBeVassalizedEvent();
			base.DeepCloneDiplomaticEventParts(requestToBeVassalizedEvent);
			clone = requestToBeVassalizedEvent;
		}
	}
}
