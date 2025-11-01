using System;
using System.Linq;

namespace LoG
{
	// Token: 0x02000153 RID: 339
	public class WPAnyFixturesWithFreeSlots : WorldProperty
	{
		// Token: 0x060006B0 RID: 1712 RVA: 0x00021652 File Offset: 0x0001F852
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return viewContext.CurrentTurn.GetPlacesOfPower(playerState.Id, true).Any((GamePiece t) => t.RemainingSlots > 0);
		}
	}
}
