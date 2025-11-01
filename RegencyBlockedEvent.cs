using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002D0 RID: 720
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class RegencyBlockedEvent : GameEvent
	{
		// Token: 0x06000E09 RID: 3593 RVA: 0x0003785A File Offset: 0x00035A5A
		[JsonConstructor]
		protected RegencyBlockedEvent()
		{
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x00037862 File Offset: 0x00035A62
		public RegencyBlockedEvent(int triggeringPlayerID, int targetPlayerID, bool wasModificationRemoved = false) : base(triggeringPlayerID)
		{
			base.AddAffectedPlayerId(targetPlayerID);
			this.WasModificationRemoved = wasModificationRemoved;
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x00037879 File Offset: 0x00035A79
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0} regency allowed = {1}", base.AffectedPlayerID, this.WasModificationRemoved);
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x0003789C File Offset: 0x00035A9C
		public override void DeepClone(out GameEvent clone)
		{
			RegencyBlockedEvent regencyBlockedEvent = new RegencyBlockedEvent();
			regencyBlockedEvent.WasModificationRemoved = this.WasModificationRemoved;
			base.DeepCloneGameEventParts<RegencyBlockedEvent>(regencyBlockedEvent);
			clone = regencyBlockedEvent;
		}

		// Token: 0x0400063C RID: 1596
		[JsonProperty]
		public bool WasModificationRemoved;
	}
}
