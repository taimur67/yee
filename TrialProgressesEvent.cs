using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200022C RID: 556
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class TrialProgressesEvent : GameEvent
	{
		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000ADC RID: 2780 RVA: 0x0002EEBA File Offset: 0x0002D0BA
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000ADD RID: 2781 RVA: 0x0002EEBD File Offset: 0x0002D0BD
		public VictoryType RelatedVictoryRule
		{
			get
			{
				return VictoryType.Prestige;
			}
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x0002EEC0 File Offset: 0x0002D0C0
		[JsonConstructor]
		private TrialProgressesEvent()
		{
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x0002EEC8 File Offset: 0x0002D0C8
		public TrialProgressesEvent(int conclaveFavorite, TrialState trialThroneState, bool isContinuing = false) : base(-1)
		{
			this.ConclaveFavorite = conclaveFavorite;
			this.TrialThroneState = trialThroneState;
			this.IsContinuing = isContinuing;
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x0002EEE6 File Offset: 0x0002D0E6
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("TrialProgress {0}", this.TrialThroneState);
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x0002EF00 File Offset: 0x0002D100
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			switch (this.TrialThroneState)
			{
			case TrialState.Assembling:
				if (!this.IsContinuing)
				{
					return TurnLogEntryType.TrialAssembling;
				}
				return TurnLogEntryType.TrialAssemblingContinues;
			case TrialState.Debating:
				if (!this.IsContinuing)
				{
					return TurnLogEntryType.TrialDebating;
				}
				return TurnLogEntryType.TrialDebatingContinues;
			case TrialState.Arguing:
				if (!this.IsContinuing)
				{
					return TurnLogEntryType.TrialArguing;
				}
				return TurnLogEntryType.TrialArguingContinues;
			case TrialState.Documenting:
				if (!this.IsContinuing)
				{
					return TurnLogEntryType.TrialDocumenting;
				}
				return TurnLogEntryType.TrialDocumentingContinues;
			case TrialState.Announcing:
				return TurnLogEntryType.TrialAnnouncing;
			default:
				return TurnLogEntryType.None;
			}
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x0002EF7C File Offset: 0x0002D17C
		public override void DeepClone(out GameEvent clone)
		{
			TrialProgressesEvent trialProgressesEvent = new TrialProgressesEvent
			{
				ConclaveFavorite = this.ConclaveFavorite,
				TrialThroneState = this.TrialThroneState,
				IsContinuing = this.IsContinuing
			};
			base.DeepCloneGameEventParts<TrialProgressesEvent>(trialProgressesEvent);
			clone = trialProgressesEvent;
		}

		// Token: 0x040004F7 RID: 1271
		[BindableValue(null, BindingOption.IntPlayerId)]
		[JsonProperty]
		public int ConclaveFavorite;

		// Token: 0x040004F8 RID: 1272
		[JsonProperty]
		public TrialState TrialThroneState;

		// Token: 0x040004F9 RID: 1273
		[JsonProperty]
		public bool IsContinuing;
	}
}
