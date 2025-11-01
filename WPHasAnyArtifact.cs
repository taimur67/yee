using System;

namespace LoG
{
	// Token: 0x0200016C RID: 364
	public class WPHasAnyArtifact : WorldProperty<WPHasAnyArtifact>
	{
		// Token: 0x06000704 RID: 1796 RVA: 0x00022426 File Offset: 0x00020626
		public static bool Check(TurnState turn, int playerId)
		{
			return IEnumerableExtensions.Any<Artifact>(turn.GetGameItemsControlledBy<Artifact>(playerId));
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x00022434 File Offset: 0x00020634
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return WPHasAnyArtifact.Check(planner.TrueTurn, playerState.Id);
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x00022447 File Offset: 0x00020647
		public override WPProvidesEffect ProvidesEffectInternal(WPHasAnyArtifact precondition)
		{
			if (WPHasAnyArtifact.Check(this.OwningPlanner.TrueTurn, this.OwningPlanner.PlayerId))
			{
				return WPProvidesEffect.Redundant;
			}
			return WPProvidesEffect.Yes;
		}
	}
}
