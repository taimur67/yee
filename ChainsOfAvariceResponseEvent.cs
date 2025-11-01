using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200050A RID: 1290
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ChainsOfAvariceResponseEvent : DiplomaticResponseEvent
	{
		// Token: 0x060018D1 RID: 6353 RVA: 0x00058998 File Offset: 0x00056B98
		[JsonConstructor]
		protected ChainsOfAvariceResponseEvent()
		{
		}

		// Token: 0x060018D2 RID: 6354 RVA: 0x000589A0 File Offset: 0x00056BA0
		public ChainsOfAvariceResponseEvent(int triggeringPlayerId, int respondingPlayerId, YesNo response, int armisticeDuration, int additionalTokenDraw, int effectDuration) : base(triggeringPlayerId, respondingPlayerId, response, armisticeDuration)
		{
			this.AdditionalTokenDraw = additionalTokenDraw;
			this.Duration = effectDuration;
		}

		// Token: 0x060018D3 RID: 6355 RVA: 0x000589C0 File Offset: 0x00056BC0
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
					return TurnLogEntryType.ChainsOfAvariceRejectedRecipient;
				}
				return TurnLogEntryType.ChainsOfAvariceAcceptedRecipient;
			}
			else if (forPlayerID == this.TriggeringPlayerID)
			{
				if (this.Response != YesNo.Yes)
				{
					return TurnLogEntryType.ChainsOfAvariceRejectedInitiator;
				}
				return TurnLogEntryType.ChainsOfAvariceAcceptedInitiator;
			}
			else
			{
				if (this.Response != YesNo.Yes)
				{
					return TurnLogEntryType.ChainsOfAvariceRejectedWitness;
				}
				return TurnLogEntryType.ChainsOfAvariceAcceptedWitness;
			}
		}

		// Token: 0x060018D4 RID: 6356 RVA: 0x00058A27 File Offset: 0x00056C27
		public override string GetDebugName(TurnContext context)
		{
			return context.DebugName(this.TriggeringPlayerID) + " has responded to " + context.DebugName(base.AffectedPlayerID) + "s chains of avarice request.";
		}

		// Token: 0x060018D5 RID: 6357 RVA: 0x00058A50 File Offset: 0x00056C50
		public override void DeepClone(out GameEvent clone)
		{
			ChainsOfAvariceResponseEvent chainsOfAvariceResponseEvent = new ChainsOfAvariceResponseEvent();
			chainsOfAvariceResponseEvent.AdditionalTokenDraw = this.AdditionalTokenDraw;
			chainsOfAvariceResponseEvent.Duration = this.Duration;
			base.DeepCloneDiplomaticResponseEventParts(chainsOfAvariceResponseEvent);
			clone = chainsOfAvariceResponseEvent;
		}

		// Token: 0x04000B90 RID: 2960
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public int AdditionalTokenDraw;

		// Token: 0x04000B91 RID: 2961
		[BindableValue("turns", BindingOption.None)]
		[JsonProperty]
		public int Duration;
	}
}
