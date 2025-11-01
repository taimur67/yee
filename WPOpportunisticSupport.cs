using System;

namespace LoG
{
	// Token: 0x02000185 RID: 389
	public class WPOpportunisticSupport : WorldProperty
	{
		// Token: 0x0600074A RID: 1866 RVA: 0x00022C6D File Offset: 0x00020E6D
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return false;
		}
	}
}
