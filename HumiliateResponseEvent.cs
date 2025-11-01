using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000275 RID: 629
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class HumiliateResponseEvent : DiplomaticResponseEvent, IGrievanceAccessor, ISelectionAccessor
	{
		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000C4F RID: 3151 RVA: 0x00031192 File Offset: 0x0002F392
		public bool PrivateGrievance
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000C50 RID: 3152 RVA: 0x00031195 File Offset: 0x0002F395
		// (set) Token: 0x06000C51 RID: 3153 RVA: 0x0003119D File Offset: 0x0002F39D
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public GrievanceContext GrievanceResponse { get; set; }

		// Token: 0x06000C52 RID: 3154 RVA: 0x000311A6 File Offset: 0x0002F3A6
		[JsonConstructor]
		private HumiliateResponseEvent()
		{
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x000311AE File Offset: 0x0002F3AE
		public HumiliateResponseEvent(int triggeringPlayerID, int respondingPlayerId, YesNo response, int armisticeDuration) : base(triggeringPlayerID, respondingPlayerId, response, armisticeDuration)
		{
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x000311BC File Offset: 0x0002F3BC
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (base.Cancelled)
			{
				return TurnLogEntryType.None;
			}
			if (forPlayerID == base.AffectedPlayerID)
			{
				if (this.Response == YesNo.Yes)
				{
					return TurnLogEntryType.HumiliationAcceptedRecipient;
				}
				if (!(this.GrievanceResponse is PraetorBattleContext))
				{
					return TurnLogEntryType.VendettaStartedRecipient;
				}
				return TurnLogEntryType.None;
			}
			else if (forPlayerID == this.TriggeringPlayerID)
			{
				if (this.Response != YesNo.Yes)
				{
					return TurnLogEntryType.HumiliateRejectedInitiator;
				}
				return TurnLogEntryType.HumiliateAcceptedInitiator;
			}
			else
			{
				if (this.Response != YesNo.Yes)
				{
					return TurnLogEntryType.HumiliationRejectedWitness;
				}
				return TurnLogEntryType.HumiliationAcceptedWitness;
			}
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x00031220 File Offset: 0x0002F420
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0} has responded to {1}s humiliation.", this.TriggeringPlayerID, base.AffectedPlayerID);
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x00031244 File Offset: 0x0002F444
		public override void DeepClone(out GameEvent clone)
		{
			HumiliateResponseEvent humiliateResponseEvent = new HumiliateResponseEvent
			{
				Prestige = this.Prestige,
				GrievanceResponse = this.GrievanceResponse.DeepClone<GrievanceContext>()
			};
			base.DeepCloneDiplomaticResponseEventParts(humiliateResponseEvent);
			clone = humiliateResponseEvent;
		}

		// Token: 0x04000559 RID: 1369
		[JsonProperty]
		[BindableValue("prestige", BindingOption.IntAsDiplomacyWager)]
		public int Prestige;
	}
}
