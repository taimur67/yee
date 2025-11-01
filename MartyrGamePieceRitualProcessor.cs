using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000681 RID: 1665
	public class MartyrGamePieceRitualProcessor : TargetedRitualActionProcessor<MartyrGamePieceRitualOrder, MartyrGamePieceRitualData, RitualCastEvent>
	{
		// Token: 0x06001E9F RID: 7839 RVA: 0x0006971C File Offset: 0x0006791C
		public override Result Validate()
		{
			Problem problem = base.Validate() as Problem;
			if (problem != null)
			{
				return problem;
			}
			GamePiece gamePiece = base._currentTurn.FetchGameItem<GamePiece>(base.request.SacrificedGamePiece);
			if (!gamePiece.IsAlive())
			{
				return new Result.CastRitualOnBanishedItemProblem(this.AbilityData.ConfigRef, gamePiece);
			}
			GamePiece gamePiece2 = base._currentTurn.FetchGameItem<GamePiece>(base.request.TargetItemId);
			if (!gamePiece2.IsAlive())
			{
				return new Result.CastRitualOnBanishedItemProblem(this.AbilityData.ConfigRef, gamePiece2);
			}
			return Result.Success;
		}

		// Token: 0x06001EA0 RID: 7840 RVA: 0x000697A4 File Offset: 0x000679A4
		private void TransferRandomAmountOfStat(GamePiece source, CombatStatType stat, GamePieceModifierStaticData target)
		{
			float num = base._currentTurn.Random.NextFloat(base.data.MinStatTransferPercentage, base.data.MaxStatTransferPercentage);
			int num2 = (int)((float)source.GetStat(stat) * num);
			target.SetValue(stat.ToGamePieceStat(), (float)num2, ModifierTarget.ValueOffset);
		}

		// Token: 0x06001EA1 RID: 7841 RVA: 0x000697FC File Offset: 0x000679FC
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			GamePiece gamePiece = base._currentTurn.FetchGameItem<GamePiece>(base.request.TargetItemId);
			RitualCastEvent ritualCastEvent;
			Problem problem = base.CheckGameItemRitualResistance(gamePiece, gamePiece.ControllingPlayerId, out ritualCastEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			GamePiece gamePiece2 = base._currentTurn.FetchGameItem<GamePiece>(base.request.SacrificedGamePiece);
			foreach (ActiveRitual activeRitual in IEnumerableExtensions.ToList<ActiveRitual>(base._currentTurn.GetActiveRituals(this._player)))
			{
				if (activeRitual.TargetContext.ItemId == gamePiece2.Id)
				{
					ItemBanishedEvent ev = this.TurnProcessContext.BanishGameItem(activeRitual, int.MinValue);
					UpkeepFailed upkeepFailed = new UpkeepFailed(this._player.Id, activeRitual.Id, GameItemCategory.ActiveRitual, 0);
					upkeepFailed.AddChildEvent<ItemBanishedEvent>(ev);
					base._currentTurn.AddGameEvent<UpkeepFailed>(upkeepFailed);
				}
			}
			GamePieceModifierStaticData gamePieceModifierStaticData = new GamePieceModifierStaticData();
			this.TransferRandomAmountOfStat(gamePiece2, CombatStatType.Ranged, gamePieceModifierStaticData);
			this.TransferRandomAmountOfStat(gamePiece2, CombatStatType.Melee, gamePieceModifierStaticData);
			this.TransferRandomAmountOfStat(gamePiece2, CombatStatType.Infernal, gamePieceModifierStaticData);
			MartyrGamePieceActiveRitual martyrGamePieceActiveRitual = this.TurnProcessContext.CreateActiveRitual(base.data, base.request, this.CalculateCost(), base.GameEvent.MaskingContext);
			this._player.RitualState.SlottedItems.Add(martyrGamePieceActiveRitual.Id);
			martyrGamePieceActiveRitual.Modifier = gamePieceModifierStaticData;
			martyrGamePieceActiveRitual.StartRitual(this.TurnProcessContext, this._player, ritualCastEvent);
			LegionKilledEvent ev2 = this.TurnProcessContext.KillGamePiece(gamePiece2, -1);
			ritualCastEvent.AddChildEvent<LegionKilledEvent>(ev2);
			return Result.Success;
		}
	}
}
