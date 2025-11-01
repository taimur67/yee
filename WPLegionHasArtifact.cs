using System;
using System.Linq;

namespace LoG
{
	// Token: 0x02000179 RID: 377
	public class WPLegionHasArtifact : WorldProperty
	{
		// Token: 0x06000729 RID: 1833 RVA: 0x000228D8 File Offset: 0x00020AD8
		public WPLegionHasArtifact(Identifier legionID, bool val)
		{
			this.LegionID = legionID;
			this.Value = val;
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x000228EE File Offset: 0x00020AEE
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return IEnumerableExtensions.Any<Artifact>(viewContext.CurrentTurn.FetchGameItem<GamePiece>(this.LegionID).GetAttachedAndPendingItems(viewContext.CurrentTurn).OfType<Artifact>()) == this.Value;
		}

		// Token: 0x04000349 RID: 841
		public Identifier LegionID;

		// Token: 0x0400034A RID: 842
		public bool Value;
	}
}
