using System;

namespace LoG
{
	// Token: 0x0200015D RID: 349
	public class WPCommandRating : WorldProperty
	{
		// Token: 0x060006DB RID: 1755 RVA: 0x00021D4C File Offset: 0x0001FF4C
		public WPCommandRating(int commandCost)
		{
			this.CommandCost = commandCost;
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x00021D5B File Offset: 0x0001FF5B
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return viewContext.CurrentTurn.HasSufficientCommandRating(playerState, this.CommandCost, 0);
		}

		// Token: 0x0400031B RID: 795
		public int CommandCost;
	}
}
