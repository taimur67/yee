using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000692 RID: 1682
	public class RemoveAttachmentsItemRitualProcessor : TargetedRitualActionProcessor<RemoveAttachmentsRitualOrder, RemoveAttachmentsRitualData, RitualCastEvent>
	{
		// Token: 0x06001EDF RID: 7903 RVA: 0x0006A4DC File Offset: 0x000686DC
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			GamePiece gamePiece = base._currentTurn.FetchGameItem<GamePiece>(base.request.TargetItemId);
			RitualCastEvent ritualCastEvent;
			Problem problem = base.CheckGamePieceRitualResistance(gamePiece, out ritualCastEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			GamePiece gamePiece2 = base._currentTurn.FetchGameItem<GamePiece>(base.request.TargetItemId);
			PlayerState playerState = base._currentTurn.FindPlayerState(gamePiece2.ControllingPlayerId, null);
			int num = 0;
			int num2 = 0;
			for (int i = gamePiece2.Slots.Count - 1; i >= 0; i--)
			{
				Identifier identifier = gamePiece2.Slots[i];
				GameItem gameItem;
				if (!base._currentTurn.TryFetchGameItem<GameItem>(identifier, out gameItem))
				{
					return new Result.CastRitualOnPlayerProblem(this.AbilityData.ConfigRef, base.request.TargetPlayerId);
				}
				if (gameItem is Stratagem)
				{
					this.TurnProcessContext.BanishGameItem(gameItem, this._player.Id);
					num++;
				}
				else
				{
					Problem problem2 = this.TurnProcessContext.MoveItemToVault(playerState, identifier) as Problem;
					if (problem2 != null)
					{
						return problem2;
					}
					num2++;
					ritualCastEvent.AddChildEvent<GamePieceAttachmentRemovedEvent>(new GamePieceAttachmentRemovedEvent(this._player.Id, playerState.Id, identifier, gamePiece2.Id));
				}
			}
			if (num == 0 && num2 == 0)
			{
				ritualCastEvent.AddChildEvent<GamePieceAttachmentRemovedEvent>(GamePieceAttachmentRemovedEvent.NothingToRemove(this._player.Id, playerState.Id, gamePiece2.Id));
			}
			if (num > 0)
			{
				ritualCastEvent.AddChildEvent<GamePieceStratagemsDestroyedEvent>(new GamePieceStratagemsDestroyedEvent(this._player.Id, gamePiece2, num));
			}
			return Result.Success;
		}

		// Token: 0x06001EE0 RID: 7904 RVA: 0x0006A66B File Offset: 0x0006886B
		protected override int GetPrestigeReward()
		{
			RitualCastEvent gameEvent = base.GameEvent;
			if (((gameEvent != null) ? IEnumerableExtensions.FirstOrDefault<DestroyTributeEvent>(gameEvent.Enumerate<DestroyTributeEvent>()) : null) == null)
			{
				return 0;
			}
			return base.GetPrestigeReward();
		}
	}
}
