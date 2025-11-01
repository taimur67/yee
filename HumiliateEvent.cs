using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200023B RID: 571
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class HumiliateEvent : DiplomaticEvent
	{
		// Token: 0x06000B29 RID: 2857 RVA: 0x0002F6E1 File Offset: 0x0002D8E1
		[JsonConstructor]
		private HumiliateEvent()
		{
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x0002F6E9 File Offset: 0x0002D8E9
		public HumiliateEvent(int triggeringPlayerId, int targetPlayerId) : base(triggeringPlayerId, targetPlayerId)
		{
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x0002F6F3 File Offset: 0x0002D8F3
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("Player {0} humiliated player {1}", this.TriggeringPlayerID, base.AffectedPlayerID);
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x0002F715 File Offset: 0x0002D915
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (forPlayerID == this.TriggeringPlayerID)
			{
				return TurnLogEntryType.HumiliateSentInitiator;
			}
			if (forPlayerID != base.AffectedPlayerID)
			{
				return TurnLogEntryType.HumiliateSentWitness;
			}
			return TurnLogEntryType.None;
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x0002F730 File Offset: 0x0002D930
		public override void DeepClone(out GameEvent clone)
		{
			HumiliateEvent humiliateEvent = new HumiliateEvent();
			base.DeepCloneDiplomaticEventParts(humiliateEvent);
			clone = humiliateEvent;
		}
	}
}
