using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x02000139 RID: 313
	public class GoalSelector_OldestFirst : GoalSelector
	{
		// Token: 0x0600063A RID: 1594 RVA: 0x0001F0DC File Offset: 0x0001D2DC
		public override GOAPNode SelectGoal(PlayerState playerState, TurnState playerViewOfTurnState, IReadOnlyList<GOAPNode> goals, in GOAPPlanHistory history, GOAPPathfinder goapPathfinder, out List<GoalRelevanceEntry> goalDebugData, out float goalRelevance, out float goalRelevanceToBeat, AIPersistentData aiPersistentData)
		{
			goalRelevance = 0f;
			goalRelevanceToBeat = 0f;
			Dictionary<GOAPNode, int> dictionary = new Dictionary<GOAPNode, int>();
			foreach (GOAPNode goapnode in goals)
			{
				int mostRecentTurnUsageOfGoal = history.GetMostRecentTurnUsageOfGoal(goapnode.ID);
				dictionary[goapnode] = mostRecentTurnUsageOfGoal;
			}
			KeyValuePair<GOAPNode, int> keyValuePair = IEnumerableExtensions.First<KeyValuePair<GOAPNode, int>>(from kvp in dictionary
			orderby kvp.Value
			select kvp);
			goalDebugData = new List<GoalRelevanceEntry>();
			return keyValuePair.Key;
		}
	}
}
