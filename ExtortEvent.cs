using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200023D RID: 573
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ExtortEvent : DiplomaticEvent
	{
		// Token: 0x06000B33 RID: 2867 RVA: 0x0002F7C5 File Offset: 0x0002D9C5
		[JsonConstructor]
		private ExtortEvent()
		{
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x0002F7CD File Offset: 0x0002D9CD
		public ExtortEvent(int triggeringPlayerId, int targetPlayerId) : base(triggeringPlayerId, targetPlayerId)
		{
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x0002F7D7 File Offset: 0x0002D9D7
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("Player {0} extorted player {1}", this.TriggeringPlayerID, base.AffectedPlayerID);
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x0002F7F9 File Offset: 0x0002D9F9
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (forPlayerID == this.TriggeringPlayerID)
			{
				return TurnLogEntryType.ExtortionSentInitiator;
			}
			if (forPlayerID != base.AffectedPlayerID)
			{
				return TurnLogEntryType.ExtortionSentWitness;
			}
			return TurnLogEntryType.None;
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x0002F814 File Offset: 0x0002DA14
		public override void DeepClone(out GameEvent clone)
		{
			ExtortEvent extortEvent = new ExtortEvent();
			extortEvent.CostReduced = this.CostReduced;
			base.DeepCloneDiplomaticEventParts(extortEvent);
			clone = extortEvent;
		}

		// Token: 0x04000510 RID: 1296
		[JsonProperty]
		public bool CostReduced;
	}
}
