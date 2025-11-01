using System;

namespace LoG
{
	// Token: 0x02000176 RID: 374
	public class WPIsActionPlannedThisTurn : WorldProperty
	{
		// Token: 0x06000721 RID: 1825 RVA: 0x00022801 File Offset: 0x00020A01
		public WPIsActionPlannedThisTurn(ActionID action)
		{
			this.Action = action;
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x00022810 File Offset: 0x00020A10
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return planner.IsActionPlannedThisTurn(this.Action);
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x00022820 File Offset: 0x00020A20
		public override WPProvidesEffect ProvidesEffect(WorldProperty precondition)
		{
			WPIsActionPlannedThisTurn wpisActionPlannedThisTurn = precondition as WPIsActionPlannedThisTurn;
			if (wpisActionPlannedThisTurn != null && wpisActionPlannedThisTurn.Action == this.Action)
			{
				return WPProvidesEffect.Yes;
			}
			return WPProvidesEffect.No;
		}

		// Token: 0x04000345 RID: 837
		public ActionID Action;
	}
}
