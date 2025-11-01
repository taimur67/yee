using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x020000E6 RID: 230
	public abstract class ActionOrderGOAPNode<T> : GOAPNode where T : ActionableOrder
	{
		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600036C RID: 876 RVA: 0x0001036C File Offset: 0x0000E56C
		protected T Order
		{
			get
			{
				T result;
				if ((result = this._order) == null)
				{
					result = (this._order = this.GenerateOrder());
				}
				return result;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600036D RID: 877 RVA: 0x00010397 File Offset: 0x0000E597
		public virtual bool ReducePriorityWhenTitansNeedActions
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600036E RID: 878
		protected abstract T GenerateOrder();

		// Token: 0x0600036F RID: 879 RVA: 0x0001039A File Offset: 0x0000E59A
		public override bool IsOrderOfType<TOrderType>()
		{
			return this.Order is TOrderType;
		}

		// Token: 0x06000370 RID: 880 RVA: 0x000103AF File Offset: 0x0000E5AF
		public override void Clear()
		{
			this._order = default(T);
			base.Clear();
		}

		// Token: 0x06000371 RID: 881 RVA: 0x000103C4 File Offset: 0x0000E5C4
		public override void Prepare()
		{
			AbilityHelper abilityHelper = this.GetAbilityHelper();
			AbilityStaticData abilityData = abilityHelper.GetAbilityData(this.Order);
			if (abilityData != null && !this.OwningPlanner.IsUnlocked(abilityData.ConfigRef))
			{
				base.Disable("Action " + abilityData.Id + " locked");
				return;
			}
			this.Cost = this.DetermineCost(abilityHelper);
			if (!this.Cost.IsZero)
			{
				base.AddPrecondition(new WPTribute(this.Cost + this.CostSurplus));
			}
			base.AddConstraint(new WPAbilityAvailable(this.Order));
			this.DiscountForSchemes();
			if (this.ReducePriorityWhenTitansNeedActions && !WPEveryTitanHasOrders.Check(this.OwningPlanner))
			{
				base.AddScalarCostIncrease(1f, PFCostModifier.Heuristic_Bonus);
			}
		}

		// Token: 0x06000372 RID: 882 RVA: 0x00010494 File Offset: 0x0000E694
		public void DiscountForSchemes()
		{
			float amount = 0.9f;
			if (this.ContributesToSchemes())
			{
				base.AddScalarCostReduction(amount, PFCostModifier.Scheme);
				base.AddEffect(new WPContributesToScheme());
			}
		}

		// Token: 0x06000373 RID: 883 RVA: 0x000104C4 File Offset: 0x0000E6C4
		public virtual Cost DetermineCost(in AbilityHelper helper)
		{
			AbilityHelper abilityHelper = helper;
			return abilityHelper.CalculateCost(this.Order);
		}

		// Token: 0x06000374 RID: 884 RVA: 0x000104EC File Offset: 0x0000E6EC
		public override Result SubmitAction(TurnContext context, PlayerState playerState)
		{
			AbilityHelper abilityHelper = this.GetAbilityHelper();
			bool flag = true;
			if (!this.Cost.IsZero)
			{
				flag = abilityHelper.TryAutoPay_AICheat(this.OwningPlanner.AIPreviewPlayerState, this.Order, this.Cost);
			}
			if (!flag)
			{
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					logger.Warn("AI Failed to pay for action in node: " + this.ActionName);
				}
				return Result.Failure;
			}
			if (!this.OwningPlanner.PlannedTurn.CheckConflicts(this.Order).successful)
			{
				SimLogger logger2 = SimLogger.Logger;
				if (logger2 != null)
				{
					logger2.Error("Attempted to submit conflicting action");
				}
				return Result.Failure;
			}
			this.OwningPlanner.AddActionToPlan(this.Order);
			return Result.Success;
		}

		// Token: 0x06000375 RID: 885 RVA: 0x000105B4 File Offset: 0x0000E7B4
		public AbilityHelper GetAbilityHelper()
		{
			return new AbilityHelper(this.OwningPlanner.AIPreviewContext, this.OwningPlanner.AIPreviewPlayerState, this.OwningPlanner.PlannedTurn);
		}

		// Token: 0x06000376 RID: 886 RVA: 0x000105DC File Offset: 0x0000E7DC
		public bool ContributesToSchemes()
		{
			foreach (ObjectiveCondition objectiveCondition in this.OwningPlanner.AIPreviewPlayerState.EnumerateActiveObjectives(this.OwningPlanner.TrueTurn))
			{
				if (!objectiveCondition.IsComplete && this.ContributesToScheme(objectiveCondition))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000377 RID: 887 RVA: 0x00010650 File Offset: 0x0000E850
		public virtual bool ContributesToScheme(ObjectiveCondition objectiveCondition)
		{
			return false;
		}

		// Token: 0x06000378 RID: 888 RVA: 0x00010654 File Offset: 0x0000E854
		public override void OnActionFailed()
		{
			base.OnActionFailed();
			OrderMoveLegion orderMoveLegion = this._order as OrderMoveLegion;
			if (orderMoveLegion != null)
			{
				this.OwningPlanner.AITransientData.LegionsWithBlockedMovement.Add(orderMoveLegion.GamePieceId);
			}
		}

		// Token: 0x06000379 RID: 889 RVA: 0x00010698 File Offset: 0x0000E898
		public override void OnActionSubmitted()
		{
			base.OnActionSubmitted();
			OrderMoveLegion orderMoveLegion = this._order as OrderMoveLegion;
			if (orderMoveLegion != null)
			{
				this.OwningPlanner.AIPersistentData.RemoveBlockingPiece(orderMoveLegion.GamePieceId);
			}
		}

		// Token: 0x0400020F RID: 527
		private T _order;

		// Token: 0x04000210 RID: 528
		private Cost Cost = Cost.None;

		// Token: 0x04000211 RID: 529
		public Cost CostSurplus = Cost.None;
	}
}
