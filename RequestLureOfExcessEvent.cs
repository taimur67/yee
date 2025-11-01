using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000243 RID: 579
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class RequestLureOfExcessEvent : DiplomaticEvent
	{
		// Token: 0x06000B4E RID: 2894 RVA: 0x0002FA21 File Offset: 0x0002DC21
		[JsonConstructor]
		public RequestLureOfExcessEvent()
		{
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x0002FA29 File Offset: 0x0002DC29
		public RequestLureOfExcessEvent(int triggeringPlayerId, int targetPlayerId) : base(triggeringPlayerId, targetPlayerId)
		{
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x0002FA33 File Offset: 0x0002DC33
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (forPlayerID == this.TriggeringPlayerID)
			{
				return TurnLogEntryType.LureOfExcessSentInitiator;
			}
			if (forPlayerID != base.AffectedPlayerID)
			{
				return TurnLogEntryType.LureOfExcessSentWitness;
			}
			return base.GetTurnLogEntryType(forPlayerID);
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x0002FA5C File Offset: 0x0002DC5C
		public override void DeepClone(out GameEvent clone)
		{
			RequestLureOfExcessEvent requestLureOfExcessEvent = new RequestLureOfExcessEvent();
			base.DeepCloneDiplomaticEventParts(requestLureOfExcessEvent);
			clone = requestLureOfExcessEvent;
		}
	}
}
