using System;

namespace LoG
{
	// Token: 0x02000159 RID: 345
	public class WPChokingMap : WorldProperty
	{
		// Token: 0x060006C6 RID: 1734 RVA: 0x000218BA File Offset: 0x0001FABA
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return false;
		}
	}
}
