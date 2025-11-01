using System;
using System.Collections.Generic;
using System.Linq;
using Game.StaticData;

namespace LoG
{
	// Token: 0x020002EE RID: 750
	public static class KnowledgeTransactions
	{
		// Token: 0x06000EA8 RID: 3752 RVA: 0x0003A684 File Offset: 0x00038884
		public static void RemoveKnowledgeFromPlayers(this TurnProcessContext context, Guid modifier)
		{
			foreach (PlayerState playerState in context.CurrentTurn.EnumeratePlayerStates(false, false))
			{
				playerState.RemoveKnowledgeModifier(modifier);
			}
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x0003A6D8 File Offset: 0x000388D8
		public static void RemoveItemFromPlayersKnowledge(this TurnProcessContext context, Identifier item)
		{
			foreach (PlayerState player in context.CurrentTurn.EnumeratePlayerStates(false, false))
			{
				context.RemoveItemFromPlayersKnowledge(player, item);
			}
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x0003A730 File Offset: 0x00038930
		public static void RemoveItemFromPlayersKnowledge(this TurnProcessContext context, PlayerState player, Identifier item)
		{
			foreach (PlayerKnowledgeContext playerKnowledgeContext in player.PlayerKnowledgeContexts.Values)
			{
				playerKnowledgeContext.RemoveContent(context, item);
			}
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x0003A788 File Offset: 0x00038988
		public static void RemoveItemFromPlayersKnowledge(this TurnProcessContext context, PlayerState player, PlayerIndex target, Identifier item)
		{
			PlayerKnowledgeContext playerKnowledgeContext;
			if (player.PlayerKnowledgeContexts.TryGetValue((int)target, out playerKnowledgeContext))
			{
				playerKnowledgeContext.RemoveContent(context, item);
			}
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x0003A7AD File Offset: 0x000389AD
		public static void RevealTributeToPlayer(this TurnProcessContext context, PlayerState revealToPlayer, PlayerState targetPlayer)
		{
			revealToPlayer.GetOrCreateKnowledgeContext(targetPlayer.Id).UpdateResources(context, targetPlayer.Resources.Total());
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x0003A7CC File Offset: 0x000389CC
		public static void RevealPowersToPlayer(this TurnProcessContext context, PlayerState revealToPlayer, PlayerState targetPlayer, PowerRevelations whatToReveal)
		{
			TurnState currentTurn = context.CurrentTurn;
			if (whatToReveal == PowerRevelations.None)
			{
				return;
			}
			List<PowerType> list = IEnumerableExtensions.ToList<PowerType>(PlayerPowersLevels.PowerTypes);
			if (whatToReveal == PowerRevelations.All)
			{
				for (int i = 0; i < list.Count; i++)
				{
					context.RevealPowerToPlayer(revealToPlayer, targetPlayer, list[i]);
				}
				return;
			}
			int num = int.MaxValue;
			int num2 = int.MinValue;
			for (int j = 0; j < list.Count; j++)
			{
				PowerType type = list[j];
				int value = targetPlayer.PowersLevels[type].CurrentLevel.Value;
				if (value > num2)
				{
					num2 = value;
				}
				if (value < num)
				{
					num = value;
				}
			}
			for (int k = 0; k < list.Count; k++)
			{
				PowerType powerType = list[k];
				int value2 = targetPlayer.PowersLevels[powerType].CurrentLevel.Value;
				bool flag;
				if (whatToReveal != PowerRevelations.OnlyWeakest)
				{
					flag = (whatToReveal == PowerRevelations.AllExceptStrongest && value2 != num2);
				}
				else
				{
					flag = (value2 == num);
				}
				if (flag)
				{
					context.RevealPowerToPlayer(revealToPlayer, targetPlayer, powerType);
				}
			}
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x0003A8D0 File Offset: 0x00038AD0
		public static void RevealPowerToPlayer(this TurnProcessContext context, PlayerState revealToPlayer, PlayerState targetPlayer, PowerType powerType)
		{
			PlayerKnowledgeContext orCreateKnowledgeContext = revealToPlayer.GetOrCreateKnowledgeContext(targetPlayer.Id);
			PlayerPowerLevel level = targetPlayer.PowersLevels[powerType].DeepClone<PlayerPowerLevel>();
			orCreateKnowledgeContext.UpdatePower(context, powerType, level);
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x0003A904 File Offset: 0x00038B04
		public static void RevealVaultToPlayer(this TurnProcessContext context, PlayerState revealToPlayer, PlayerState targetPlayer)
		{
			TurnState currentTurn = context.CurrentTurn;
			IEnumerable<Identifier> second = from x in targetPlayer.EnumerateEventCards(currentTurn)
			select x.Id;
			revealToPlayer.GetOrCreateKnowledgeContext(targetPlayer.Id).UpdateVaultContents(context, targetPlayer.VaultedItems.Except(second));
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x0003A964 File Offset: 0x00038B64
		public static void RevealEventsToPlayer(this TurnProcessContext context, PlayerState revealToPlayer, PlayerState targetPlayer)
		{
			IEnumerable<Identifier> contents = from x in targetPlayer.EnumerateEventCards(context.CurrentTurn)
			select x.Id;
			revealToPlayer.GetOrCreateKnowledgeContext(targetPlayer.Id).UpdateEventContents(context, contents);
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x0003A9B5 File Offset: 0x00038BB5
		public static void RevealRitualsToPlayer(this TurnProcessContext context, PlayerState revealToPlayer, PlayerState targetPlayer)
		{
			revealToPlayer.GetOrCreateKnowledgeContext(targetPlayer.Id).UpdateRituals(context, targetPlayer.RitualState.SlottedItems);
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x0003A9D4 File Offset: 0x00038BD4
		public static void RevealRelicsToPlayer(this TurnProcessContext context, PlayerState revealToPlayer, PlayerState targetPlayer)
		{
			revealToPlayer.GetOrCreateKnowledgeContext(targetPlayer.Id).UpdateRelics(context, targetPlayer.ActiveRelics);
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x0003A9EE File Offset: 0x00038BEE
		public static void RevealSchemesToPlayer(this TurnProcessContext context, PlayerState revealToPlayer, PlayerState targetPlayer)
		{
			revealToPlayer.GetOrCreateKnowledgeContext(targetPlayer.Id).UpdateSchemes(context, targetPlayer.ActiveSchemeCards);
		}
	}
}
