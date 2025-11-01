using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000259 RID: 601
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class RegencyChangedEvent : GameEvent
	{
		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000BC1 RID: 3009 RVA: 0x00030336 File Offset: 0x0002E536
		public int NewRegentIndex
		{
			get
			{
				return base.AffectedPlayerID;
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000BC2 RID: 3010 RVA: 0x0003033E File Offset: 0x0002E53E
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x00030341 File Offset: 0x0002E541
		[JsonConstructor]
		private RegencyChangedEvent()
		{
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x00030349 File Offset: 0x0002E549
		public RegencyChangedEvent(int previousRegentIndex, int newRegentIndex) : base(previousRegentIndex)
		{
			this.PreviousRegentIndex = previousRegentIndex;
			base.AddAffectedPlayerId(newRegentIndex);
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x00030360 File Offset: 0x0002E560
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("Regency changed from {0} to {1}", this.PreviousRegentIndex, this.NewRegentIndex);
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x00030382 File Offset: 0x0002E582
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (forPlayerID != this.NewRegentIndex)
			{
				return TurnLogEntryType.None;
			}
			if (base.Contains<ReceiveEventCardEvent>() || base.Contains<DecisionAddedEvent>())
			{
				return TurnLogEntryType.RegencyAcquired;
			}
			return TurnLogEntryType.RegencyAcquiredNoEvent;
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x000303AC File Offset: 0x0002E5AC
		public override void DeepClone(out GameEvent clone)
		{
			RegencyChangedEvent regencyChangedEvent = new RegencyChangedEvent
			{
				PreviousRegentIndex = this.PreviousRegentIndex
			};
			base.DeepCloneGameEventParts<RegencyChangedEvent>(regencyChangedEvent);
			clone = regencyChangedEvent;
		}

		// Token: 0x04000527 RID: 1319
		[JsonProperty]
		public int PreviousRegentIndex;
	}
}
