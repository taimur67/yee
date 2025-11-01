using System;

namespace LoG
{
	// Token: 0x0200063B RID: 1595
	public class BidActionActionProcessor : ActionProcessor<OrderMakeBid>
	{
		// Token: 0x06001D7C RID: 7548 RVA: 0x00065D20 File Offset: 0x00063F20
		public override Result IsAvailable()
		{
			if (this._player.BlockBazaarAccess)
			{
				return new Result.BazaarUnavailableProblem(base.request.Item);
			}
			GameItem gameItem = base._currentTurn.FetchGameItem(base.request.Item);
			if (gameItem == null)
			{
				return Result.InvalidItem(base.request.Item);
			}
			if (!BidProcessor.CanPlayerPurchase(base._currentTurn, this._player, gameItem))
			{
				return new Result.ItemUnavailableProblem(gameItem);
			}
			int legionCount;
			if (!base._currentTurn.HasSufficientCommandRating(this._player, gameItem, out legionCount, 0))
			{
				return new Result.CommandRatingTooLowForBidProblem(base.request.Item, legionCount, this._player.CommandRating);
			}
			GamePiece gamePiece = gameItem as GamePiece;
			HexCoord hexCoord;
			if (gamePiece != null && !LegionMovementProcessor.TryFindSpawnPointFor(this.TurnProcessContext, this._player, gamePiece, out hexCoord))
			{
				return new Result.NoRoomToSpawnPurchaseProblem(gamePiece);
			}
			return Result.Success;
		}

		// Token: 0x06001D7D RID: 7549 RVA: 0x00065E08 File Offset: 0x00064008
		public override Result Process(ActionProcessContext context)
		{
			GameItem gameItem = base._currentTurn.FetchGameItem(base.request.Item);
			if (gameItem == null)
			{
				return Result.InvalidItem(base.request.Item);
			}
			this.TurnProcessContext.BiddingContext.AddBid(gameItem, new PlayerBid
			{
				Player = this._player,
				Payment = base.request.Payment,
				OrderSlotIndex = context.OrderSlotIndex
			});
			return Result.Success;
		}

		// Token: 0x06001D7E RID: 7550 RVA: 0x00065E8B File Offset: 0x0006408B
		public override Result Validate()
		{
			return this.IsAvailable();
		}

		// Token: 0x06001D7F RID: 7551 RVA: 0x00065E93 File Offset: 0x00064093
		public override Result AIPreview(ActionProcessContext context)
		{
			base._currentTurn.FetchGameItem(base.request.Item).Status = GameItemStatus.Unavailable;
			return Result.Success;
		}

		// Token: 0x06001D80 RID: 7552 RVA: 0x00065EB6 File Offset: 0x000640B6
		public override Cost CalculateCost()
		{
			GameItem gameItem = base._currentTurn.FetchGameItem(base.request.Item);
			return ((gameItem != null) ? gameItem.Cost : null) ?? Cost.None;
		}
	}
}
