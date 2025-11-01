using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200013B RID: 315
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class GoalRelevanceEntry
	{
		// Token: 0x0600063E RID: 1598 RVA: 0x0001F1E7 File Offset: 0x0001D3E7
		[JsonConstructor]
		private GoalRelevanceEntry()
		{
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x0001F1EF File Offset: 0x0001D3EF
		public GoalRelevanceEntry(ActionID actionID, GoalRelevance relevance, string actionName)
		{
			this.ActionID = actionID;
			this.Relevance = relevance;
			this.ActionName = actionName;
		}

		// Token: 0x040002F5 RID: 757
		[JsonProperty]
		public ActionID ActionID;

		// Token: 0x040002F6 RID: 758
		[JsonProperty]
		public GoalRelevance Relevance;

		// Token: 0x040002F7 RID: 759
		[JsonProperty]
		public string ActionName;
	}
}
