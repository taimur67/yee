using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x02000181 RID: 385
	public class WPNeutralCantonsAvailable : WorldProperty
	{
		// Token: 0x06000740 RID: 1856 RVA: 0x00022BBC File Offset: 0x00020DBC
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			HashSet<HexCoord> hashSet;
			return planner.TerrainInfluenceMap.TryGetUnclaimedBorderForPlayer(playerState.Id, out hashSet) && hashSet.Count > 0;
		}
	}
}
