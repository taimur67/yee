using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000250 RID: 592
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class PandaemoniumCapturedEvent : GameItemOwnershipChanged
	{
		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000B91 RID: 2961 RVA: 0x0003003A File Offset: 0x0002E23A
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000B92 RID: 2962 RVA: 0x0003003D File Offset: 0x0002E23D
		public VictoryType RelatedVictoryRule
		{
			get
			{
				return VictoryType.Pandaemonium;
			}
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x00030040 File Offset: 0x0002E240
		[JsonConstructor]
		private PandaemoniumCapturedEvent()
		{
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x00030048 File Offset: 0x0002E248
		public PandaemoniumCapturedEvent(int capturingPlayerId, int previousOwnerId, Identifier pandemonium, Identifier capturingLegion, int turns) : base(previousOwnerId, capturingPlayerId, pandemonium, GameItemCategory.GamePiece)
		{
			this.CapturingLegion = capturingLegion;
			this.Turns = turns;
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x00030064 File Offset: 0x0002E264
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("Panda Captured by player {0}", base.Owner);
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x0003007B File Offset: 0x0002E27B
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (this.TriggeringPlayerID != forPlayerID)
			{
				return TurnLogEntryType.PandaemoniumCaptured;
			}
			return TurnLogEntryType.PandaemoniumCapturedInitiator;
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x00030090 File Offset: 0x0002E290
		public override void DeepClone(out GameEvent clone)
		{
			PandaemoniumCapturedEvent pandaemoniumCapturedEvent = new PandaemoniumCapturedEvent
			{
				CapturingLegion = this.CapturingLegion,
				Turns = this.Turns
			};
			base.DeepCloneGameItemOwnershipChangedParts(pandaemoniumCapturedEvent);
			clone = pandaemoniumCapturedEvent;
		}

		// Token: 0x04000521 RID: 1313
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Identifier CapturingLegion;

		// Token: 0x04000522 RID: 1314
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public int Turns;
	}
}
