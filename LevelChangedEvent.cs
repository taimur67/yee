using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000269 RID: 617
	public class LevelChangedEvent : GameEvent
	{
		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000C1C RID: 3100 RVA: 0x00030A46 File Offset: 0x0002EC46
		[JsonProperty]
		public int NumLevelsGained
		{
			get
			{
				return this.NewLevel - this.PreviousLevel;
			}
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x00030A55 File Offset: 0x0002EC55
		[JsonConstructor]
		protected LevelChangedEvent()
		{
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x00030A5D File Offset: 0x0002EC5D
		public LevelChangedEvent(int playerId, Identifier gameItem, int previousLevel, int newLevel) : base(playerId)
		{
			this.Item = gameItem;
			this.PreviousLevel = previousLevel;
			this.NewLevel = newLevel;
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x00030A7C File Offset: 0x0002EC7C
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0} Gained {1} XP. Final: ({2})", this.Item, this.NumLevelsGained, this.NewLevel);
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x00030AAC File Offset: 0x0002ECAC
		public override void DeepClone(out GameEvent clone)
		{
			LevelChangedEvent levelChangedEvent = new LevelChangedEvent
			{
				Item = this.Item,
				PreviousLevel = this.PreviousLevel,
				NewLevel = this.NewLevel
			};
			base.DeepCloneGameEventParts<LevelChangedEvent>(levelChangedEvent);
			clone = levelChangedEvent;
		}

		// Token: 0x04000535 RID: 1333
		[JsonProperty]
		public Identifier Item;

		// Token: 0x04000536 RID: 1334
		[JsonProperty]
		public int PreviousLevel;

		// Token: 0x04000537 RID: 1335
		[JsonProperty]
		public int NewLevel;
	}
}
