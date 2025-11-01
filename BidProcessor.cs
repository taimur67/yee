using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x020003E0 RID: 992
	public static class BidProcessor
	{
		// Token: 0x06001384 RID: 4996 RVA: 0x0004A594 File Offset: 0x00048794
		public static void ProcessBids(TurnProcessContext context, Dictionary<GameItem, List<PlayerBid>> bids)
		{
			foreach (KeyValuePair<GameItem, List<PlayerBid>> keyValuePair in bids)
			{
				GameItem gameItem;
				List<PlayerBid> list;
				keyValuePair.Deconstruct(out gameItem, out list);
				GameItem item = gameItem;
				List<PlayerBid> bids2 = list;
				try
				{
					BidProcessor.ProcessBids(context, item, bids2);
				}
				catch (Exception e)
				{
					context.OnException(e);
				}
			}
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x0004A610 File Offset: 0x00048810
		public static void ProcessBids(TurnProcessContext context, Identifier itemId, List<PlayerBid> bids)
		{
			BidProcessor.ProcessBids(context, context.CurrentTurn.FetchGameItem(itemId), bids);
		}

		// Token: 0x06001386 RID: 4998 RVA: 0x0004A625 File Offset: 0x00048825
		public static void ProcessBids(TurnProcessContext context, GameItem item, params PlayerBid[] bids)
		{
			BidProcessor.ProcessBids(context, item, IEnumerableExtensions.ToList<PlayerBid>(bids));
		}

		// Token: 0x06001387 RID: 4999 RVA: 0x0004A634 File Offset: 0x00048834
		public static void ProcessBids(TurnProcessContext context, GameItem item, List<PlayerBid> bids)
		{
			bids.Sort((PlayerBid lhs, PlayerBid rhs) => BidProcessor.CompareBids(context.CurrentTurn, lhs, rhs));
			bids.Reverse();
			BidProcessor.ProcessSortedBids(context, item, bids);
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x0004A674 File Offset: 0x00048874
		private static void ProcessSortedBids(TurnProcessContext context, GameItem item, List<PlayerBid> bids)
		{
			if (bids.Count == 0)
			{
				return;
			}
			TurnState currentTurn = context.CurrentTurn;
			BazaarBidEvent bazaarBidEvent = new BazaarBidEvent(int.MinValue, item.Id, item.Category, context.IsBackroomItem(item));
			currentTurn.AddGameEvent<BazaarBidEvent>(bazaarBidEvent);
			foreach (PlayerState playerState in currentTurn.EnumeratePlayerStates(false, false))
			{
				if (playerState.HasBazaarPurchaseKnowledge)
				{
					bazaarBidEvent.PurchaseKnowledgeHolders.Set(playerState.Id);
				}
			}
			if (bids.Count == 1)
			{
				PlayerBid playerBid = bids[0];
				Result result = BidProcessor.ProcessPurchase(context, playerBid.Player, item);
				BidProcessor.FinalizeBid(currentTurn, playerBid, result, bazaarBidEvent);
				return;
			}
			PlayerBid playerBid2 = bids[0];
			PlayerBid rhs = bids[1];
			if (BidProcessor.CompareBids(currentTurn, playerBid2, rhs) == 0)
			{
				foreach (PlayerBid playerBid3 in bids)
				{
					BidProcessor.FinalizeBid(currentTurn, playerBid3, Result.DeadLock(), bazaarBidEvent);
				}
				return;
			}
			Result result2 = BidProcessor.ProcessPurchase(context, playerBid2.Player, item);
			if (!result2.successful)
			{
				BidProcessor.FinalizeBid(currentTurn, playerBid2, Result.AwardFailure(result2), bazaarBidEvent);
				bids.Remove(playerBid2);
				BidProcessor.ProcessSortedBids(context, item, bids);
				return;
			}
			BidProcessor.FinalizeBid(currentTurn, playerBid2, Result.Success, bazaarBidEvent);
			Result.OutbidProblem result3 = new Result.OutbidProblem(item, playerBid2.Player.Id);
			Result.OutbidProblem result4 = new Result.OutbidProblem(item, -1);
			for (int i = 1; i < bids.Count; i++)
			{
				PlayerBid playerBid4 = bids[i];
				if (item.Category == GameItemCategory.GamePiece)
				{
					BidProcessor.FinalizeBid(currentTurn, playerBid4, result3, bazaarBidEvent);
				}
				else
				{
					BidProcessor.FinalizeBid(currentTurn, playerBid4, result4, bazaarBidEvent);
				}
			}
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x0004A854 File Offset: 0x00048A54
		public static int CompareBids(TurnState turn, PlayerBid lhs, PlayerBid rhs)
		{
			int num = BidProcessor.CalculateRelativeValue(turn, lhs.Payment).CompareTo(BidProcessor.CalculateRelativeValue(turn, rhs.Payment));
			if (num != 0)
			{
				return num;
			}
			num = lhs.Player.Rank.CompareTo(rhs.Player.Rank);
			if (num != 0)
			{
				return num;
			}
			num = lhs.OrderSlotIndex.CompareTo(rhs.OrderSlotIndex);
			if (num != 0)
			{
				return -num;
			}
			int num2 = turn.TurnsUntilRegency(lhs.Player);
			int value = turn.TurnsUntilRegency(rhs.Player);
			num = num2.CompareTo(value);
			return -num;
		}

		// Token: 0x0600138A RID: 5002 RVA: 0x0004A8F5 File Offset: 0x00048AF5
		public static void FinalizeBid(TurnState turn, in PlayerBid playerBid, Result result, BazaarBidEvent bidEvent)
		{
			if (result.successful)
			{
				bidEvent.TriggeringPlayerID = playerBid.Player.Id;
				return;
			}
			bidEvent.AddAffectedPlayerId(playerBid.Player.Id);
			BidProcessor.RefundBid(turn, playerBid.Player, playerBid.Payment);
		}

		// Token: 0x0600138B RID: 5003 RVA: 0x0004A934 File Offset: 0x00048B34
		public static void RefundBid(TurnState turn, PlayerState player, Payment payment)
		{
			player.GivePayment(payment);
		}

		// Token: 0x0600138C RID: 5004 RVA: 0x0004A93E File Offset: 0x00048B3E
		public static float CalculateRelativeValue(TurnState turn, Payment payment)
		{
			return BidProcessor.CalculateRelativeValue(turn, payment.Total);
		}

		// Token: 0x0600138D RID: 5005 RVA: 0x0004A94C File Offset: 0x00048B4C
		public static float CalculateRelativeValue(TurnState turn, ResourceAccumulation accumulation)
		{
			return (float)accumulation.ValueSum;
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x0004A955 File Offset: 0x00048B55
		public static bool CanPlayerPurchase(TurnState turn, PlayerState player, GameItem item)
		{
			return item != null && turn.BazaarState.IsForSale(item);
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x0004A968 File Offset: 0x00048B68
		public static Result ProcessPurchase(TurnProcessContext context, PlayerState player, GameItem item)
		{
			TurnState currentTurn = context.CurrentTurn;
			if (!BidProcessor.CanPlayerPurchase(currentTurn, player, item))
			{
				return new Result.ItemUnavailableProblem(item.Id);
			}
			int legionCount;
			if (!currentTurn.HasSufficientCommandRating(player, item, out legionCount, 0))
			{
				return new Result.CommandRatingTooLowForBidProblem(item, legionCount, player.CommandRating);
			}
			Problem problem = BidProcessor.AwardToPlayer(context, player, item) as Problem;
			if (problem != null)
			{
				return problem;
			}
			currentTurn.BazaarState.RemoveItemFromBazaar(item);
			BidProcessor.OnAwardToPlayer(currentTurn, player, item);
			return Result.Success;
		}

		// Token: 0x06001390 RID: 5008 RVA: 0x0004A9E9 File Offset: 0x00048BE9
		public static bool HasSufficientCommandRating(this TurnState turn, PlayerState player, GameItem requestedItem, out int currentCommandCost, int pendingCommandCost = 0)
		{
			return turn.HasSufficientCommandRating(player, requestedItem.CommandCost, out currentCommandCost, pendingCommandCost);
		}

		// Token: 0x06001391 RID: 5009 RVA: 0x0004A9FC File Offset: 0x00048BFC
		public static bool HasSufficientCommandRating(this TurnState turn, PlayerState player, int requestedItemCommandCost, int pendingCommandCost = 0)
		{
			int num;
			return requestedItemCommandCost == 0 || turn.HasSufficientCommandRating(player, requestedItemCommandCost, out num, pendingCommandCost);
		}

		// Token: 0x06001392 RID: 5010 RVA: 0x0004AA1C File Offset: 0x00048C1C
		public static bool HasSufficientCommandRating(this TurnState turn, PlayerState player, int requestedItemCommandCost, out int currentCommandCost, int pendingCommandCost = 0)
		{
			int num = turn.CalculateTotalFieldedCommandCost(player);
			currentCommandCost = num + pendingCommandCost;
			return requestedItemCommandCost == 0 || currentCommandCost + requestedItemCommandCost <= player.CommandRating.Value;
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x0004AA50 File Offset: 0x00048C50
		public static int CalculateTotalFieldedCommandCost(this TurnState turn, PlayerState player)
		{
			IEnumerable<GameItem> fieldedGameItemsControlledBy = turn.GetFieldedGameItemsControlledBy(player.Id);
			int num = 0;
			foreach (GameItem gameItem in fieldedGameItemsControlledBy)
			{
				num += gameItem.CommandCost;
			}
			return num;
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x0004AAA8 File Offset: 0x00048CA8
		public static Result AwardToPlayer(TurnProcessContext context, PlayerState player, GameItem item)
		{
			GamePiece gamePiece = item as GamePiece;
			if (gamePiece != null)
			{
				return BidProcessor.AwardToPlayer(context, player, gamePiece);
			}
			player.AddToVault(item.Id);
			return Result.Success;
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x0004AADC File Offset: 0x00048CDC
		public static Result AwardToPlayer(TurnProcessContext context, PlayerState player, GamePiece piece)
		{
			piece.ControllingPlayerId = player.Id;
			HexCoord location;
			if (!LegionMovementProcessor.TryFindSpawnPointFor(context, player, piece, out location))
			{
				piece.ControllingPlayerId = -1;
				return new Result.NoRoomToSpawnPurchaseProblem(piece);
			}
			LegionSpawnedEvent legionSpawnedEvent = new LegionSpawnedEvent(player.Id, piece.Id, location, LegionSpawnedEvent.LegionSpawnType.Bazaar, piece);
			legionSpawnedEvent.AddChildEvent(context.Place(piece, location));
			context.CurrentTurn.AddGameEvent<LegionSpawnedEvent>(legionSpawnedEvent);
			return Result.Success;
		}

		// Token: 0x06001396 RID: 5014 RVA: 0x0004AB4A File Offset: 0x00048D4A
		public static void OnAwardToPlayer(TurnState turn, PlayerState player, GameItem item)
		{
			item.Status = GameItemStatus.InPlay;
		}
	}
}
