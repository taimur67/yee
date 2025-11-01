using System;

namespace LoG
{
	// Token: 0x02000164 RID: 356
	public class WPDuelAdvantage : WorldProperty<WPDuelAdvantage>
	{
		// Token: 0x060006EC RID: 1772 RVA: 0x00022000 File Offset: 0x00020200
		public WPDuelAdvantage(int playerID)
		{
			this.PlayerID = playerID;
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x0002200F File Offset: 0x0002020F
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return false;
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x00022012 File Offset: 0x00020212
		public override WPProvidesEffect ProvidesEffectInternal(WPDuelAdvantage precondition)
		{
			if (this.PlayerID == precondition.PlayerID)
			{
				return WPProvidesEffect.Yes;
			}
			return WPProvidesEffect.No;
		}

		// Token: 0x04000331 RID: 817
		public int PlayerID;
	}
}
