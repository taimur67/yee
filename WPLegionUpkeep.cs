using System;

namespace LoG
{
	// Token: 0x0200017D RID: 381
	public class WPLegionUpkeep : WorldProperty<WPLegionUpkeep>
	{
		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000734 RID: 1844 RVA: 0x00022A87 File Offset: 0x00020C87
		public override bool MustBeSuccessfullFulfilledAsPrecondition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x00022A8A File Offset: 0x00020C8A
		public WPLegionUpkeep(Identifier legionID)
		{
			this.LegionID = legionID;
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x00022A99 File Offset: 0x00020C99
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return false;
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x00022A9C File Offset: 0x00020C9C
		public override WPProvidesEffect ProvidesEffectInternal(WPLegionUpkeep upkeepPrecondition)
		{
			if (this.LegionID != upkeepPrecondition.LegionID)
			{
				return WPProvidesEffect.No;
			}
			return WPProvidesEffect.Yes;
		}

		// Token: 0x0400034E RID: 846
		public Identifier LegionID;
	}
}
