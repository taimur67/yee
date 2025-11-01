using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x0200069D RID: 1693
	public class StealGameItemRitualProcessor : TargetedRitualActionProcessor<StealGameItemRitualOrder, StealGameItemRitualData, StealGameItemRitualEvent>
	{
		// Token: 0x06001F1D RID: 7965 RVA: 0x0006B456 File Offset: 0x00069656
		public override void OnActionFailure(ActionProcessContext context, Result failureResult)
		{
			this.TurnProcessContext.RemoveItemFromPlayersKnowledge(this._player, base.request.TargetItemId);
		}

		// Token: 0x06001F1E RID: 7966 RVA: 0x0006B474 File Offset: 0x00069674
		protected override StealGameItemRitualEvent CreateRitualEvent(int targetId)
		{
			StealGameItemRitualEvent stealGameItemRitualEvent = base.CreateRitualEvent(targetId);
			stealGameItemRitualEvent.ItemCategory = base._currentTurn.FetchGameItem<GameItem>(base.request.TargetItemId).Category;
			return stealGameItemRitualEvent;
		}

		// Token: 0x06001F1F RID: 7967 RVA: 0x0006B4A0 File Offset: 0x000696A0
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			GameItem item = base._currentTurn.FetchGameItem<GameItem>(base.request.TargetItemId);
			StealGameItemRitualEvent stealGameItemRitualEvent;
			Problem problem = base.CheckGameItemRitualResistance(item, base.request.TargetPlayerId, out stealGameItemRitualEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			GameItem gameItem = base._currentTurn.FetchGameItem(base.request.TargetItemId);
			if (gameItem == null)
			{
				return new Result.CastRitualOnPlayerProblem(this.AbilityData.ConfigRef, base.request.TargetPlayerId);
			}
			ValueTuple<Result, GameEvent> valueTuple = this.TurnProcessContext.StealItem(this._player, base._currentTurn.FindPlayerState(base.request.TargetPlayerId, null), gameItem);
			Result item2 = valueTuple.Item1;
			GameEvent item3 = valueTuple.Item2;
			Problem problem2 = item2 as Problem;
			if (problem2 != null)
			{
				return problem2;
			}
			stealGameItemRitualEvent.AddChildEvent(item3);
			return Result.Success;
		}
	}
}
