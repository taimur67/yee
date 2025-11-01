using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000611 RID: 1553
	public class RemoveResourceOfTypeEventOrder : PlayGrandEventOrder
	{
		// Token: 0x06001CD7 RID: 7383 RVA: 0x000638AD File Offset: 0x00061AAD
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			return Enumerable.Empty<ActionPhase>();
		}
	}
}
