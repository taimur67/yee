using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200023C RID: 572
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class MakeDemandEvent : DiplomaticEvent
	{
		// Token: 0x06000B2E RID: 2862 RVA: 0x0002F74D File Offset: 0x0002D94D
		[JsonConstructor]
		private MakeDemandEvent()
		{
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x0002F755 File Offset: 0x0002D955
		public MakeDemandEvent(int triggeringPlayerId, int targetPlayerId) : base(triggeringPlayerId, targetPlayerId)
		{
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x0002F75F File Offset: 0x0002D95F
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("Player {0} made a demand of player {1}", this.TriggeringPlayerID, base.AffectedPlayerID);
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x0002F781 File Offset: 0x0002D981
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (forPlayerID == this.TriggeringPlayerID)
			{
				return TurnLogEntryType.DemandSentInitiator;
			}
			if (forPlayerID != base.AffectedPlayerID)
			{
				return TurnLogEntryType.DemandSentWitness;
			}
			return TurnLogEntryType.None;
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x0002F79C File Offset: 0x0002D99C
		public override void DeepClone(out GameEvent clone)
		{
			MakeDemandEvent makeDemandEvent = new MakeDemandEvent();
			makeDemandEvent.CostReduced = this.CostReduced;
			base.DeepCloneDiplomaticEventParts(makeDemandEvent);
			clone = makeDemandEvent;
		}

		// Token: 0x0400050F RID: 1295
		[JsonProperty]
		public bool CostReduced;
	}
}
