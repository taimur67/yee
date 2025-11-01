using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002CF RID: 719
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class PrestigeFromResistingRitualEvent : GameEvent
	{
		// Token: 0x06000E05 RID: 3589 RVA: 0x0003777C File Offset: 0x0003597C
		[JsonConstructor]
		protected PrestigeFromResistingRitualEvent()
		{
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x00037784 File Offset: 0x00035984
		public PrestigeFromResistingRitualEvent(int triggeringPlayerID, PowerType power, int minValue, int maxValue, bool removed = false) : base(triggeringPlayerID)
		{
			this.PowerTypeToResist = power;
			this.MinPrestigeGained = minValue;
			this.MaxPrestigeGained = maxValue;
			this.WasModificationRemoved = removed;
			base.AddAffectedPlayerId(triggeringPlayerID);
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x000377B4 File Offset: 0x000359B4
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0} will gain {1}-{2} when they resist {3} rituals", new object[]
			{
				this.TriggeringPlayerID,
				this.MinPrestigeGained,
				this.MaxPrestigeGained,
				this.PowerTypeToResist
			});
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x0003780C File Offset: 0x00035A0C
		public override void DeepClone(out GameEvent clone)
		{
			PrestigeFromResistingRitualEvent prestigeFromResistingRitualEvent = new PrestigeFromResistingRitualEvent();
			prestigeFromResistingRitualEvent.PowerTypeToResist = this.PowerTypeToResist;
			prestigeFromResistingRitualEvent.MinPrestigeGained = this.MinPrestigeGained;
			prestigeFromResistingRitualEvent.MaxPrestigeGained = this.MaxPrestigeGained;
			prestigeFromResistingRitualEvent.WasModificationRemoved = this.WasModificationRemoved;
			base.DeepCloneGameEventParts<PrestigeFromResistingRitualEvent>(prestigeFromResistingRitualEvent);
			clone = prestigeFromResistingRitualEvent;
		}

		// Token: 0x04000638 RID: 1592
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public PowerType PowerTypeToResist;

		// Token: 0x04000639 RID: 1593
		[BindableValue("min_value", BindingOption.None)]
		[JsonProperty]
		public int MinPrestigeGained;

		// Token: 0x0400063A RID: 1594
		[BindableValue("max_value", BindingOption.None)]
		[JsonProperty]
		public int MaxPrestigeGained;

		// Token: 0x0400063B RID: 1595
		[JsonProperty]
		public bool WasModificationRemoved;
	}
}
