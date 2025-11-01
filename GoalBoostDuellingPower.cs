using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x0200013F RID: 319
	public class GoalBoostDuellingPower : GoalGOAPNode
	{
		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000653 RID: 1619 RVA: 0x0001FC54 File Offset: 0x0001DE54
		public override ActionID ID
		{
			get
			{
				return ActionID.Goal_Increase_Duelling;
			}
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x0001FC58 File Offset: 0x0001DE58
		public GoalBoostDuellingPower(int targetPlayerId)
		{
			this.TargetPlayerID = targetPlayerId;
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x0001FC67 File Offset: 0x0001DE67
		public override void Prepare()
		{
			base.Prepare();
			base.AddPrecondition(new WPDuelAdvantage(this.TargetPlayerID));
			base.AddPrecondition(new WPHasAnyPraetor());
			base.Prepare();
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x0001FC94 File Offset: 0x0001DE94
		public override float CalcGoalSelectorRelevance(TurnState playerViewOfTurnState, PlayerState playerState)
		{
			int num;
			if (playerViewOfTurnState.CurrentDiplomaticTurn.IsVassalOfAny(playerState.Id, out num))
			{
				return 0f;
			}
			if (!WPEveryTitanHasOrders.Check(this.OwningPlanner))
			{
				return 0f;
			}
			if (this.OwningPlanner.IsInvasionPending())
			{
				return 0f;
			}
			if (playerState.Excommunicated)
			{
				return 0f;
			}
			if (this.OwningPlanner.IsEndGame && !this.OwningPlanner.IsWinning(-2147483648))
			{
				return 0f;
			}
			float result = WPHasAnyPraetor.Check(playerViewOfTurnState, playerState.Id) ? 0.1f : 0.6f;
			PlayerState them;
			float num2;
			if (playerViewOfTurnState.TryGetNemesis(playerState, out them, out num2))
			{
				float num3;
				if (!this.OwningPlanner.PraetorHeuristics.TryGetDuelAdvantage(playerState, them, out num3))
				{
					SimLogger logger = SimLogger.Logger;
					if (logger != null)
					{
						logger.Error("Could not retrieve duel advantage");
					}
					return 0f;
				}
				float num4 = 1f - num3;
				ref result.LerpTo01(num2 * num4);
			}
			using (List<AITag>.Enumerator enumerator = playerState.AITags.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					float amount;
					if (enumerator.Current == AITag.Duellist)
					{
						amount = 0.5f;
					}
					else
					{
						amount = 0f;
					}
					ref result.LerpTo01(amount);
				}
			}
			if (playerViewOfTurnState.CheckVendettaWithAnyPlayer(this.OwningPlanner.PlayerState.Id, true))
			{
				ref result.LerpTo01(-0.8f);
			}
			if (!this.OwningPlanner.IsWinning(-2147483648) && this.OwningPlanner.GameProgress > 0.9f)
			{
				ref result.LerpTo01(-0.5f);
			}
			return result;
		}

		// Token: 0x040002FA RID: 762
		public int TargetPlayerID;
	}
}
