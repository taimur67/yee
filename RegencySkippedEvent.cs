using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000217 RID: 535
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class RegencySkippedEvent : GameEvent
	{
		// Token: 0x06000A6E RID: 2670 RVA: 0x0002E3F0 File Offset: 0x0002C5F0
		[JsonConstructor]
		private RegencySkippedEvent()
		{
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x0002E3F8 File Offset: 0x0002C5F8
		public RegencySkippedEvent(int triggeringPlayerID, int affectedPlayerId) : base(triggeringPlayerID)
		{
			base.AddAffectedPlayerId(affectedPlayerId);
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x0002E408 File Offset: 0x0002C608
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0} was skipped as regent.", base.AffectedPlayerID);
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x0002E420 File Offset: 0x0002C620
		public override void DeepClone(out GameEvent clone)
		{
			RegencySkippedEvent regencySkippedEvent = new RegencySkippedEvent();
			base.DeepCloneGameEventParts<RegencySkippedEvent>(regencySkippedEvent);
			clone = regencySkippedEvent;
		}
	}
}
