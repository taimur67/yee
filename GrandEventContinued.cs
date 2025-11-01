using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000224 RID: 548
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class GrandEventContinued : GameEvent
	{
		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000AB3 RID: 2739 RVA: 0x0002EBFC File Offset: 0x0002CDFC
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x0002EBFF File Offset: 0x0002CDFF
		[JsonConstructor]
		private GrandEventContinued()
		{
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x0002EC07 File Offset: 0x0002CE07
		public GrandEventContinued(string eventId, params int[] affectedPlayerIds)
		{
			this.EventEffectId = eventId;
			base.AddAffectedPlayerIds(affectedPlayerIds);
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x0002EC1D File Offset: 0x0002CE1D
		public override string GetDebugName(TurnContext context)
		{
			return "Grand Event Effects Continue: " + this.EventEffectId;
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x0002EC2F File Offset: 0x0002CE2F
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			return TurnLogEntryType.EventEffectContinues;
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x0002EC38 File Offset: 0x0002CE38
		public override void DeepClone(out GameEvent clone)
		{
			GrandEventContinued grandEventContinued = new GrandEventContinued
			{
				EventEffectId = this.EventEffectId.DeepClone()
			};
			base.DeepCloneGameEventParts<GrandEventContinued>(grandEventContinued);
			clone = grandEventContinued;
		}

		// Token: 0x040004EF RID: 1263
		[BindableValue(null, BindingOption.StaticDataId)]
		[JsonProperty]
		public string EventEffectId;
	}
}
