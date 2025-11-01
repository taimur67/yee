using System;

namespace LoG
{
	// Token: 0x02000150 RID: 336
	public class WPAbilityAvailable : WorldProperty
	{
		// Token: 0x060006A9 RID: 1705 RVA: 0x000214CB File Offset: 0x0001F6CB
		public WPAbilityAvailable(ActionableOrder order)
		{
			this.Order = order;
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x000214DC File Offset: 0x0001F6DC
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			ITarget target = this.Order as ITarget;
			return (target == null || !this.Order.NeedsTargetContext || this.Order.IsValidTarget(viewContext, playerState.Id, target.Target)) && this._abilityHelper.IsAvailable(this.Order, false);
		}

		// Token: 0x04000305 RID: 773
		public ActionableOrder Order;
	}
}
