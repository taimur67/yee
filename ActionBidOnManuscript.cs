using System;
using System.Linq;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x020000DF RID: 223
	public class ActionBidOnManuscript : ActionOrderGOAPNode<OrderMakeBid>
	{
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000331 RID: 817 RVA: 0x0000ECA9 File Offset: 0x0000CEA9
		public override bool DoDynamicScoring
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000332 RID: 818 RVA: 0x0000ECAC File Offset: 0x0000CEAC
		public override string ActionName
		{
			get
			{
				return "Bid on manuscript: " + base.Context.DebugName(this.ManuscriptID);
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000333 RID: 819 RVA: 0x0000ECC9 File Offset: 0x0000CEC9
		public override ActionID ID
		{
			get
			{
				return ActionID.Bid_On_Manuscript;
			}
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0000ECCD File Offset: 0x0000CECD
		public ActionBidOnManuscript(Identifier identifier)
		{
			this.ManuscriptID = identifier;
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000ECEC File Offset: 0x0000CEEC
		private void CalcCost()
		{
			Manuscript manuscript = this.OwningPlanner.TrueTurn.FetchGameItem<Manuscript>(this.ManuscriptID);
			int manuscriptCurrentFragmentCount = this.OwningPlanner.TrueContext.GetManuscriptCurrentFragmentCount(this.OwningPlanner.PlayerId, manuscript);
			int num = manuscript.GetFragmentCount(this.OwningPlanner.Database) - manuscriptCurrentFragmentCount;
			if (num <= 0)
			{
				base.Disable("Already has enough fragments for manuscript " + manuscript.NameKey);
				return;
			}
			int num2 = num - 1;
			if (num2 > 0)
			{
				float amount = MathF.Min(1f, (float)(num2 * num2) * 0.2f);
				base.AddScalarCostIncrease(amount, PFCostModifier.Heuristic_Bonus);
			}
			if (this.OwningPlanner.PlayerState.AITags.Any((AITag t) => t == AITag.Hoarder || t == AITag.Duellist))
			{
				base.AddScalarCostReduction(0.5f, PFCostModifier.Heuristic_Bonus);
			}
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000EDC8 File Offset: 0x0000CFC8
		public override void Prepare()
		{
			Manuscript manuscript = this.OwningPlanner.TrueTurn.TryFetchGameItem<Manuscript>(this.ManuscriptID);
			if (manuscript == null)
			{
				base.Disable(string.Format("No manuscript exists with id {0}", this.ManuscriptID));
				return;
			}
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
			base.AddConstraint(new WPActionCooldown(ActionID.Bid_On_Manuscript, this.Cooldown));
			this.CalcCost();
			base.Prepare();
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000EEB0 File Offset: 0x0000D0B0
		protected override OrderMakeBid GenerateOrder()
		{
			return new OrderMakeBid(this.ManuscriptID);
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0000EEBD File Offset: 0x0000D0BD
		public override Result SubmitAction(TurnContext context, PlayerState playerState)
		{
			Result result = base.SubmitAction(context, playerState);
			if (result == Result.Success)
			{
				this.OwningPlanner.AIPersistentData.RecordActionUsed(ActionID.Bid_On_Manuscript, context.CurrentTurn);
			}
			return result;
		}

		// Token: 0x040001FF RID: 511
		public Identifier ManuscriptID = Identifier.Invalid;

		// Token: 0x04000200 RID: 512
		public int Cooldown = 3;
	}
}
