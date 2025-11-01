using System;

namespace LoG
{
	// Token: 0x0200016D RID: 365
	public class WPHasAnyPraetor : WorldProperty<WPHasAnyPraetor>
	{
		// Token: 0x06000708 RID: 1800 RVA: 0x00022471 File Offset: 0x00020671
		public static bool Check(TurnState turn, int playerId)
		{
			return IEnumerableExtensions.Any<Praetor>(turn.GetGameItemsControlledBy<Praetor>(playerId));
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x0002247F File Offset: 0x0002067F
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return WPHasAnyPraetor.Check(planner.TrueTurn, playerState.Id);
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x00022492 File Offset: 0x00020692
		public override WPProvidesEffect ProvidesEffectInternal(WPHasAnyPraetor precondition)
		{
			if (WPHasAnyPraetor.Check(this.OwningPlanner.TrueTurn, this.OwningPlanner.PlayerId))
			{
				return WPProvidesEffect.Redundant;
			}
			return WPProvidesEffect.Yes;
		}
	}
}
