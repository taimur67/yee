using System;

namespace LoG
{
	// Token: 0x02000145 RID: 325
	public abstract class GoalGOAPNode : GOAPNode
	{
		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000670 RID: 1648 RVA: 0x00020970 File Offset: 0x0001EB70
		public override GOAPNodeType NodeType
		{
			get
			{
				return GOAPNodeType.Goal;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000671 RID: 1649 RVA: 0x00020973 File Offset: 0x0001EB73
		protected virtual bool IsFulfilledByMovingOutOfDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x00020976 File Offset: 0x0001EB76
		protected GoalGOAPNode()
		{
			base.BaseCost = GoapNodeCosts.BaseGoal;
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x00020989 File Offset: 0x0001EB89
		public override Result SubmitAction(TurnContext context, PlayerState playerState)
		{
			return Result.Failure;
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x00020990 File Offset: 0x0001EB90
		public override bool ShouldIncludeInPath()
		{
			return false;
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x00020993 File Offset: 0x0001EB93
		public virtual float CalcGoalSelectorRelevance(TurnState playerViewOfTurnState, PlayerState playerState)
		{
			return 0.5f;
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x0002099A File Offset: 0x0001EB9A
		public virtual TargetContext GetTarget()
		{
			return null;
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x0002099D File Offset: 0x0001EB9D
		public override void Prepare()
		{
			if (this.OwningPlanner.PlayerState.ArchfiendId == "Belphegor")
			{
				base.AddPrecondition(new WPChokingMap());
			}
			if (this.IsFulfilledByMovingOutOfDanger)
			{
				this.GenerateLegionInDangerPreconditions();
			}
			base.Prepare();
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x000209DC File Offset: 0x0001EBDC
		private void GenerateLegionInDangerPreconditions()
		{
			foreach (Identifier legionID in this.OwningPlanner.AITransientData.LegionsInDangerousPositions)
			{
				base.AddPrecondition(new WPLegionTileSafety(legionID));
			}
		}
	}
}
