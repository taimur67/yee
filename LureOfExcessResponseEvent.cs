using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000511 RID: 1297
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class LureOfExcessResponseEvent : DiplomaticResponseEvent
	{
		// Token: 0x06001918 RID: 6424 RVA: 0x00058FB1 File Offset: 0x000571B1
		[JsonConstructor]
		protected LureOfExcessResponseEvent()
		{
		}

		// Token: 0x06001919 RID: 6425 RVA: 0x00058FB9 File Offset: 0x000571B9
		public LureOfExcessResponseEvent(int triggeringPlayerId, int respondingPlayerId, YesNo response, int armisticeDuration, int rejectPrestigeCost, int effectDuration) : base(triggeringPlayerId, respondingPlayerId, response, armisticeDuration)
		{
			this.RejectPrestige = rejectPrestigeCost;
			this.Duration = effectDuration;
		}

		// Token: 0x0600191A RID: 6426 RVA: 0x00058FD8 File Offset: 0x000571D8
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
					return TurnLogEntryType.LureOfExcessRejectedRecipient;
				}
				return TurnLogEntryType.LureOfExcessAcceptedRecipient;
			}
			else if (forPlayerID == this.TriggeringPlayerID)
			{
				if (this.Response != YesNo.Yes)
				{
					return TurnLogEntryType.LureOfExcessRejectedInitiator;
				}
				return TurnLogEntryType.LureOfExcessAcceptedInitiator;
			}
			else
			{
				if (this.Response != YesNo.Yes)
				{
					return TurnLogEntryType.LureOfExcessRejectedWitness;
				}
				return TurnLogEntryType.LureOfExcessAcceptedWitness;
			}
		}

		// Token: 0x0600191B RID: 6427 RVA: 0x0005903F File Offset: 0x0005723F
		public override string GetDebugName(TurnContext context)
		{
			return context.DebugName(this.TriggeringPlayerID) + " has responded to " + context.DebugName(base.AffectedPlayerID) + "s lure of excess request.";
		}

		// Token: 0x0600191C RID: 6428 RVA: 0x00059068 File Offset: 0x00057268
		public override void DeepClone(out GameEvent clone)
		{
			LureOfExcessResponseEvent lureOfExcessResponseEvent = new LureOfExcessResponseEvent();
			lureOfExcessResponseEvent.RejectPrestige = this.RejectPrestige;
			lureOfExcessResponseEvent.Duration = this.Duration;
			base.DeepCloneDiplomaticResponseEventParts(lureOfExcessResponseEvent);
			clone = lureOfExcessResponseEvent;
		}

		// Token: 0x04000B96 RID: 2966
		[BindableValue("turns", BindingOption.None)]
		[JsonProperty]
		public int Duration;

		// Token: 0x04000B97 RID: 2967
		[BindableValue("prestige", BindingOption.None)]
		[JsonProperty]
		public int RejectPrestige;
	}
}
