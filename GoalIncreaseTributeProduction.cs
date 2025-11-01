using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x02000147 RID: 327
	public class GoalIncreaseTributeProduction : GoalGOAPNode
	{
		// Token: 0x17000178 RID: 376
		// (get) Token: 0x0600067D RID: 1661 RVA: 0x00020CD0 File Offset: 0x0001EED0
		public override ActionID ID
		{
			get
			{
				return ActionID.Goal_Increase_Tribute_Production;
			}
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x00020CD4 File Offset: 0x0001EED4
		public override void Prepare()
		{
			base.AddPrecondition(new WPOpportunisticHeal());
			base.AddPrecondition(new WPTributeBoost());
			base.AddPrecondition(new WPTributeDraw(this.MaxDraw));
			base.Prepare();
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x00020D04 File Offset: 0x0001EF04
		public override float CalcGoalSelectorRelevance(TurnState playerViewOfTurnState, PlayerState playerState)
		{
			if (playerState.Resources.Count > 48)
			{
				return 0f;
			}
			if (!WPEveryTitanHasOrders.Check(this.OwningPlanner))
			{
				return 0f;
			}
			float result = 0.3f;
			int gameDuration = base.GameRules.GameDuration;
			float num = 1f - Math.Clamp((float)playerViewOfTurnState.TurnValue / (float)gameDuration, 0f, 1f);
			ref result.LerpTo01(num * 0.8f);
			float num2;
			if (!this.OwningPlanner.ArchfiendHeuristics.TryGetEconomicPower(playerState.Id, out num2))
			{
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					logger.Error(string.Format("Cannot retrieve economic power for {0}", playerState.Id));
				}
				return 0f;
			}
			float num3 = 1f - num2;
			ref result.LerpTo01(num3 * 0.9f);
			if (playerViewOfTurnState.GetConclaveFavourite().Id == playerState.Id)
			{
				ref result.LerpTo01(0.35f);
			}
			using (List<AITag>.Enumerator enumerator = playerState.AITags.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					float amount;
					switch (enumerator.Current)
					{
					case AITag.Assassin:
						amount = -0.2f;
						break;
					case AITag.Conqueror:
						amount = -0.2f;
						break;
					case AITag.Diplomat:
					case AITag.Duellist:
						goto IL_137;
					case AITag.Hoarder:
						amount = 0.6f;
						break;
					default:
						goto IL_137;
					}
					IL_13E:
					ref result.LerpTo01(amount);
					continue;
					IL_137:
					amount = 0f;
					goto IL_13E;
				}
			}
			PlayerState playerState2;
			float num4;
			if (playerViewOfTurnState.TryGetNemesis(playerState, out playerState2, out num4))
			{
				ref result.LerpTo01(-0.3f * num4);
			}
			if (!IEnumerableExtensions.Any<GamePiece>(playerViewOfTurnState.GetAllActiveLegionsForPlayer(playerState.Id)))
			{
				ref result.LerpTo01(-0.5f);
			}
			if (this.OwningPlanner.IsInvasionPending())
			{
				return -0.6f;
			}
			if (playerViewOfTurnState.CheckVendettaWithAnyPlayer(this.OwningPlanner.PlayerState.Id, true))
			{
				ref result.LerpTo01(-0.8f);
			}
			if (!this.OwningPlanner.IsWinning(-2147483648) && this.OwningPlanner.GameProgress > 0.8f)
			{
				ref result.LerpTo01(-0.5f);
			}
			return result;
		}

		// Token: 0x040002FF RID: 767
		public int MaxDraw = 10;
	}
}
