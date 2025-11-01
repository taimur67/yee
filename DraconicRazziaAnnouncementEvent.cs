using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200023F RID: 575
	[BindableGameEvent]
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class DraconicRazziaAnnouncementEvent : DiplomaticEvent
	{
		// Token: 0x06000B3D RID: 2877 RVA: 0x0002F8A9 File Offset: 0x0002DAA9
		[JsonConstructor]
		protected DraconicRazziaAnnouncementEvent()
		{
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x0002F8B1 File Offset: 0x0002DAB1
		public DraconicRazziaAnnouncementEvent(int triggeringPlayerId, int targetPlayerId) : base(triggeringPlayerId, targetPlayerId)
		{
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x0002F8BB File Offset: 0x0002DABB
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (forPlayerID == this.TriggeringPlayerID)
			{
				return TurnLogEntryType.DraconicRazziaAnnounceInitiator;
			}
			if (forPlayerID == base.AffectedPlayerID)
			{
				return TurnLogEntryType.DraconicRazziaAnnounceTarget;
			}
			return TurnLogEntryType.DraconicRazziaAnnounceWitness;
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x0002F8E0 File Offset: 0x0002DAE0
		public override void DeepClone(out GameEvent clone)
		{
			DraconicRazziaAnnouncementEvent draconicRazziaAnnouncementEvent = new DraconicRazziaAnnouncementEvent
			{
				Turn = this.Turn
			};
			base.DeepCloneDiplomaticEventParts(draconicRazziaAnnouncementEvent);
			clone = draconicRazziaAnnouncementEvent;
		}

		// Token: 0x04000511 RID: 1297
		[JsonProperty]
		public int Turn;
	}
}
