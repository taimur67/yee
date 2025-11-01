using System;
using System.Linq;
using Game.Simulation.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x0200061E RID: 1566
	public class ManuscriptActionProcessor<T, Q> : ActionProcessor<T, Q> where T : InvokeManuscriptOrder, new() where Q : ManuscriptAbilityStaticData
	{
		// Token: 0x06001CF4 RID: 7412 RVA: 0x00063E44 File Offset: 0x00062044
		public override Result Validate()
		{
			Manuscript manuscript = base._currentTurn.FetchGameItem<Manuscript>(base.request.ManuscriptId);
			if (manuscript == null)
			{
				return new Result.InvokeIncompleteManuscriptProblem(base.request.ManuscriptId);
			}
			ManuscriptStaticData manuscriptStaticData = base._database.Fetch<ManuscriptStaticData>(manuscript.StaticDataId);
			if (this.TurnProcessContext.GetManuscriptCurrentFragmentCount(this._player.Id, manuscript.StaticDataId) < manuscriptStaticData.FragmentCount)
			{
				return new Result.InvokeIncompleteManuscriptProblem(base.request.ManuscriptId);
			}
			return Result.Success;
		}

		// Token: 0x06001CF5 RID: 7413 RVA: 0x00063ED8 File Offset: 0x000620D8
		protected Result ValidateTargetItem(out GameItem target)
		{
			target = base._currentTurn.FetchGameItem(base.request.TargetItemId);
			if (target == null)
			{
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					logger.Error(string.Format("Invoking manuscript on invalid item Id {0}", base.request.TargetItemId));
				}
				return new Result.InvokeManuscriptProblem(base.request.ManuscriptId);
			}
			if (target.Status == GameItemStatus.Banished)
			{
				return new Result.InvokeManuscriptOnBanishedGameItemProblem(base.request.ManuscriptId, base.request.TargetItemId);
			}
			if (!base._currentTurn.DoesPlayerControlItem(this._player.Id, base.request.TargetItemId))
			{
				return new Result.InvokeManuscriptOnStolenGameItemProblem(base.request.ManuscriptId, base.request.TargetItemId);
			}
			return Result.Success;
		}

		// Token: 0x06001CF6 RID: 7414 RVA: 0x00063FD0 File Offset: 0x000621D0
		public override Result Preview(ActionProcessContext context)
		{
			Manuscript manuscript = base._currentTurn.FetchGameItem<Manuscript>(base.request.ManuscriptId);
			ManuscriptStaticData manuscriptStaticData = base._database.Fetch<ManuscriptStaticData>(manuscript.StaticDataId);
			foreach (Manuscript manuscript2 in this.TurnProcessContext.GetAllManuscriptFragmentsControlledBy(this._player.Id, manuscript.StaticDataId).Take(manuscriptStaticData.FragmentCount))
			{
				manuscript2.Status = GameItemStatus.Pending;
			}
			return Result.Success;
		}

		// Token: 0x06001CF7 RID: 7415 RVA: 0x00064070 File Offset: 0x00062270
		public override Result Process(ActionProcessContext context)
		{
			Problem problem = this.Validate() as Problem;
			if (problem != null)
			{
				return problem;
			}
			Manuscript manuscript = base._currentTurn.FetchGameItem<Manuscript>(base.request.ManuscriptId);
			ManuscriptStaticData manuscriptStaticData = base._database.Fetch<ManuscriptStaticData>(manuscript.StaticDataId);
			Problem problem2 = this.ConsumeManuscriptFragments(manuscriptStaticData.Id, manuscriptStaticData.FragmentCount) as Problem;
			if (problem2 != null)
			{
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					logger.Error("Validate method passed, but unable to consume manuscript fragments");
				}
				return problem2;
			}
			base._currentTurn.AddGameEvent<ManuscriptEvent>(this.InvokeManuscript(manuscript, manuscriptStaticData));
			return Result.Success;
		}

		// Token: 0x06001CF8 RID: 7416 RVA: 0x00064108 File Offset: 0x00062308
		protected virtual ManuscriptEvent InvokeManuscript(Manuscript manuscript, ManuscriptStaticData manuscriptData)
		{
			return new ManuscriptEvent(this._player.Id, manuscript.GetCategory(base._database), new int[]
			{
				this._player.Id
			})
			{
				ManuscriptId = manuscript.Id,
				TargetId = base.request.TargetItemId
			};
		}

		// Token: 0x06001CF9 RID: 7417 RVA: 0x00064168 File Offset: 0x00062368
		protected Result ConsumeManuscriptFragments(string manuscriptId, int count)
		{
			foreach (Manuscript item in this.TurnProcessContext.GetAllManuscriptFragmentsControlledBy(this._player.Id, manuscriptId).Take(count))
			{
				this.TurnProcessContext.BanishGameItem(item, int.MinValue);
			}
			return Result.Success;
		}
	}
}
