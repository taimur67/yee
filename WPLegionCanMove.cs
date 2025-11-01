using System;

namespace LoG
{
	// Token: 0x02000178 RID: 376
	public class WPLegionCanMove : WorldProperty
	{
		// Token: 0x06000727 RID: 1831 RVA: 0x000228B6 File Offset: 0x00020AB6
		public WPLegionCanMove(GamePiece legion)
		{
			this.Legion = legion;
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x000228C5 File Offset: 0x00020AC5
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			GamePiece legion = this.Legion;
			return legion != null && legion.CanMove;
		}

		// Token: 0x04000348 RID: 840
		public GamePiece Legion;
	}
}
