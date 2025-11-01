using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x0200060D RID: 1549
	public class ReducePowerEventOrder : PlayGrandEventOrder
	{
		// Token: 0x06001CC9 RID: 7369 RVA: 0x0006358F File Offset: 0x0006178F
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			return Enumerable.Empty<ActionPhase>();
		}
	}
}
