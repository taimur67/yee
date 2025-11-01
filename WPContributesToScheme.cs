using System;

namespace LoG
{
	// Token: 0x0200015F RID: 351
	public class WPContributesToScheme : WorldProperty
	{
		// Token: 0x060006E3 RID: 1763 RVA: 0x00021E96 File Offset: 0x00020096
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return false;
		}
	}
}
