using System;

namespace LoG
{
	// Token: 0x02000646 RID: 1606
	public class ReturnGameItemToVaultProcessor : ActionProcessor<OrderReturnGameItemToVault, ReturnGameItemToVaultPieceStaticData>
	{
		// Token: 0x06001DA5 RID: 7589 RVA: 0x000663BC File Offset: 0x000645BC
		public override Result Process(ActionProcessContext context)
		{
			Problem problem = this.Validate() as Problem;
			if (problem != null)
			{
				return problem;
			}
			Identifier gamePieceId;
			Problem problem2 = this.TurnProcessContext.MoveItemToVault(base.request, this._player, base.request.GameItemId, out gamePieceId) as Problem;
			if (problem2 != null)
			{
				return problem2;
			}
			GameItemVaultedEvent gameEvent = new GameItemVaultedEvent(this._player.Id, base.request.GameItemId, gamePieceId);
			base._currentTurn.AddGameEvent<GameItemVaultedEvent>(gameEvent);
			this.TurnProcessContext.RecalculateAllModifiersFor(this._player);
			return Result.Success;
		}

		// Token: 0x06001DA6 RID: 7590 RVA: 0x00066449 File Offset: 0x00064649
		public override Result Validate()
		{
			return this.TurnProcessContext.ValidateReturnGameItemToVault(base.request, this._player, base.request.GameItemId);
		}
	}
}
