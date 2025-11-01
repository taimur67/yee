using System;

namespace LoG
{
	// Token: 0x02000197 RID: 407
	public class WPTributeBoost : WorldProperty
	{
		// Token: 0x06000779 RID: 1913 RVA: 0x0002331E File Offset: 0x0002151E
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return false;
		}
	}
}
