using System;
using System.Collections.Generic;
using System.Linq;
using Game.StaticData;

namespace LoG
{
	// Token: 0x020006F3 RID: 1779
	public static class VictoryProcessor
	{
		// Token: 0x06002220 RID: 8736 RVA: 0x00076D51 File Offset: 0x00074F51
		public static void EliminatePlayer(this TurnProcessContext context, PlayerState player)
		{
			context.EliminatePlayer(player, context.CurrentTurn.ForceMajeurePlayer);
		}

		// Token: 0x06002221 RID: 8737 RVA: 0x00076D68 File Offset: 0x00074F68
		public static void EliminatePlayer(this TurnProcessContext context, PlayerState player, PlayerState killer)
		{
			IEnumerable<DiplomaticPairStatus> enumerable = from x in context.Diplomacy.Standings
			where x.IsDiplomacyStatusOfPlayer(player.Id)
			select x;
			player.Eliminate(context);
			foreach (DiplomaticPairStatus diplomaticPairStatus in enumerable)
			{
				if (!diplomaticPairStatus.IsDiplomacyStatusOfPlayer(-1))
				{
					diplomaticPairStatus.SetEliminated(context, player.Id);
				}
			}
			context.DestroyAllOwnedBy(player);
			PlayerEliminatedEvent playerEliminatedEvent = new PlayerEliminatedEvent(killer.Id, player.Id);
			ArchFiendStaticData archFiendStaticData;
			GameItemStaticData data;
			if (killer.Id != -1 && context.Database.TryFetch<ArchFiendStaticData>(player.ArchfiendId, out archFiendStaticData) && archFiendStaticData.EliminationTrophy.Id.Length > 0 && context.Database.TryFetch<GameItemStaticData>(archFiendStaticData.EliminationTrophy, out data))
			{
				GameItem item = context.SpawnGameItem(data, null);
				if (BidProcessor.AwardToPlayer(context, killer, item).successful)
				{
					playerEliminatedEvent.TrophyItem = item;
				}
			}
			context.CurrentTurn.AddGameEvent<PlayerEliminatedEvent>(playerEliminatedEvent);
		}

		// Token: 0x06002222 RID: 8738 RVA: 0x00076EA0 File Offset: 0x000750A0
		public static void ProcessKingmakerDecisions(TurnProcessContext context)
		{
			if (context.CurrentTurn.TurnValue + 1 > context.Rules.KingmakerDecisionTurns)
			{
				return;
			}
			foreach (PlayerState playerState in context.CurrentTurn.EnumeratePlayerStates(false, false))
			{
				if (playerState.IsKingmaker && playerState.KingmakerPuppetId == -2147483648)
				{
					SelectKingmakerTargetRequest selectKingmakerTargetRequest = new SelectKingmakerTargetRequest(context.CurrentTurn);
					selectKingmakerTargetRequest.MustDecide = (context.CurrentTurn.TurnValue + 1 >= context.Rules.KingmakerDecisionTurns);
					context.CurrentTurn.AddDecisionToAskPlayer(playerState.Id, selectKingmakerTargetRequest);
				}
			}
		}

		// Token: 0x06002223 RID: 8739 RVA: 0x00076F6C File Offset: 0x0007516C
		public static void ProcessTurn(TurnProcessContext context)
		{
			VictoryProcessor.ProcessKingmakerDecisions(context);
			bool flag = false;
			foreach (VictoryRuleProcessor victoryRuleProcessor in context.CurrentTurn.VictoryRuleProcessors)
			{
				bool flag2 = victoryRuleProcessor.Process(context);
				if (victoryRuleProcessor.IsInEndGame())
				{
					flag = true;
				}
				if (flag2)
				{
					GameVictory victory = victoryRuleProcessor.DecideWinner(context);
					context.CurrentTurn.Victory = victory;
				}
			}
			if (!flag)
			{
				context.CurrentTurn.SetTurnPhase(TurnPhase.None);
			}
		}
	}
}
