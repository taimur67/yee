using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x02000135 RID: 309
	public class ActionHistory
	{
		// Token: 0x06000605 RID: 1541 RVA: 0x0001D95E File Offset: 0x0001BB5E
		public bool IsFallbackHistory()
		{
			return !string.IsNullOrEmpty(this.FallbackAction);
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x0001D96E File Offset: 0x0001BB6E
		public bool IsGrandEventHistory()
		{
			return !string.IsNullOrEmpty(this.GrandEvent);
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x0001D980 File Offset: 0x0001BB80
		public string GetTitle()
		{
			if (this.IsFallbackHistory())
			{
				return "Fallback: " + this.FallbackAction;
			}
			if (this.IsGrandEventHistory())
			{
				return "GrandEvent: " + this.GrandEvent;
			}
			GOAPNode action = this.Action;
			return ((action != null) ? action.ActionName : null) ?? "None";
		}

		// Token: 0x040002DA RID: 730
		public GOAPNode Goal;

		// Token: 0x040002DB RID: 731
		public GOAPNode Action;

		// Token: 0x040002DC RID: 732
		public List<GOAPDebugInfo> ActionPlan = new List<GOAPDebugInfo>();

		// Token: 0x040002DD RID: 733
		public int Turn;

		// Token: 0x040002DE RID: 734
		public List<GoalRelevanceEntry> GoalDebugData = new List<GoalRelevanceEntry>();

		// Token: 0x040002DF RID: 735
		public string FallbackAction;

		// Token: 0x040002E0 RID: 736
		public string GrandEvent;

		// Token: 0x040002E1 RID: 737
		public float GoalRelevanceScoreToBeat;

		// Token: 0x040002E2 RID: 738
		public string CurrentGoalAtTimeOfPlanning;
	}
}
