using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x020003A6 RID: 934
	public static class PlayerStateExtensions
	{
		// Token: 0x060011F9 RID: 4601 RVA: 0x0004484C File Offset: 0x00042A4C
		public static List<ResourceNFT> GetVulnerableTribute(this TurnProcessContext context, int playerId)
		{
			PlayerState playerState = context.CurrentTurn.FindPlayerState(playerId, null);
			List<ResourceNFT> list = IEnumerableExtensions.ToList<ResourceNFT>(playerState.Resources);
			foreach (ActionableOrder actionableOrder in playerState.PlayerTurn.Orders)
			{
				using (IEnumerator<ResourceNFT> enumerator2 = actionableOrder.GetReservedResources().GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						ResourceNFT res = enumerator2.Current;
						list.RemoveAll((ResourceNFT x) => res.Id == x.Id);
					}
				}
			}
			foreach (DecisionResponse decisionResponse in playerState.PlayerTurn.Decisions)
			{
				using (IEnumerator<ResourceNFT> enumerator2 = decisionResponse.GetReservedResources().GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						ResourceNFT res = enumerator2.Current;
						list.RemoveAll((ResourceNFT x) => res.Id == x.Id);
					}
				}
			}
			return list;
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x000449A4 File Offset: 0x00042BA4
		public static bool CanReceivePrestige(this PlayerState player)
		{
			return player != null && !player.Excommunicated && !player.Eliminated;
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x000449BC File Offset: 0x00042BBC
		public static bool IsValidConclaveFavourite(this PlayerState player, TurnState turn)
		{
			int num;
			bool flag = turn.CurrentDiplomaticTurn.IsVassalOfAny(player.Id, out num);
			return !player.Eliminated && !player.Excommunicated && !flag;
		}
	}
}
