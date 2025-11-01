using System;

namespace LoG
{
	// Token: 0x0200017A RID: 378
	public class WPLegionHasCombatCard : WorldProperty
	{
		// Token: 0x0600072B RID: 1835 RVA: 0x0002291E File Offset: 0x00020B1E
		public WPLegionHasCombatCard(Identifier legionID)
		{
			this.LegionID = legionID;
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x0002292D File Offset: 0x00020B2D
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return IEnumerableExtensions.Any<Stratagem>(viewContext.CurrentTurn.FetchGameItem<GamePiece>(this.LegionID).GetAttachedItems(viewContext.CurrentTurn));
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x00022950 File Offset: 0x00020B50
		public override WPProvidesEffect ProvidesEffect(WorldProperty precondition)
		{
			WPLegionHasCombatCard wplegionHasCombatCard = precondition as WPLegionHasCombatCard;
			if (wplegionHasCombatCard != null && wplegionHasCombatCard.LegionID == this.LegionID)
			{
				return WPProvidesEffect.Yes;
			}
			return WPProvidesEffect.No;
		}

		// Token: 0x0400034B RID: 843
		public Identifier LegionID;
	}
}
