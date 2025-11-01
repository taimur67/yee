using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002D5 RID: 725
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class PrestigeFromThirdPartyDiplomacyEvent : GameEvent
	{
		// Token: 0x06000E22 RID: 3618 RVA: 0x00037E2C File Offset: 0x0003602C
		[JsonConstructor]
		protected PrestigeFromThirdPartyDiplomacyEvent()
		{
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x00037E34 File Offset: 0x00036034
		public PrestigeFromThirdPartyDiplomacyEvent(int triggeringPlayerID, int minValue, int maxValue, bool removed = false) : base(triggeringPlayerID)
		{
			this.MinPrestigeGained = minValue;
			this.MaxPrestigeGained = maxValue;
			this.WasModificationRemoved = removed;
			base.AddAffectedPlayerId(triggeringPlayerID);
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x00037E5A File Offset: 0x0003605A
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0} will gain {1}-{2} when third party diplomacy occurs", this.TriggeringPlayerID, this.MinPrestigeGained, this.MaxPrestigeGained);
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x00037E88 File Offset: 0x00036088
		public override void DeepClone(out GameEvent clone)
		{
			PrestigeFromThirdPartyDiplomacyEvent prestigeFromThirdPartyDiplomacyEvent = new PrestigeFromThirdPartyDiplomacyEvent();
			prestigeFromThirdPartyDiplomacyEvent.MinPrestigeGained = this.MinPrestigeGained;
			prestigeFromThirdPartyDiplomacyEvent.MaxPrestigeGained = this.MaxPrestigeGained;
			prestigeFromThirdPartyDiplomacyEvent.WasModificationRemoved = this.WasModificationRemoved;
			base.DeepCloneGameEventParts<PrestigeFromThirdPartyDiplomacyEvent>(prestigeFromThirdPartyDiplomacyEvent);
			clone = prestigeFromThirdPartyDiplomacyEvent;
		}

		// Token: 0x04000642 RID: 1602
		[BindableValue("min_value", BindingOption.None)]
		[JsonProperty]
		public int MinPrestigeGained;

		// Token: 0x04000643 RID: 1603
		[BindableValue("max_value", BindingOption.None)]
		[JsonProperty]
		public int MaxPrestigeGained;

		// Token: 0x04000644 RID: 1604
		[JsonProperty]
		public bool WasModificationRemoved;
	}
}
