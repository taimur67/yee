using System;

namespace LoG
{
	// Token: 0x02000174 RID: 372
	public class WPInfernalRank : WorldProperty
	{
		// Token: 0x0600071D RID: 1821 RVA: 0x000227B0 File Offset: 0x000209B0
		public WPInfernalRank(Rank rank)
		{
			this.InfernalRank = rank;
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x000227BF File Offset: 0x000209BF
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return playerState.Rank >= this.InfernalRank;
		}

		// Token: 0x04000342 RID: 834
		public Rank InfernalRank;
	}
}
