using System;

namespace LoG
{
	// Token: 0x02000154 RID: 340
	public class WPArchfiendEliminated : WorldProperty<WPArchfiendEliminated>
	{
		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060006B2 RID: 1714 RVA: 0x00021692 File Offset: 0x0001F892
		public override bool MustBeSuccessfullFulfilledAsPrecondition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x00021695 File Offset: 0x0001F895
		public WPArchfiendEliminated(int archfiendID)
		{
			this.ArchfiendID = archfiendID;
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x000216A4 File Offset: 0x0001F8A4
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return false;
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x000216A7 File Offset: 0x0001F8A7
		public override WPProvidesEffect ProvidesEffectInternal(WPArchfiendEliminated property)
		{
			if (this.ArchfiendID != property.ArchfiendID)
			{
				return WPProvidesEffect.No;
			}
			return WPProvidesEffect.Yes;
		}

		// Token: 0x0400030A RID: 778
		public int ArchfiendID;
	}
}
