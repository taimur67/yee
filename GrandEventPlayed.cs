using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000226 RID: 550
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class GrandEventPlayed : GameEvent
	{
		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000ABF RID: 2751 RVA: 0x0002ECD3 File Offset: 0x0002CED3
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x0002ECD6 File Offset: 0x0002CED6
		protected GrandEventPlayed()
		{
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x0002ECDE File Offset: 0x0002CEDE
		public GrandEventPlayed(int playerID, string eventId) : base(playerID)
		{
			this.EventEffectId = eventId;
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x0002ECEE File Offset: 0x0002CEEE
		public override string GetDebugName(TurnContext context)
		{
			return "Grand Event Played: " + this.EventEffectId;
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x0002ED00 File Offset: 0x0002CF00
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			return TurnLogEntryType.EventCardPlayed;
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x0002ED04 File Offset: 0x0002CF04
		public override SequencePlaybackType GetSequencePlaybackType(int forPlayerID)
		{
			if (!base.CanSeeEvent(forPlayerID))
			{
				return SequencePlaybackType.None;
			}
			return SequencePlaybackType.PostPlayback;
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x0002ED12 File Offset: 0x0002CF12
		protected void DeepCloneGrandEventPlayedParts(GrandEventPlayed grandEventPlayed)
		{
			grandEventPlayed.EventEffectId = this.EventEffectId.DeepClone();
			grandEventPlayed.Duration = this.Duration;
			grandEventPlayed.FoundTarget = this.FoundTarget;
			base.DeepCloneGameEventParts<GrandEventPlayed>(grandEventPlayed);
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x0002ED48 File Offset: 0x0002CF48
		public override void DeepClone(out GameEvent clone)
		{
			GrandEventPlayed grandEventPlayed = new GrandEventPlayed();
			this.DeepCloneGrandEventPlayedParts(grandEventPlayed);
			clone = grandEventPlayed;
		}

		// Token: 0x040004F1 RID: 1265
		[BindableValue(null, BindingOption.StaticDataId)]
		[JsonProperty]
		public string EventEffectId;

		// Token: 0x040004F2 RID: 1266
		[BindableValue("turns", BindingOption.None)]
		[JsonProperty]
		public int Duration;

		// Token: 0x040004F3 RID: 1267
		[JsonProperty]
		public bool FoundTarget;
	}
}
