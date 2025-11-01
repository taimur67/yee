using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000208 RID: 520
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class DiplomaticStateCancelledEvent : GameEvent
	{
		// Token: 0x06000A25 RID: 2597 RVA: 0x0002DCF3 File Offset: 0x0002BEF3
		[JsonConstructor]
		private DiplomaticStateCancelledEvent()
		{
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x0002DCFB File Offset: 0x0002BEFB
		public DiplomaticStateCancelledEvent(int actorId, int targetId) : base(actorId)
		{
			base.AddAffectedPlayerId(targetId);
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x0002DD0B File Offset: 0x0002BF0B
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			return TurnLogEntryType.DiplomaticStateCancelled;
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x0002DD14 File Offset: 0x0002BF14
		public override void DeepClone(out GameEvent clone)
		{
			DiplomaticStateCancelledEvent diplomaticStateCancelledEvent = new DiplomaticStateCancelledEvent();
			base.DeepCloneGameEventParts<DiplomaticStateCancelledEvent>(diplomaticStateCancelledEvent);
			clone = diplomaticStateCancelledEvent;
		}
	}
}
