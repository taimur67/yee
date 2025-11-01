using System;

namespace LoG
{
	// Token: 0x02000184 RID: 388
	public class WPOpportunisticHeal : WorldProperty
	{
		// Token: 0x06000748 RID: 1864 RVA: 0x00022C62 File Offset: 0x00020E62
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return false;
		}
	}
}
