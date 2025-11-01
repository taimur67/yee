using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x0200067F RID: 1663
	public class InfernalJuggernautRitualProcessor : TargetedRitualActionProcessor<InfernalJuggernautRitualOrder, InfernalJuggernautRitualData, RitualCastEvent>
	{
		// Token: 0x06001E96 RID: 7830 RVA: 0x00069594 File Offset: 0x00067794
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			GamePiece gamePiece = base._currentTurn.FetchGameItem<GamePiece>(base.request.TargetContext.ItemId);
			RitualCastEvent ritualCastEvent;
			Problem problem = base.CheckGameItemRitualResistance(gamePiece, gamePiece.ControllingPlayerId, out ritualCastEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			InfernalJuggernautActiveRitual infernalJuggernautActiveRitual = this.TurnProcessContext.CreateActiveRitual(base.data, base.request, this.CalculateCost(), base.GameEvent.MaskingContext);
			this._player.RitualState.SlottedItems.Add(infernalJuggernautActiveRitual.Id);
			infernalJuggernautActiveRitual.StartRitual(this.TurnProcessContext, this._player, ritualCastEvent);
			return Result.Success;
		}
	}
}
