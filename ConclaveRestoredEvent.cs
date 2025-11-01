using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000255 RID: 597
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ConclaveRestoredEvent : GameEvent
	{
		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000BAE RID: 2990 RVA: 0x0003021A File Offset: 0x0002E41A
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000BAF RID: 2991 RVA: 0x0003021D File Offset: 0x0002E41D
		public ConclaveRestoredEvent() : base(-1)
		{
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x00030226 File Offset: 0x0002E426
		public override string GetDebugName(TurnContext context)
		{
			return "Conclave has been restored";
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x0003022D File Offset: 0x0002E42D
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			return TurnLogEntryType.ConclaveRestored;
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x00030234 File Offset: 0x0002E434
		public override void DeepClone(out GameEvent clone)
		{
			ConclaveRestoredEvent conclaveRestoredEvent = new ConclaveRestoredEvent();
			base.DeepCloneGameEventParts<ConclaveRestoredEvent>(conclaveRestoredEvent);
			clone = conclaveRestoredEvent;
		}
	}
}
