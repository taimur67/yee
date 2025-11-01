using System;

namespace LoG
{
	// Token: 0x02000187 RID: 391
	public class WPPoPStolenFrom : WorldProperty
	{
		// Token: 0x0600074E RID: 1870 RVA: 0x00022CFB File Offset: 0x00020EFB
		public WPPoPStolenFrom(int archfiendID)
		{
			this.ArchfiendID = archfiendID;
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x00022D0A File Offset: 0x00020F0A
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return false;
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x00022D10 File Offset: 0x00020F10
		public override WPProvidesEffect ProvidesEffect(WorldProperty precondition)
		{
			WPPoPStolenFrom wppoPStolenFrom = precondition as WPPoPStolenFrom;
			if (wppoPStolenFrom != null && wppoPStolenFrom.ArchfiendID == this.ArchfiendID)
			{
				return WPProvidesEffect.Yes;
			}
			return WPProvidesEffect.No;
		}

		// Token: 0x04000357 RID: 855
		public int ArchfiendID;
	}
}
