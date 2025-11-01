using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000687 RID: 1671
	public class ModifyGamePieceRitualProcessor : TargetedRitualActionProcessor<ModifyGamePieceRitualOrder, ModifyGamePieceRitualData, RitualCastEvent>
	{
		// Token: 0x06001EB6 RID: 7862 RVA: 0x00069C24 File Offset: 0x00067E24
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			GamePiece gamePiece = base._currentTurn.FetchGameItem<GamePiece>(base.request.TargetItemId);
			RitualCastEvent ritualCastEvent;
			Problem problem = base.CheckGamePieceRitualResistance(gamePiece, out ritualCastEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			GamePieceModifier gamePieceModifier = new GamePieceModifier(base.data.Modifiers);
			gamePieceModifier.Source = new RitualContext(this._player.Id, this._player.ArchfiendId, base.request.RitualId);
			GamePieceModifierActiveRitual gamePieceModifierActiveRitual = this.TurnProcessContext.CreateActiveRitual(base.data, base.request, this.CalculateCost(), base.GameEvent.MaskingContext);
			gamePieceModifierActiveRitual.Modifier = gamePieceModifier;
			this._player.RitualState.SlottedItems.Add(gamePieceModifierActiveRitual.Id);
			gamePieceModifierActiveRitual.StartRitual(this.TurnProcessContext, this._player, ritualCastEvent);
			return Result.Success;
		}
	}
}
