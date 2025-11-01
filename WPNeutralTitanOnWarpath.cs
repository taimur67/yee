using System;
using System.Linq;

namespace LoG
{
	// Token: 0x02000182 RID: 386
	public class WPNeutralTitanOnWarpath : WorldProperty
	{
		// Token: 0x06000742 RID: 1858 RVA: 0x00022BF1 File Offset: 0x00020DF1
		public static bool Check(TurnState turnView)
		{
			return turnView.GetAllActiveLegionsForPlayer(-1).Any((GamePiece legion) => legion.Level > 5);
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x00022C1E File Offset: 0x00020E1E
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return WPNeutralTitanOnWarpath.Check(viewContext.CurrentTurn);
		}
	}
}
