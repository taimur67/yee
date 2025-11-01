using System;

namespace LoG
{
	// Token: 0x0200013E RID: 318
	public class GoalAvoidElimination : GoalGOAPNode
	{
		// Token: 0x17000169 RID: 361
		// (get) Token: 0x0600064D RID: 1613 RVA: 0x0001F840 File Offset: 0x0001DA40
		public override ActionID ID
		{
			get
			{
				return ActionID.Goal_Avoid_Elimination;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x0600064E RID: 1614 RVA: 0x0001F844 File Offset: 0x0001DA44
		public override string ActionName
		{
			get
			{
				return "Goal - Avoid elimination";
			}
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x0001F853 File Offset: 0x0001DA53
		public override void Prepare()
		{
			base.AddPrecondition(new WPReducedDiplomaticStress());
			base.AddPrecondition(new WPOpportunisticHeal());
			base.AddPrecondition(new WPOpportunisticSupport());
			base.AddPrecondition(new WPReinforceStronghold());
			base.Prepare();
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x0001F888 File Offset: 0x0001DA88
		public static bool AnyThreat(GOAPPlanner planner)
		{
			PlayerState playerState = planner.PlayerState;
			if (playerState.Excommunicated)
			{
				return true;
			}
			TurnState trueTurn = planner.TrueTurn;
			if (trueTurn.IsPlayerDisgraced(playerState.Id))
			{
				return true;
			}
			foreach (PlayerState playerState2 in trueTurn.EnumeratePlayerStates(false, false))
			{
				if (playerState2.Id != playerState.Id)
				{
					if (trueTurn.CurrentDiplomaticTurn.GetDiplomaticState(new PlayerPair(playerState.Id, playerState2.Id)).AllowStrongholdCapture(trueTurn.CurrentDiplomaticTurn, playerState.Id, playerState2.Id))
					{
						return true;
					}
					float num;
					if (!planner.ArchfiendHeuristics.TryGetMilitarySuperiority(playerState2.Id, playerState.Id, out num))
					{
						SimLogger logger = SimLogger.Logger;
						if (logger != null)
						{
							logger.Error(string.Format("Cannot retrieve superiority {0}->{1}", playerState2.Id, playerState.Id));
						}
					}
					else if (num >= 0.6f && planner.TrueContext.HasEnoughVendettasForBloodFeud(playerState2, playerState.Id))
					{
						return true;
					}
				}
			}
			PlayerState playerState3;
			if (trueTurn.TryGetNemesis(playerState, out playerState3))
			{
				float num2;
				if (!planner.ArchfiendHeuristics.TryGetMilitarySuperiority(playerState3.Id, playerState.Id, out num2))
				{
					SimLogger logger2 = SimLogger.Logger;
					if (logger2 != null)
					{
						logger2.Error(string.Format("Cannot retrieve superiority {0}->{1}", playerState3.Id, playerState.Id));
					}
					return false;
				}
				if (num2 < 0.7f)
				{
					return false;
				}
				float num3;
				if (!planner.ArchfiendHeuristics.TryGetDiplomaticStress(playerState3.Id, out num3))
				{
					SimLogger logger3 = SimLogger.Logger;
					if (logger3 != null)
					{
						logger3.Error(string.Format("Cannot retrieve {0} diplomatic stress", playerState3.Id));
					}
					return false;
				}
				if (num3 > 0.4f)
				{
					return false;
				}
				if (planner.ArchfiendHeuristics.GetThreatens(playerState3.Id, playerState.Id))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x0001FA94 File Offset: 0x0001DC94
		public override float CalcGoalSelectorRelevance(TurnState playerViewOfTurnState, PlayerState playerState)
		{
			float result = 0f;
			ArchfiendHeuristics archfiendHeuristics = this.OwningPlanner.ArchfiendHeuristics;
			float num;
			if (!archfiendHeuristics.TryGetDiplomaticStress(playerState.Id, out num))
			{
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					logger.Error(string.Format("Cannot retrieve diplomatic stress for {0}", playerState.Id));
				}
				return 0f;
			}
			float num2;
			if (!archfiendHeuristics.TryGetMilitaryPower(playerState.Id, out num2))
			{
				SimLogger logger2 = SimLogger.Logger;
				if (logger2 != null)
				{
					logger2.Error(string.Format("Cannot retrieve military power for {0}", playerState.Id));
				}
				return 0f;
			}
			ref result.LerpTo01(num * 0.5f);
			ref result.LerpTo01((1f - num2) * 0.5f);
			if (!GoalAvoidElimination.AnyThreat(this.OwningPlanner))
			{
				ref result.LerpTo01(-0.5f);
			}
			else
			{
				foreach (PlayerState playerState2 in playerViewOfTurnState.EnumeratePlayerStates(false, false))
				{
					if (playerState2.Id != playerState.Id && archfiendHeuristics.GetThreatens(playerState2.Id, playerState.Id))
					{
						ref result.LerpTo01(0.2f);
					}
				}
			}
			foreach (AITag aitag in playerState.AITags)
			{
				float amount;
				if (aitag != AITag.Conqueror)
				{
					if (aitag != AITag.Diplomat)
					{
						if (aitag == AITag.Peaceful)
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
						amount = 0.3f;
					}
				}
				else
				{
					amount = -0.3f;
				}
				ref result.LerpTo01(amount);
			}
			return result;
		}
	}
}
