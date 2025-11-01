using System;

namespace LoG
{
	// Token: 0x02000157 RID: 343
	public class WPCanDuel : WorldProperty
	{
		// Token: 0x060006BF RID: 1727 RVA: 0x000217E7 File Offset: 0x0001F9E7
		public WPCanDuel(int archfiendID)
		{
			this.ArchfiendID = archfiendID;
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x000217F6 File Offset: 0x0001F9F6
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return playerState.Id != this.ArchfiendID && viewContext.CurrentTurn.IsDuelInProgress(playerState.Id, this.ArchfiendID);
		}

		// Token: 0x0400030E RID: 782
		public int ArchfiendID;
	}
}
