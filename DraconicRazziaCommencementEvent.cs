using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000240 RID: 576
	[BindableGameEvent]
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class DraconicRazziaCommencementEvent : DiplomaticEvent
	{
		// Token: 0x06000B41 RID: 2881 RVA: 0x0002F909 File Offset: 0x0002DB09
		[JsonConstructor]
		protected DraconicRazziaCommencementEvent()
		{
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x0002F911 File Offset: 0x0002DB11
		public DraconicRazziaCommencementEvent(int triggeringPlayerId, int targetPlayerId) : base(triggeringPlayerId, targetPlayerId)
		{
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x0002F91B File Offset: 0x0002DB1B
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (forPlayerID == this.TriggeringPlayerID)
			{
				return TurnLogEntryType.DraconicRazziaCommencementInitiator;
			}
			if (forPlayerID == base.AffectedPlayerID)
			{
				return TurnLogEntryType.DraconicRazziaCommencementTarget;
			}
			return TurnLogEntryType.DraconicRazziaCommencementWitness;
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x0002F940 File Offset: 0x0002DB40
		public override void DeepClone(out GameEvent clone)
		{
			DraconicRazziaCommencementEvent draconicRazziaCommencementEvent = new DraconicRazziaCommencementEvent();
			base.DeepCloneDiplomaticEventParts(draconicRazziaCommencementEvent);
			clone = draconicRazziaCommencementEvent;
		}
	}
}
