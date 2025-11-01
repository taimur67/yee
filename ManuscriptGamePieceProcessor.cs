using System;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000622 RID: 1570
	public class ManuscriptGamePieceProcessor : ManuscriptActionProcessor<ManuscriptGamePieceOrder, ModifyGamePieceData>
	{
		// Token: 0x06001D05 RID: 7429 RVA: 0x00064484 File Offset: 0x00062684
		public override Result Validate()
		{
			Problem problem = base.Validate() as Problem;
			if (problem != null)
			{
				return problem;
			}
			GameItem gameItem;
			Problem problem2 = base.ValidateTargetItem(out gameItem) as Problem;
			if (problem2 != null)
			{
				return problem2;
			}
			GamePiece gamePiece = gameItem as GamePiece;
			if (gamePiece == null)
			{
				return new Result.InvokeManuscriptOnGameItemProblem(base.request.ManuscriptId, gameItem.Id);
			}
			if (gamePiece.LearntManuscriptsCount >= gamePiece.Level)
			{
				return new Result.InvokeManuscriptOnGameItemAboveCapacityProblem(base.request.ManuscriptId, gameItem.Id, gamePiece.LearntManuscriptsCount, gamePiece.Level);
			}
			return Result.Success;
		}

		// Token: 0x06001D06 RID: 7430 RVA: 0x0006450C File Offset: 0x0006270C
		public override Result Process(ActionProcessContext context)
		{
			Problem problem = this.Validate() as Problem;
			if (problem != null)
			{
				return problem;
			}
			GamePiece gamePiece = base._currentTurn.FetchGameItem<GamePiece>(base.request.TargetItemId);
			if (gamePiece == null)
			{
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					logger.Error(string.Format("Invoking GamePiece manuscript on invalid item {0}", base.request.TargetItemId));
				}
				return new Result.InvokeManuscriptProblem(base.request.ManuscriptId);
			}
			Manuscript manuscript = base._currentTurn.FetchGameItem<Manuscript>(base.request.ManuscriptId);
			ManuscriptStaticData manuscriptStaticData = base._database.Fetch<ManuscriptStaticData>(manuscript.StaticDataId);
			Problem problem2 = base.ConsumeManuscriptFragments(manuscriptStaticData.Id, manuscriptStaticData.FragmentCount) as Problem;
			if (problem2 != null)
			{
				SimLogger logger2 = SimLogger.Logger;
				if (logger2 != null)
				{
					logger2.Error("Validate method passed, but unable to consume manuscript fragments");
				}
				return problem2;
			}
			new GamePieceModifier(base.data.Modifiers)
			{
				Source = new GameItemContext(manuscript.StaticDataId)
			}.InstallInto(gamePiece, base._currentTurn, false);
			foreach (ConfigRef<ItemAbilityStaticData> cref in base.data.Abilities)
			{
				ItemAbilityStaticData itemAbilityStaticData = base._database.Fetch(cref);
				gamePiece.AddAbility(new Ability(itemAbilityStaticData)
				{
					ProviderId = gamePiece.Id,
					SourceId = itemAbilityStaticData.Id,
					Name = itemAbilityStaticData.Id
				});
			}
			gamePiece.LearntManuscriptsCount++;
			ManuscriptEvent manuscriptEvent = new ManuscriptEvent(this._player.Id, manuscript.GetCategory(base._database), new int[]
			{
				this._player.Id
			});
			manuscriptEvent.ManuscriptId = manuscript.Id;
			manuscriptEvent.TargetId = gamePiece.Id;
			base._currentTurn.AddGameEvent<ManuscriptEvent>(manuscriptEvent);
			return Result.Success;
		}
	}
}
