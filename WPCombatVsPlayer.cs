using System;

namespace LoG
{
	// Token: 0x0200015C RID: 348
	public class WPCombatVsPlayer : WorldProperty
	{
		// Token: 0x060006D8 RID: 1752 RVA: 0x00021D10 File Offset: 0x0001FF10
		public WPCombatVsPlayer(int targetPlayer)
		{
			this.PlayerID = targetPlayer;
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x00021D1F File Offset: 0x0001FF1F
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return false;
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x00021D24 File Offset: 0x0001FF24
		public override WPProvidesEffect ProvidesEffect(WorldProperty precondition)
		{
			WPCombatVsPlayer wpcombatVsPlayer = precondition as WPCombatVsPlayer;
			if (wpcombatVsPlayer != null && this.PlayerID == wpcombatVsPlayer.PlayerID)
			{
				return WPProvidesEffect.Yes;
			}
			return WPProvidesEffect.No;
		}

		// Token: 0x0400031A RID: 794
		public int PlayerID;
	}
}
