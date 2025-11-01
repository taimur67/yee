using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000613 RID: 1555
	public class RemoveResourcesEventOrder : PlayGrandEventOrder
	{
		// Token: 0x06001CDC RID: 7388 RVA: 0x000639A3 File Offset: 0x00061BA3
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			return Enumerable.Empty<ActionPhase>();
		}
	}
}
