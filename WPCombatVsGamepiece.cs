using System;

namespace LoG
{
	// Token: 0x0200015B RID: 347
	public class WPCombatVsGamepiece : WorldProperty
	{
		// Token: 0x060006D5 RID: 1749 RVA: 0x00021CC9 File Offset: 0x0001FEC9
		public WPCombatVsGamepiece(Identifier combatTargetID)
		{
			this.CombatTargetID = combatTargetID;
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x00021CD8 File Offset: 0x0001FED8
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return planner.IsCombatScheduled(this.CombatTargetID);
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x00021CE8 File Offset: 0x0001FEE8
		public override WPProvidesEffect ProvidesEffect(WorldProperty precondition)
		{
			WPCombatVsGamepiece wpcombatVsGamepiece = precondition as WPCombatVsGamepiece;
			if (wpcombatVsGamepiece != null && this.CombatTargetID == wpcombatVsGamepiece.CombatTargetID)
			{
				return WPProvidesEffect.Yes;
			}
			return WPProvidesEffect.No;
		}

		// Token: 0x04000319 RID: 793
		public Identifier CombatTargetID;
	}
}
