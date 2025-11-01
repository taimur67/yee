using System;

namespace LoG
{
	// Token: 0x0200014A RID: 330
	public class GoalUndermine : GoalGOAPNode
	{
		// Token: 0x1700017F RID: 383
		// (get) Token: 0x0600068E RID: 1678 RVA: 0x00021104 File Offset: 0x0001F304
		public override ActionID ID
		{
			get
			{
				return ActionID.Goal_Undermine;
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x0600068F RID: 1679 RVA: 0x00021108 File Offset: 0x0001F308
		public override string ActionName
		{
			get
			{
				return "Goal - Undermine " + base.Context.DebugName(this.TargetPlayerId);
			}
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00021125 File Offset: 0x0001F325
		public GoalUndermine(int targetPlayerId)
		{
			this.TargetPlayerId = targetPlayerId;
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x00021134 File Offset: 0x0001F334
		public override TargetContext GetTarget()
		{
			TargetContext targetContext = new TargetContext();
			targetContext.SetTargetPlayer(this.TargetPlayerId);
			return targetContext;
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x00021147 File Offset: 0x0001F347
		public override void Prepare()
		{
			base.AddPrecondition(new WPUndermineArchfiend(this.TargetPlayerId));
			base.Prepare();
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x00021160 File Offset: 0x0001F360
		public override float CalcGoalSelectorRelevance(TurnState playerViewOfTurnState, PlayerState playerState)
		{
			float result = playerState.Animosity.GetValue(this.TargetPlayerId) * 0.5f;
			float num;
			if (!this.OwningPlanner.ArchfiendHeuristics.TryGetMilitarySuperiority(this.TargetPlayerId, playerState.Id, out num))
			{
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					logger.Error(string.Format("Cannot retrieve military superiority {0}->{1}", this.TargetPlayerId, playerState.Id));
				}
				return 0f;
			}
			if (num > 0.5f)
			{
				ref result.LerpTo01(num - 0.5f);
			}
			foreach (AITag aitag in playerState.AITags)
			{
				float amount;
				if (aitag != AITag.Assassin)
				{
					if (aitag != AITag.Peaceful)
					{
						if (aitag == AITag.Trickster)
						{
							amount = 0.6f;
						}
						else
						{
							amount = 0f;
						}
					}
					else
					{
						amount = -0.4f;
					}
				}
				else
				{
					amount = 0.2f;
				}
				ref result.LerpTo01(amount);
			}
			if (this.OwningPlanner.IsEndGame && !this.OwningPlanner.IsWinning(-2147483648))
			{
				ref result.LerpTo01(-0.4f);
			}
			if (!WPEveryTitanHasOrders.Check(this.OwningPlanner))
			{
				ref result.LerpTo01(-0.3f);
			}
			if (playerViewOfTurnState.CheckInstigatedVendettaWithAnyPlayer(playerState, false))
			{
				ref result.LerpTo01(-0.8f);
			}
			return result;
		}

		// Token: 0x04000302 RID: 770
		public int TargetPlayerId;
	}
}
