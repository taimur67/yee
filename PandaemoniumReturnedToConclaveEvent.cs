using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000252 RID: 594
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PandaemoniumReturnedToConclaveEvent : GameEvent
	{
		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000B9C RID: 2972 RVA: 0x000300E8 File Offset: 0x0002E2E8
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x000300EB File Offset: 0x0002E2EB
		public PandaemoniumReturnedToConclaveEvent() : base(-1)
		{
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x000300F4 File Offset: 0x0002E2F4
		public PandaemoniumReturnedToConclaveEvent(int triggeringPlayerId, int prestigeGained)
		{
			this.TriggeringPlayerID = triggeringPlayerId;
			this.ReturningPlayerId = triggeringPlayerId;
			this.PrestigeGained = prestigeGained;
		}

		// Token: 0x06000B9F RID: 2975 RVA: 0x0003011E File Offset: 0x0002E31E
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("Panda returned to Conclave by player {0}", this.TriggeringPlayerID);
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x00030135 File Offset: 0x0002E335
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			return TurnLogEntryType.PandaemoniumRestored;
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x0003013C File Offset: 0x0002E33C
		public override void DeepClone(out GameEvent clone)
		{
			PandaemoniumReturnedToConclaveEvent pandaemoniumReturnedToConclaveEvent = new PandaemoniumReturnedToConclaveEvent
			{
				ReturningPlayerId = this.ReturningPlayerId,
				PrestigeGained = this.PrestigeGained
			};
			base.DeepCloneGameEventParts<PandaemoniumReturnedToConclaveEvent>(pandaemoniumReturnedToConclaveEvent);
			clone = pandaemoniumReturnedToConclaveEvent;
		}

		// Token: 0x04000523 RID: 1315
		[BindableValue(null, BindingOption.IntPlayerId)]
		[JsonProperty]
		public int ReturningPlayerId;

		// Token: 0x04000524 RID: 1316
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public int PrestigeGained;
	}
}
