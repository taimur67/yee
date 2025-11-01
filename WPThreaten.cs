using System;

namespace LoG
{
	// Token: 0x02000194 RID: 404
	public class WPThreaten : WorldProperty
	{
		// Token: 0x06000770 RID: 1904 RVA: 0x0002315D File Offset: 0x0002135D
		public WPThreaten(int aggressorPlayerID, int targetPlayerID)
		{
			this.AggressorPlayerID = aggressorPlayerID;
			this.TargetPlayerID = targetPlayerID;
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x00023174 File Offset: 0x00021374
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			PlayerState playerState2 = viewContext.CurrentTurn.FindPlayerState(this.TargetPlayerID, null);
			return playerState2 != null && planner.ArchfiendHeuristics.GetThreatens(playerState.Id, playerState2.Id);
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x000231B0 File Offset: 0x000213B0
		public override WPProvidesEffect ProvidesEffect(WorldProperty precondition)
		{
			WPThreaten wpthreaten = precondition as WPThreaten;
			if (wpthreaten != null && wpthreaten.TargetPlayerID == this.TargetPlayerID && wpthreaten.AggressorPlayerID == this.AggressorPlayerID)
			{
				return WPProvidesEffect.Yes;
			}
			return WPProvidesEffect.No;
		}

		// Token: 0x04000365 RID: 869
		public int AggressorPlayerID;

		// Token: 0x04000366 RID: 870
		public int TargetPlayerID;
	}
}
