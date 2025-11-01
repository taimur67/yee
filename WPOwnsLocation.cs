using System;

namespace LoG
{
	// Token: 0x02000186 RID: 390
	public class WPOwnsLocation : WorldProperty<WPOwnsLocation>
	{
		// Token: 0x0600074B RID: 1867 RVA: 0x00022C70 File Offset: 0x00020E70
		public WPOwnsLocation(HexCoord hexCoord, int playerId)
		{
			this.HexCoord = hexCoord;
			this.PlayerId = playerId;
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x00022C88 File Offset: 0x00020E88
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return viewContext.CurrentTurn.HexBoard[this.HexCoord].ControllingPlayerID == this.PlayerId || planner.AITransientData.PotentialCapture.Contains(this.HexCoord);
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x00022CD5 File Offset: 0x00020ED5
		public override WPProvidesEffect ProvidesEffectInternal(WPOwnsLocation precondition)
		{
			if (precondition.PlayerId == this.PlayerId && precondition.HexCoord == this.HexCoord)
			{
				return WPProvidesEffect.Yes;
			}
			return WPProvidesEffect.No;
		}

		// Token: 0x04000355 RID: 853
		public readonly HexCoord HexCoord;

		// Token: 0x04000356 RID: 854
		public readonly int PlayerId;
	}
}
