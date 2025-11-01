using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000669 RID: 1641
	public class DebuffGamePieceRitualProcessor : TargetedRitualActionProcessor<DebuffGamePieceRitualOrder, DebuffGamePieceRitualData, RitualCastEvent>
	{
		// Token: 0x06001E4A RID: 7754 RVA: 0x00068654 File Offset: 0x00066854
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			GamePiece gamePiece = base._currentTurn.FetchGameItem<GamePiece>(base.request.TargetItemId);
			RitualCastEvent ritualCastEvent;
			Problem problem = base.CheckGamePieceRitualResistance(gamePiece, out ritualCastEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			GamePieceModifierStaticData gamePieceModifierStaticData = new GamePieceModifierStaticData();
			foreach (CombatStatType stat in base.request.TargetStats)
			{
				int randomRoll = base._currentTurn.GetRandomRoll(base.data.MinBaseDebuff, base.data.MaxBaseDebuff, this._player.HasTag<EntityTag_CheatLuckyRitualEffectRolls>());
				gamePieceModifierStaticData.SetValue(stat.ToGamePieceStat(), (float)(-(float)randomRoll), ModifierTarget.ValueOffset);
			}
			GamePieceModifier modifier = new GamePieceModifier(gamePieceModifierStaticData)
			{
				Source = new RitualContext(this._player.Id, this._player.ArchfiendId, base.request.RitualId)
			};
			GamePieceModifierActiveRitual gamePieceModifierActiveRitual = this.TurnProcessContext.CreateActiveRitual(base.data, base.request, this.CalculateCost(), base.GameEvent.MaskingContext);
			gamePieceModifierActiveRitual.Modifier = modifier;
			gamePieceModifierActiveRitual.StartRitual(this.TurnProcessContext, this._player, ritualCastEvent);
			this._player.RitualState.SlottedItems.Add(gamePieceModifierActiveRitual.Id);
			return Result.Success;
		}
	}
}
