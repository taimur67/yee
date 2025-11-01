using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200021A RID: 538
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class VendettaNotPursuedEvent : GameEvent
	{
		// Token: 0x06000A7F RID: 2687 RVA: 0x0002E58F File Offset: 0x0002C78F
		[JsonConstructor]
		private VendettaNotPursuedEvent()
		{
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x0002E597 File Offset: 0x0002C797
		public VendettaNotPursuedEvent(int triggeringPlayerId, int targetPlayerId) : base(triggeringPlayerId)
		{
			base.AddAffectedPlayerId(targetPlayerId);
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x0002E5A7 File Offset: 0x0002C7A7
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (forPlayerID != base.AffectedPlayerID)
			{
				return TurnLogEntryType.None;
			}
			return TurnLogEntryType.VendettaNotPursued;
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x0002E5BC File Offset: 0x0002C7BC
		public override void DeepClone(out GameEvent clone)
		{
			VendettaNotPursuedEvent vendettaNotPursuedEvent = new VendettaNotPursuedEvent();
			base.DeepCloneGameEventParts<VendettaNotPursuedEvent>(vendettaNotPursuedEvent);
			clone = vendettaNotPursuedEvent;
		}
	}
}
