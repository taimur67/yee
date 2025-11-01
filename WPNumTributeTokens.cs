using System;

namespace LoG
{
	// Token: 0x02000183 RID: 387
	public class WPNumTributeTokens : WorldProperty
	{
		// Token: 0x06000745 RID: 1861 RVA: 0x00022C33 File Offset: 0x00020E33
		public WPNumTributeTokens(int numTokens)
		{
			this.NumTokens = numTokens;
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x00022C42 File Offset: 0x00020E42
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return playerState.Resources.Count >= this.NumTokens;
		}

		// Token: 0x04000354 RID: 852
		public int NumTokens;
	}
}
