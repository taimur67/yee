using System;

namespace LoG
{
	// Token: 0x02000170 RID: 368
	public class WPHasSchemes : WorldProperty
	{
		// Token: 0x06000711 RID: 1809 RVA: 0x00022620 File Offset: 0x00020820
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			bool flag = planner.IsActionPlannedThisTurn(ActionID.Draw_New_Scheme);
			return playerState.NumSchemes >= playerState.SchemeSlots || flag;
		}
	}
}
