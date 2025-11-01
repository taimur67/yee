using System;

namespace LoG
{
	// Token: 0x02000130 RID: 304
	public static class GOAPNodeExtensions
	{
		// Token: 0x060005B0 RID: 1456 RVA: 0x0001AD9C File Offset: 0x00018F9C
		public static void AddEffectsIfAttackingDarkPylon(this GOAPNode goapNode, GamePiece targetGamePiece)
		{
			bool flag = false;
			TurnState aipreviewTurn = goapNode.OwningPlanner.AIPreviewTurn;
			if (targetGamePiece.StaticDataId == "The Dark Pylon")
			{
				foreach (GamePiece gamePiece in aipreviewTurn.GetActiveGamePiecesForPlayer(goapNode.OwningPlanner.PlayerId))
				{
					foreach (Aura aura in aipreviewTurn.AurasFromPiece(targetGamePiece))
					{
						if (aipreviewTurn.AuraAffectsControllingPlayer(gamePiece.ControllingPlayerId, aura))
						{
							if (gamePiece == goapNode.OwningPlanner.PlayerState.StrongholdId)
							{
								goapNode.AddEffect(new WPReinforceStronghold());
							}
							goapNode.AddEffect(WPCombatAdvantage.FromHealing(gamePiece, 1f));
							goapNode.AddEffect(new WPLegionTileSafety(gamePiece));
							flag = true;
						}
					}
				}
				goapNode.AddEffect(new WPUndermineArchfiend(targetGamePiece.ControllingPlayerId));
			}
			if (flag)
			{
				goapNode.AddScalarCostReduction(0.5f, PFCostModifier.Heuristic_Bonus);
			}
		}
	}
}
