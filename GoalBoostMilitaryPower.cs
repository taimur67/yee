using System;
using System.Linq;

namespace LoG
{
	// Token: 0x02000140 RID: 320
	public class GoalBoostMilitaryPower : GoalGOAPNode
	{
		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000657 RID: 1623 RVA: 0x0001FE38 File Offset: 0x0001E038
		public override ActionID ID
		{
			get
			{
				return ActionID.Goal_Increase_Military_Power;
			}
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x0001FE3C File Offset: 0x0001E03C
		public override void Prepare()
		{
			TurnState playerViewOfTurnState = this.OwningPlanner.PlayerViewOfTurnState;
			PlayerState playerState = this.OwningPlanner.PlayerState;
			PlayerState playerState2;
			float num;
			if (playerViewOfTurnState.TryGetNemesis(playerState, out playerState2, out num))
			{
				base.AddPrecondition(new WPMilitarySuperiority(playerState.Id, playerState2.Id, 0.3f));
			}
			else
			{
				foreach (PlayerState playerState3 in playerViewOfTurnState.EnumeratePlayerStatesExcept(this.OwningPlanner.PlayerId, false, false))
				{
					base.AddPrecondition(new WPMilitarySuperiority(playerState.Id, playerState3.Id, 0.5f));
				}
			}
			base.AddPrecondition(new WPOpportunisticHeal());
			foreach (GamePiece gamePiece in playerViewOfTurnState.GetAllActiveLegionsForPlayer(playerState.Id))
			{
				if (this.OwningPlanner.TerrainInfluenceMap.NumSwampHexes > 0)
				{
					base.AddPrecondition(new WPMapHazardWalk(gamePiece.Id, TerrainType.Swamp));
				}
				if (this.OwningPlanner.TerrainInfluenceMap.NumRavineHexes > 0)
				{
					base.AddPrecondition(new WPMapHazardWalk(gamePiece.Id, TerrainType.Ravine));
				}
				if (this.OwningPlanner.TerrainInfluenceMap.NumLavaHexes > 0)
				{
					base.AddPrecondition(new WPMapHazardWalk(gamePiece.Id, TerrainType.Lava));
				}
			}
			base.Prepare();
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x0001FFBC File Offset: 0x0001E1BC
		public override float CalcGoalSelectorRelevance(TurnState playerViewOfTurnState, PlayerState playerState)
		{
			if (!WPEveryTitanHasOrders.Check(this.OwningPlanner))
			{
				return 0f;
			}
			int num = playerViewOfTurnState.GetAllActiveLegionsForPlayer(playerState.Id).Count<GamePiece>();
			float num2;
			switch (num)
			{
			case 0:
				num2 = 0.7f;
				break;
			case 1:
				num2 = 0.35f;
				break;
			case 2:
				num2 = 0.2f;
				break;
			default:
				num2 = 0.05f;
				break;
			}
			float result = num2;
			ArchfiendHeuristics archfiendHeuristics = this.OwningPlanner.ArchfiendHeuristics;
			PlayerState playerState2;
			float num3;
			if (playerViewOfTurnState.TryGetNemesis(playerState, out playerState2, out num3))
			{
				float num4;
				if (!archfiendHeuristics.TryGetMilitarySuperiority(playerState2.Id, playerState.Id, out num4))
				{
					SimLogger logger = SimLogger.Logger;
					if (logger != null)
					{
						logger.Error(string.Format("Cannot retrieve military superiority for {0}->{1}", playerState2.Id, playerState.Id));
					}
					return 0f;
				}
				ref result.LerpTo01(num4 * num3);
				if (playerViewOfTurnState.CombatAuthorizedBetween(playerState.Id, playerState2.Id))
				{
					ref result.LerpTo01(-0.4f);
				}
			}
			else
			{
				float num5;
				if (!archfiendHeuristics.TryGetMilitaryPower(playerState.Id, out num5))
				{
					SimLogger logger2 = SimLogger.Logger;
					if (logger2 != null)
					{
						logger2.Error(string.Format("Cannot retrieve military power for {0}", playerState.Id));
					}
					return 0f;
				}
				ref result.LerpTo01(0.5f - num5);
			}
			foreach (AITag aitag in playerState.AITags)
			{
				if (aitag != AITag.Conqueror)
				{
					if (aitag != AITag.Duellist)
					{
						num2 = 0f;
					}
					else
					{
						num2 = -0.4f;
					}
				}
				else
				{
					num2 = 0.4f;
				}
				ref result.LerpTo01(num2);
			}
			if (playerViewOfTurnState.CheckInstigatedVendettaWithAnyPlayer(playerState, true) && num < playerState.OrderSlots)
			{
				ref result.LerpTo01(-0.5f);
			}
			if (!this.OwningPlanner.IsWinning(-2147483648) && this.OwningPlanner.GameProgress > 0.9f)
			{
				ref result.LerpTo01(-0.5f);
			}
			return result;
		}
	}
}
