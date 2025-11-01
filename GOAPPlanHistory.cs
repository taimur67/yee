using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x02000134 RID: 308
	public class GOAPPlanHistory
	{
		// Token: 0x06000600 RID: 1536 RVA: 0x0001D84C File Offset: 0x0001BA4C
		public ActionHistory AddActionPlan(GOAPNode goal, int turnNumber)
		{
			ActionHistory actionHistory = new ActionHistory
			{
				Goal = goal,
				Turn = turnNumber
			};
			this.ActionArtifacts.Add(actionHistory);
			return actionHistory;
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x0001D87C File Offset: 0x0001BA7C
		public ActionHistory AddFallback(string text, int turnNumber)
		{
			ActionHistory actionHistory = new ActionHistory
			{
				Turn = turnNumber,
				FallbackAction = text
			};
			this.ActionArtifacts.Add(actionHistory);
			return actionHistory;
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x0001D8AC File Offset: 0x0001BAAC
		public ActionHistory AddGrandEvent(string text, int turnNumber)
		{
			ActionHistory actionHistory = new ActionHistory
			{
				Turn = turnNumber,
				GrandEvent = text
			};
			this.ActionArtifacts.Add(actionHistory);
			return actionHistory;
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x0001D8DC File Offset: 0x0001BADC
		public int GetMostRecentTurnUsageOfGoal(ActionID goalID)
		{
			if (this.ActionArtifacts.Count > 0)
			{
				for (int i = this.ActionArtifacts.Count - 1; i >= 0; i--)
				{
					GOAPNode goal = this.ActionArtifacts[i].Goal;
					if (goal != null && goal.ID == goalID)
					{
						return this.ActionArtifacts[i].Turn;
					}
				}
			}
			return -1;
		}

		// Token: 0x040002D8 RID: 728
		public List<ActionHistory> ActionArtifacts = new List<ActionHistory>();

		// Token: 0x040002D9 RID: 729
		public List<string> WrapUpDecisionResponses = new List<string>();
	}
}
