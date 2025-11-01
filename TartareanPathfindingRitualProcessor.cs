using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x020006BD RID: 1725
	public class TartareanPathfindingRitualProcessor : TargetedRitualActionProcessor<TartareanPathfindingRitualOrder, TartareanPathfindingRitualData, RitualCastEvent>
	{
		// Token: 0x06001F91 RID: 8081 RVA: 0x0006C6BC File Offset: 0x0006A8BC
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			GamePiece gamePiece = base._currentTurn.FetchGameItem<GamePiece>(base.request.TargetItemId);
			RitualCastEvent ritualCastEvent;
			Problem problem = base.CheckGamePieceRitualResistance(gamePiece, out ritualCastEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			TartareanPathfindingActiveRitual tartareanPathfindingActiveRitual = this.TurnProcessContext.CreateActiveRitual(base.data, base.request, this.CalculateCost(), base.GameEvent.MaskingContext);
			tartareanPathfindingActiveRitual.AbilityDataConfigRef = base.data.ProvidedAbility;
			this._player.RitualState.SlottedItems.Add(tartareanPathfindingActiveRitual.Id);
			tartareanPathfindingActiveRitual.StartRitual(this.TurnProcessContext, this._player, ritualCastEvent);
			return Result.Success;
		}
	}
}
