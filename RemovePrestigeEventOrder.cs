using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x0200060F RID: 1551
	public class RemovePrestigeEventOrder : PlayGrandEventOrder
	{
		// Token: 0x06001CD2 RID: 7378 RVA: 0x000637A7 File Offset: 0x000619A7
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			return Enumerable.Empty<ActionPhase>();
		}
	}
}
