using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x02000149 RID: 329
	public class GoalPursueScheme : GoalGOAPNode
	{
		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000689 RID: 1673 RVA: 0x00021073 File Offset: 0x0001F273
		public override ActionID ID
		{
			get
			{
				return ActionID.Goal_Pursue_Scheme;
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x0600068A RID: 1674 RVA: 0x00021077 File Offset: 0x0001F277
		public override string ActionName
		{
			get
			{
				return "Goal - Pursue Scheme";
			}
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x00021086 File Offset: 0x0001F286
		public override void Prepare()
		{
			base.AddPrecondition(new WPContributesToScheme());
			base.Prepare();
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x0002109C File Offset: 0x0001F29C
		public override float CalcGoalSelectorRelevance(TurnState playerViewOfTurnState, PlayerState playerState)
		{
			float num = 0.75f;
			float result = 0.75f;
			using (IEnumerator<ObjectiveCondition> enumerator = playerState.EnumerateActiveObjectives(playerViewOfTurnState).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Progress >= num)
					{
						return result;
					}
				}
			}
			return 0.33f;
		}
	}
}
