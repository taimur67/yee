using System;

namespace LoG
{
	// Token: 0x02000172 RID: 370
	public class WPHellsMaw : WorldProperty
	{
		// Token: 0x06000716 RID: 1814 RVA: 0x0002269E File Offset: 0x0002089E
		public static bool Check(TurnState turn, PlayerState caster)
		{
			return turn.IsRitualActive(caster, "beelzebub_hells_maw");
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x000226AC File Offset: 0x000208AC
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return WPHellsMaw.Check(viewContext.CurrentTurn, playerState);
		}
	}
}
