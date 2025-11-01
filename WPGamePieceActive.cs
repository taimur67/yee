using System;

namespace LoG
{
	// Token: 0x02000167 RID: 359
	public class WPGamePieceActive : WorldProperty
	{
		// Token: 0x060006FA RID: 1786 RVA: 0x0002228C File Offset: 0x0002048C
		public WPGamePieceActive(GameItem item)
		{
			this.GameItem = item;
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x0002229B File Offset: 0x0002049B
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return this.GameItem != null && this.GameItem.IsActive;
		}

		// Token: 0x04000336 RID: 822
		public GameItem GameItem;
	}
}
