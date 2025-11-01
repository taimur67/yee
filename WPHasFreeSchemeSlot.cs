using System;

namespace LoG
{
	// Token: 0x0200016F RID: 367
	public class WPHasFreeSchemeSlot : WorldProperty
	{
		// Token: 0x0600070F RID: 1807 RVA: 0x00022598 File Offset: 0x00020798
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			int num = playerState.NumSchemes;
			foreach (DecisionRequest decisionRequest in this.OwningPlanner.PlayerState.DecisionRequests)
			{
				SelectSchemeDecisionRequest selectSchemeDecisionRequest = decisionRequest as SelectSchemeDecisionRequest;
				if (selectSchemeDecisionRequest != null && !planner.GetDecisionHandledByPlan(selectSchemeDecisionRequest))
				{
					num++;
				}
			}
			return num < playerState.SchemeSlots;
		}
	}
}
