using System;
using System.Linq;
using Core.StaticData;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x020000E1 RID: 225
	public class ActionInvokeManuscript_Manual : ActionOrderGOAPNode<InvokeManuscriptOrder>
	{
		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000341 RID: 833 RVA: 0x0000F157 File Offset: 0x0000D357
		public override ActionID ID
		{
			get
			{
				return ActionID.Invoke_Manuscript_Manual;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000342 RID: 834 RVA: 0x0000F15B File Offset: 0x0000D35B
		public override string ActionName
		{
			get
			{
				return string.Format("Invoke manual: {0} on praetor {1}", this.StaticData.Id, this.PraetorID);
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000343 RID: 835 RVA: 0x0000F17D File Offset: 0x0000D37D
		public override ActionOrderPriority Priority
		{
			get
			{
				return ActionOrderPriority.Low_AlwaysLast;
			}
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000F180 File Offset: 0x0000D380
		public ActionInvokeManuscript_Manual(ManuscriptStaticData staticData, Identifier praetorID, ConfigRef<PraetorCombatMoveStaticData> techniqueToReplace, bool replacesDuplicate)
		{
			this.StaticData = staticData;
			this.PraetorID = praetorID;
			this.TechniqueToReplace = techniqueToReplace;
			this.ReplacesDuplicate = replacesDuplicate;
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000F1AC File Offset: 0x0000D3AC
		public override void Prepare()
		{
			base.AddConstraint(new WPActionCooldown(ActionID.Invoke_Manuscript_Manual, this.Cooldown));
			base.AddPrecondition(new WPCompletedManuscript(this.StaticData, ""));
			foreach (PlayerState playerState in this.OwningPlanner.TrueTurn.EnumeratePlayerStates(false, false))
			{
				if (playerState.Id != this.OwningPlanner.PlayerId)
				{
					base.AddEffect(new WPDuelAdvantage(playerState.Id));
				}
			}
			base.AddScalarCostReduction(this.ReplacesDuplicate ? 0.9f : 0.65f, PFCostModifier.Heuristic_Bonus);
			base.Prepare();
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000F26C File Offset: 0x0000D46C
		protected override InvokeManuscriptOrder GenerateOrder()
		{
			PraetorCombatMoveUpgrade_ManuscriptOrder praetorCombatMoveUpgrade_ManuscriptOrder = new PraetorCombatMoveUpgrade_ManuscriptOrder();
			TargetContext targetContext = new TargetContext(this.PraetorID);
			praetorCombatMoveUpgrade_ManuscriptOrder.TargetContext = targetContext;
			praetorCombatMoveUpgrade_ManuscriptOrder.AbilityId = this.StaticData.ProvidedAbility.Id;
			return praetorCombatMoveUpgrade_ManuscriptOrder;
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000F2A8 File Offset: 0x0000D4A8
		public override Result SubmitAction(TurnContext context, PlayerState playerState)
		{
			PraetorCombatMoveUpgrade_ManuscriptOrder praetorCombatMoveUpgrade_ManuscriptOrder = base.Order as PraetorCombatMoveUpgrade_ManuscriptOrder;
			if (praetorCombatMoveUpgrade_ManuscriptOrder != null)
			{
				Manuscript manuscript = context.CurrentTurn.GetGameItemsControlledBy<Manuscript>(playerState.Id).FirstOrDefault((Manuscript t) => t.StaticDataId == this.StaticData.Id);
				if (manuscript == null || manuscript.Id == Identifier.Invalid)
				{
					return Result.Failure;
				}
				praetorCombatMoveUpgrade_ManuscriptOrder.ManuscriptId = manuscript.Id;
				praetorCombatMoveUpgrade_ManuscriptOrder.Technique = this.TechniqueToReplace;
			}
			Result result = base.SubmitAction(context, playerState);
			if (result.successful)
			{
				this.OwningPlanner.AIPersistentData.RecordActionUsed(ActionID.Invoke_Manuscript_Manual, this.OwningPlanner.TrueTurn);
			}
			return result;
		}

		// Token: 0x04000202 RID: 514
		public ManuscriptStaticData StaticData;

		// Token: 0x04000203 RID: 515
		public Identifier PraetorID;

		// Token: 0x04000204 RID: 516
		public ConfigRef<PraetorCombatMoveStaticData> TechniqueToReplace;

		// Token: 0x04000205 RID: 517
		public int Cooldown = 1;

		// Token: 0x04000206 RID: 518
		public bool ReplacesDuplicate;
	}
}
