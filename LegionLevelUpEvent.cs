using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000231 RID: 561
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class LegionLevelUpEvent : GameEvent
	{
		// Token: 0x06000AF6 RID: 2806 RVA: 0x0002F1A2 File Offset: 0x0002D3A2
		[JsonConstructor]
		private LegionLevelUpEvent()
		{
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x0002F1AA File Offset: 0x0002D3AA
		public LegionLevelUpEvent(int playerId, Identifier legionId, int oldLevel, int newLevel) : base(playerId)
		{
			base.AddAffectedPlayerId(playerId);
			this.LegionId = legionId;
			this.OldLevel = oldLevel;
			this.NewLevel = newLevel;
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x0002F1D0 File Offset: 0x0002D3D0
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("Legion {0} is now level {1}", this.LegionId, this.NewLevel);
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x0002F1F4 File Offset: 0x0002D3F4
		public override void DeepClone(out GameEvent clone)
		{
			LegionLevelUpEvent legionLevelUpEvent = new LegionLevelUpEvent
			{
				LegionId = this.LegionId,
				NewLevel = this.NewLevel,
				OldLevel = this.OldLevel
			};
			base.DeepCloneGameEventParts<LegionLevelUpEvent>(legionLevelUpEvent);
			clone = legionLevelUpEvent;
		}

		// Token: 0x04000502 RID: 1282
		[JsonProperty]
		public Identifier LegionId;

		// Token: 0x04000503 RID: 1283
		[JsonProperty]
		public int NewLevel;

		// Token: 0x04000504 RID: 1284
		[JsonProperty]
		public int OldLevel;
	}
}
