using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000242 RID: 578
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class RequestChainsOfAvariceEvent : DiplomaticEvent
	{
		// Token: 0x06000B4A RID: 2890 RVA: 0x0002F9C9 File Offset: 0x0002DBC9
		[JsonConstructor]
		public RequestChainsOfAvariceEvent()
		{
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x0002F9D1 File Offset: 0x0002DBD1
		public RequestChainsOfAvariceEvent(int triggeringPlayerId, int targetPlayerId) : base(triggeringPlayerId, targetPlayerId)
		{
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x0002F9DB File Offset: 0x0002DBDB
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (forPlayerID == this.TriggeringPlayerID)
			{
				return TurnLogEntryType.ChainsOfAvariceSentInitiator;
			}
			if (forPlayerID != base.AffectedPlayerID)
			{
				return TurnLogEntryType.ChainsOfAvariceSentWitness;
			}
			return base.GetTurnLogEntryType(forPlayerID);
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x0002FA04 File Offset: 0x0002DC04
		public override void DeepClone(out GameEvent clone)
		{
			RequestChainsOfAvariceEvent requestChainsOfAvariceEvent = new RequestChainsOfAvariceEvent();
			base.DeepCloneDiplomaticEventParts(requestChainsOfAvariceEvent);
			clone = requestChainsOfAvariceEvent;
		}
	}
}
