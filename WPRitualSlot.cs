using System;

namespace LoG
{
	// Token: 0x0200018F RID: 399
	public class WPRitualSlot : WorldProperty
	{
		// Token: 0x06000764 RID: 1892 RVA: 0x00023054 File Offset: 0x00021254
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return playerState.RitualState.HasSpace;
		}
	}
}
