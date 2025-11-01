using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000276 RID: 630
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class InsultResponseEvent : DiplomaticResponseEvent, IGrievanceAccessor, ISelectionAccessor
	{
		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000C57 RID: 3159 RVA: 0x0003127E File Offset: 0x0002F47E
		// (set) Token: 0x06000C58 RID: 3160 RVA: 0x00031286 File Offset: 0x0002F486
		[JsonProperty]
		[BindableValue(null, BindingOption.None)]
		public GrievanceContext GrievanceResponse { get; set; }

		// Token: 0x06000C59 RID: 3161 RVA: 0x0003128F File Offset: 0x0002F48F
		[JsonConstructor]
		private InsultResponseEvent()
		{
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x00031297 File Offset: 0x0002F497
		public InsultResponseEvent(int triggeringPlayerId, int respondingPlayerId, YesNo response, int armisticeDuration) : base(triggeringPlayerId, respondingPlayerId, response, armisticeDuration)
		{
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x000312A4 File Offset: 0x0002F4A4
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
					return TurnLogEntryType.None;
				}
				return TurnLogEntryType.InsultAcceptedRecipient;
			}
			else if (forPlayerID == this.TriggeringPlayerID)
			{
				if (this.Response != YesNo.Yes)
				{
					return TurnLogEntryType.InsultRejectedInitiator;
				}
				return TurnLogEntryType.InsultAcceptedInitiator;
			}
			else
			{
				if (this.Response != YesNo.Yes)
				{
					return TurnLogEntryType.InsultRejectedWitness;
				}
				return TurnLogEntryType.InsultAcceptedWitness;
			}
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x000312F8 File Offset: 0x0002F4F8
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0} has responded to {1}s insult.", this.TriggeringPlayerID, base.AffectedPlayerID);
		}

		// Token: 0x06000C5D RID: 3165 RVA: 0x0003131C File Offset: 0x0002F51C
		public override void DeepClone(out GameEvent clone)
		{
			InsultResponseEvent insultResponseEvent = new InsultResponseEvent
			{
				Prestige = this.Prestige,
				GrievanceResponse = this.GrievanceResponse.DeepClone<GrievanceContext>()
			};
			base.DeepCloneDiplomaticResponseEventParts(insultResponseEvent);
			clone = insultResponseEvent;
		}

		// Token: 0x0400055B RID: 1371
		[JsonProperty]
		[BindableValue("prestige", BindingOption.IntAsDiplomacyWager)]
		public int Prestige;
	}
}
