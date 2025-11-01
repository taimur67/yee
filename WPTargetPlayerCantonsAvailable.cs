using System;

namespace LoG
{
	// Token: 0x02000193 RID: 403
	public class WPTargetPlayerCantonsAvailable : WorldProperty
	{
		// Token: 0x0600076E RID: 1902 RVA: 0x00023134 File Offset: 0x00021334
		public WPTargetPlayerCantonsAvailable(int archfiendID)
		{
			this.ArchfiendID = archfiendID;
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x00023143 File Offset: 0x00021343
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return IEnumerableExtensions.Any<HexCoord>(viewContext.GetClaimableBorderCantons(playerState, true, this.ArchfiendID));
		}

		// Token: 0x04000364 RID: 868
		public int ArchfiendID;
	}
}
