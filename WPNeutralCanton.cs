using System;

namespace LoG
{
	// Token: 0x02000180 RID: 384
	public class WPNeutralCanton : WorldProperty
	{
		// Token: 0x0600073E RID: 1854 RVA: 0x00022BAE File Offset: 0x00020DAE
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return false;
		}
	}
}
