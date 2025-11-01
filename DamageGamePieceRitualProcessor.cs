using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000665 RID: 1637
	public class DamageGamePieceRitualProcessor : TargetedRitualActionProcessor<DamageGamePieceRitualOrder, DamageGamePieceRitualData, RitualCastEvent>
	{
		// Token: 0x06001E3A RID: 7738 RVA: 0x000682B0 File Offset: 0x000664B0
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			GamePiece gamePiece = base._currentTurn.FetchGameItem<GamePiece>(base.request.TargetContext.ItemId);
			if (gamePiece == null)
			{
				return new Result.CastRitualOnPlayerProblem(this.AbilityData.ConfigRef, base.request.TargetPlayerId);
			}
			RitualCastEvent ritualCastEvent;
			Problem problem = base.CheckGameItemRitualResistance(gamePiece, gamePiece.ControllingPlayerId, out ritualCastEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			int randomRoll = base._currentTurn.GetRandomRoll(base.data.MinDamage, base.data.MaxDamage, this._player.HasTag<EntityTag_CheatLuckyRitualEffectRolls>());
			BattleProcessor.DamageEvent ev = this.TurnProcessContext.DealDamage(gamePiece, new BattleProcessor.DamageContext(this._player.Id)
			{
				Damage = randomRoll,
				DamageType = DamageType.True,
				IsPermanent = base.data.PermanentDamage,
				AllowCapture = false
			});
			ritualCastEvent.AddChildEvent<BattleProcessor.DamageEvent>(ev);
			return Result.Success;
		}

		// Token: 0x06001E3B RID: 7739 RVA: 0x0006839C File Offset: 0x0006659C
		protected override int GetPrestigeReward()
		{
			GamePiece gamePiece = base._currentTurn.FetchGameItem<GamePiece>(base.request.TargetContext.ItemId);
			int prestigeReward = base.GetPrestigeReward();
			if (!gamePiece.IsAlive())
			{
				return prestigeReward + prestigeReward * base.data.KillPrestigeMultiplier;
			}
			return prestigeReward;
		}
	}
}
