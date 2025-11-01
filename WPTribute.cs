using System;
using System.Collections.Generic;
using System.Linq;
using LoG.Simulation.Extensions;

namespace LoG
{
	// Token: 0x02000196 RID: 406
	public class WPTribute : WorldProperty
	{
		// Token: 0x06000773 RID: 1907 RVA: 0x000231E6 File Offset: 0x000213E6
		public static WPTribute PrestigeOnly(int prestigeAmount)
		{
			Cost cost = new Cost();
			cost.Set(ResourceTypes.Prestige, prestigeAmount);
			return new WPTribute(cost);
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x000231FB File Offset: 0x000213FB
		public WPTribute(Cost cost)
		{
			this.Cost = cost;
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x0002320A File Offset: 0x0002140A
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return this.Cost.IsFulfilledBy(planner.PreviewAvailableResources, planner.PreviewNumResources);
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x00023224 File Offset: 0x00021424
		public FulfilmentAmount IsFulfilledByOtherTributeEffect(PlayerState playerState, TurnState playerViewOfTurnState, GOAPPlanner planner, WPTribute tributeEffect)
		{
			ResourceAccumulation resourceAccumulation = planner.PreviewAvailableResources.DeepClone<ResourceAccumulation>();
			int num = resourceAccumulation.GetAllTypesCantAfford(this.Cost).Count<ResourceTypes>();
			resourceAccumulation.Add(tributeEffect.Cost);
			if (resourceAccumulation.MeetsValueObligations(this.Cost))
			{
				return FulfilmentAmount.Full;
			}
			if (resourceAccumulation.GetAllTypesCantAfford(this.Cost).Count<ResourceTypes>() < num)
			{
				return FulfilmentAmount.Partial;
			}
			return FulfilmentAmount.None;
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x00023284 File Offset: 0x00021484
		public override float CalculateDynamicCostScalar(GOAPPlanner planner, IReadOnlyList<WorldProperty> effects)
		{
			WPTribute tributeEffect;
			if (!effects.FirstOrDefault(out tributeEffect))
			{
				return 1f;
			}
			FulfilmentAmount fulfilmentAmount = this.IsFulfilledByOtherTributeEffect(planner.AIPreviewPlayerState, planner.PlayerViewOfTurnState, planner, tributeEffect);
			if (fulfilmentAmount == FulfilmentAmount.Full)
			{
				return GoapNodeCosts.Discount_Scalar_FullyFufilled;
			}
			if (fulfilmentAmount == FulfilmentAmount.Partial)
			{
				return GoapNodeCosts.Discount_Scalar_PartialFufilled;
			}
			return GoapNodeCosts.Penalty_Scalar_NotFulfilled;
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x000232D0 File Offset: 0x000214D0
		public override WPProvidesEffect ProvidesEffect(WorldProperty precondition)
		{
			WPTribute wptribute = precondition as WPTribute;
			if (wptribute == null)
			{
				return WPProvidesEffect.No;
			}
			if (this.Cost.IsPrestigeOnly() != wptribute.Cost.IsPrestigeOnly())
			{
				return WPProvidesEffect.No;
			}
			if (this.OwningPlanner.PreviewAvailableResources.MeetsValueObligations(wptribute.Cost))
			{
				return WPProvidesEffect.Redundant;
			}
			return WPProvidesEffect.Yes;
		}

		// Token: 0x0400036B RID: 875
		public Cost Cost;
	}
}
