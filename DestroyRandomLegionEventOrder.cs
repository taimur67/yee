using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x020005F4 RID: 1524
	public class DestroyRandomLegionEventOrder : PlayGrandEventOrder
	{
		// Token: 0x06001C89 RID: 7305 RVA: 0x000626E8 File Offset: 0x000608E8
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			return Enumerable.Empty<ActionPhase>();
		}
	}
}
