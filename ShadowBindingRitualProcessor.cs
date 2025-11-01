using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x0200069B RID: 1691
	public class ShadowBindingRitualProcessor : TargetedRitualActionProcessor<ShadowBindingRitualOrder, ShadowBindingRitualData, RitualCastEvent>
	{
		// Token: 0x06001F18 RID: 7960 RVA: 0x0006B37C File Offset: 0x0006957C
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			GamePiece gamePiece = base._currentTurn.FetchGameItem<GamePiece>(base.request.TargetContext.ItemId);
			RitualCastEvent ritualCastEvent;
			Problem problem = base.CheckGameItemRitualResistance(gamePiece, gamePiece.ControllingPlayerId, out ritualCastEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			ShadowBindingActiveRitual shadowBindingActiveRitual = this.TurnProcessContext.CreateActiveRitual(base.data, base.request, this.CalculateCost(), base.GameEvent.MaskingContext);
			this._player.RitualState.SlottedItems.Add(shadowBindingActiveRitual.Id);
			shadowBindingActiveRitual.StartRitual(this.TurnProcessContext, this._player, ritualCastEvent);
			return Result.Success;
		}
	}
}
