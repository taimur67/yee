using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200066F RID: 1647
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class SchemeDiscardedEvent : GameEvent
	{
		// Token: 0x06001E63 RID: 7779 RVA: 0x00068DCA File Offset: 0x00066FCA
		[JsonConstructor]
		protected SchemeDiscardedEvent()
		{
		}

		// Token: 0x06001E64 RID: 7780 RVA: 0x00068DD2 File Offset: 0x00066FD2
		public SchemeDiscardedEvent(int triggeringPlayerID, int targetPlayerID, Identifier schemeID, bool wasPrivate) : base(triggeringPlayerID)
		{
			this.SchemeId = schemeID;
			this.WasPrivate = wasPrivate;
			base.AddAffectedPlayerId(targetPlayerID);
		}

		// Token: 0x06001E65 RID: 7781 RVA: 0x00068DF4 File Offset: 0x00066FF4
		public override void DeepClone(out GameEvent clone)
		{
			SchemeDiscardedEvent schemeDiscardedEvent = new SchemeDiscardedEvent
			{
				SchemeId = this.SchemeId,
				WasPrivate = this.WasPrivate
			};
			base.DeepCloneGameEventParts<SchemeDiscardedEvent>(schemeDiscardedEvent);
			clone = schemeDiscardedEvent;
		}

		// Token: 0x04000CD7 RID: 3287
		[JsonProperty]
		[BindableValue(null, BindingOption.None)]
		public Identifier SchemeId;

		// Token: 0x04000CD8 RID: 3288
		[JsonProperty]
		public bool WasPrivate;
	}
}
