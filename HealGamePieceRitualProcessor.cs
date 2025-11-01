using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000678 RID: 1656
	public class HealGamePieceRitualProcessor : TargetedRitualActionProcessor<HealGamePieceRitualOrder, HealGamePieceRitualData, RitualCastEvent>
	{
		// Token: 0x06001E80 RID: 7808 RVA: 0x00069274 File Offset: 0x00067474
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			GamePiece gamePiece = base._currentTurn.FetchGameItem<GamePiece>(base.request.TargetContext.ItemId);
			RitualCastEvent ritualCastEvent;
			Problem problem = base.CheckGameItemRitualResistance(gamePiece, gamePiece.ControllingPlayerId, out ritualCastEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			int num = base.data.FullHeal ? gamePiece.TotalHP : base._currentTurn.GetRandomRoll(base.data.MinHeal, base.data.MaxHeal, this._player.HasTag<EntityTag_CheatLuckyRitualEffectRolls>());
			int num2 = gamePiece.TotalHP - gamePiece.HP;
			if (num > num2)
			{
				num = num2;
			}
			HealGamePieceEvent ev = gamePiece.Heal(num);
			ritualCastEvent.AddChildEvent<HealGamePieceEvent>(ev);
			return Result.Success;
		}
	}
}
