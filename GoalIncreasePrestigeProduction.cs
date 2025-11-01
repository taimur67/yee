using System;
using System.Linq;

namespace LoG
{
	// Token: 0x02000146 RID: 326
	public class GoalIncreasePrestigeProduction : GoalGOAPNode
	{
		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000679 RID: 1657 RVA: 0x00020A40 File Offset: 0x0001EC40
		public override ActionID ID
		{
			get
			{
				return ActionID.Goal_Increase_Prestige_Production;
			}
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x00020A4C File Offset: 0x0001EC4C
		public override void Prepare()
		{
			base.AddPrecondition(new WPHasSchemes());
			base.AddPrecondition(new WPOpportunisticHeal());
			base.AddPrecondition(new WPOpportunisticSupport());
			base.AddPrecondition(new WPPrestigeProduction(WorldProperty.MaxWeight));
			base.Prepare();
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x00020A88 File Offset: 0x0001EC88
		public override float CalcGoalSelectorRelevance(TurnState playerViewOfTurnState, PlayerState playerState)
		{
			if (!WPEveryTitanHasOrders.Check(this.OwningPlanner))
			{
				return 0f;
			}
			int num;
			if (playerViewOfTurnState.CurrentDiplomaticTurn.IsVassalOfAny(playerState.Id, out num))
			{
				return 0f;
			}
			float result = 0.3f;
			int num2 = base.GameRules.GameDuration / 2;
			int num3 = Math.Abs(num2 - playerViewOfTurnState.TurnValue);
			float amount = 1f - Math.Clamp((float)num3 / (float)num2, 0f, 1f);
			ref result.LerpTo01(amount);
			if (playerViewOfTurnState.GetAllActiveLegionsForPlayer(playerState.Id).Count<GamePiece>() > 0 && !IEnumerableExtensions.Any<GamePiece>(playerViewOfTurnState.GetPlacesOfPower(playerState.Id, false)))
			{
				ref result.LerpTo01(0.9f);
			}
			float amount2;
			if (!this.OwningPlanner.ArchfiendHeuristics.TryGetEconomicPower(playerState.Id, out amount2))
			{
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					logger.Error(string.Format("Cannot retrieve economic power for {0}", playerState.Id));
				}
				return 0f;
			}
			ref result.LerpTo01(amount2);
			PlayerState playerState2;
			float num4;
			if (playerViewOfTurnState.TryGetNemesis(playerState, out playerState2, out num4))
			{
				ref result.LerpTo01(-num4 * 0.3f);
			}
			if (playerViewOfTurnState.GetConclaveFavourite().Id == playerState.Id)
			{
				ref result.LerpTo01(-0.25f);
			}
			foreach (AITag aitag in playerState.AITags)
			{
				float amount3;
				if (aitag != AITag.Iconoclast)
				{
					if (aitag == AITag.Narcissist)
					{
						amount3 = 0.4f;
					}
					else
					{
						amount3 = 0f;
					}
				}
				else
				{
					amount3 = -0.4f;
				}
				ref result.LerpTo01(amount3);
			}
			if (this.OwningPlanner.IsInvasionPending())
			{
				ref result.LerpTo01(-0.6f);
			}
			if (playerViewOfTurnState.CheckVendettaWithAnyPlayer(this.OwningPlanner.PlayerState.Id, true))
			{
				ref result.LerpTo01(-0.8f);
			}
			if (playerState.Excommunicated)
			{
				ref result.LerpTo01(-0.9f);
			}
			if (this.OwningPlanner.IsWinning(-2147483648))
			{
				ref result.LerpTo01(-0.2f);
			}
			if (this.OwningPlanner.GameProgress > 0.95f)
			{
				ref result.LerpTo01(-0.5f);
			}
			return result;
		}
	}
}
