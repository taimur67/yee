using System;
using System.Collections.Generic;
using System.Linq;
using Core.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x0200064D RID: 1613
	public static class TransactionUtils
	{
		// Token: 0x06001DB9 RID: 7609 RVA: 0x000667BC File Offset: 0x000649BC
		public static GameEvent TransferOwnership(this TurnProcessContext context, Identifier itemId, PlayerState stealingPlayer, bool claimAttachments = false)
		{
			GameItem gameItem;
			if (!context.CurrentTurn.TryFetchGameItem<GameItem>(itemId, out gameItem))
			{
				return null;
			}
			GamePiece gamePiece = gameItem as GamePiece;
			if (gamePiece != null)
			{
				if (gamePiece.ControllingPlayerId != stealingPlayer.Id)
				{
					bool claimUnderlyingCanton = gamePiece.IsActive && gamePiece.SubCategory.IsFixture();
					context.CaptureGamePiece(gamePiece, stealingPlayer.Id, claimUnderlyingCanton, claimAttachments);
				}
				return null;
			}
			if (IEnumerableExtensions.Contains<Identifier>(stealingPlayer.VaultedItems, itemId))
			{
				return null;
			}
			context.RemoveItemFromAnySlot(itemId);
			stealingPlayer.AddToVault(itemId);
			return new ItemsReceivedEvent(stealingPlayer.Id, new List<Identifier>
			{
				itemId
			});
		}

		// Token: 0x06001DBA RID: 7610 RVA: 0x00066858 File Offset: 0x00064A58
		public static GameEvent GiveItems<T>(this TurnProcessContext context, PlayerState receivingPlayer, List<T> items) where T : GameItem
		{
			items = IEnumerableExtensions.ToList<T>(from x in items
			where x.CanBePlacedInVault
			select x);
			if (!IEnumerableExtensions.Any<T>(items))
			{
				return null;
			}
			foreach (T t in items)
			{
				receivingPlayer.AddToVault(t);
			}
			return new ItemsReceivedEvent(receivingPlayer.Id, IEnumerableExtensions.ToList<Identifier>(from x in items
			select x.Id));
		}

		// Token: 0x06001DBB RID: 7611 RVA: 0x0006691C File Offset: 0x00064B1C
		public static ValueTuple<Result, GameEvent> StealItem(this TurnProcessContext context, PlayerState stealingPlayer, PlayerState targetPlayer, Identifier itemId)
		{
			GameItem gameItem;
			if (!context.CurrentTurn.TryFetchGameItem<GameItem>(itemId, out gameItem))
			{
				return new ValueTuple<Result, GameEvent>(Result.InvalidItem(itemId), null);
			}
			GamePiece gamePiece = gameItem as GamePiece;
			if (gamePiece != null)
			{
				GameEvent item = null;
				if (gamePiece.ControllingPlayerId != stealingPlayer.Id)
				{
					bool claimUnderlyingCanton = gamePiece.IsActive && gamePiece.SubCategory.IsFixture();
					item = context.CaptureGamePiece(gamePiece, stealingPlayer.Id, claimUnderlyingCanton, false);
				}
				return new ValueTuple<Result, GameEvent>(Result.Success, item);
			}
			if (IEnumerableExtensions.Contains<Identifier>(stealingPlayer.VaultedItems, itemId))
			{
				return new ValueTuple<Result, GameEvent>(Result.Success, null);
			}
			Problem problem = context.RemoveItemFromAnySlotControlledByPlayer(targetPlayer, itemId) as Problem;
			if (problem != null)
			{
				return new ValueTuple<Result, GameEvent>(problem, null);
			}
			context.RemoveItemFromPlayersKnowledge(stealingPlayer, itemId);
			stealingPlayer.AddToVault(itemId);
			GameItemOwnershipChanged item2 = new GameItemOwnershipChanged(targetPlayer.Id, stealingPlayer.Id, gameItem.Id, gameItem.Category);
			return new ValueTuple<Result, GameEvent>(Result.Success, item2);
		}

		// Token: 0x06001DBC RID: 7612 RVA: 0x00066A04 File Offset: 0x00064C04
		public static GameItem TransferStealOrSpawn(this TurnProcessContext context, PlayerState player, GameItemStaticData data)
		{
			GameItem gameItem;
			if (!context.CurrentTurn.TryFetchGameItem<GameItem>(data.Id, out gameItem))
			{
				gameItem = context.SpawnGameItem(data, player);
				if (gameItem != null)
				{
					BidProcessor.AwardToPlayer(context, player, gameItem);
				}
			}
			else
			{
				context.TransferOwnership(gameItem.Id, player, true);
			}
			return gameItem;
		}

		// Token: 0x06001DBD RID: 7613 RVA: 0x00066A50 File Offset: 0x00064C50
		public static GameItemOwnershipChanged CaptureGamePiece(this TurnProcessContext context, GamePiece gamePiece, int newPlayerID, bool claimUnderlyingCanton = true, bool claimAttachedItems = true)
		{
			int controllingPlayerId = gamePiece.ControllingPlayerId;
			TurnState currentTurn = context.CurrentTurn;
			GameItemOwnershipChanged gameItemOwnershipChanged = new GameItemOwnershipChanged(controllingPlayerId, newPlayerID, gamePiece.Id, gamePiece.Category);
			if (!claimAttachedItems)
			{
				context.ReturnItemsToPlayersVault(gamePiece);
			}
			if (claimUnderlyingCanton)
			{
				gameItemOwnershipChanged.AddChildEvent<CantonClaimedEvent>(context.ClaimCanton(gamePiece.Location, newPlayerID));
			}
			gamePiece.HP = Math.Max(1, gamePiece.HP);
			gamePiece.ControllingPlayerId = newPlayerID;
			context.RecalculateAllModifiersFor(currentTurn.FindPlayerState(controllingPlayerId, null));
			context.RecalculateAllModifiersFor(currentTurn.FindPlayerState(newPlayerID, null));
			return gameItemOwnershipChanged;
		}

		// Token: 0x06001DBE RID: 7614 RVA: 0x00066AD7 File Offset: 0x00064CD7
		public static IEnumerable<ItemBanishedEvent> BanishHeldItems(this TurnProcessContext context, GamePiece gamePiece)
		{
			List<Identifier> list = IEnumerableExtensions.ToList<Identifier>(gamePiece.Slots);
			foreach (Identifier itemId in list)
			{
				yield return context.BanishGameItem(itemId, int.MinValue);
			}
			List<Identifier>.Enumerator enumerator = default(List<Identifier>.Enumerator);
			gamePiece.ClearModifiers();
			gamePiece.Slots.Clear();
			context.RecalculateModifiers(gamePiece);
			yield break;
			yield break;
		}

		// Token: 0x06001DBF RID: 7615 RVA: 0x00066AEE File Offset: 0x00064CEE
		public static GameEvent RestoreGameItem(this TurnProcessContext context, Identifier itemId, PlayerState toPlayer)
		{
			return context.RestoreGameItem(context.CurrentTurn.FetchGameItem(itemId), toPlayer);
		}

		// Token: 0x06001DC0 RID: 7616 RVA: 0x00066B03 File Offset: 0x00064D03
		public static GameEvent RestoreGameItem(this TurnProcessContext context, GameItem item, PlayerState player)
		{
			if (item.Status != GameItemStatus.Banished)
			{
				return null;
			}
			item.Status = GameItemStatus.InPlay;
			BidProcessor.AwardToPlayer(context, player, item);
			return null;
		}

		// Token: 0x06001DC1 RID: 7617 RVA: 0x00066B21 File Offset: 0x00064D21
		public static void BanishGameItemSilent(this TurnProcessContext context, Identifier itemId)
		{
			context.BanishGameItem(itemId, int.MinValue);
		}

		// Token: 0x06001DC2 RID: 7618 RVA: 0x00066B30 File Offset: 0x00064D30
		public static ItemBanishedEvent BanishGameItem(this TurnProcessContext context, Identifier itemId, int instigatorId = -2147483648)
		{
			TurnState currentTurn = context.CurrentTurn;
			GameItem gameItem = currentTurn.FetchGameItem(itemId);
			if (gameItem == null)
			{
				return null;
			}
			PlayerState playerState = currentTurn.FindControllingPlayer(itemId);
			if (instigatorId == -2147483648 && playerState != null)
			{
				instigatorId = playerState.Id;
			}
			ItemBanishedEvent itemBanishedEvent = gameItem.OnBanished(context, playerState, instigatorId);
			GamePiece gamePiece = gameItem as GamePiece;
			if (gamePiece != null)
			{
				itemBanishedEvent.AddChildEvent(context.BanishHeldItems(gamePiece));
				context.RecalculateAurasFromGamePiece(gamePiece);
			}
			else if (playerState != null)
			{
				context.RemoveItemFromAnySlotControlledByPlayer(playerState, gameItem.Id);
			}
			context.RemoveItemFromPlayersKnowledge(itemId);
			return itemBanishedEvent;
		}

		// Token: 0x06001DC3 RID: 7619 RVA: 0x00066BB4 File Offset: 0x00064DB4
		public static IEnumerable<GameItem> EnumerateGameItems(this TurnState turn, ConfigRef id)
		{
			return from t in turn.AllGameItems
			where t.StaticDataReference == id
			select t;
		}

		// Token: 0x06001DC4 RID: 7620 RVA: 0x00066BE8 File Offset: 0x00064DE8
		public static bool RemoveGameItemsFromGame(this TurnProcessContext context, ConfigRef id)
		{
			bool flag = false;
			foreach (GameItem item in IEnumerableExtensions.ToList<GameItem>(context.CurrentTurn.EnumerateGameItems(id)))
			{
				flag |= context.RemoveGameItemFromGameNoRecord(item);
			}
			context.CurrentTurn.DeadItemReferences.Add(id);
			return flag;
		}

		// Token: 0x06001DC5 RID: 7621 RVA: 0x00066C60 File Offset: 0x00064E60
		public static bool RemoveGameItemFromGame(this TurnProcessContext context, GameItem item)
		{
			if (!context.RemoveGameItemFromGameNoRecord(item))
			{
				return false;
			}
			if (item.RecordAsDeadEntityOnBanish && !item.StaticDataReference.IsEmpty())
			{
				context.CurrentTurn.DeadItemReferences.Add(item.StaticDataReference);
			}
			return true;
		}

		// Token: 0x06001DC6 RID: 7622 RVA: 0x00066C9C File Offset: 0x00064E9C
		public static bool RemoveGameItemFromGameNoRecord(this TurnProcessContext context, Identifier itemId)
		{
			GameItem item;
			return context.CurrentTurn.TryFetchGameItem(itemId, out item) && context.RemoveGameItemFromGameNoRecord(item);
		}

		// Token: 0x06001DC7 RID: 7623 RVA: 0x00066CC2 File Offset: 0x00064EC2
		public static bool RemoveGameItemFromGameNoRecord(this TurnProcessContext context, GameItem item)
		{
			context.RemoveItemFromAnySlot(item.Id);
			return context.CurrentTurn.RemoveGameItem<GameItem>(item);
		}

		// Token: 0x06001DC8 RID: 7624 RVA: 0x00066CE0 File Offset: 0x00064EE0
		public static bool FindHexCoordOf(this TurnState turn, GameItem item, out HexCoord coord)
		{
			coord = HexCoord.Invalid;
			GamePiece associatedGamePiece = turn.GetAssociatedGamePiece(item);
			if (associatedGamePiece != null)
			{
				coord = associatedGamePiece.Location;
			}
			return coord != HexCoord.Invalid;
		}
	}
}
