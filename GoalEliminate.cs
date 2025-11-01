using System;

namespace LoG
{
	// Token: 0x02000143 RID: 323
	public class GoalEliminate : GoalGOAPNode
	{
		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000664 RID: 1636 RVA: 0x0002053C File Offset: 0x0001E73C
		public override ActionID ID
		{
			get
			{
				return ActionID.Goal_Eliminate_Opponent;
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000665 RID: 1637 RVA: 0x00020540 File Offset: 0x0001E740
		public override string ActionName
		{
			get
			{
				return "Goal - Eliminate " + base.Context.DebugName(this.TargetPlayerId);
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000666 RID: 1638 RVA: 0x0002055D File Offset: 0x0001E75D
		protected override bool IsFulfilledByMovingOutOfDanger
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x00020560 File Offset: 0x0001E760
		public GoalEliminate(int targetPlayerId)
		{
			this.TargetPlayerId = targetPlayerId;
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x0002056F File Offset: 0x0001E76F
		public override TargetContext GetTarget()
		{
			TargetContext targetContext = new TargetContext();
			targetContext.SetTargetPlayer(this.TargetPlayerId);
			return targetContext;
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x00020582 File Offset: 0x0001E782
		public override void Prepare()
		{
			base.AddPrecondition(new WPArchfiendEliminated(this.TargetPlayerId));
			base.Prepare();
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x0002059C File Offset: 0x0001E79C
		public override float CalcGoalSelectorRelevance(TurnState playerViewOfTurnState, PlayerState playerState)
		{
			float result = 0.1f;
			float value = playerState.Animosity.GetValue(this.TargetPlayerId);
			ref result.LerpTo01(value);
			ArchfiendHeuristics archfiendHeuristics = this.OwningPlanner.ArchfiendHeuristics;
			float num;
			float num2;
			if (!archfiendHeuristics.TryGetMilitarySuperiority(this.TargetPlayerId, playerState.Id, out num) || !archfiendHeuristics.TryGetRitualSuperiority(this.TargetPlayerId, playerState.Id, out num2))
			{
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					logger.Error(string.Format("Cannot retrieve military / ritual superiority {0}->{1}", this.TargetPlayerId, playerState.Id));
				}
				return 0f;
			}
			float num3 = (num + num2) / 2f;
			if (num3 > 0.5f)
			{
				ref result.LerpTo01(0.5f - num3);
			}
			if (!WPEveryTitanHasOrders.Check(this.OwningPlanner))
			{
				ref result.LerpTo01(0.3f);
			}
			foreach (AITag aitag in playerState.AITags)
			{
				float amount;
				if (aitag != AITag.Assassin)
				{
					if (aitag != AITag.Diplomat)
					{
						if (aitag != AITag.Peaceful)
						{
							amount = 0f;
						}
						else
						{
							amount = -0.4f;
						}
					}
					else
					{
						amount = -0.4f;
					}
				}
				else
				{
					amount = 0.4f;
				}
				ref result.LerpTo01(amount);
			}
			if (this.OwningPlanner.IsWinning(-2147483648))
			{
				ref result.LerpTo01(-0.5f);
			}
			else
			{
				ref result.LerpTo01(this.OwningPlanner.GameProgress * 0.9f);
			}
			if (playerState.OrderSlots.Value < 4)
			{
				ref result.LerpTo01(-0.5f);
			}
			if (playerViewOfTurnState.CheckInstigatedVendettaWithAnyPlayer(playerState, false))
			{
				ref result.LerpTo01(-0.8f);
			}
			if (WPNeutralTitanOnWarpath.Check(playerViewOfTurnState))
			{
				ref result.LerpTo01(-0.6f);
			}
			if (this.OwningPlanner.IsEndGame && playerState.Excommunicated)
			{
				ref result.LerpTo01(0.9f);
			}
			return result;
		}

		// Token: 0x040002FE RID: 766
		public int TargetPlayerId;
	}
}
