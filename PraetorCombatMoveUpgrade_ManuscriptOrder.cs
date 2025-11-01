using System;
using System.Collections.Generic;
using Core.StaticData;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003D6 RID: 982
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PraetorCombatMoveUpgrade_ManuscriptOrder : InvokeManuscriptOrder
	{
		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x0600131A RID: 4890 RVA: 0x0004893A File Offset: 0x00046B3A
		protected override bool GameItemValidationRequiresInPlay
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600131B RID: 4891 RVA: 0x0004893D File Offset: 0x00046B3D
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			yield return new ActionPhase_TargetGameItem(delegate(Identifier x)
			{
				this.TargetContext.SetTargetGameItem(x);
			}, new ActionPhase_Target<Identifier>.IsValidFunc(this.IsValidGameItem), new ActionPhase_TargetGameItem.IsValidArchfiendFunc(this.IsValidArchfiend), new ActionPhase_TargetGameItem.IsSelectableGameItemFunc(base.FilterGameItem), 1, ActionPhaseType.None);
			yield return new ActionPhase_SelectPraetorCombatMove(this.TargetContext, new Action<ConfigRef<PraetorCombatMoveStaticData>>(this.SetTarget), new ActionPhase_SingleTarget<ConfigRef<PraetorCombatMoveStaticData>>.IsValidFunc(this.ValidateTarget));
			yield break;
		}

		// Token: 0x0600131C RID: 4892 RVA: 0x0004894D File Offset: 0x00046B4D
		private void SetTarget(ConfigRef<PraetorCombatMoveStaticData> target)
		{
			this.Technique = target;
		}

		// Token: 0x0600131D RID: 4893 RVA: 0x00048958 File Offset: 0x00046B58
		private Result ValidateTarget(TurnContext context, ConfigRef<PraetorCombatMoveStaticData> target, int invokingPlayerId)
		{
			Manuscript manuscript;
			ManuscriptStaticData manuscriptStaticData;
			if (!context.TryGetItemAndData(this.ManuscriptId, out manuscript, out manuscriptStaticData))
			{
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					logger.Error(string.Format("Invalid target Manuscript {0}", this.ManuscriptId));
				}
				return Result.Failure;
			}
			UpgradePraetorAbilityStaticData upgradePraetorAbilityStaticData;
			if (!context.Database.TryFetch<UpgradePraetorAbilityStaticData>(manuscriptStaticData.ProvidedAbility, out upgradePraetorAbilityStaticData))
			{
				return new Result.InvokeManuscriptProblem(this.ManuscriptId);
			}
			return this.ValidateReplacement(context, invokingPlayerId, base.TargetItemId, target, upgradePraetorAbilityStaticData.Move);
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x000489D8 File Offset: 0x00046BD8
		public Result ValidateReplacement(TurnContext context, int playerId, Identifier prospectiveTargetId, ConfigRef<PraetorCombatMoveStaticData> techniqueToReplace, ConfigRef<PraetorCombatMoveStaticData> replacingTechnique)
		{
			Praetor praetor;
			if (!context.CurrentTurn.TryFetchGameItem<Praetor>(prospectiveTargetId, out praetor))
			{
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					logger.Error(string.Format("Invalid target Praetor {0}", prospectiveTargetId));
				}
				return new Result.InvokeManuscriptProblem(this.ManuscriptId);
			}
			PraetorCombatMoveStaticData praetorCombatMoveStaticData;
			if (!context.Database.TryFetch<PraetorCombatMoveStaticData>(replacingTechnique, out praetorCombatMoveStaticData))
			{
				SimLogger logger2 = SimLogger.Logger;
				if (logger2 != null)
				{
					logger2.Error(string.Format("Trying to apply invalid move {0} to Praetor", replacingTechnique));
				}
				return new Result.InvokeManuscriptOnGameItemProblem(this.ManuscriptId, prospectiveTargetId);
			}
			PraetorCombatMoveInstance praetorCombatMoveInstance;
			if (!praetor.TryGetTechniqueInstance(techniqueToReplace, out praetorCombatMoveInstance))
			{
				return new Result.InvalidTargetTechniqueProblem(this.ManuscriptId, prospectiveTargetId, techniqueToReplace);
			}
			PraetorCombatMoveInstance praetorCombatMoveInstance2;
			if (praetor.TryGetTechniqueInstance(replacingTechnique, out praetorCombatMoveInstance2) && praetorCombatMoveInstance.CombatMoveReference.Id != praetorCombatMoveInstance2.CombatMoveReference.Id)
			{
				return new Result.DuplicateTechniqueOnPraetorProblem(this.ManuscriptId, prospectiveTargetId, techniqueToReplace);
			}
			PlayerState player = context.CurrentTurn.FindPlayerState(playerId, null);
			switch (PraetorDuelTransactions.GetTechniqueReplacementMode(context, player, praetor))
			{
			case PraetorDuelTransactions.TechniqueReplacementMode.None:
				return new Result.PraetorCannotLearnTechniquesProblem(this.ManuscriptId, prospectiveTargetId, techniqueToReplace);
			case PraetorDuelTransactions.TechniqueReplacementMode.SameType:
			{
				PraetorCombatMoveStaticData praetorCombatMoveStaticData2 = context.Database.Fetch(techniqueToReplace);
				PraetorCombatMoveStaticData praetorCombatMoveStaticData3 = context.Database.Fetch(replacingTechnique);
				if (praetorCombatMoveStaticData2.TechniqueType.Id != praetorCombatMoveStaticData3.TechniqueType.Id)
				{
					return new Result.ChangePraetorTechniqueTypeProblem(this.ManuscriptId, prospectiveTargetId, techniqueToReplace, praetorCombatMoveStaticData3.TechniqueType);
				}
				break;
			}
			}
			return Result.Success;
		}

		// Token: 0x040008D9 RID: 2265
		[JsonProperty]
		public ConfigRef<PraetorCombatMoveStaticData> Technique;
	}
}
