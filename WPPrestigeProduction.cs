using System;

namespace LoG
{
	// Token: 0x0200018C RID: 396
	public class WPPrestigeProduction : WorldProperty
	{
		// Token: 0x0600075E RID: 1886 RVA: 0x0002302C File Offset: 0x0002122C
		public WPPrestigeProduction(int prestigeIncrease)
		{
			this.PrestigeIncrease = prestigeIncrease;
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x0002303B File Offset: 0x0002123B
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return false;
		}

		// Token: 0x04000361 RID: 865
		public int PrestigeIncrease;
	}
}
