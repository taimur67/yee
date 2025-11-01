using System;

namespace LoG
{
	// Token: 0x02000192 RID: 402
	public class WPStolenCanton : WorldProperty
	{
		// Token: 0x0600076B RID: 1899 RVA: 0x000230F8 File Offset: 0x000212F8
		public WPStolenCanton(int archfiendID)
		{
			this.ArchfiendID = archfiendID;
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x00023107 File Offset: 0x00021307
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return false;
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x0002310C File Offset: 0x0002130C
		public override WPProvidesEffect ProvidesEffect(WorldProperty precondition)
		{
			WPStolenCanton wpstolenCanton = precondition as WPStolenCanton;
			if (wpstolenCanton != null && wpstolenCanton.ArchfiendID == this.ArchfiendID)
			{
				return WPProvidesEffect.Yes;
			}
			return WPProvidesEffect.No;
		}

		// Token: 0x04000363 RID: 867
		public int ArchfiendID;
	}
}
