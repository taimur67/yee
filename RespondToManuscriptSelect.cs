using System;
using System.Collections.Generic;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000129 RID: 297
	public class RespondToManuscriptSelect : DecisionResponseGOAPNode<SelectTributeDecisionRequest, SelectTributeDecisionResponse>
	{
		// Token: 0x1700013C RID: 316
		// (get) Token: 0x0600056D RID: 1389 RVA: 0x0001A2C5 File Offset: 0x000184C5
		public override ActionID ID
		{
			get
			{
				return ActionID.Decision_RespondToManuscriptSelect;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x0600056E RID: 1390 RVA: 0x0001A2C9 File Offset: 0x000184C9
		public override string ActionName
		{
			get
			{
				return "Respond to Manuscript Select";
			}
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x0001A2D0 File Offset: 0x000184D0
		public RespondToManuscriptSelect(SelectTributeDecisionRequest request, IEnumerable<int> selection)
		{
			this.Request = request;
			this.Selection = IEnumerableExtensions.ToList<int>(selection);
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x0001A2EC File Offset: 0x000184EC
		public override void Prepare()
		{
			base.AddConstraint(new WPDecisionNeedsMaking(this.Request));
			TurnState trueTurn = this.OwningPlanner.TrueTurn;
			PlayerState playerState = this.OwningPlanner.PlayerState;
			float amount = 0.5f;
			foreach (Manuscript manuscript in this.Request.Candidates.Manuscripts)
			{
				float num = (float)this.OwningPlanner.TrueContext.GetManuscriptCurrentFragmentCount(playerState.Id, manuscript) / (float)manuscript.GetFragmentCount(this.OwningPlanner.Database);
				bool flag = num >= 0f;
				if (num >= 1f)
				{
					ref amount.LerpTo01(0.5f);
				}
				else if (flag)
				{
					ref amount.LerpTo01(-0.25f);
				}
				if (this.Selection.Contains((int)manuscript.Id))
				{
					ManuscriptStaticData manuscriptStaticData = base.GameDatabase.Fetch<ManuscriptStaticData>(manuscript.StaticDataId);
					if (manuscript.GetCategory(base.GameDatabase) == ManuscriptCategory.Manual)
					{
						ConfigRef<AbilityStaticData> providedAbility = manuscriptStaticData.ProvidedAbility;
						ConfigRef<PraetorCombatMoveStaticData> move = base.GameDatabase.Fetch<UpgradePraetorAbilityStaticData>(providedAbility.Id).Move;
						PraetorCombatMoveStaticData praetorCombatMoveStaticData = base.GameDatabase.Fetch(move);
						PraetorCombatMoveStyle praetorCombatMoveStyle = base.GameDatabase.Fetch(praetorCombatMoveStaticData.TechniqueType);
						base.AddEffect(new WPCompletedManuscript(manuscriptStaticData, praetorCombatMoveStyle.Id));
					}
					else
					{
						base.AddEffect(new WPCompletedManuscript(manuscriptStaticData, ""));
					}
				}
			}
			base.AddScalarCostIncrease(amount, PFCostModifier.Heuristic_Bonus);
			base.Prepare();
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x0001A49C File Offset: 0x0001869C
		protected override SelectTributeDecisionResponse GenerateDecision()
		{
			SelectTributeDecisionResponse selectTributeDecisionResponse = base.GenerateDecision();
			foreach (int selectedId in this.Selection)
			{
				selectTributeDecisionResponse.Select(selectedId);
			}
			return selectTributeDecisionResponse;
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x0001A4F8 File Offset: 0x000186F8
		public override Result SubmitAction(TurnContext context, PlayerState playerState)
		{
			return base.SubmitAction(context, playerState);
		}

		// Token: 0x040002B5 RID: 693
		public List<int> Selection;
	}
}
