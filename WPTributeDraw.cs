using System;

namespace LoG
{
	// Token: 0x02000198 RID: 408
	public class WPTributeDraw : WorldProperty
	{
		// Token: 0x0600077B RID: 1915 RVA: 0x00023329 File Offset: 0x00021529
		public WPTributeDraw(int requiredAmount)
		{
			this.RequiredAmount = requiredAmount;
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x00023338 File Offset: 0x00021538
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return playerState.NumTributeDraws.Value >= this.RequiredAmount;
		}

		// Token: 0x0400036C RID: 876
		public int RequiredAmount;
	}
}
