using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x02000144 RID: 324
	public class GoalExpandTerritory : GoalGOAPNode
	{
		// Token: 0x17000173 RID: 371
		// (get) Token: 0x0600066B RID: 1643 RVA: 0x00020798 File Offset: 0x0001E998
		public override string ActionName
		{
			get
			{
				return "Goal - Claim Neutral Cantons";
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x0600066C RID: 1644 RVA: 0x0002079F File Offset: 0x0001E99F
		public override ActionID ID
		{
			get
			{
				return ActionID.Goal_Expand_Territory;
			}
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x000207A3 File Offset: 0x0001E9A3
		public override void Prepare()
		{
			base.AddPrecondition(new WPNeutralCanton());
			base.Prepare();
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x000207B8 File Offset: 0x0001E9B8
		public override float CalcGoalSelectorRelevance(TurnState playerViewOfTurnState, PlayerState playerState)
		{
			if (!WPEveryTitanHasOrders.Check(this.OwningPlanner))
			{
				return 0f;
			}
			if (this.OwningPlanner.IsInvasionPending())
			{
				return 0f;
			}
			float result = 0.1f;
			int gameDuration = base.GameRules.GameDuration;
			float num = 1f - Math.Clamp((float)playerViewOfTurnState.TurnValue / (float)gameDuration, 0f, 1f);
			ref result.LerpTo01(num * num * 0.95f);
			if ((from t in playerViewOfTurnState.HexBoard.GetAllHexes()
			where playerViewOfTurnState.HexBoard[t.HexCoord].ControllingPlayerID == playerState.Id
			select t).Count<Hex>() <= 10)
			{
				ref result.LerpTo01(0.2f);
			}
			float unclaimedCantonProportion = this.OwningPlanner.TerrainInfluenceMap.UnclaimedCantonProportion;
			ref result.LerpTo01(unclaimedCantonProportion * unclaimedCantonProportion);
			PlayerState playerState2;
			float num2;
			if (playerViewOfTurnState.TryGetNemesis(playerState, out playerState2, out num2))
			{
				ref result.LerpTo01(-num2);
			}
			using (List<AITag>.Enumerator enumerator = playerState.AITags.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					float amount;
					if (enumerator.Current == AITag.Conqueror)
					{
						amount = 0.4f;
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
			if (WPNeutralTitanOnWarpath.Check(playerViewOfTurnState))
			{
				ref result.LerpTo01(-0.8f);
			}
			return result;
		}
	}
}
