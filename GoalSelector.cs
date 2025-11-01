using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x02000138 RID: 312
	public abstract class GoalSelector
	{
		// Token: 0x06000638 RID: 1592
		public abstract GOAPNode SelectGoal(PlayerState playerState, TurnState playerViewOfTurnState, IReadOnlyList<GOAPNode> goals, in GOAPPlanHistory history, GOAPPathfinder goapPathfinder, out List<GoalRelevanceEntry> goalDebugData, out float goalRelevance, out float goalRelevanceToBeat, AIPersistentData aiPersistentData);
	}
}
