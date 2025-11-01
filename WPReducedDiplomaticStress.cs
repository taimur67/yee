using System;

namespace LoG
{
	// Token: 0x0200018D RID: 397
	public class WPReducedDiplomaticStress : WorldProperty
	{
		// Token: 0x06000760 RID: 1888 RVA: 0x0002303E File Offset: 0x0002123E
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return false;
		}
	}
}
