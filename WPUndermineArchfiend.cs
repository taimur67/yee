using System;

namespace LoG
{
	// Token: 0x0200019A RID: 410
	public class WPUndermineArchfiend : WorldProperty<WPUndermineArchfiend>
	{
		// Token: 0x06000781 RID: 1921 RVA: 0x000233CC File Offset: 0x000215CC
		public WPUndermineArchfiend(int archfiendID)
		{
			this.ArchfiendID = archfiendID;
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x000233DB File Offset: 0x000215DB
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return false;
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x000233DE File Offset: 0x000215DE
		public override WPProvidesEffect ProvidesEffectInternal(WPUndermineArchfiend property)
		{
			if (this.ArchfiendID == property.ArchfiendID)
			{
				return WPProvidesEffect.Yes;
			}
			return WPProvidesEffect.No;
		}

		// Token: 0x0400036E RID: 878
		public int ArchfiendID;
	}
}
