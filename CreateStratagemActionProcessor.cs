using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;
using LoG.Simulation.Extensions;

namespace LoG
{
	// Token: 0x02000631 RID: 1585
	public class CreateStratagemActionProcessor : ActionProcessor<OrderCreateStratagem, CreateStratagemStaticData>
	{
		// Token: 0x06001D44 RID: 7492 RVA: 0x00065008 File Offset: 0x00063208
		public override Result IsUnLocked()
		{
			if (this._player.PowersLevels[PowerType.Wrath] <= 0)
			{
				return new Result.StratagemsNotUnlockedProblem(base.request.GetAbilityRef(this.TurnProcessContext), base.request.TargetPieceId);
			}
			return Result.Success;
		}

		// Token: 0x06001D45 RID: 7493 RVA: 0x00065055 File Offset: 0x00063255
		public override Result IsAvailable()
		{
			if (!this._player.BlockStratagemUse)
			{
				return Result.Success;
			}
			return new Result.StratagemsBlockedProblem(base.request.GetAbilityRef(this.TurnProcessContext), base.request.TargetPieceId);
		}

		// Token: 0x06001D46 RID: 7494 RVA: 0x00065090 File Offset: 0x00063290
		public override Result Process(ActionProcessContext context)
		{
			Problem problem = this.Validate() as Problem;
			if (problem != null)
			{
				return problem;
			}
			GamePiece gamePiece;
			if (!base._currentTurn.TryFetchGameItem<GamePiece>(base.request.TargetPieceId, out gamePiece))
			{
				return Result.SimulationError(string.Format("Validate did not fail despite invalid GamePiece {0}", base.request.TargetPieceId));
			}
			IEnumerable<StratagemTacticLevelStaticData> enumerable = base.request.TacticsIds.ExcludeNull<string>().Select(new Func<string, StratagemTacticLevelStaticData>(this.GetHighestLevelTacticOfType)).ExcludeNull<StratagemTacticLevelStaticData>();
			if (!IEnumerableExtensions.Any<StratagemTacticLevelStaticData>(enumerable))
			{
				return Result.SimulationError("Validate did not fail despite invalid tactics");
			}
			Stratagem item;
			Problem problem2 = this.TurnProcessContext.SpawnStratagem(gamePiece, this._player, enumerable, out item) as Problem;
			if (problem2 != null)
			{
				this.TurnProcessContext.RemoveGameItemFromGameNoRecord(item);
				return problem2;
			}
			this.TurnProcessContext.CurrentTurn.AddGameEvent<StratagemCreatedEvent>(new StratagemCreatedEvent(this._player, item, gamePiece));
			return Result.Success;
		}

		// Token: 0x06001D47 RID: 7495 RVA: 0x00065184 File Offset: 0x00063384
		public override Result Preview(ActionProcessContext context)
		{
			Problem problem = this.Validate() as Problem;
			if (problem != null)
			{
				return problem;
			}
			StratagemStaticData stratagemStaticData = base._database.FetchSingle<StratagemStaticData>();
			AbilityPlaceholder item = base._currentTurn.AddGameItem<AbilityPlaceholder>().SetId(base.request.ActionInstanceId).SetStaticDataId(stratagemStaticData.Id).SetAttachableTo(stratagemStaticData.SlotType).SetItemType(GameItemCategory.Stratagem);
			Problem problem2 = this.TurnProcessContext.AttachItemToGamePiece(this._player, item, base.request.TargetPieceId) as Problem;
			if (problem2 != null)
			{
				this.TurnProcessContext.RemoveGameItemFromGameNoRecord(item);
				return problem2;
			}
			return Result.Success;
		}

		// Token: 0x06001D48 RID: 7496 RVA: 0x00065228 File Offset: 0x00063428
		private StratagemTacticStaticData GetTacticFromLevel(string requestedTacticLevelId, int wrathLevel = -1)
		{
			if (string.IsNullOrEmpty(requestedTacticLevelId))
			{
				return null;
			}
			StratagemTacticLevelStaticData tacticLevel = base._database.Fetch<StratagemTacticLevelStaticData>(requestedTacticLevelId);
			return this.GetTacticFromLevel(tacticLevel, wrathLevel);
		}

