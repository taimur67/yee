using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x0200013D RID: 317
	public class GoalAttackPlayer : GoalGOAPNode
	{
		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000645 RID: 1605 RVA: 0x0001F498 File Offset: 0x0001D698
		public override ActionID ID
		{
			get
			{
				return ActionID.Goal_Attack_Player;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000646 RID: 1606 RVA: 0x0001F49C File Offset: 0x0001D69C
		public override string ActionName
		{
			get
			{
				return "Goal - Attack Player " + base.Context.DebugName(this.TargetPlayerID);
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000647 RID: 1607 RVA: 0x0001F4B9 File Offset: 0x0001D6B9
		protected override bool IsFulfilledByMovingOutOfDanger
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000648 RID: 1608 RVA: 0x0001F4BC File Offset: 0x0001D6BC
		public override bool DoDynamicScoring
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x0001F4BF File Offset: 0x0001D6BF
		public GoalAttackPlayer(int target)
		{
			this.TargetPlayerID = target;
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x0001F4CE File Offset: 0x0001D6CE
		public override TargetContext GetTarget()
		{
			TargetContext targetContext = new TargetContext();
			targetContext.SetTargetPlayer(this.TargetPlayerID);
			return targetContext;
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x0001F4E1 File Offset: 0x0001D6E1
		public override void Prepare()
		{
			base.AddPrecondition(new WPOpportunisticSupport());
			base.AddPrecondition(new WPCombatVsPlayer(this.TargetPlayerID));
			base.Prepare();
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x0001F508 File Offset: 0x0001D708
		public override float CalcGoalSelectorRelevance(TurnState playerViewOfTurnState, PlayerState playerState)
		{
			float result = 0.05f;
			float value = playerState.Animosity.GetValue(this.TargetPlayerID);
			ref result.LerpTo01(value * 0.8f);
			if (ActionDraconicRazzia.CanBeUsedByArchfiend(playerState) && playerViewOfTurnState.GetDiplomaticStatus(playerState.Id, this.TargetPlayerID).DiplomaticState is DiplomaticState_DraconicRazzia)
			{
				ref result.LerpTo01(0.9f);
			}
			ArchfiendHeuristics archfiendHeuristics = this.OwningPlanner.ArchfiendHeuristics;
			float num;
			float num2;
			if (!archfiendHeuristics.TryGetMilitarySuperiority(this.TargetPlayerID, playerState.Id, out num) || !archfiendHeuristics.TryGetRitualSuperiority(this.TargetPlayerID, playerState.Id, out num2))
			{
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					logger.Error(string.Format("Cannot retrieve military / ritual superiority {0}->{1}", this.TargetPlayerID, playerState.Id));
				}
				return 0f;
			}
			float num3 = (num + num2) / 2f;
			if (num3 > 0.5f)
			{
				ref result.LerpTo01(0.5f - num3);
			}
			float num4;
			float num5;
			if (!archfiendHeuristics.TryGetDiplomaticStress(playerState.Id, out num4) || !archfiendHeuristics.TryGetDiplomaticStress(this.TargetPlayerID, out num5))
			{
				SimLogger logger2 = SimLogger.Logger;
				if (logger2 != null)
				{
					logger2.Error(string.Format("Cannot retrieve diplomatic stress of {0} and {1}", this.TargetPlayerID, playerState.Id));
				}
				return 0f;
			}
			float num6 = num4 - num5;
			if (num6 > 0f)
			{
				ref result.LerpTo01(-num6);
			}
			float num7;
			if (!archfiendHeuristics.TryGetTerritorialCohesion(this.TargetPlayerID, out num7))
			{
				SimLogger logger3 = SimLogger.Logger;
				if (logger3 != null)
				{
					logger3.Error(string.Format("Cannot retrieve territorial cohesion of {0}", this.TargetPlayerID));
				}
				return 0f;
			}
			if (num7 < 0.5f)
			{
				ref result.LerpTo01(num7 - 0.5f);
			}
			if (!WPEveryTitanHasOrders.Check(this.OwningPlanner))
			{
				ref result.LerpTo01(0.4f);
			}
			using (List<AITag>.Enumerator enumerator = playerState.AITags.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					float amount;
					switch (enumerator.Current)
					{
					case AITag.Assassin:
						amount = 0.4f;
						break;
					case AITag.Conqueror:
						amount = 0.2f;
						break;
					case AITag.Diplomat:
						amount = -0.4f;
						break;
					default:
						amount = 0f;
						break;
					}
					ref result.LerpTo01(amount);
				}
			}
			TurnContext trueContext = this.OwningPlanner.TrueContext;
			if (RivalryProcessor.IsStrongholdChoking(trueContext, playerState) && RivalryProcessor.IsSourceOfChoking(trueContext, this.TargetPlayerID))
			{
				ref result.LerpTo01(0.3f);
			}
			else
			{
				if (this.OwningPlanner.IsWinning(-2147483648))
				{
					ref result.LerpTo01(-0.5f);
				}
				if (this.OwningPlanner.GameProgress > 0.3f && playerState.OrderSlots.Value < 3)
				{
					ref result.LerpTo01(-0.5f);
				}
			}
			if (playerViewOfTurnState.CombatAuthorizedBetween(playerState.Id, this.TargetPlayerID))
			{
				ref result.LerpTo01(0.8f);
			}
			if (WPNeutralTitanOnWarpath.Check(playerViewOfTurnState))
			{
				ref result.LerpTo01(-0.6f);
			}
			if (this.OwningPlanner.IsEndGame && playerState.Excommunicated)
			{
				ref result.LerpTo01(0.5f);
			}
			return result;
		}

		// Token: 0x040002F9 RID: 761
		public int TargetPlayerID;
	}
}
