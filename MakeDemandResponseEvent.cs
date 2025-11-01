using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000277 RID: 631
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class MakeDemandResponseEvent : DiplomaticResponseEvent
	{
		// Token: 0x06000C5E RID: 3166 RVA: 0x00031356 File Offset: 0x0002F556
		[JsonConstructor]
		private MakeDemandResponseEvent()
		{
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x0003135E File Offset: 0x0002F55E
		public MakeDemandResponseEvent(int triggeringPlayerID, int respondingPlayerId, YesNo response, int armisticeDuration) : base(triggeringPlayerID, respondingPlayerId, response, armisticeDuration)
		{
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x0003136C File Offset: 0x0002F56C
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
					return TurnLogEntryType.DemandRejectedRecipient;
				}
				return TurnLogEntryType.DemandAcceptedRecipient;
			}
			else if (forPlayerID == this.TriggeringPlayerID)
			{
				if (this.Response != YesNo.Yes)
				{
					return TurnLogEntryType.None;
				}
				return TurnLogEntryType.DemandAcceptedInitiator;
			}
			else
			{
				if (this.Response != YesNo.Yes)
				{
					return TurnLogEntryType.DemandRejectedWitness;
				}
				return TurnLogEntryType.DemandAcceptedWitness;
			}
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x000313C0 File Offset: 0x0002F5C0
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0} has responded to {1}s demand", this.TriggeringPlayerID, base.AffectedPlayerID);
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x000313E4 File Offset: 0x0002F5E4
		public override void DeepClone(out GameEvent clone)
		{
			MakeDemandResponseEvent makeDemandResponseEvent = new MakeDemandResponseEvent();
			base.DeepCloneDiplomaticResponseEventParts(makeDemandResponseEvent);
			clone = makeDemandResponseEvent;
		}
	}
}
