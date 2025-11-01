using System;
using System.Linq;
using Game.Simulation.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x020000E4 RID: 228
	public class ActionInvokeManuscript_Treatise : ActionOrderGOAPNode<InvokeManuscriptOrder>
	{
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000359 RID: 857 RVA: 0x0000F9A2 File Offset: 0x0000DBA2
		public override ActionID ID
		{
			get
			{
				return ActionID.Invoke_Manuscript_Treatise;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600035A RID: 858 RVA: 0x0000F9A6 File Offset: 0x0000DBA6
		public override string ActionName
		{
			get
			{
				return "Invoke treatise: " + this.StaticData.Id;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600035B RID: 859 RVA: 0x0000F9BD File Offset: 0x0000DBBD
		public override ActionOrderPriority Priority
		{
			get
			{
				return ActionOrderPriority.Low_AlwaysLast;
			}
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000F9C0 File Offset: 0x0000DBC0
		public ActionInvokeManuscript_Treatise(ManuscriptStaticData staticData)
		{
			this.StaticData = staticData;
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000F9D8 File Offset: 0x0000DBD8
		private bool AnyEffectiveModifications(out PowerType modifierPower, out int effectiveOffset)
		{
			modifierPower = PowerType.None;
			effectiveOffset = 0;
			ModifyArchfiendData modifyArchfiendData = this.OwningPlanner.Database.Fetch(this.StaticData.ProvidedAbility) as ModifyArchfiendData;
			if (modifyArchfiendData == null)
			{
				return false;
			}
			foreach (ModifierStaticDataExtensions.ArchfiendStatModification archfiendStatModification in modifyArchfiendData.Modifiers.EffectiveModifications(this.OwningPlanner.PlayerState))
			{
				if (archfiendStatModification.EffectiveOffset > 0)
				{
					modifierPower = archfiendStatModification.Stat.ToPowerType();
					effectiveOffset = archfiendStatModification.EffectiveOffset;
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000FA80 File Offset: 0x0000DC80
		public override void Prepare()
		{
			PowerType powerType;
			int num;
			if (!this.AnyEffectiveModifications(out powerType, out num))
			{
				base.Disable("Power is already maxed out");
				return;
			}
			base.AddConstraint(new WPActionCooldown(ActionID.Invoke_Manuscript_Treatise, this.Cooldown));
			base.AddPrecondition(new WPCompletedManuscript(this.StaticData, ""));
			if (num > 1)
			{
				base.AddScalarCostReduction(0.5f, PFCostModifier.Heuristic_Bonus);
			}
			int newLevel = this.OwningPlanner.PlayerState.PowersLevels.Get(powerType) + num;
			ActionLevelUpArchfiend.ApplyEffects(this, powerType, newLevel);
			base.Prepare();
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000FB0A File Offset: 0x0000DD0A
		protected override InvokeManuscriptOrder GenerateOrder()
		{
			return new ManuscriptArchfiendOrder
			{
				AbilityId = this.StaticData.ProvidedAbility.Id
			};
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000FB28 File Offset: 0x0000DD28
		public override Result SubmitAction(TurnContext context, PlayerState playerState)
		{
			Manuscript manuscript = context.CurrentTurn.GetGameItemsControlledBy<Manuscript>(playerState.Id).First((Manuscript t) => t.StaticDataId == this.StaticData.Id);
			if (manuscript != null && manuscript.Id != Identifier.Invalid)
			{
				base.Order.ManuscriptId = manuscript.Id;
				Result result = base.SubmitAction(context, playerState);
				if (result.successful)
				{
					this.OwningPlanner.AIPersistentData.RecordActionUsed(ActionID.Invoke_Manuscript_Treatise, this.OwningPlanner.TrueTurn);
				}
				return result;
			}
			return Result.Failure;
		}

		// Token: 0x0400020B RID: 523
		public int Cooldown = 1;

		// Token: 0x0400020C RID: 524
		public ManuscriptStaticData StaticData;
	}
}
