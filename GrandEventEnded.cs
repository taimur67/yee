using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000225 RID: 549
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class GrandEventEnded : GameEvent
	{
		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000AB9 RID: 2745 RVA: 0x0002EC67 File Offset: 0x0002CE67
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x0002EC6A File Offset: 0x0002CE6A
		[JsonConstructor]
		private GrandEventEnded()
		{
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x0002EC72 File Offset: 0x0002CE72
		public GrandEventEnded(string eventId, params int[] affectedPlayerIds)
		{
			this.EventEffectId = eventId;
			base.AddAffectedPlayerIds(affectedPlayerIds);
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x0002EC88 File Offset: 0x0002CE88
		public override string GetDebugName(TurnContext context)
		{
			return "Grand Event Effects Ended: " + this.EventEffectId;
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x0002EC9A File Offset: 0x0002CE9A
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			return TurnLogEntryType.EventEffectEnded;
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x0002ECA4 File Offset: 0x0002CEA4
		public override void DeepClone(out GameEvent clone)
		{
			GrandEventEnded grandEventEnded = new GrandEventEnded
			{
				EventEffectId = this.EventEffectId.DeepClone()
			};
			base.DeepCloneGameEventParts<GrandEventEnded>(grandEventEnded);
			clone = grandEventEnded;
		}

		// Token: 0x040004F0 RID: 1264
		[BindableValue(null, BindingOption.StaticDataId)]
		[JsonProperty]
		public string EventEffectId;
	}
}
