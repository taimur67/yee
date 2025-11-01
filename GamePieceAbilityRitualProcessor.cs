using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000675 RID: 1653
	public class GamePieceAbilityRitualProcessor : TargetedRitualActionProcessor<GamePieceAbilityRitualOrder, GamePieceAbilityRitualData, RitualCastEvent>
	{
		// Token: 0x06001E74 RID: 7796 RVA: 0x00069084 File Offset: 0x00067284
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			GamePiece gamePiece = base._currentTurn.FetchGameItem<GamePiece>(base.request.TargetItemId);
			RitualCastEvent ritualCastEvent;
			Problem problem = base.CheckGamePieceRitualResistance(gamePiece, out ritualCastEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			GamePieceAbilityActiveRitual gamePieceAbilityActiveRitual = this.TurnProcessContext.CreateActiveRitual(base.data, base.request, this.CalculateCost(), base.GameEvent.MaskingContext);
			gamePieceAbilityActiveRitual.AbilityDataConfigRef = base.data.ProvidedAbility;
			this._player.RitualState.SlottedItems.Add(gamePieceAbilityActiveRitual.Id);
			gamePieceAbilityActiveRitual.StartRitual(this.TurnProcessContext, this._player, ritualCastEvent);
			return Result.Success;
		}
	}
}