		// Token: 0x06001D49 RID: 7497 RVA: 0x00065254 File Offset: 0x00063454
		private StratagemTacticStaticData GetTacticFromLevel(StratagemTacticLevelStaticData tacticLevel, int wrathLevel = -1)
		{
			if (wrathLevel < 0)
			{
				wrathLevel = this._player.PowersLevels[PowerType.Wrath].CurrentLevel;
			}
			foreach (StratagemTacticStaticData stratagemTacticStaticData in base._currentTurn.GetAvailableTactics(base._database, this._player))
			{
				for (int i = 0; i <= wrathLevel; i++)
				{
					StratagemTacticLevelStaticData tacticAtLevel = base._database.GetTacticAtLevel(stratagemTacticStaticData, i);
					if (tacticAtLevel != null && tacticLevel.Id == tacticAtLevel.Id)
					{
						return stratagemTacticStaticData;
					}
				}
			}
			return null;
		}

		// Token: 0x06001D4A RID: 7498 RVA: 0x00065308 File Offset: 0x00063508
		private StratagemTacticLevelStaticData GetHighestLevelTacticOfType(string requestedTacticLevelId)
		{
			StratagemTacticStaticData tacticFromLevel = this.GetTacticFromLevel(requestedTacticLevelId, 6);
			if (tacticFromLevel == null)
			{
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					logger.Error("Could not find tactic for tacticLevel " + requestedTacticLevelId);
				}
				return null;
			}
			int powerLevel = this._player.PowersLevels[PowerType.Wrath].CurrentLevel;
			return base._database.GetTacticAtLevel(tacticFromLevel, powerLevel);
		}

		// Token: 0x06001D4B RID: 7499 RVA: 0x00065368 File Offset: 0x00063568
		private bool CanUseTacticType(string requestedTacticLevelId)
		{
			if (string.IsNullOrEmpty(requestedTacticLevelId))
			{
				return true;
			}
			StratagemTacticLevelStaticData stratagemTacticLevelStaticData = base._database.Fetch<StratagemTacticLevelStaticData>(requestedTacticLevelId);
			return stratagemTacticLevelStaticData == null || this.CanUseTacticLevel(stratagemTacticLevelStaticData, 6);
		}

		// Token: 0x06001D4C RID: 7500 RVA: 0x00065399 File Offset: 0x00063599
		private bool CanUseTacticLevel(StratagemTacticLevelStaticData requestedTacticLevel, int wrathLevel = -1)
		{
			return this.GetTacticFromLevel(requestedTacticLevel, wrathLevel) != null;
		}

		// Token: 0x06001D4D RID: 7501 RVA: 0x000653A8 File Offset: 0x000635A8
		public override Result Validate()
		{
			Problem problem = this.IsUnLocked() as Problem;
			if (problem != null)
			{
				return problem;
			}
			Problem problem2 = this.IsAvailable() as Problem;
			if (problem2 != null)
			{
				return problem2;
			}
			if (this._player.StratagemTacticSlots < base.request.TacticsIds.Count((string id) => id != null))
			{
				return new Result.NotEnoughTacticsSlotsProblem(base.request.GetAbilityRef(this.TurnProcessContext), base.request.TargetPieceId);
			}
			if (base.request.TacticsIds.Any((string id) => !this.CanUseTacticType(id)))
			{
				return new Result.TacticUnavailableProblem(base.request.GetAbilityRef(this.TurnProcessContext), base.request.TargetPieceId);
			}
			StratagemStaticData stratagemStaticData = base._database.FetchSingle<StratagemStaticData>();
			AbilityPlaceholder item = base._currentTurn.AddGameItem<AbilityPlaceholder>().SetId(base.request.ActionInstanceId).SetStaticDataId(stratagemStaticData.Id).SetItemType(GameItemCategory.Stratagem).SetAttachableTo(stratagemStaticData.SlotType);
			Result result = this.TurnProcessContext.ValidateAttachItemToGamePiece(base.request, this._player, item, base.request.TargetPieceId, false);
			this.TurnProcessContext.RemoveGameItemFromGameNoRecord(item);
			return result;
		}

		// Token: 0x06001D4E RID: 7502 RVA: 0x000654F4 File Offset: 0x000636F4
		public override Cost CalculateCost()
		{
			Cost cost = new Cost();
			foreach (string text in base.request.TacticsIds)
			{
				if (!string.IsNullOrEmpty(text))
				{
					StratagemTacticLevelStaticData stratagemTacticLevelStaticData = base._database.Fetch<StratagemTacticLevelStaticData>(text);
					if (stratagemTacticLevelStaticData != null)
					{
						cost += stratagemTacticLevelStaticData.Cost;
					}
				}
			}
			return cost;
		}
	}
}
