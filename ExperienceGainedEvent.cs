using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000268 RID: 616
	public class ExperienceGainedEvent : GameEvent
	{
		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000C17 RID: 3095 RVA: 0x0003099E File Offset: 0x0002EB9E
		[JsonIgnore]
		public int FinalXP
		{
			get
			{
				return this.StartingXP + this.XPGained;
			}
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x000309AD File Offset: 0x0002EBAD
		[JsonConstructor]
		protected ExperienceGainedEvent()
		{
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x000309B5 File Offset: 0x0002EBB5
		public ExperienceGainedEvent(int playerId, Identifier gameItem, int startingXP, int gain) : base(playerId)
		{
			this.Item = gameItem;
			this.StartingXP = startingXP;
			this.XPGained = gain;
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x000309D4 File Offset: 0x0002EBD4
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0} Gained {1} XP. Final: ({2})", this.Item, this.XPGained, this.FinalXP);
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x00030A04 File Offset: 0x0002EC04
		public override void DeepClone(out GameEvent clone)
		{
			ExperienceGainedEvent experienceGainedEvent = new ExperienceGainedEvent
			{
				Item = this.Item,
				StartingXP = this.StartingXP,
				XPGained = this.XPGained
			};
			base.DeepCloneGameEventParts<ExperienceGainedEvent>(experienceGainedEvent);
			clone = experienceGainedEvent;
		}

		// Token: 0x04000532 RID: 1330
		[JsonProperty]
		public Identifier Item;

		// Token: 0x04000533 RID: 1331
		[JsonProperty]
		public int StartingXP;

		// Token: 0x04000534 RID: 1332
		[JsonProperty]
		public int XPGained;
	}
}
