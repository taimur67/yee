using System;

namespace LoG
{
	// Token: 0x02000161 RID: 353
	public class WPDecisionNeedsMaking : WorldProperty
	{
		// Token: 0x060006E8 RID: 1768 RVA: 0x00021F51 File Offset: 0x00020151
		public WPDecisionNeedsMaking(DecisionRequest request)
		{
			this.Request = request;
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x00021F60 File Offset: 0x00020160
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return !planner.GetDecisionHandledByPlan(this.Request);
		}

		// Token: 0x04000321 RID: 801
		public DecisionRequest Request;
	}
}
