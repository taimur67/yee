using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x020003A9 RID: 937
	public static class PlayerStatUtils
	{
		// Token: 0x06001245 RID: 4677 RVA: 0x000460F4 File Offset: 0x000442F4
		public static ModifiableValue GetStat(this PlayerState player, PlayerStatUtils.PlayerVariable var)
		{
			ModifiableValue result;
			switch (var)
			{
			case PlayerStatUtils.PlayerVariable.HealingRate:
				result = player.HealingRate;
				break;
			case PlayerStatUtils.PlayerVariable.PassivePrestige:
				result = player.PassivePrestige;
				break;
			case PlayerStatUtils.PlayerVariable.CommandRating:
				result = player.CommandRating;
				break;
			case PlayerStatUtils.PlayerVariable.OrderSlots:
				result = player.OrderSlots;
				break;
			case PlayerStatUtils.PlayerVariable.MaxEventCards:
				result = player.MaxEventCards;
				break;
			case PlayerStatUtils.PlayerVariable.SchemeSlots:
				result = player.SchemeSlots;
				break;
			case PlayerStatUtils.PlayerVariable.NumBasicSchemeOptions:
				result = player.NumBasicSchemeOptions;
				break;
			case PlayerStatUtils.PlayerVariable.NumGrandSchemeOptions:
				result = player.NumGrandSchemeOptions;
				break;
			case PlayerStatUtils.PlayerVariable.NumSchemeSelections:
				result = player.NumSchemeSelections;
				break;
			case PlayerStatUtils.PlayerVariable.TributeQuality:
				result = player.TributeQuality.Value;
				break;
			default:
				result = null;
				break;
			}
			return result;
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x00046198 File Offset: 0x00044398
		public static int CountEventCards(this PlayerState player, TurnState turn)
		{
			if (player == null)
			{
				return -1;
			}
			return turn.GetGameItemsControlledBy<EventCard>(player.Id).Count<EventCard>();
		}

		// Token: 0x06001247 RID: 4679 RVA: 0x000461B0 File Offset: 0x000443B0
		public static Result GiveEventCard(this PlayerState player, TurnState turn, Identifier eventCardId)
		{
			if (player == null)
			{
				return Result.Failure;
			}
			if (player.CountEventCards(turn) >= player.MaxEventCards)
			{
				return Result.Failure;
			}
			player.AddToVault(eventCardId);
			return Result.Success;
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x000461E1 File Offset: 0x000443E1
		public static IEnumerable<EventCard> EnumerateEventCards(this PlayerState player, TurnState turn)
		{
			return player.VaultedItems.Select(new Func<Identifier, GameItem>(turn.FetchGameItem)).OfType<EventCard>();
		}

		// Token: 0x06001249 RID: 4681 RVA: 0x00046200 File Offset: 0x00044400
		public static bool HasDiplomaticOrderWithTargetAndType(this PlayerState player, DiplomaticOrder diplomaticOrder, int targetID, Type type)
		{
			foreach (DiplomaticOrder diplomaticOrder2 in player.GetOrders<DiplomaticOrder>())
			{
				if (diplomaticOrder2.TargetID == targetID && diplomaticOrder2.GetType() == type && diplomaticOrder2.ActionInstanceId != diplomaticOrder.ActionInstanceId)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600124A RID: 4682 RVA: 0x00046278 File Offset: 0x00044478
		public static bool HasDiplomaticOrderWithTarget(this PlayerState player, DiplomaticOrder diplomaticOrder, int targetID)
		{
			foreach (DiplomaticOrder diplomaticOrder2 in player.GetOrders<DiplomaticOrder>())
			{
				if (diplomaticOrder2.TargetID == targetID && diplomaticOrder2.ActionInstanceId != diplomaticOrder.ActionInstanceId)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600124B RID: 4683 RVA: 0x000462E4 File Offset: 0x000444E4
		public static bool HasOrderOfType<T>(this PlayerState player, OrderTypes type) where T : ActionableOrder
		{
			return IEnumerableExtensions.ToList<T>(player.GetOrders<T>()).Count > 0;
		}

		// Token: 0x0600124C RID: 4684 RVA: 0x000462FC File Offset: 0x000444FC
		public static IEnumerable<T> GetOrders<T>(this PlayerState player) where T : ActionableOrder
		{
			return player.PlayerTurn.Orders.OfType<T>();
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x00046310 File Offset: 0x00044510
		public static IEnumerable<Identifier> GetAttachedAndPendingRitualTableArtifacts(this PlayerState playerState, TurnState turn)
		{
			IEnumerable<Identifier> first = from attachedItem in playerState.RitualState.SlottedItems
			where turn.FetchGameItem<Artifact>(attachedItem) != null
			select attachedItem;
			IEnumerable<Identifier> second = from attachArtifactOrder in playerState.PlayerTurn.Orders.OfType<OrderAttachGameItemToRitualSlot>()
			select attachArtifactOrder.GameItemId;
			return first.Concat(second);
		}

		// Token: 0x0200093F RID: 2367
		public enum PlayerVariable
		{
			// Token: 0x0400155E RID: 5470
			HealingRate,
			// Token: 0x0400155F RID: 5471
			PassivePrestige,
			// Token: 0x04001560 RID: 5472
			CommandRating,
			// Token: 0x04001561 RID: 5473
			OrderSlots,
			// Token: 0x04001562 RID: 5474
			MaxEventCards,
			// Token: 0x04001563 RID: 5475
			SchemeSlots,
			// Token: 0x04001564 RID: 5476
			NumBasicSchemeOptions,
			// Token: 0x04001565 RID: 5477
			NumGrandSchemeOptions,
			// Token: 0x04001566 RID: 5478
			NumSchemeSelections,
			// Token: 0x04001567 RID: 5479
			TributeQuality
		}
	}
}
