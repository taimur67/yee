using System;

namespace LoG
{
	// Token: 0x02000148 RID: 328
	public class GoalPrepareInvasion : GoalGOAPNode
	{
		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000681 RID: 1665 RVA: 0x00020F3C File Offset: 0x0001F13C
		public override ActionID ID
		{
			get
			{
				return ActionID.Goal_Prepare_Invasion;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000682 RID: 1666 RVA: 0x00020F40 File Offset: 0x0001F140
		public override string ActionName
		{
			get
			{
				return "Goal - Prepare invasion " + base.Context.DebugName(this._aggressorPlayerID) + " vs " + base.Context.DebugName(this._targetPlayerID);
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000683 RID: 1667 RVA: 0x00020F73 File Offset: 0x0001F173
		protected override bool IsFulfilledByMovingOutOfDanger
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000684 RID: 1668 RVA: 0x00020F76 File Offset: 0x0001F176
		public override bool DoDynamicScoring
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x00020F79 File Offset: 0x0001F179
		public GoalPrepareInvasion(int aggressor, int target)
		{
			this._aggressorPlayerID = aggressor;
			this._targetPlayerID = target;
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x00020F8F File Offset: 0x0001F18F
		public override TargetContext GetTarget()
		{
			TargetContext targetContext = new TargetContext();
			targetContext.SetTargetPlayer(this._targetPlayerID);
			return targetContext;
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x00020FA2 File Offset: 0x0001F1A2
		public override void Prepare()
		{
			base.AddPrecondition(new WPThreaten(this.OwningPlanner.PlayerId, this._targetPlayerID));
			base.Prepare();
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x00020FC8 File Offset: 0x0001F1C8
		public override float CalcGoalSelectorRelevance(TurnState playerViewOfTurnState, PlayerState playerState)
		{
			if (ActionDraconicRazzia.CanBeUsedByArchfiend(playerState) && playerViewOfTurnState.GetDiplomaticStatus(playerState.Id, this._targetPlayerID).DiplomaticState is PendingDiplomacy_DraconicRazzia)
			{
				return 1f;
			}
			float result = 0.5f;
			if (this._aggressorPlayerID == playerState.Id)
			{
				ref result.LerpTo01(0.5f);
			}
			float value = playerState.Animosity.GetValue(this._targetPlayerID);
			ref result.LerpTo01(value * 0.95f);
			if (!WPEveryTitanHasOrders.Check(this.OwningPlanner))
			{
				ref result.LerpTo01(0.4f);
			}
			if (WPNeutralTitanOnWarpath.Check(playerViewOfTurnState))
			{
				ref result.LerpTo01(-0.6f);
			}
			return result;
		}

		// Token: 0x04000300 RID: 768
		private int _aggressorPlayerID;

		// Token: 0x04000301 RID: 769
		private int _targetPlayerID;
	}
}
