using System;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x020003D7 RID: 983
	public class PraetorCombatMoveUpgrade_ManuscriptProcessor : ManuscriptActionProcessor<PraetorCombatMoveUpgrade_ManuscriptOrder, UpgradePraetorAbilityStaticData>
	{
		// Token: 0x06001321 RID: 4897 RVA: 0x00048B58 File Offset: 0x00046D58
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
			if (!(gameItem is Praetor))
			{
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					logger.Error(string.Format("Invoking Praetor manuscript on non-Praetor item {0}", base.request.TargetItemId));
				}
				return new Result.InvokeManuscriptProblem(base.request.ManuscriptId);
			}
			Problem problem3 = base.request.ValidateReplacement(this.TurnProcessContext, this._player.Id, base.request.TargetItemId, base.request.Technique, base.data.Move) as Problem;
			if (problem3 != null)
			{
				return problem3;
			}
			return Result.Success;
		}

		// Token: 0x06001322 RID: 4898 RVA: 0x00048C1C File Offset: 0x00046E1C
		public override Result Process(ActionProcessContext context)
		{
			Problem problem = this.Validate() as Problem;
			if (problem != null)
			{
				return problem;
			}
			return base.Process(context);
		}

		// Token: 0x06001323 RID: 4899 RVA: 0x00048C44 File Offset: 0x00046E44
		protected override ManuscriptEvent InvokeManuscript(Manuscript manuscript, ManuscriptStaticData manuscriptData)
		{
			ManuscriptEvent manuscriptEvent = base.InvokeManuscript(manuscript, manuscriptData);
			Praetor praetor = base._currentTurn.FetchGameItem<Praetor>(base.request.TargetItemId);
			PraetorCombatMoveInstance praetorCombatMoveInstance;
			praetor.TryGetTechniqueInstance(base.request.Technique, out praetorCombatMoveInstance);
			GameEvent gameEvent = manuscriptEvent.AddChildEvent<PraetorCombatMoveTypeChangedEvent>(this.TurnProcessContext.ChangeCombatMove(this._player, praetor, praetorCombatMoveInstance, base.data.Move));
			UpgradePraetorAbilityStaticData upgradePraetorAbilityStaticData = base._database.Fetch<UpgradePraetorAbilityStaticData>(manuscriptData.ProvidedAbility.Id);
			int upgradePower = base._database.Fetch(upgradePraetorAbilityStaticData.Move).UpgradePower;
			gameEvent.AddChildEvent<PraetorCombatMovePowerChangedEvent>(this.TurnProcessContext.AdjustCombatMovePower(this._player, praetor, praetorCombatMoveInstance, upgradePower));
			return manuscriptEvent;
		}
	}
}
