using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x0200013A RID: 314
	public class GoalSelector_Random : GoalSelector
	{
		// Token: 0x0600063C RID: 1596 RVA: 0x0001F190 File Offset: 0x0001D390
		public override GOAPNode SelectGoal(PlayerState playerState, TurnState playerViewOfTurnState, IReadOnlyList<GOAPNode> goals, in GOAPPlanHistory history, GOAPPathfinder goapPathfinder, out List<GoalRelevanceEntry> goalDebugData, out float goalRelevance, out float goalRelevanceToBeat, AIPersistentData aiPersistentData)
		{
			goalRelevance = 0f;
			goalRelevanceToBeat = 0f;
			goalDebugData = new List<GoalRelevanceEntry>();
			List<GOAPNode> list = IEnumerableExtensions.ToList<GOAPNode>(goals);
			while (list.Count > 0)
			{
				GOAPNode goapnode = list.PopRandom(playerViewOfTurnState.Random);
				if (goapnode.AreAllConstraintsMet())
				{
					return goapnode;
				}
			}
			return null;
		}
	}
}
