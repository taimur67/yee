using System;

namespace LoG
{
	// Token: 0x02000175 RID: 373
	public class WPIsActionPlannedAmountOfTimes : WorldProperty
	{
		// Token: 0x0600071F RID: 1823 RVA: 0x000227D2 File Offset: 0x000209D2
		public WPIsActionPlannedAmountOfTimes(ActionID action, int count)
		{
			this.Action = action;
			this.Count = count;
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x000227E8 File Offset: 0x000209E8
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return planner.CountTimesActionIsPlannedThisTurn(this.Action) >= this.Count;
		}

		// Token: 0x04000343 RID: 835
		public ActionID Action;

		// Token: 0x04000344 RID: 836
		public int Count;
	}
}
