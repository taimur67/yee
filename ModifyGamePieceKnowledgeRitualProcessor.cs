using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000685 RID: 1669
	public class ModifyGamePieceKnowledgeRitualProcessor : TargetedRitualActionProcessor<ModifyGamePieceKnowledgeRitualOrder, ModifyGamePieceKnowledgeRitualData, RitualCastEvent>
	{
		// Token: 0x06001EB0 RID: 7856 RVA: 0x00069AF0 File Offset: 0x00067CF0
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			GamePiece gamePiece = base._currentTurn.FetchGameItem<GamePiece>(base.request.TargetItemId);
			RitualCastEvent ritualCastEvent;
			Problem problem = base.CheckGamePieceRitualResistance(gamePiece, out ritualCastEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			CombatStatType targetStatType = base.request.TargetStatType;
			int num = base._currentTurn.GetRandomRoll(base.data.MinBaseValue, base.data.MaxBaseValue, this._player.HasTag<EntityTag_CheatLuckyRitualEffectRolls>());
			if (base.request.StrongerOrWeaker == StrongerWeaker.Weaker)
			{
				num = -num;
			}
			GamePieceKnowledgeModifierActiveRitual gamePieceKnowledgeModifierActiveRitual = this.TurnProcessContext.CreateActiveRitual(base.data, base.request, this.CalculateCost(), base.GameEvent.MaskingContext);
			gamePieceKnowledgeModifierActiveRitual.TargetStat = targetStatType;
			gamePieceKnowledgeModifierActiveRitual.ModifierValue = num;
			this._player.RitualState.SlottedItems.Add(gamePieceKnowledgeModifierActiveRitual.Id);
			gamePieceKnowledgeModifierActiveRitual.StartRitual(this.TurnProcessContext, this._player, ritualCastEvent);
			return Result.Success;
		}
	}
}
