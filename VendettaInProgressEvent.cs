using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200021C RID: 540
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class VendettaInProgressEvent : VendettaGameEvent
	{
		// Token: 0x06000A89 RID: 2697 RVA: 0x0002E681 File Offset: 0x0002C881
		[JsonConstructor]
		private VendettaInProgressEvent()
		{
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x0002E689 File Offset: 0x0002C889
		public VendettaInProgressEvent(int triggeringPlayerId, int targetPlayerId, VendettaObjective objective, int prestige, int turns) : base(triggeringPlayerId, objective, prestige, turns)
		{
			base.AddAffectedPlayerId(targetPlayerId);
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x0002E69E File Offset: 0x0002C89E
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("The vendetta between {0} and {1} continues.", this.TriggeringPlayerID, base.AffectedPlayerID);
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x0002E6C0 File Offset: 0x0002C8C0
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (base.Cancelled)
			{
				return TurnLogEntryType.None;
			}
			if (forPlayerID == this.TriggeringPlayerID)
			{
				return TurnLogEntryType.VendettaContinuesInitiator;
			}
			if (forPlayerID == base.AffectedPlayerID)
			{
				return TurnLogEntryType.VendettaContinuesRecipient;
			}
			return TurnLogEntryType.None;
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x0002E6E8 File Offset: 0x0002C8E8
		public override void DeepClone(out GameEvent clone)
		{
			VendettaInProgressEvent vendettaInProgressEvent = new VendettaInProgressEvent();
			base.DeepCloneVendettaGameEventParts(vendettaInProgressEvent);
			clone = vendettaInProgressEvent;
		}
	}
}
