using System;

namespace LoG
{
	// Token: 0x02000142 RID: 322
	public class GoalConquerPandaemonium : GoalGOAPNode
	{
		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000660 RID: 1632 RVA: 0x0002036C File Offset: 0x0001E56C
		public override ActionID ID
		{
			get
			{
				return ActionID.Goal_Conquer_Pandaemonium;
			}
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x00020370 File Offset: 0x0001E570
		public GoalConquerPandaemonium(Identifier pandaID)
		{
			this.PandaID = pandaID;
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x0002037F File Offset: 0x0001E57F
		public override void Prepare()
		{
			base.AddPrecondition(new WPHasTitan());
			base.AddPrecondition(new WPSpecificPopCaptured(this.PandaID));
			base.AddPrecondition(new WPOpportunisticHeal());
			base.AddPrecondition(new WPOpportunisticSupport());
			base.Prepare();
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x000203BC File Offset: 0x0001E5BC
		public override float CalcGoalSelectorRelevance(TurnState playerViewOfTurnState, PlayerState playerState)
		{
			if (this.OwningPlanner.IsWinning(-2147483648))
			{
				return 0f;
			}
			if (playerViewOfTurnState.CheckInstigatedVendettaWithAnyPlayer(playerState, false))
			{
				return 0f;
			}
			GamePiece gamePiece = playerViewOfTurnState.FetchGameItem<GamePiece>(this.PandaID);
			int num;
			if (gamePiece == null || !this.OwningPlanner.ArchfiendHeuristics.CouldReachGamePieceIfOwnersTerritoryCouldBeCrossed(playerState.Id, gamePiece, out num))
			{
				return 0f;
			}
			Identifier identifier;
			float num2;
			if (this.OwningPlanner.ArchfiendHeuristics.TryGetBestLegionToAttackPandaemonium(playerState.Id, out identifier, out num2) && num2 > 0.6f)
			{
				return 1f;
			}
			if (playerViewOfTurnState.CheckVendettaWithAnyPlayer(this.OwningPlanner.PlayerState.Id, false))
			{
				return 0f;
			}
			int gameDuration = base.GameRules.GameDuration;
			float result = 0.9f * Math.Clamp((float)playerViewOfTurnState.TurnValue / (float)gameDuration, 0f, 1f);
			foreach (AITag aitag in playerState.AITags)
			{
				float amount;
				if (aitag != AITag.Iconoclast)
				{
					if (aitag != AITag.Narcissist)
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
					amount = 0.6f;
				}
				ref result.LerpTo01(amount);
			}
			if (WPNeutralTitanOnWarpath.Check(playerViewOfTurnState))
			{
				ref result.LerpTo01(-0.4f);
			}
			if (!WPEveryTitanHasOrders.Check(this.OwningPlanner))
			{
				ref result.LerpTo01(0.3f);
			}
			return result;
		}

		// Token: 0x040002FD RID: 765
		public Identifier PandaID;
	}
}
