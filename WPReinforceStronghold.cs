using System;

namespace LoG
{
	// Token: 0x0200018E RID: 398
	public class WPReinforceStronghold : WorldProperty
	{
		// Token: 0x06000763 RID: 1891 RVA: 0x00023051 File Offset: 0x00021251
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return false;
		}
	}
}
