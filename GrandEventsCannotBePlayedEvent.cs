using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002D1 RID: 721
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class GrandEventsCannotBePlayedEvent : GameEvent
	{
		// Token: 0x06000E0D RID: 3597 RVA: 0x000378C6 File Offset: 0x00035AC6
		[JsonConstructor]
		protected GrandEventsCannotBePlayedEvent()
		{
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x000378CE File Offset: 0x00035ACE
		public GrandEventsCannotBePlayedEvent(int triggeringPlayerID, int targetPlayerID, bool wasModificationRemoved = false) : base(triggeringPlayerID)
		{
			base.AddAffectedPlayerId(targetPlayerID);
			this.WasModificationRemoved = wasModificationRemoved;
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x000378E5 File Offset: 0x00035AE5
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0} Grand Events allowed = {1}", base.AffectedPlayerID, this.WasModificationRemoved);
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x00037908 File Offset: 0x00035B08
		public override void DeepClone(out GameEvent clone)
		{
			GrandEventsCannotBePlayedEvent grandEventsCannotBePlayedEvent = new GrandEventsCannotBePlayedEvent();
			grandEventsCannotBePlayedEvent.WasModificationRemoved = this.WasModificationRemoved;
			base.DeepCloneGameEventParts<GrandEventsCannotBePlayedEvent>(grandEventsCannotBePlayedEvent);
			clone = grandEventsCannotBePlayedEvent;
		}

		// Token: 0x0400063D RID: 1597
		[JsonProperty]
		public bool WasModificationRemoved;
	}
}
