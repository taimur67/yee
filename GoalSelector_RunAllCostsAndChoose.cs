using System;
using System.Collections.Generic;
using System.Linq;
using LoG.Simulation.Extensions;

namespace LoG
{
	// Token: 0x0200013C RID: 316
	public class GoalSelector_RunAllCostsAndChoose : GoalSelector
	{
		// Token: 0x06000640 RID: 1600 RVA: 0x0001F20C File Offset: 0x0001D40C
		private bool IsPathInvalid(List<GOAPNode> path)
		{
			return path == null || path.Count < 1 || (path.Count <= 1 && path[0] is GoalGOAPNode);
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x0001F23C File Offset: 0x0001D43C
		public static float GetTargetRelevanceScore(bool haveCurrentGoal, float currentGoalScore, AIPersistentData aiPersistentData, TurnState playerViewOfTurnState)
		{
			int num = playerViewOfTurnState.TurnValue - aiPersistentData.TurnGoalLastChanged;
			if (haveCurrentGoal)
			{
				return Math.Clamp(currentGoalScore + MathF.Pow(GoalSelector_RunAllCostsAndChoose.Stubbornness, (float)num), 0f, 1f);
			}
			return 0f;
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x0001F280 File Offset: 0x0001D480
		public override GOAPNode SelectGoal(PlayerState playerState, TurnState playerViewOfTurnState, IReadOnlyList<GOAPNode> goals, in GOAPPlanHistory history, GOAPPathfinder goapPathfinder, out List<GoalRelevanceEntry> goalDebugData, out float goalRelevance, out float goalRelevanceToBeat, AIPersistentData aiPersistentData)
		{
			Dictionary<GOAPNode, float> dictionary = new Dictionary<GOAPNode, float>();
			goalDebugData = new List<GoalRelevanceEntry>();
			GOAPNode goapnode = null;
			float num = 0f;
			foreach (GoalGOAPNode goalGOAPNode in goals.OfType<GoalGOAPNode>())
			{
				bool flag = aiPersistentData.IsPreviouslySelectedGoal(goalGOAPNode);
				if (flag)
				{
					goapnode = goalGOAPNode;
				}
				List<GOAPNode> list;
				if (!goapPathfinder.TryFindPath(goalGOAPNode, null, null, out list, null) || this.IsPathInvalid(list))
				{
					if (flag)
					{
						goapnode = null;
					}
				}
				else
				{
					float pathCost = 0f;
					foreach (GOAPNode goapnode2 in list)
					{
						if (!(goapnode2 is GoalGOAPNode))
						{
							pathCost = goapnode2.GetTotalCost();
						}
					}
					float num2 = goalGOAPNode.CalcGoalSelectorRelevance(playerViewOfTurnState, playerState);
					float num3 = num2;
					if (flag)
					{
						num = num3;
					}
					dictionary.Add(goalGOAPNode, num3);
					goalDebugData.Add(new GoalRelevanceEntry(goalGOAPNode.ID, new GoalRelevance(num2, pathCost, num3), goalGOAPNode.ActionName));
				}
			}
			goalRelevanceToBeat = GoalSelector_RunAllCostsAndChoose.GetTargetRelevanceScore(goapnode != null, num, aiPersistentData, playerViewOfTurnState);
			if (goapnode == null)
			{
				KeyValuePair<GOAPNode, float> keyValuePair = dictionary.SelectMaxOrDefault((KeyValuePair<GOAPNode, float> t) => t.Value, default(KeyValuePair<GOAPNode, float>));
				goalRelevance = keyValuePair.Value;
				return keyValuePair.Key;
			}
			float minScoreForChange = goalRelevanceToBeat;
			KeyValuePair<GOAPNode, float> keyValuePair2 = (from t in dictionary
			where t.Value >= minScoreForChange
			select t).SelectMaxOrDefault((KeyValuePair<GOAPNode, float> t) => t.Value, default(KeyValuePair<GOAPNode, float>));
			if (keyValuePair2.Key != null)
			{
				goalRelevance = keyValuePair2.Value;
				return keyValuePair2.Key;
			}
			goalRelevance = num;
			return goapnode;
		}

		// Token: 0x040002F8 RID: 760
		public static float Stubbornness = 0.2f;
	}
}
