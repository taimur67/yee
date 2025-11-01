using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200028A RID: 650
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class VileCalumnyResponseEvent : DiplomaticResponseEvent
	{
		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000C9B RID: 3227 RVA: 0x00031DDE File Offset: 0x0002FFDE
		// (set) Token: 0x06000C9C RID: 3228 RVA: 0x00031DE6 File Offset: 0x0002FFE6
		[JsonProperty]
		[BindableValue(null, BindingOption.None)]
		public GrievanceContext GrievanceResponse { get; set; }

		// Token: 0x06000C9D RID: 3229 RVA: 0x00031DEF File Offset: 0x0002FFEF
		[JsonConstructor]
		private VileCalumnyResponseEvent()
		{
		}

		// Token: 0x06000C9E RID: 3230 RVA: 0x00031DF7 File Offset: 0x0002FFF7
		public VileCalumnyResponseEvent(int targetPlayerId, int sourcePlayerId, int scapegoatId, YesNo response, int armisticeDuration) : base(targetPlayerId, scapegoatId, response, armisticeDuration)
		{
			this.SourceId = sourcePlayerId;
			base.AddAffectedPlayerId(sourcePlayerId);
		}

		// Token: 0x06000C9F RID: 3231 RVA: 0x00031E14 File Offset: 0x00030014
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
					return TurnLogEntryType.VileCalumnyRejectedScapegoat;
				}
				return TurnLogEntryType.VileCalumnyAcceptedScapegoat;
			}
			else
			{
				if (forPlayerID != this.SourceId)
				{
					return TurnLogEntryType.None;
				}
				if (this.Response != YesNo.Yes)
				{
					return TurnLogEntryType.VileCalumnyRejectedInitiator;
				}
				return TurnLogEntryType.VileCalumnyAcceptedInitiator;
			}
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x00031E68 File Offset: 0x00030068
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0} has responded to {1}s insult.", this.TriggeringPlayerID, base.AffectedPlayerID);
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x00031E8A File Offset: 0x0003008A
		public override bool IsAssociatedWith(int playerId)
		{
			return playerId == this.SourceId || base.IsAssociatedWith(playerId);
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x00031EA0 File Offset: 0x000300A0
		public override void DeepClone(out GameEvent clone)
		{
			VileCalumnyResponseEvent vileCalumnyResponseEvent = new VileCalumnyResponseEvent
			{
				Prestige = this.Prestige,
				GrievanceResponse = this.GrievanceResponse.DeepClone<GrievanceContext>(),
				SourceId = this.SourceId
			};
			base.DeepCloneDiplomaticResponseEventParts(vileCalumnyResponseEvent);
			clone = vileCalumnyResponseEvent;
		}

		// Token: 0x04000597 RID: 1431
		[JsonProperty]
		[BindableValue("prestige", BindingOption.None)]
		public int Prestige;

		// Token: 0x04000599 RID: 1433
		[JsonProperty]
		[BindableValue("true_source_name", BindingOption.IntPlayerId)]
		public int SourceId;
	}
}
